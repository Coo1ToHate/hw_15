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

        public int Id { get; set; }

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
