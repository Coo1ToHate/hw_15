using System.Windows;
using BankLibrary.BankAccount;
using BankLibrary.Client;
using BankLibrary.Exceptions;
using hw_15.Command;
using hw_15.View;

namespace hw_15.ViewModel
{
    class AccountEditViewModel : ApplicationViewModel
    {
        private Client client;
        private BankAccount accountEdit;
        private string amountAdd;
        private string errorMessage;

        public AccountEditViewModel(Client client, BankAccount account)
        {
            this.client = client;
            AccountEdit = account;
        }

        public BankAccount AccountEdit
        {
            get => accountEdit;
            set
            {
                accountEdit = value;
                OnPropertyChanged();
            }
        }

        public string AmountAdd
        {
            get => amountAdd;
            set
            {
                amountAdd = value;
                OnPropertyChanged();
            }
        }

        public string TypeAccount
        {
            get => AccountEdit.TypeName;
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

        private RelayCommand plusCommand;
        private RelayCommand minusCommand;
        private RelayCommand transferCommand;

        public RelayCommand PlusCommand
        {
            get
            {
                return plusCommand ??
                       (plusCommand = new RelayCommand(obj =>
                       {
                           if (!int.TryParse(AmountAdd.Replace(" ", ""), out int amount) || amount <= 0)
                           {
                               ErrorMessage = "Введена неверная сумма! Сумма должна быть от 0!";
                               return;
                           }
                           AccountEdit.PutAmount(amount);

                           Window window = obj as Window;
                           window.DialogResult = true;
                           window.Close();
                       },
                           obj => !string.IsNullOrEmpty(AmountAdd)));
            }
        }

        public RelayCommand MinusCommand
        {
            get
            {
                return minusCommand ??
                       (minusCommand = new RelayCommand(obj =>
                           {
                               if (!int.TryParse(AmountAdd.Replace(" ", ""), out int amount) || amount <= 0)
                               {
                                   ErrorMessage = "Введена неверная сумма! Сумма должна быть от 0!";
                                   return;
                               }

                               try
                               {
                                   AccountEdit.WithdrawAmount(amount);
                                   Window window = obj as Window;
                                   window.DialogResult = true;
                                   window.Close();
                               }
                               catch (AccountHasNoAmount msg)
                               {
                                   ErrorMessage = $"Нельзя снять больше {AccountEdit.Amount}!";
                                   MessageBox.Show($"{msg.Message}\nНельзя снять больше {AccountEdit.Amount}!");
                               }
                           },
                           obj => !string.IsNullOrEmpty(AmountAdd)));
            }
        }

        public RelayCommand TransferCommand
        {
            get
            {
                return transferCommand ??
                       (transferCommand = new RelayCommand(obj =>
                           {
                               if (!int.TryParse(AmountAdd.Replace(" ", ""), out int amount) || amount <= 0)
                               {
                                   ErrorMessage = "Введена неверная сумма! Сумма должна быть от 0!";
                                   return;
                               }

                               AccountTransferWindow accountTransferWindow = new AccountTransferWindow()
                               {
                                   DataContext = new AccountTransferViewModel(client, AccountEdit, amount)
                               };

                               accountTransferWindow.Owner = obj as Window;
                               accountTransferWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                               accountTransferWindow.ShowDialog();


                               Window window = obj as Window;
                               window.DialogResult = true;
                               window.Close();

                           },
                           obj => !string.IsNullOrEmpty(AmountAdd)));
            }
        }
    }
}
