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

        public override void Initialise(CompositeDataServiceMetadataProvider metadataProvider)
        {
          //  Load the metadata.
          WcfDataServiceMetadataLoader metadataLoader = new WcfDataServiceMetadataLoader();
          metadataLoader.LoadMetadata(serviceUri);

          //  Go through each entity type and map it to a resource type.
          foreach (var entityType in metadataLoader.EntityTypes)
          {
            metadataProvider.AddResourceType(EntityToResourceMapping.MapEntityType(entityType, context, metadataProvider.ContainerNamespace), this);
          }

          //  Go through each entity set and map it to a resource set.
          foreach(var entitySet in metadataLoader.EntitySets)
          {
            ResourceType setType;
            if(metadataProvider.TryResolveResourceType(entitySet.ElementType.Name, out setType))
            {
              metadataProvider.AddResourceSet(new ResourceSet(entitySet.Name, setType), this);
            }
          }
        }

        public override IQueryable GetQueryRootForResourceSet(ResourceSet resourceSet)
        {
          //  The resource set name will be an IQueryable set in our context.
          PropertyInfo pi = typeof(T).GetProperty(resourceSet.Name);
          return pi.GetValue(context, null) as IQueryable;
        }
      
        private DataService<T> dataService;
        private T context;
        private Uri serviceUri;
    }
}
