using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Services;

namespace CompositeDataServiceFramework.Server
{
    public class EntityFrameworkDataSource<T> where T : ObjectContext
    {
        public EntityFrameworkDataSource(DataService<T> dataService)
        {

        }
    }
}
