using System.Windows;
using BankLibrary.BankAccount;
using hw_15.Command;

namespace hw_15.ViewModel
{
    class CreditAddViewModel : ApplicationViewModel
    {
        private Credit account;
        private string amount;
        private string percent;
        private string period;
        private string errorMessage;

        private Visibility _periodVisibility;
        private Visibility _percentVisibility;
        private Visibility _capitalizationVisibility;

        public CreditAddViewModel(Credit account)
        {
            this.account = account;
            PeriodVisibility = Visibility.Visible;
            PercentVisibility = Visibility.Visible;
            CapitalizationVisibility = Visibility.Hidden;
        }

        public string Amount
        {
            get => amount;
            set
            {
                amount = value;
                OnPropertyChanged();
            }
        }

        public string Percent
        {
            get => percent;
            set
            {
                percent = value;
                OnPropertyChanged();
            }
        }

        public string Period
        {
            get => period;
            set
            {
                period = value;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                OnPropertyChanged();
            }
        }

        public Visibility PeriodVisibility
        {
            get => _periodVisibility;
            set
            {
                _periodVisibility = value;
                OnPropertyChanged();
            }
        }

        public Visibility PercentVisibility
        {
            get => _percentVisibility;
            set
            {
                _percentVisibility = value;
                OnPropertyChanged();
            }
        }

        public Visibility CapitalizationVisibility
        {
            get => _capitalizationVisibility;
            set
            {
                _capitalizationVisibility = value;
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
                               decimal percent = default;
                               int period = default;

                               if (!decimal.TryParse(Amount.Replace(" ", ""), out decimal amount) || amount < 100)
                               {
                                   ErrorMessage = "Введена неверная сумма! Сумма должна быть от 100!";
                                   return;
                               }

                               if (PercentVisibility == Visibility.Visible)
                               {
                                   if (string.IsNullOrEmpty(Percent))
                                   {
                                       ErrorMessage = "Введите процент!";
                                       return;
                                   }
                                   if (!decimal.TryParse(Percent.Replace('.', ','), out percent))
                                   {
                                       ErrorMessage = "Введен неверный процент!";
                                       return;
                                   }
                               }

                               if (PeriodVisibility == Visibility.Visible)
                               {
                                   if (string.IsNullOrEmpty(Period))
                                   {
                                       ErrorMessage = "Введите срок!";
                                       return;
                                   }
                                   if (!int.TryParse(Period.Replace(" ", ""), out period) || period < 12 || period > 62)
                                   {
                                       ErrorMessage = "Введен неверный срок кредитования! От 12 до 60 месяцев!";
                                       return;
                                   }
                               }

                               account.Percent = percent;
                               account.Period = period;
                               account.SetAmount(amount);

                               Window window = obj as Window;
                               window.DialogResult = true;
                               window.Close();
                           },
                           obj => !string.IsNullOrEmpty(Amount)));
            }
        }
    }
}