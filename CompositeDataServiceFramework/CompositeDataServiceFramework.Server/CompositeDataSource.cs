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

        /// <summary>
        /// Gets the query root for a resource set.
        /// </summary>
        /// <param name="resourceSet">The resource set.</param>
        /// <returns></returns>
        public abstract IQueryable GetQueryRootForResourceSet(ResourceSet resourceSet);
    }
}
