using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apex.MVVM;
using System.Collections.ObjectModel;

namespace CompDataSvcUtil
{
    public class EntityContainerViewModel : ViewModel
    {
        private NotifyingProperty NameProperty =
          new NotifyingProperty("Name", typeof(string), default(string));

        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        private ObservableCollection<EntitySetViewModel> entitySets = new ObservableCollection<EntitySetViewModel>();

        private ObservableCollection<EntityTypeViewModel> entityTypes = new ObservableCollection<EntityTypeViewModel>();

        private ObservableCollection<AssociationTypeViewModel> associationTypes = new ObservableCollection<AssociationTypeViewModel>();

        private ObservableCollection<ServiceOperationViewModel> serviceOperations = new ObservableCollection<ServiceOperationViewModel>();

        public ObservableCollection<EntitySetViewModel> EntitySets
        {
            get { return entitySets; }
        }

        public ObservableCollection<EntityTypeViewModel> EntityTypes
        {
            get { return entityTypes; }
        }

        public ObservableCollection<AssociationTypeViewModel> AssociationTypes
        {
            get { return associationTypes; }
        }

        public ObservableCollection<ServiceOperationViewModel> ServiceOperations
        {
            get { return serviceOperations; }
        }

        public IEnumerable<ViewModel> Children
        {
            get
            {
                foreach (var e in entitySets)
                    yield return e;
                foreach (var e in entityTypes)
                    yield return e;
                foreach (var e in associationTypes)
                    yield return e;
                foreach (var e in serviceOperations)
                    yield return e;
            }
        }
    }
}
