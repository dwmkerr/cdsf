using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Services;
using System.IO;
using System.Data.Services.Providers;
using System.Reflection;
using System.Data.Metadata.Edm;
using System.ServiceModel.Web;

namespace CompositeDataServiceFramework.Server
{
    /// <summary>
    /// A WcfDataServiceDataSource is a data source that is based
    /// on a Wcf data service.
    /// </summary>
    /// <typeparam name="T">The ObjectContext the data service is exposing.</typeparam>
    public class WcfDataServiceDataSource<T> : CompositeDataSource where T : ObjectContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WcfDataServiceDataSource&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="dataService">The data service.</param>
        /// <param name="context">The context.</param>
        /// <param name="serviceUri">The service URI.</param>
        public WcfDataServiceDataSource(DataService<T> dataService, T context, Uri serviceUri)
        {
          this.dataService = dataService;
          this.context = context;
          this.serviceUri = serviceUri;
        }

        /// <summary>
        /// Cancels the changes.
        /// </summary>
        public override void CancelChanges()
        {
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        public override void SaveChanges()
        {
          context.SaveChanges();
        }

        /// <summary>
        /// Initialises the instance, putting all metadata into the supplied metadata provider.
        /// </summary>
        /// <param name="metadataProvider">The metadata provider.</param>
        public override void Initialise(CompositeDataServiceMetadataProvider metadataProvider)
        {
            //  Load the metadata.
            WcfDataServiceMetadataLoader metadataLoader = new WcfDataServiceMetadataLoader();
            metadataLoader.LoadMetadata(serviceUri);

            //  Go through each entity type and map it to a resource type.
            foreach (var entityType in metadataLoader.EntityTypes)
            {
                //    Create the composite resource type.
                CompositeResourceType compositeResourceType = new CompositeResourceType()
                {
                    ResourceType = MapEntityType(entityType, context, metadataProvider.ContainerNamespace),
                    DataSource = this,
                    Name = entityType.Name
                };

              //  Create Add relationship.
                compositeResourceType.AddReferenceToCollectionAction +=
                  (resource, propertyName, resourceToAdd) =>
                  {
                    PropertyInfo pi = compositeResourceType.ResourceType.InstanceType.GetProperty(propertyName);
                    var propertyValue = pi.GetValue(resource, null);
                    MethodInfo mi = propertyValue.GetType().GetMethod("Add");
                    mi.Invoke(propertyValue, new object[] { resourceToAdd });
                  };

                //  Create Remove relationship.
                compositeResourceType.RemoveReferenceFromCollectionAction +=
                  (resource, propertyName, resourceToRemove) =>
                  {
                    PropertyInfo pi = compositeResourceType.ResourceType.InstanceType.GetProperty(propertyName);
                    var propertyValue = pi.GetValue(resource, null);
                    MethodInfo mi = propertyValue.GetType().GetMethod("Remove");
                    mi.Invoke(propertyValue, new object[] { resourceToRemove });
                  };

                //    Add the composite resource type.
                metadataProvider.AddCompositeResourceType(compositeResourceType);
            }

            //  Go through each entity set and map it to a resource set.
            foreach (var entitySet in metadataLoader.EntitySets)
            {
                //    If we cannot determine the resource type, then we must bail.
                ResourceType setType;
                if (metadataProvider.TryResolveResourceType(entitySet.ElementType.Name, out setType) == false)
                    throw new Exception("Failed to resource the resource type " + entitySet.ElementType.Name + " for the set " + entitySet.Name);

                //    Create the composite resource set.
                CompositeResourceSet compositeResourceSet = new CompositeResourceSet();
                compositeResourceSet.Name = entitySet.Name;
                compositeResourceSet.ResourceSet = new ResourceSet(entitySet.Name, setType);

                //    We must be able to get the query root for the resource set.
                //  The resource set name will be an IQueryable set in our context.
                PropertyInfo pi = typeof(T).GetProperty(entitySet.Name);
                compositeResourceSet.QueryRoot = pi.GetValue(context, null) as IQueryable;

                //  Get the 'AddObject' and 'DeleteObject' functions.
                MethodInfo miAddObject = pi.GetValue(context, null).GetType().GetMethod("AddObject");
                MethodInfo miDeleteObject = pi.GetValue(context, null).GetType().GetMethod("DeleteObject");

                //  The Create Resource function must return an instance of the resource type.
                compositeResourceSet.CreateResourceAction +=
                    () =>
                    {
                        return Activator.CreateInstance(compositeResourceSet.ResourceSet.ResourceType.InstanceType);
                    };

                //  The Add Resource function must add the resource.
                compositeResourceSet.AddResourceAction +=
                    (resourceType, resourceValue) =>
                    {
                        //  Invoke the 'AddObject' function.
                        miAddObject.Invoke(compositeResourceSet.QueryRoot, new object[] { resourceValue });
                    };

                //  The Delete Resource function must delete the resource.
                compositeResourceSet.DeleteResourceAction +=
                    (resourceValue) =>
                    {
                        //  Invoke the 'DeleteObject' function.
                        miDeleteObject.Invoke(compositeResourceSet.QueryRoot, new object[] { resourceValue });
                    };

                //    Add the resource set.
                metadataProvider.AddCompositeResourceSet(compositeResourceSet);
            }

            //  Go through each entity type and create the navigation properties.
            foreach (var entityType in metadataLoader.EntityTypes)
            {
                CreateNavigationProperties(metadataProvider, entityType, context);
            }

            //  Go through each entity type create the associations.
            foreach (var associationType in metadataLoader.AssociationTypes)
            {
                CreateAssociation(metadataProvider, associationType, context);
            }

            //  Go through each service operation and add it to the metadata.
            foreach (var functionImport in metadataLoader.FunctionImports)
            {
                CreateServiceOperation(metadataProvider, functionImport);
            }
            
            //  Freeze the metadata.
            metadataProvider.Freeze();
        }
        
        private void CreateNavigationProperties(CompositeDataServiceMetadataProvider metadataProvider, EntityType entityType, ObjectContext context)
        {
            //  Go through each navigation property.
            foreach (var navigationProperty in entityType.NavigationProperties)
            {
                CompositeResourceType fromType, toType;
                metadataProvider.TryResolveCompositeResourceType(entityType.Name, out fromType);
                metadataProvider.TryResolveCompositeResourceType(navigationProperty.ToEndMember.GetEntityType().Name, out toType);

                //  Create the resource property.
                ResourceProperty resourceProperty = new ResourceProperty(
                    navigationProperty.Name,
                    navigationProperty.ToEndMember.RelationshipMultiplicity == RelationshipMultiplicity.Many 
                    ? ResourcePropertyKind.ResourceSetReference : ResourcePropertyKind.ResourceReference,
                    toType.ResourceType);

                //  Tag the resource property with the association name.
                resourceProperty.CustomState = navigationProperty.RelationshipType.Name;

                //  Add the resource property.
                fromType.ResourceType.AddProperty(resourceProperty);
            }
        }

        

        /// <summary>
        /// Creates the service operation.
        /// </summary>
        /// <param name="edmFunction">The edm function.</param>
        private void CreateServiceOperation(CompositeDataServiceMetadataProvider metadataProvider, EdmFunction edmFunction)
        {
            //  Create a set of service operation parameters.
            List<ServiceOperationParameter> serviceOperationParameters = new List<ServiceOperationParameter>();

            //  Add each service operation parameter.
            foreach (var edmParameter in edmFunction.Parameters)
            {
                //  Add the service operation parameter.
                serviceOperationParameters.Add(new ServiceOperationParameter(edmParameter.Name, 
                    CompositeDataServiceMetadataProvider.MapEdmTypeToResourceType(edmParameter.TypeUsage.EdmType)));
            }

            //  Unless we determine otherwise, the result kind will be void
            //  and the result set will be null.
            ServiceOperationResultKind resultKind = ServiceOperationResultKind.Void;
            ResourceType resultType = null;
            ResourceSet resultSet = null;

            //  We can only set return type information if the return
            //  type is not set (i.e. it's void).
            if (edmFunction.ReturnParameter != null)
            {
                //  Get the return parameter type.
                var edmReturnType = edmFunction.ReturnParameter.TypeUsage.EdmType;

                //  Are we returning a collection?
                if (edmReturnType is CollectionType)
                {
                    //  Cast the collection.
                    CollectionType edmCollection = edmReturnType as CollectionType;

                    //  Set the collection type.
                    resultKind = ServiceOperationResultKind.QueryWithMultipleResults;

                    //  Set the result set.
                    CompositeResourceSet compositeResourceSet;
                    if (metadataProvider.TryResolveCompositeResourceSetForResourceType(edmCollection.TypeUsage.EdmType.Name,
                        out compositeResourceSet) == false)
                        throw new DataServiceException("Unable to build metadata for the Service Operation '" + edmFunction.Name + "'.");
                    resultSet = compositeResourceSet.ResourceSet;

                    //  Set the resource type.
                    resultType = compositeResourceSet.ResourceSet.ResourceType;
                }
            }

            //  TODO: Create the method. At the moment we force get
            //  but we want to support POST as well.
            string method = "GET";

            //  Create the service operation.
            ServiceOperation serviceOperation = new ServiceOperation(edmFunction.Name,
                resultKind, resultType, resultSet, method, serviceOperationParameters);

            //  Create the composite service operation.  
            CompositeServiceOperation compositeServiceOperation = new CompositeServiceOperation()
            {
                DataSource = this,
                Name = edmFunction.Name,
                ServiceOperation = serviceOperation
            };

            //  Add the service operation.
            metadataProvider.AddCompositeServiceOperation(compositeServiceOperation);
        }

        private IEnumerable<ResourceProperty> GetAssociationEnds(CompositeDataServiceMetadataProvider metadataProvider, string assocationName)
        {
            foreach (var resourceType in metadataProvider.Types)
            {
              foreach(var resourceProperty in resourceType.Properties.Where((rp) => { return rp.CustomState is string ? (string)rp.CustomState == assocationName : false; } ))
                   yield return resourceProperty;
            }
        }

        private void CreateAssociation(CompositeDataServiceMetadataProvider metadataProvider, AssociationType associationType, ObjectContext context)
        {
            IEnumerable<ResourceProperty> props = GetAssociationEnds(metadataProvider, associationType.Name);
            var end1Property = props.ElementAt(0);
            var end2Property = (props.Count() < 2) ? null : props.ElementAt(1);

          //  Get resource types.
            CompositeResourceType resourceType1, resourceType2;
            metadataProvider.TryResolveCompositeResourceType(associationType.AssociationEndMembers[0].Name, out resourceType1);
            metadataProvider.TryResolveCompositeResourceType(associationType.AssociationEndMembers[1].Name, out resourceType2);

          //  switch.
            if (resourceType1.Name == end1Property.ResourceType.Name)
            {
              var temp = resourceType1;
              resourceType1 = resourceType2;
              resourceType2 = temp;
            }
            
            //  Get the from and to sets.
            ResourceSet end1Set = (from crs in metadataProvider.ResourceSets where crs.ResourceType.Name == resourceType1.ResourceType.Name select crs).First();
            ResourceSet end2Set = (from crs in metadataProvider.ResourceSets where crs.ResourceType.Name == resourceType2.ResourceType.Name select crs).First();

            //  Create the association.
            ResourceAssociationSet associationSet = new ResourceAssociationSet(
                associationType.Name,
                new ResourceAssociationSetEnd(end1Set, resourceType1.ResourceType, end1Property),
                new ResourceAssociationSetEnd(end2Set, resourceType2.ResourceType, end2Property));
            end1Property.CustomState = associationSet;
            if(end2Property != null)
              end2Property.CustomState = associationSet;

            //  Add the association set.
            metadataProvider.AddCompositeResourceAssociationSet(
                new CompositeResourceAssociationSet()
                {
                    Name = associationSet.Name,
                    ResourceAssociationSet = associationSet,
                    DataSource = this
                });
        }

        private ResourceType MapEntityType(EntityType entityType, ObjectContext context, string namespaceName)
        {
            //  *** How do we get the CLR tye/
            string parentNamespace = context.GetType().Namespace;
            var ss = context.GetType().Assembly;
            var type = (from t in ss.GetTypes() where t.Name == entityType.Name select t).FirstOrDefault();

            //  Create the resource type.
            ResourceType resourceType = new ResourceType(
              type,
              ResourceTypeKind.EntityType,
              null, // base types not supported.
              namespaceName,
              entityType.Name,
              entityType.Abstract
              );

            //  Add each property.
            //  *** TODO Complex types are not supported. We assume the first property is the key.
            bool first = true;
            foreach (var propertyType in entityType.Properties)
            {
                ResourcePropertyKind kind = ResourcePropertyKind.Primitive;
                if (first)
                {
                    kind |= ResourcePropertyKind.Key;
                    first = false;
                }

                var resourceProperty = new ResourceProperty(
                       propertyType.Name,
                       kind,
                       CompositeDataServiceMetadataProvider.MapEdmTypeToResourceType(propertyType.TypeUsage.EdmType)
                    );
                resourceType.AddProperty(resourceProperty);
            }
            
            return resourceType;
        }

        /// <summary>
        /// The data service.
        /// </summary>
        private DataService<T> dataService;

        /// <summary>
        /// The Data Context.
        /// </summary>
        private T context;

        /// <summary>
        /// The Service Uri.
        /// </summary>
        private Uri serviceUri;

        /// <summary>
        /// Gets the context.
        /// </summary>
        public T Context
        {
            get { return context; }
        }
    }
}