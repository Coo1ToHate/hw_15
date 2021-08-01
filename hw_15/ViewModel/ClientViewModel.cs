using System.Collections.Generic;
using System.Windows;
using BankLibrary;
using BankLibrary.Client;
using hw_15.Command;

namespace hw_15.ViewModel
{
    class ClientViewModel : ApplicationViewModel
    {
        private Client client;
        private BankDepartament selectedDepartament;
        private BankDepartament oldBankDepartament;

        private IEnumerable<BankDepartament> departaments;

        public IEnumerable<BankDepartament> Departaments
        {
            get => departaments;
        }

        private string name;

        public ClientViewModel(Bank bank, Client client)
        {
            this.client = client;
            this.oldBankDepartament = client.Departament;
            SelectedDepartament = client.Departament;
            this.name = client.Name;

            departaments = new List<BankDepartament>
            {
                bank.PersonDepartament,
                bank.VipBankDepartament,
                bank.LegalDepartament
            };
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
                               if (oldBankDepartament != null && oldBankDepartament != SelectedDepartament)
                               {
                                   oldBankDepartament.Clients.Remove(client);
                                   SelectedDepartament.Clients.Add(client);
                               }

                               client.Name = Name;
                               client.Departament = SelectedDepartament;

                               Window window = obj as Window;
                               window.DialogResult = true;
                               window.Close();
                           },
                           obj => !string.IsNullOrEmpty(Name) && SelectedDepartament != null));
            }
        }
    }
}
