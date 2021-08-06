using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using BankLibrary.Annotations;

namespace BankLibrary.Client
{
    public class Client : INotifyPropertyChanged
    {
        public Client()
        {
        }

        /// <summary>
        /// Класс Клиент
        /// </summary>
        public Client(string name, BankDepartament departament)
        {
            Name = name;
            Departament = departament;
        }

        private string _name;

        /// <summary>
        /// Фамилия
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public int Id { get; set; }
        public int DepartmentId { get; set; }

        /// <summary>
        /// Отдел
        /// </summary>
        public BankDepartament Departament { get; set; }

        /// <summary>
        /// Счета клиента
        /// </summary>
        public ObservableCollection<BankAccount.BankAccount> Accounts { get; set; } = new ObservableCollection<BankAccount.BankAccount>();

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
