using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Services.Providers;

namespace CompositeDataServiceFramework.Server
{
    public abstract class CompositeDataSource
    {
      /// <summary>
      /// Initialises the instance, putting all metadata into the supplied metadata provider.
      /// </summary>
      /// <param name="metadataProvider">The metadata provider.</param>
        public abstract void Initialise(CompositeDataServiceMetadataProvider metadataProvider);
    }
}
