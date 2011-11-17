using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Services.Providers;

namespace CompositeDataServiceFramework.Server
{
  public delegate void AddReferenceToCollectionDelegate(object resource, string propertyName, object resourceToBeAdded);
  public delegate void RemoveReferenceFromCollectionDelegate(object resource, string propertyName, object resourceToBeRemoved);

    public class CompositeResourceType
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

        public ResourceType ResourceType
        {
            get;
            set;
        }

        public void DoAddReferenceToCollection(object resource, string propertyName, object resourceToBeAdded)
        {
          var action = AddReferenceToCollectionAction;
          if (action != null)
            action(resource, propertyName, resourceToBeAdded);
        }

        public void DoRemoveReferenceFromCollection(object resource, string propertyName, object resourceToBeRemoved)
        {
          var action = RemoveReferenceFromCollectionAction;
          if (action != null)
            action(resource, propertyName, resourceToBeRemoved);
        }


        public event AddReferenceToCollectionDelegate AddReferenceToCollectionAction;

        public event RemoveReferenceFromCollectionDelegate RemoveReferenceFromCollectionAction;
    }
}
