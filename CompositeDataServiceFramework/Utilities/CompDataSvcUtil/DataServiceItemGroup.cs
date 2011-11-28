using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompDataSvcUtil
{
    public class DataServiceItemGroup
    {
        public string Name
        {
            get;
            set;
        }

        public IEnumerable<object> Children
        {
            get;
            set;
        }
    }
}
