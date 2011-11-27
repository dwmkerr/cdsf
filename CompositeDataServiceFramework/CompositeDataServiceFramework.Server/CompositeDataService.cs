using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Services;
using System.Data.Services.Providers;
using System.Reflection;
using System.ServiceModel.Web;

namespace CompositeDataServiceFramework.Server
{
    public class CompositeDataService : DataService<CompositeDataServiceContext>,
        IServiceProvider
    {
        public CompositeDataService()
        {
            //  Create the metadata provider.
            metadataProvider = GetMetadataProvider();

            //  Create the query provider.
            queryProvider = GetQueryProvider(metadataProvider);

            //    Create the update provider.
            updateProvider = GetUpdateProvider(metadataProvider, queryProvider);
        }

        /// <summary>
        /// Creates the data source.
        /// </summary>
        /// <returns></returns>
        protected override CompositeDataServiceContext CreateDataSource()
        {
            return context;
        }

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>
        /// A service object of type <paramref name="serviceType"/>.-or- null if there is no service object of type <paramref name="serviceType"/>.
        /// </returns>
        public object GetService(Type serviceType)
        {
            //  This function must return the correct
            //  object for the requested service type.

            //  Support IDataServiceMetadataProvider.
            if (serviceType == typeof(IDataServiceMetadataProvider))
                return GetMetadataProvider();

            //  Support IDataServiceQueryProvider.
            if (serviceType == typeof(IDataServiceQueryProvider))
                return GetQueryProvider(metadataProvider);

            //  Support IDataServiceUpdateProvider.
            if (serviceType == typeof(IDataServiceUpdateProvider))
                return GetUpdateProvider(metadataProvider, queryProvider);

            return null;
        }

        private CompositeDataServiceMetadataProvider GetMetadataProvider()
        {
            //  If we already have the metadata provider, return it.
            if (metadataProvider != null)
                return metadataProvider;

            //  Create the metadata provider.
            metadataProvider = new CompositeDataServiceMetadataProvider();

            //  Set the base name.
            metadataProvider.BaseName = GetType().Name;

            return metadataProvider; 
        }

        private CompositeDataServiceQueryProvider GetQueryProvider(CompositeDataServiceMetadataProvider metadataProvider)
        {
            //  Return the query provider if it already exists.
            if (queryProvider != null)
                return queryProvider;

            //  Create the query provider.
            queryProvider = new CompositeDataServiceQueryProvider(metadataProvider);

            //  Return the query provider.
            return queryProvider;
        }

        private CompositeDataServiceUpdateProvider GetUpdateProvider(CompositeDataServiceMetadataProvider metadataProvider, CompositeDataServiceQueryProvider queryProvider)
        {
            //  Return the update provider if it already exists.
            if (updateProvider != null)
                return updateProvider;

            //  Create the update provider.
            updateProvider = new CompositeDataServiceUpdateProvider(context, metadataProvider, queryProvider);

            //  Return the query provider.
            return updateProvider;
        }

        public void AddDataSource(CompositeDataSource source)
        {
            //  Add the composite data source.
            compositeDataSources.Add(source);
            context.AddDataSource(source);
        }

        public void Initialise()
        {
            //  Initialise each data source.
            foreach (var dataSource in compositeDataSources)
                dataSource.Initialise(metadataProvider);

            foreach (var serviceOperation in metadataProvider.ExposeServiceOperationsFromObject(this))
            {
                //  Add the service operation.
                metadataProvider.AddCompositeServiceOperation(serviceOperation);
            }

            //  Freeze the metadata provider.
            metadataProvider.Freeze();
        }

        private CompositeDataServiceContext context = new CompositeDataServiceContext();

        /// <summary>
        /// The Metadata Provider.
        /// </summary>
        private CompositeDataServiceMetadataProvider metadataProvider;

        /// <summary>
        /// The query provider.
        /// </summary>
        private CompositeDataServiceQueryProvider queryProvider;

        /// <summary>
        /// The update provider.
        /// </summary>
        private CompositeDataServiceUpdateProvider updateProvider;

        /// <summary>
        /// The composite data sources.
        /// </summary>
        private List<CompositeDataSource> compositeDataSources =
            new List<CompositeDataSource>();
    }
}
