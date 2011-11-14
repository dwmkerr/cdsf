using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Services;
using System.IO;
using System.Data.Services.Providers;
using System.Reflection;

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
                    ResourceType = EntityToResourceMapping.MapEntityType(entityType, context, metadataProvider.ContainerNamespace),
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
                        miAddObject.Invoke(compositeResourceSet.QueryRoot, new object[] {resourceValue});
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
        }

        private DataService<T> dataService;
        private T context;
        private Uri serviceUri;
    }
}