using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apex.MVVM;

namespace CompDataSvcUtil
{
    public class AssociationTypeViewModel : ViewModel
    {
        private NotifyingProperty NameProperty =
          new NotifyingProperty("Name", typeof(string), default(string));

        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }
                
    }
}
