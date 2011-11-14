using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Services.Providers;

namespace CompositeDataServiceFramework.Server
{
    public class CompositeResourceAssociationSet
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

        public ResourceAssociationSet ResourceAssociationSet
        {
            get;
            set;
        }
    }
}
