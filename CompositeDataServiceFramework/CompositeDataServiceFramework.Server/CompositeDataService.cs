using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Services;
using System.Data.Services.Providers;

namespace CompositeDataServiceFramework.Server
{
    public class Product
    {
        public int ProdKey { get; set; }
        public string Name { get; set; }
        public Decimal Price { get; set; }
        public Decimal Cost { get; set; }
    }

    public class CompositeDataService : DataService<CompositeDataServiceContext>,
        IServiceProvider
    {
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
                return GetQueryProvider();

            return null;
        }

        private CompositeDataServiceMetadataProvider GetMetadataProvider()
        {
            //  If we already have the metadata provider, return it.
            if (metadataProvider != null)
                return metadataProvider;

            //  Create the metadata provider.
            metadataProvider = new CompositeDataServiceMetadataProvider();
            var productType = new ResourceType(
        typeof(Product), // CLR type backing this Resource 
        ResourceTypeKind.EntityType, // Entity, ComplexType etc 
        null, // BaseType 
        "Namespace", // Namespace 
        "Product", // Name 
        false // Abstract? 
    );
            var prodKey = new ResourceProperty(
               "ProdKey",
               ResourcePropertyKind.Key |
               ResourcePropertyKind.Primitive,
               ResourceType.GetPrimitiveResourceType(typeof(int))
            );
            var prodName = new ResourceProperty(
               "Name",
               ResourcePropertyKind.Primitive,
               ResourceType.GetPrimitiveResourceType(typeof(string))
            );
            var prodPrice = new ResourceProperty(
               "Price",
               ResourcePropertyKind.Primitive,
               ResourceType.GetPrimitiveResourceType(typeof(Decimal))
            );
            productType.AddProperty(prodKey);
            productType.AddProperty(prodName);
            productType.AddProperty(prodPrice);
            metadataProvider.AddResourceType(productType);
            metadataProvider.AddResourceSet(
               new ResourceSet("Products", productType)
            );


            return metadataProvider; 
        }

        private CompositeDataServiceQueryProvider GetQueryProvider()
        {
            //  Return the query provider if it already exists.
            if (queryProvider != null)
                return queryProvider;

            //  Create the query provider.
            queryProvider = new CompositeDataServiceQueryProvider();

            //  Return the query provider.
            return queryProvider;
        }

        public void AddDataSource(CompositeDataSource source)
        {
            //  Add the composite data source.
            compositeDataSources.Add(source.Name, source);
        }

        public void Initialise()
        {
            //  Initialise each data source.
            foreach (var dataSource in compositeDataSources.Values)
                dataSource.Initialise();
        }

        /// <summary>
        /// The Metadata Provider.
        /// </summary>
        private CompositeDataServiceMetadataProvider metadataProvider;

        /// <summary>
        /// The query provider.
        /// </summary>
        private CompositeDataServiceQueryProvider queryProvider;

        /// <summary>
        /// The composite data sources.
        /// </summary>
        private Dictionary<string, CompositeDataSource> compositeDataSources =
            new Dictionary<string, CompositeDataSource>();
    }
}
