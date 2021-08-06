using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using BankLibrary.BankAccount;
using BankLibrary.Client;
using hw_15.Actions;
using hw_15.Command;

namespace hw_15.ViewModel
{
    class AccountAddViewModel : ApplicationViewModel
    {
        private Client client;
        private string amount;
        private string period;
        private string percent;
        private string selectedType;
        private string errorMessage;

        private Visibility _periodVisibility;
        private Visibility _percentVisibility;

        public AccountAddViewModel(Client client)
        {
            this.client = client;
            typeAccount = new AccountActions().AccountTypeList;
            SelectedType = TypeAccount.First();
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

        public string Period
        {
            get => period;
            set
            {
                period = value;
                OnPropertyChanged();
            }
        }

        public string Percent
        {
            get => percent;
            set {

                percent = value;
                OnPropertyChanged();
            }
        }

        public string SelectedType
        {
            get => selectedType;
            set
            {
                selectedType = value;
                OnPropertyChanged();
                if (value.Equals("Кредит", StringComparison.OrdinalIgnoreCase))
                {
                    PeriodVisibility = Visibility.Visible;
                    PercentVisibility = Visibility.Visible;
                }
                else if(value.Contains("Вклад"))
                {
                    PeriodVisibility = Visibility.Hidden;
                    PercentVisibility = Visibility.Visible;
                }
                else
                {
                    PeriodVisibility = Visibility.Hidden;
                    PercentVisibility = Visibility.Hidden;
                }
            }
        }

        private IEnumerable<string> typeAccount;

        public IEnumerable<string> TypeAccount
        {
            get => typeAccount;
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

        private RelayCommand saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ??
                       (saveCommand = new RelayCommand(obj =>
                           {
                               DateTime dtNow = DateTime.Now;
                               string dtNowString = $"{dtNow.Year:0000}{dtNow.Month:00}{dtNow.Day:00}{dtNow.Hour:00}{dtNow.Minute:00}{dtNow.Second:00}";
                               ulong id = ulong.Parse(dtNowString);

                               double percent = default;
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
                                   if (!double.TryParse(Percent.Replace('.', ','), out percent))
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
                               /*
                               switch (SelectedType)
                               {
                                   case "Вклад":
                                       client.Accounts.Add(new DepositAccount(id, amount, percent));
                                       break;
                                   case "Вклад+":
                                       client.Accounts.Add(new DepositPlusAccount(id, amount, percent));
                                       break;
                                   case "Кредит":
                                       client.Accounts.Add(new Credit(id, amount, percent, period));
                                       break;
                                   default:
                                       client.Accounts.Add(new BankRegularAccount(id, amount));
                                       break;
                               }*/

                               Window window = obj as Window;
                               window.DialogResult = true;
                               window.Close();
                           },
                           obj => !string.IsNullOrEmpty(Amount)));
            }
        }
    }
}
