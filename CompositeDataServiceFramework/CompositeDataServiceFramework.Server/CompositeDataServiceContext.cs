using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompositeDataServiceFramework.Server
{
    public class CompositeDataServiceContext
    {
      /// <summary>
      /// The composite data sources.
      /// </summary>
      private List<CompositeDataSource> compositeDataSources =
          new List<CompositeDataSource>();
    }
}
