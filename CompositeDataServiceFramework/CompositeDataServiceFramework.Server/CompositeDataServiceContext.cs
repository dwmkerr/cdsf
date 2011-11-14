using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompositeDataServiceFramework.Server
{
    public class CompositeDataServiceContext
    {
      
        public void AddDataSource(CompositeDataSource source)
        {
            //  Add the composite data source.
            compositeDataSources.Add(source);
        }

      public void SaveChanges()
      {
        foreach (var compositeDataSource in compositeDataSources)
          compositeDataSource.SaveChanges();
      }

      public void ClearChanges()
      {
        foreach (var compositeDataSource in compositeDataSources)
          compositeDataSource.CancelChanges();
      }

      /// <summary>
      /// The composite data sources.
      /// </summary>
      private List<CompositeDataSource> compositeDataSources =
          new List<CompositeDataSource>();
    }
}
