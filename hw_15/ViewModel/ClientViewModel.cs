using System.Collections.Generic;
using System.Linq;
using System.Windows;
using BankLibrary;
using BankLibrary.Client;
using hw_15.Command;
using hw_15.Utils;

namespace hw_15.ViewModel
{
    class ClientViewModel : ApplicationViewModel
    {
        private Client client;
        private BankDepartament selectedDepartament;

        private IEnumerable<BankDepartament> departaments;
        private string name;

        public ClientViewModel(Client client)
        {
            this.client = client;
            Name = client.Name;
            Departaments = EF.GetAllDepartments();
            if(Name != null) SelectedDepartament = Departaments.First(d => d.Id == client.DepartmentId);

        }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }
        public IEnumerable<BankDepartament> Departaments
        {
            get => departaments;
            set
            {
                this.departaments = value;
                OnPropertyChanged();
            }
        }

        public BankDepartament SelectedDepartament
        {
            get => selectedDepartament;
            set
            {
                selectedDepartament = value;
                OnPropertyChanged();
            }
        }

        private RelayCommand saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ??
                       (saveCommand = new RelayCommand(obj =>
                           {
                               client.Name = Name;
                               client.DepartmentId = SelectedDepartament.Id;

                               Window window = obj as Window;
                               window.DialogResult = true;
                               window.Close();
                           },
                           obj => !string.IsNullOrEmpty(Name) && SelectedDepartament != null));
            }
        }
    }
}
