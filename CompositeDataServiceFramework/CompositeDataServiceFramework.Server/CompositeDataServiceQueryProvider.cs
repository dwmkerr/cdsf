using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Services.Providers;

namespace CompositeDataServiceFramework.Server
{
    public class CompositeDataServiceQueryProvider : IDataServiceQueryProvider
    {
        public CompositeDataServiceQueryProvider(CompositeDataServiceMetadataProvider metadataProvider)
        {
            this.metadataProvider = metadataProvider;
        }

        private CompositeDataServiceMetadataProvider metadataProvider;

        private CompositeDataServiceContext compositeDataServiceContext;

        public object CurrentDataSource
        {
            get
            {
                return compositeDataServiceContext;
            }
            set
            {
                compositeDataServiceContext = (CompositeDataServiceContext)value;
            }
        }

        public object GetOpenPropertyValue(object target, string propertyName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<string, object>> GetOpenPropertyValues(object target)
        {
            throw new NotImplementedException();
        }

        public object GetPropertyValue(object target, ResourceProperty resourceProperty)
        {
            throw new NotImplementedException();
        }

        public IQueryable GetQueryRootForResourceSet(ResourceSet resourceSet)
        {
            CompositeDataSource source;
            if(metadataProvider.TryResolveResourceSet(resourceSet.Name, out resourceSet, out source) == false)
                return null;
            return source.GetQueryRootForResourceSet(resourceSet);
        }

        public ResourceType GetResourceType(object target)
        {
            Type type = target.GetType();
            return metadataProvider.Types.Single(t => t.InstanceType == type); 
        }

        public object InvokeServiceOperation(ServiceOperation serviceOperation, object[] parameters)
        {
            throw new NotImplementedException();
        }

        public bool IsNullPropagationRequired
        {
            get { return true; }
        }
    }
}
