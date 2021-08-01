using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BankLibrary
{
    /// <summary>
    /// Клас отдел банка
    /// </summary>
    public class BankDepartament : INotifyPropertyChanged
    {
        private string _name;

        public BankDepartament(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Название отдела
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

        /// <summary>
        /// Клиенты отдела
        /// </summary>
        public ObservableCollection<Client.Client> Clients { get; set; } = new ObservableCollection<Client.Client>();

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
