using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Services.Providers;

namespace CompositeDataServiceFramework.Server
{
    public class CompositeDataServiceQueryProvider : IDataServiceQueryProvider
    {
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
            throw new NotImplementedException();
        }

        public ResourceType GetResourceType(object target)
        {
            throw new NotImplementedException();
        }

        public object InvokeServiceOperation(ServiceOperation serviceOperation, object[] parameters)
        {
            throw new NotImplementedException();
        }

        public bool IsNullPropagationRequired
        {
            get { throw new NotImplementedException(); }
        }
    }
}
