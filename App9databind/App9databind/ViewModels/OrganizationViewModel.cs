using App9databind.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App9databind.ViewModels
{
    class OrganizationViewModel : NotificationBase
    {
        Organization organization;

        public OrganizationViewModel()
        {
            organization = new Organization();
            _SelectedIndex = -1;
            // Load the database
            foreach (var dog in organization.dogs)
            {
                var np = new DogViewModel(dog);
                np.PropertyChanged += clsDogs_OnNotifyPropertyChanged;
                _dog.Add(np);
            }
        }

        ObservableCollection<DogViewModel> _dog
           = new ObservableCollection<DogViewModel>();
        public ObservableCollection<DogViewModel> Dog
        {
            get { return _dog; }
            set { SetProperty(ref _dog, value); }
        }

        int _SelectedIndex;
        public int SelectedIndex
        {
            get { return _SelectedIndex; }
            set
            {
                if (SetProperty(ref _SelectedIndex, value))
                { RaisePropertyChanged(nameof(SelectedDog)); }
            }
        }

        public DogViewModel SelectedDog
        {
            get { return (_SelectedIndex >= 0) ? _dog[_SelectedIndex] : null; }
        }

        public void Add()
        {
            var dog = new DogViewModel();
            dog.PropertyChanged += clsDogs_OnNotifyPropertyChanged;
            Dog.Add(dog);
            organization.Add(dog);
            SelectedIndex = Dog.IndexOf(dog);
        }

        public void Delete()
        {
            if (SelectedIndex != -1)
            {
                var dog = Dog[SelectedIndex];
                Dog.RemoveAt(SelectedIndex);
                organization.Delete(dog);
            }
        }

        void clsDogs_OnNotifyPropertyChanged(Object sender, PropertyChangedEventArgs e)
        {
            organization.Update((DogViewModel)sender);
        }
    }
}
