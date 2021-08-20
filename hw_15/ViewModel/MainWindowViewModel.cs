using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using BankLibrary;
using BankLibrary.BankAccount;
using BankLibrary.Client;
using BankLibrary.Event;
using hw_15.Command;
using hw_15.Utils;
using hw_15.View;

namespace hw_15.ViewModel
{
    public class MainWindowViewModel : ApplicationViewModel
    {
        private EF eF;

        private BankDepartament selectedDepartament;
        private ObservableCollection<BankDepartament> departments;
        private ObservableCollection<Client> clientsInDepartment;
        private Client selectedClient;
        private ObservableCollection<BankRegularAccount> clientBankAccounts;
        private ObservableCollection<DepositAccount> clientBankDepositAccounts;
        private ObservableCollection<Credit> clientCredits;
        private BankRegularAccount selectedBankAccount;
        private DepositAccount selectedBankDepositAccount;
        private Credit selectedCredit;

        private string statusBarMsg;

        private int monthCount;
        private string path = "log.txt";

        public MainWindowViewModel()
        {
            eF = new EF();
            Departments = eF.GetAllDepartments();
            SelectedDepartament = Departments.FirstOrDefault();
            BankAccount.Notify += AccountActionsOnNotify;
            BankAccount.Notify += AccountActionsLogging;
        }

        private async void AccountActionsLogging(object sender, AccountEventArgs e)
        {
            using (StreamWriter sw = new StreamWriter(path, true))
            {
                await sw.WriteLineAsync(DateTime.Now + " " + e.Message);
            }
        }

        private void AccountActionsOnNotify(object sender, AccountEventArgs e)
        {
            Debug.Print(e.Message);
            StatusBarMsg = e.Message;
        }

        public string StatusBarMsg
        {
            get => statusBarMsg;
            set
            {
                statusBarMsg = value;
                OnPropertyChanged();
            }
        }

        public BankDepartament SelectedDepartament
        {
            get => selectedDepartament;
            set
            {
                selectedDepartament = value;
                if (SelectedDepartament != null)
                {
                    ClientsInDepartment = eF.GetClientsInDepartment(SelectedDepartament);
                    SelectedClient = ClientsInDepartment.FirstOrDefault();
                }
                OnPropertyChanged();
            }
        }

