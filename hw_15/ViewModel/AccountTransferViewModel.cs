using System.Collections.Generic;
using System.Windows;
using BankLibrary.BankAccount;
using BankLibrary.Client;
using BankLibrary.Exceptions;
using hw_15.Command;

namespace hw_15.ViewModel
{
    class AccountTransferViewModel : ApplicationViewModel
    {
        private int amountTransfer;
        private BankAccount oldAccount;
        private IEnumerable<BankAccount> accounts;
        private BankAccount newAccount;

        private string message;

        public IEnumerable<BankAccount> Accounts
        {
            get => accounts;
            set => accounts = value;
        }

        public BankAccount NewAccount
        {
            get => newAccount;
            set
            {
                newAccount = value;
                OnPropertyChanged();
            }
        }

        public string Message
        {
            get => message;
            set => message = value;
        }

        public AccountTransferViewModel(Client client, BankAccount oldAccount, int amount)
        {
            this.oldAccount = oldAccount;
            this.amountTransfer = amount;
            Accounts = client.Accounts;
            Message = $"Перевести {amount:N}";
        }

        private RelayCommand transferGoCommand;

        public RelayCommand TransferGoCommand
        {
            get
            {
                return transferGoCommand ??
                       (transferGoCommand = new RelayCommand(obj =>
                       {
                           try
                           {
                               oldAccount.SendTo(NewAccount, amountTransfer);
                           }
                           catch (AccountHasNoAmount e)
                           {
                               MessageBox.Show($"{e.Message}\nНельзя перевести больше {oldAccount.Amount}!");
                           }
                           Window window = obj as Window;
                           window.DialogResult = true;
                           window.Close();
                       }));
            }
        }
    }
}
