using System.Windows;
using BankLibrary;
using hw_15.Command;

namespace hw_15.ViewModel
{
    class DepartmentAddViewModel : ApplicationViewModel
    {
        private BankDepartament departament;
        private string name;

        public DepartmentAddViewModel(BankDepartament bankDepartament)
        {
            this.departament = bankDepartament;
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

        private RelayCommand saveCommand;

        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ??
                       (saveCommand = new RelayCommand(obj =>
                           {
                               departament.Name = Name;

                               Window window = obj as Window;
                               window.DialogResult = true;
                               window.Close();
                           },
                           obj => !string.IsNullOrEmpty(Name)));
            }
        }

    }
}
