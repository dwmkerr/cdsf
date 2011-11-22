using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Data.Services.Common;
using System.Web;
using System.ServiceModel.Web;
$if$ ($targetframeworkversion$ >= 3.5)using System.Linq;
$endif$using System.Text;
using CompositeDataServiceFramework.Server;

namespace $rootnamespace$
{
	public class $safeitemrootname$ : CompositeDataService
	{  
        public $safeitemrootname$()
        {
          //  Create a data service and data model instance.
          //  Example:          
          //var dataService1 = new DataService1();
          //var dataModel1 = new DataModelContainer1();
          //var dataService2 = new DataService2();
          //var dataModel2 = new DataModelContainer2();
          
          //  Add each data service.
          //  Example:
          //AddDataSource(new EntityFrameworkDataSource<DataModelContainer1>(dataService1, dataModel1, new Uri("http://localhost/DataService1.svc")));
          //AddDataSource(new EntityFrameworkDataSource<DataModelContainer2>(dataService2, dataModel2, new Uri("http://localhost/DataService2.svc")));

          // Initialise the data service.
          Initialise();
        }
        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(DataServiceConfiguration config)
        {
            // Examples:
            //config.SetEntitySetAccessRule("*", EntitySetRights.All);
            // config.SetServiceOperationAccessRule("MyServiceOperation", ServiceOperationRights.All);
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V2;
        }
	}
}