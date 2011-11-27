using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Data.Services.Common;
using System.Linq;
using System.ServiceModel.Web;
using System.Web;
using CompositeDataServiceFramework.Server;
using OrdersDataModel;
using UsersDataModel;

namespace CompositeDataServiceSample
{
    /// <summary>
    /// This is an example of a composite data service.
    /// </summary>
    public class CompositeDataServiceSample : CompositeDataServiceFramework.Server.CompositeDataService
    {
        /// <summary>
        /// A WcfDataServiceDataSource for the Orders Data Service.
        /// </summary>
        private WcfDataServiceDataSource<OrdersModelContainer> ordersDataSource;

        /// <summary>
        /// A WcfDataServiceDataSource for the Users Data Service.
        /// </summary>
        private WcfDataServiceDataSource<UsersDataModelContainer> usersDataSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeDataServiceSample"/> class.
        /// </summary>
        public CompositeDataServiceSample()
        {
            //  Create the orders data source, by providing an Orders Data Service, Orders Model Container
            //  and the Url to the data service.
            ordersDataSource = new WcfDataServiceDataSource<OrdersModelContainer>(
                new OrdersDataService(),
                new OrdersModelContainer(),
                new Uri("http://localhost:65110/OrdersDataService.svc"));

            //  Create the users data source, by providing a Users Data Service, Users Model Container
            //  and the Url to the data service.
            usersDataSource = new WcfDataServiceDataSource<UsersDataModelContainer>(
                new UsersDataService(),
                new UsersDataModelContainer(),
                new Uri("http://localhost:65110/UsersDataService.svc"));
            
            //  Add each data source.
            AddDataSource(ordersDataSource);
            AddDataSource(usersDataSource);

            //  Initialise the data service.
            Initialise();
        }

        /// <summary>
        /// Initializes the service.
        /// </summary>
        /// <param name="config">The config.</param>
        public static void InitializeService(DataServiceConfiguration config)
        {
            //  Allow full access to all entity sets and all service operations.
            config.SetEntitySetAccessRule("*", EntitySetRights.All);
            config.SetServiceOperationAccessRule("*", ServiceOperationRights.All);
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V2;
        }


        [WebGet]
        public IQueryable<Product> GetProductsStartingWith(string startingWith)
        {
            return from p in ordersDataSource.Context.Products where p.Name.StartsWith(startingWith) select p;
        }

        [WebGet]
        public Product FunctionReturningEntity()
        {
            return (from p in ordersDataSource.Context.Products orderby p.Price descending select p).FirstOrDefault();
        }

        [WebGet]
        public string FunctionReturningPrimitive()
        {
            return "abc";
        }

        [WebGet]
        public void FunctionReturningVoid(int a, int b)
        {
        }

        [WebGet]
        public IEnumerable<Product> FunctionReturningEnumerable()
        {
            return ordersDataSource.Context.Products;
        }
    }
}
