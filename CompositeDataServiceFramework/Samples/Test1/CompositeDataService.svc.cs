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
        public CompositeDataService()
        {
          var ordersDS = new OrdersDataService();
          var ordersDM = new OrdersModelContainer();
          var usersDS = new UsersDataService();
          var usersDM = new UsersDataModelContainer();

            AddDataSource(new EntityFrameworkDataSource<OrdersModelContainer>(ordersDS, ordersDM, new Uri("http://localhost:53282/OrdersDataService.svc")));
            AddDataSource(new EntityFrameworkDataSource<UsersDataModelContainer>(usersDS, usersDM, new Uri("http://localhost:53282/UsersDataService.svc")));

            Initialise();
        }

        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(DataServiceConfiguration config)
        {
            // TODO: set rules to indicate which entity sets and service operations are visible, updatable, etc.
            // Examples:
            config.SetEntitySetAccessRule("*", EntitySetRights.All);
            // config.SetServiceOperationAccessRule("MyServiceOperation", ServiceOperationRights.All);
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V2;
        }
    }
}
