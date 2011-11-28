using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apex.MVVM;

namespace CompDataSvcUtil
{
    public class EntityTypeViewModel : ViewModel
    {
        public EntityTypeViewModel(System.Data.Metadata.Edm.EntityType entityType)
        {
            Name = entityType.Name;
            FullName = entityType.FullName;
            NamespaceName = entityType.NamespaceName;
        }

        private NotifyingProperty NameProperty =
          new NotifyingProperty("Name", typeof(string), default(string));

        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        
        private NotifyingProperty FullNameProperty =
          new NotifyingProperty("FullName", typeof(string), default(string));

        public string FullName
        {
            get { return (string)GetValue(FullNameProperty); }
            set { SetValue(FullNameProperty, value); }
        }

        
        private NotifyingProperty NamespaceNameProperty =
          new NotifyingProperty("NamespaceName", typeof(string), default(string));

        public string NamespaceName
        {
            get { return (string)GetValue(NamespaceNameProperty); }
            set { SetValue(NamespaceNameProperty, value); }
        }
                
    }
}
