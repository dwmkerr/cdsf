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

namespace Test1
{
    public class CompositeDataService : CompositeDataServiceFramework.Server.CompositeDataService
    {
        private WcfDataServiceDataSource<OrdersModelContainer> ordersDataService;
        private WcfDataServiceDataSource<UsersDataModelContainer> usersDataService;

        public CompositeDataService()
        {
            var ordersDS = new OrdersDataService();
            var ordersDM = new OrdersModelContainer();
            var usersDS = new UsersDataService();
            var usersDM = new UsersDataModelContainer();

            ordersDataService = new WcfDataServiceDataSource<OrdersModelContainer>(ordersDS, 
                ordersDM, new Uri("http://localhost:53282/OrdersDataService.svc"));

            usersDataService = new WcfDataServiceDataSource<UsersDataModelContainer>(usersDS,
                usersDM, new Uri("http://localhost:53282/UsersDataService.svc"));
            
            AddDataSource(ordersDataService);
            AddDataSource(usersDataService);

            Initialise();
        }

        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(DataServiceConfiguration config)
        {
            // Examples:
            config.SetEntitySetAccessRule("*", EntitySetRights.All);
            config.SetServiceOperationAccessRule("*", ServiceOperationRights.All);
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V2;
        }


        [WebGet]
        public IQueryable<Product> GetProductsStartingWith(string startingWith)
        {
            return from p in ordersDataService.Context.Products where p.Name.StartsWith(startingWith) select p;
        }

        [WebGet]
        public Product FunctionReturningEntity()
        {
            return (from p in ordersDataService.Context.Products orderby p.Price descending select p).FirstOrDefault();
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
            return ordersDataService.Context.Products;
        }
    }
}
