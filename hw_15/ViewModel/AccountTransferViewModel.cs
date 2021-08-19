using System.Collections.Generic;
using System.Linq;
using System.Windows;
using BankLibrary.BankAccount;
using BankLibrary.Client;
using BankLibrary.Exceptions;
using hw_15.Command;
using hw_15.Utils;

namespace hw_15.ViewModel
{
    class AccountTransferViewModel : ApplicationViewModel
    {
        private EF eF;

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
            eF = new EF();

            this.oldAccount = oldAccount;
            this.amountTransfer = amount;

            List<BankAccount> temp = new List<BankAccount>();
            temp.AddRange(eF.GetAccountsClients(client));
            temp.AddRange(eF.GetDepositAccountsClients(client));
            temp.AddRange(eF.GetCreditsClients(client));

            Accounts = temp;

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

                               if (oldAccount.TypeName.Equals("Кредит"))
                               {
                                   eF.UpdateCredit(oldAccount as Credit);
                               }
                               else if (oldAccount.TypeName.Contains("Вклад"))
                               {
                                   eF.UpdateDepositAccount(oldAccount as DepositAccount);
                               }
                               else
                               {
                                   eF.UpdateAccount(oldAccount as BankRegularAccount);
                               }

                               if (newAccount.TypeName.Equals("Кредит"))
                               {
                                   eF.UpdateCredit(newAccount as Credit);
                               }
                               else if (newAccount.TypeName.Contains("Вклад"))
                               {
                                   eF.UpdateDepositAccount(newAccount as DepositAccount);
                               }
                               else
                               {
                                   eF.UpdateAccount(newAccount as BankRegularAccount);
                               }
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