        public ObservableCollection<BankDepartament> Departments
        {
            get => departments;
            set
            {
                departments = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Client> ClientsInDepartment
        {
            get => clientsInDepartment;
            set
            {
                clientsInDepartment = value;
                OnPropertyChanged();
            }
        }

        public Client SelectedClient
        {
            get => selectedClient;
            set
            {
                selectedClient = value;
                if (selectedClient != null)
                {
                    ClientBankAccounts = eF.GetAccountsClients(selectedClient);
                    ClientBankDepositAccounts = eF.GetDepositAccountsClients(selectedClient);
                    ClientCredits = eF.GetCreditsClients(selectedClient);
                }
                OnPropertyChanged();
            }
        }

        public ObservableCollection<BankRegularAccount> ClientBankAccounts
        {
            get => clientBankAccounts;
            set
            {
                clientBankAccounts = value;
                foreach (var a in clientBankAccounts)
                {
                    a.SetFutureAmount(MonthCount);
                }
                OnPropertyChanged();
            }
        }

        public ObservableCollection<DepositAccount> ClientBankDepositAccounts
        {
            get => clientBankDepositAccounts;
            set
            {
                clientBankDepositAccounts = value;
                foreach (var a in clientBankDepositAccounts)
                {
                    a.SetFutureAmount(MonthCount);
                }
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Credit> ClientCredits
        {
            get => clientCredits;
            set
            {
                clientCredits = value;
                foreach (var a in clientCredits)
                {
                    a.SetFutureAmount(MonthCount);
                }
                OnPropertyChanged();
            }
        }

        public int MonthCount
        {
            get => monthCount;
            set
            {
                monthCount = value;
                OnPropertyChanged();
            }
        }

        public BankRegularAccount SelectedBankAccount
        {
            get => selectedBankAccount;
            set => selectedBankAccount = value;
        }

        public DepositAccount SelectedBankDepositAccount
        {
            get => selectedBankDepositAccount;
            set => selectedBankDepositAccount = value;
        }

        public Credit SelectedCredit
        {
            get => selectedCredit;
            set => selectedCredit = value;
        }

        private RelayCommand addClientCommand;
        private RelayCommand editClientCommand;
        private RelayCommand delClientCommand;
        private RelayCommand addRegularAccountCommand;
        private RelayCommand addDepositAccountCommand;
        private RelayCommand addCreditCommand;
        private RelayCommand editAccountCommand;
        private RelayCommand editDepositAccountCommand;
        private RelayCommand editCreditCommand;
        private RelayCommand delAccountCommand;
        private RelayCommand delDepositAccountCommand;
        private RelayCommand delCreditCommand;
        private RelayCommand logCommand;

        private RelayCommand minusMonthCommand;
        private RelayCommand plusMonthCommand;
        private RelayCommand resetMonthCommand;


        public RelayCommand AddClientCommand
        {
            get
            {
                return addClientCommand ??
                       (addClientCommand = new RelayCommand(obj =>
                       {
                           Client newClient = new Client();

                           ClientWindow clientWindow = new ClientWindow
                           {
                               DataContext = new ClientViewModel(newClient),
                               Owner = obj as Window,
                               WindowStartupLocation = WindowStartupLocation.CenterOwner
                           };

                           clientWindow.ShowDialog();

                           if (clientWindow.DialogResult.Value)
                           {
                               eF.InsertClient(newClient);
                               ClientsInDepartment = eF.GetClientsInDepartment(SelectedDepartament);
                           }
                       }));

            }
        }

        public RelayCommand EditClientCommand
        {
            get
            {
                return editClientCommand ??
                       (editClientCommand = new RelayCommand(obj =>
                           {
                               Client updatedClient = SelectedClient;

                               ClientWindow clientWindow = new ClientWindow()
                               {
                                   DataContext = new ClientViewModel(updatedClient)
                               };

                               clientWindow.Owner = obj as Window;
                               clientWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                               clientWindow.ShowDialog();

                               if (clientWindow.DialogResult.Value)
                               {
                                   eF.UpdateClient(updatedClient);
                                   ClientsInDepartment = eF.GetClientsInDepartment(SelectedDepartament);
                               }
                           },
                           obj => SelectedClient != null));

            }
        }

        public RelayCommand DelClientCommand
        {
            get
            {
                return delClientCommand ??
                       (delClientCommand = new RelayCommand(obj =>
                           {
                               eF.DeleteClient(SelectedClient);
                               ClientsInDepartment = eF.GetClientsInDepartment(SelectedDepartament);
                               SelectedClient = ClientsInDepartment.FirstOrDefault();
                           },
                           obj => SelectedClient != null));
            }
        }

        public RelayCommand AddRegularAccountCommand
        {
            get
            {
                return addRegularAccountCommand ??
                       (addRegularAccountCommand = new RelayCommand(obj =>
                           {
                               BankRegularAccount account = new BankRegularAccount();
                               account.ClientId = selectedClient.Id;

                               AccountAddWindow accountAddWindow = new AccountAddWindow()
                               {
                                   DataContext = new AccountAddViewModel(account)
                               };

                               accountAddWindow.Owner = obj as Window;
                               accountAddWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                               accountAddWindow.ShowDialog();

                               if (accountAddWindow.DialogResult.Value)
                               {
                                   eF.InsertAccount(account);
                                   ClientBankAccounts = eF.GetAccountsClients(selectedClient);
                                   ClientBankDepositAccounts = eF.GetDepositAccountsClients(selectedClient);
                                   ClientCredits = eF.GetCreditsClients(selectedClient);
                               }
                           },
                           obj => SelectedClient != null));
            }
        }

        public RelayCommand AddDepositAccountCommand
        {
            get
            {
                return addDepositAccountCommand ??
                       (addDepositAccountCommand = new RelayCommand(obj =>
                           {
                               DepositAccount account = new DepositAccount();
                               account.ClientId = selectedClient.Id;

                               AccountAddWindow accountAddWindow = new AccountAddWindow()
                               {
                                   DataContext = new DepositAddViewModel(account)
                               };

                               accountAddWindow.Owner = obj as Window;
                               accountAddWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                               accountAddWindow.ShowDialog();

                               if (accountAddWindow.DialogResult.Value)
                               {
                                   eF.InsertDepositAccount(account);
                                   ClientBankAccounts = eF.GetAccountsClients(selectedClient);
                                   ClientBankDepositAccounts = eF.GetDepositAccountsClients(selectedClient);
                                   ClientCredits = eF.GetCreditsClients(selectedClient);
                               }
                           },
                           obj => SelectedClient != null));

            }
        }

        public RelayCommand AddCreditCommand
        {
            get
            {
                return addCreditCommand ??
                       (addCreditCommand = new RelayCommand(obj =>
                           {
                               Credit account = new Credit();
                               account.ClientId = selectedClient.Id;

                               AccountAddWindow accountAddWindow = new AccountAddWindow()
                               {
                                   DataContext = new CreditAddViewModel(account)
                               };

                               accountAddWindow.Owner = obj as Window;
                               accountAddWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                               accountAddWindow.ShowDialog();

                               if (accountAddWindow.DialogResult.Value)
                               {
                                   eF.InsertCredit(account);
                                   ClientBankAccounts = eF.GetAccountsClients(selectedClient);
                                   ClientBankDepositAccounts = eF.GetDepositAccountsClients(selectedClient);
                                   ClientCredits = eF.GetCreditsClients(selectedClient);
                               }
                           },
                           obj => SelectedClient != null));

            }
        }

        public RelayCommand EditAccountCommand
        {
            get
            {
                return editAccountCommand ??
                       (editAccountCommand = new RelayCommand(obj =>
                           {
                               AccountEditWindow accountEditWindow = new AccountEditWindow()
                               {
                                   DataContext = new AccountEditViewModel(SelectedClient, SelectedBankAccount)
                               };

                               accountEditWindow.Owner = obj as Window;
                               accountEditWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                               accountEditWindow.ShowDialog();

                               if (accountEditWindow.DialogResult.Value)
                               {
                                   ClientBankAccounts = eF.GetAccountsClients(selectedClient);
                                   ClientBankDepositAccounts = eF.GetDepositAccountsClients(selectedClient);
                                   ClientCredits = eF.GetCreditsClients(selectedClient);
                               }
                           },
                           obj => SelectedBankAccount != null));
            }
        }

        public RelayCommand EditDepositAccountCommand
        {
            get
            {
                return editDepositAccountCommand ??
                       (editDepositAccountCommand = new RelayCommand(obj =>
                           {
                               AccountEditWindow accountEditWindow = new AccountEditWindow()
                               {
                                   DataContext = new AccountEditViewModel(SelectedClient, SelectedBankDepositAccount)
                               };

                               accountEditWindow.Owner = obj as Window;
                               accountEditWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                               accountEditWindow.ShowDialog();

                               if (accountEditWindow.DialogResult.Value)
                               {
                                   ClientBankAccounts = eF.GetAccountsClients(selectedClient);
                                   ClientBankDepositAccounts = eF.GetDepositAccountsClients(selectedClient);
                                   ClientCredits = eF.GetCreditsClients(selectedClient);
                               }
                           },
                           obj => SelectedBankDepositAccount != null));
            }
        }

        public RelayCommand EditCreditCommand
        {
            get { return editCreditCommand ??
                         (editCreditCommand = new RelayCommand(obj =>
                             {
                                 AccountEditWindow accountEditWindow = new AccountEditWindow()
                                 {
                                     DataContext = new AccountEditViewModel(SelectedClient, SelectedCredit)
                                 };

                                 accountEditWindow.Owner = obj as Window;
                                 accountEditWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                                 accountEditWindow.ShowDialog();

                                 if (accountEditWindow.DialogResult.Value)
                                 {
                                     ClientBankAccounts = eF.GetAccountsClients(selectedClient);
                                     ClientBankDepositAccounts = eF.GetDepositAccountsClients(selectedClient);
                                     ClientCredits = eF.GetCreditsClients(selectedClient);
                                 }
                             },
                             obj => SelectedCredit != null)); }
        }

        public RelayCommand DelAccountCommand
        {
            get
            {
                return delAccountCommand ??
                       (delAccountCommand = new RelayCommand(obj =>
                           {
                               eF.DeleteAccount(SelectedBankAccount);
                               ClientBankAccounts = eF.GetAccountsClients(selectedClient);
                               ClientBankDepositAccounts = eF.GetDepositAccountsClients(selectedClient);
                               ClientCredits = eF.GetCreditsClients(selectedClient);
                           },
                           obj => SelectedBankAccount != null));
            }
        }

        public RelayCommand DelDepositAccountCommand
        {
            get
            {
                return delDepositAccountCommand ??
                       (delDepositAccountCommand = new RelayCommand(obj =>
                           {
                               eF.DeleteDepositAccount(SelectedBankDepositAccount);
                               ClientBankAccounts = eF.GetAccountsClients(selectedClient);
                               ClientBankDepositAccounts = eF.GetDepositAccountsClients(selectedClient);
                               ClientCredits = eF.GetCreditsClients(selectedClient);
                           },
                           obj => SelectedBankDepositAccount != null));

            }
        }

        public RelayCommand DelCreditCommand
        {
            get => delCreditCommand ??
                   (delCreditCommand = new RelayCommand(obj =>
                       {
                           eF.DeleteCredit(SelectedCredit);
                           ClientBankAccounts = eF.GetAccountsClients(selectedClient);
                           ClientBankDepositAccounts = eF.GetDepositAccountsClients(selectedClient);
                           ClientCredits = eF.GetCreditsClients(selectedClient);
                       },
                       obj => SelectedCredit != null));
        }

        public RelayCommand MinusMonthCommand
        {
            get
            {
                return minusMonthCommand ??
                       (minusMonthCommand = new RelayCommand(obj =>
                       {
                           MonthCount--;
                           if (MonthCount < 0) MonthCount = 0;
                           foreach (var a in ClientBankAccounts)
                           {
                               a.SetFutureAmount(MonthCount);
                           }
                           foreach (var a in ClientBankDepositAccounts)
                           {
                               a.SetFutureAmount(MonthCount);
                           }
                           foreach (var a in ClientCredits)
                           {
                               a.SetFutureAmount(MonthCount);
                           }
                       }));
            }
        }

        public RelayCommand PlusMonthCommand
        {
            get
            {
                return plusMonthCommand ??
                       (plusMonthCommand = new RelayCommand(obj =>
                       {
                           MonthCount++;
                           foreach (var a in ClientBankAccounts)
                           {
                               a.SetFutureAmount(MonthCount);
                           }
                           foreach (var a in ClientBankDepositAccounts)
                           {
                               a.SetFutureAmount(MonthCount);
                           }
                           foreach (var a in ClientCredits)
                           {
                               a.SetFutureAmount(MonthCount);
                           }
                       }));
            }
        }

        public RelayCommand ResetMonthCommand
        {
            get
            {
                return resetMonthCommand ??
                       (resetMonthCommand = new RelayCommand(obj =>
                       {
                           MonthCount = 0;
                           foreach (var a in ClientBankAccounts)
                           {
                               a.SetFutureAmount(MonthCount);
                           }
                           foreach (var a in ClientBankDepositAccounts)
                           {
                               a.SetFutureAmount(MonthCount);
                           }
                           foreach (var a in ClientCredits)
                           {
                               a.SetFutureAmount(MonthCount);
                           }
                       }));
            }
        }

        public RelayCommand LogCommand
        {
            get
            {
                return logCommand ??
                       (logCommand = new RelayCommand(obj =>
                       {
                           LogWindow historyWindow = new LogWindow()
                           {
                               DataContext = new LogViewModel(path)
                           };

                           historyWindow.Owner = obj as Window;
                           historyWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                           historyWindow.ShowDialog();
                       }));
            }
        }
    }
}
