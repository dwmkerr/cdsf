using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Services.Providers;

namespace CompositeDataServiceFramework.Server
{
    public class CompositeDataServiceQueryProvider : IDataServiceQueryProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeDataServiceQueryProvider"/> class.
        /// </summary>
        /// <param name="metadataProvider">The metadata provider.</param>
        public CompositeDataServiceQueryProvider(CompositeDataServiceMetadataProvider metadataProvider)
        {
            this.metadataProvider = metadataProvider;
        }

        /// <summary>
        /// The metadata provider.
        /// </summary>
        private CompositeDataServiceMetadataProvider metadataProvider;

        /// <summary>
        /// The data service context.
        /// </summary>
        private CompositeDataServiceContext compositeDataServiceContext;

        /// <summary>
        /// The data source object from which data is provided.
        /// </summary>
        /// <returns>The data source.</returns>
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

        /// <summary>
        /// Gets the <see cref="T:System.Linq.IQueryable`1"/> that represents the container.
        /// </summary>
        /// <param name="resourceSet">The resource set.</param>
        /// <returns>
        /// An <see cref="T:System.Linq.IQueryable`1"/> that represents the resource set, or a null value if there is no resource set for the specified <paramref name="resourceSet"/>.
        /// </returns>
        public IQueryable GetQueryRootForResourceSet(ResourceSet resourceSet)
        {
            //  Get the composite resource set.
            CompositeResourceSet compositeResourceSet;
            if (metadataProvider.TryResolveCompositeResourceSet(resourceSet.Name, out compositeResourceSet) == false)
                return null;

            //  Return the composite resource set query root.
            return compositeResourceSet.QueryRoot;
        }

        /// <summary>
        /// Gets the resource type for the instance that is specified by the parameter.
        /// </summary>
        /// <param name="target">Instance to extract a resource type from.</param>
        /// <returns>
        /// The <see cref="T:System.Data.Services.Providers.ResourceType"/> of the supplied object.
        /// </returns>
        public ResourceType GetResourceType(object target)
        {
            Type type = target.GetType();
            return metadataProvider.Types.Single(t => t.InstanceType == type); 
        }

        public object InvokeServiceOperation(ServiceOperation serviceOperation, object[] parameters)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a value that indicates whether null propagation is required in expression trees.
        /// </summary>
        /// <returns>A <see cref="T:System.Boolean"/> value that indicates whether null propagation is required.</returns>
        public bool IsNullPropagationRequired
        {
            get { return true; }
        }
    }
}
