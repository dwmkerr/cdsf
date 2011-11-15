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

namespace CompositeDataServiceFramework.Server
{
    public class EntityFrameworkDataSource<T> : CompositeDataSource where T : ObjectContext
    {
        public EntityFrameworkDataSource(DataService<T> dataService, T context, Uri serviceUri)
        {
          this.dataService = dataService;
          this.context = context;
          this.serviceUri = serviceUri;
        }

        public override void CancelChanges()
        {
          //  *** TODO cancel changes?
        }

        public override void SaveChanges()
        {
          context.SaveChanges();
        }

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
            
            //  Freeze the metadata.
            metadataProvider.Freeze();
        }


        private ResourceType MapEdmType(EdmType edmType)
        {
            if (edmType is PrimitiveType)
                return ResourceType.GetPrimitiveResourceType(((PrimitiveType)edmType).ClrEquivalentType);
            throw new Exception("Don't know type " + edmType.Name);
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
            //  *** TODO Each navigation property is tagged with associationType.Name, so we should be able to get it like that.
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
                       MapEdmType(propertyType.TypeUsage.EdmType)
                    );
                resourceType.AddProperty(resourceProperty);
            }
            
            return resourceType;
        }


        private DataService<T> dataService;
        private T context;
        private Uri serviceUri;
    }
}