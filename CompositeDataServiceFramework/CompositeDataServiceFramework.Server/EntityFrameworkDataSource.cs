using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Services;
using CompositeDataService;
using System.IO;

namespace CompositeDataServiceFramework.Server
{
    public class EntityFrameworkDataSource<T> : CompositeDataSource where T : ObjectContext
    {
        public EntityFrameworkDataSource()
        {
        }

        public override void Initialise()
        {

        }
    }
}
