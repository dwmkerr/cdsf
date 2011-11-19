using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Data.Services.Common;
using System.Linq;
using System.ServiceModel.Web;
using System.Web;
using OrdersDataModel;

namespace Test1
{
    public class OrdersDataService : DataService<OrdersModelContainer>
    {
        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(DataServiceConfiguration config)
        {
            // Examples:
            config.SetEntitySetAccessRule("*", EntitySetRights.All);
            config.SetServiceOperationAccessRule("GetProductsStartingWith", ServiceOperationRights.All);
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V2;
        }

        [WebGet]
        public IQueryable<Product> GetProductsStartingWith(string startingWith)
        {
            return from p in CurrentDataSource.Products where p.Name.StartsWith(startingWith) select p;
        }
    }
}
