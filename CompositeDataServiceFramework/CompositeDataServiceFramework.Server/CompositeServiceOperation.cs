using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Services.Providers;

namespace CompositeDataServiceFramework.Server
{
    public delegate object InvokeServiceOperationDelegate(object[] parameters);

    public class CompositeServiceOperation
    {
        public string Name
        {
            get;
            set;
        }

        public CompositeDataSource DataSource
        {
            get;
            set;
        }

        public ServiceOperation ServiceOperation
        {
            get;
            set;
        }

        public event InvokeServiceOperationDelegate InvokeServiceOperationAction;

        public object DoInvokeServiceOperationAction(object[] parameters)
        {
            var action = InvokeServiceOperationAction;
            if (action != null)
                return action(parameters);
            return null;
        }
    }
}
