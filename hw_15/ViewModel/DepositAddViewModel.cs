using System;
using System.Windows;
using BankLibrary.BankAccount;
using hw_15.Command;

namespace hw_15.ViewModel
{
    class DepositAddViewModel : ApplicationViewModel
    {
        private DepositAccount account;
        private string amount;
        private string percent;
        private string errorMessage;
        private bool capitalization;

        private Visibility _periodVisibility;
        private Visibility _percentVisibility;
        private Visibility _capitalizationVisibility;

        public DepositAddViewModel(DepositAccount account)
        {
            this.account = account;
            PeriodVisibility = Visibility.Hidden;
            PercentVisibility = Visibility.Visible;
            CapitalizationVisibility = Visibility.Visible;
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

        public bool Capitalization
        {
            get => capitalization;
            set {
                capitalization = value;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get => errorMessage;
            set {
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

        private RelayCommand selectCapitalizationRadioButton;
        public RelayCommand SelectCapitalizationRadioButton
        {
            get
            {
                return selectCapitalizationRadioButton ??
                       (selectCapitalizationRadioButton = new RelayCommand(obj =>
                       {
                           Capitalization = Boolean.Parse((string)obj);
                       }));
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

                           account.Amount = amount;
                           account.Percent = percent;
                           account.Capitalization = Capitalization;

                           Window window = obj as Window;
                           window.DialogResult = true;
                           window.Close();
                       },
                           obj => !string.IsNullOrEmpty(Amount)));
            }
        }

    }
}
