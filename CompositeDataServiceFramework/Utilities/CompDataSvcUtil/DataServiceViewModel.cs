using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apex.MVVM;
using System.Collections.ObjectModel;

namespace CompDataSvcUtil
{
    public class DataServiceViewModel : ViewModel
    {
        /// <summary>
        /// The entity containers.
        /// </summary>
        private ObservableCollection<EntityContainerViewModel> entityContainers = new ObservableCollection<EntityContainerViewModel>();

        /// <summary>
        /// Gets the entity containers.
        /// </summary>
        public ObservableCollection<EntityContainerViewModel> EntityContainers
        {
            get { return entityContainers; }
        }

        
        private NotifyingProperty SelectedItemProperty =
          new NotifyingProperty("SelectedItem", typeof(object), default(object));

        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }
                
    }
}
