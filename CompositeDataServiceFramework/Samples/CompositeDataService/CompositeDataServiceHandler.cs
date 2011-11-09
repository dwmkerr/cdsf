using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using UsersDataModel;
using OrdersDataModel;
using System.Data.Services;

namespace CompositeDataService
{
    public class CompositeDataServiceHandler : IHttpHandler
    {
        public CompositeDataServiceHandler()
        {
            Initialise();
        }

        public void Initialise()
        {
            //  Initialise the services.
            usersDataProvider.Initialise(new UsersDataService(),
                new Uri("http://localhost:1212/UsersDataService.svc"));
            ordersDataProvider.Initialise(new OrdersDataService(),
                new Uri("http://localhost:1212/OrdersDataService.svc"));

            metadataProvider.AppendMetadata(usersDataProvider.GetMetadata());
            metadataProvider.AppendMetadata(ordersDataProvider.GetMetadata());
        }

        public bool IsReusable 
        { 
            get
            { 
                return true; 
            } 
        }

        public void ProcessRequest(HttpContext context)
        {
            //  Get the URI.
            Uri uri = context.Request.Url;


            //  Unify the respose from both.
            context.Response.Write(usersDataProvider.Respond(context));
            context.Response.Write(ordersDataProvider.Respond(context));
        }

        EntityFrameworkDataProvider<UsersDataModelContainer> usersDataProvider = 
            new EntityFrameworkDataProvider<UsersDataModelContainer>();


        EntityFrameworkDataProvider<OrdersModelContainer> ordersDataProvider =
            new EntityFrameworkDataProvider<OrdersModelContainer>();

        MetadataProvider metadataProvider = new MetadataProvider();
    }
}