using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Services;
using System.IO;
using System.Text;

namespace CompositeDataService
{
    public class EntityFrameworkDataProvider<T>
    {
        public void Initialise(DataService<T> service, Uri uri)
        {
            dataService = service;
            serviceUri = uri;
        }

        public string GetMetadata()
        {
            StringBuilder builder = new StringBuilder();

            HttpContext ctx = new HttpContext(
                new HttpRequest("", odataUri.ToString() + "$metadata", null),
                new HttpResponse(new StringWriter(builder)));

            return Respond(ctx);
        }

        public string Respond(HttpContext context)
        {
            //  Create the http host for the request.
            HttpHost host = new HttpHost(serviceUri, context);

            //  Attach and process.
            dataService.AttachHost(host);
            dataService.ProcessRequest();

            //  Read response.
            var stre = host.ResponseStream;
            stre.Seek(0, SeekOrigin.Begin);
            string response = null;
            using (StreamReader reader = new StreamReader(stre))
            {
                response = reader.ReadToEnd();
            }

            host.Flush();
            host.Close();

            return response;
        }

        public void Initialise()
        {
            /*
            DataServiceConfiguration config = new DataServiceConfiguration();

            config.SetEntitySetAccessRule("*", EntitySetRights.AllRead);
            // config.SetServiceOperationAccessRule("MyServiceOperation", ServiceOperationRights.All);
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V2;*/
        }

        /// <summary>
        /// The underlying data service.
        /// </summary>
        private DataService<T> dataService = new DataService<T>();

        /// <summary>
        /// The service Uri.
        /// </summary>
        private Uri serviceUri;

        private Uri odataUri = new Uri("http://localhost:1212/odata/");
    }
}