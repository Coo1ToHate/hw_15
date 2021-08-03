﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        private BankDepartament selectedDepartament;
        private ObservableCollection<Client> clientsInDepartment;
        private Client selectedClient;
        private ObservableCollection<BankAccount> clientBankAccounts;
        private BankAccount selectedBankAccount;
        private string statusBarMsg;

        private Bank Bank1;
        private int monthCount;
        private string path = $@"logs\log{DateTime.Now.Year}_{DateTime.Now.Month}.txt";

        public MainWindowViewModel()
        {
            Bank1 = new Bank("Банк");
            FillData.Fill(Bank1);
            SelectedDepartament = Bank1.PersonDepartament;
            SelectedClient = ClientsInDepartment.First();
            if (!Directory.Exists("logs")) Directory.CreateDirectory("logs");
            var createLog = Task.Factory.StartNew(CreateLog);
            BankAccount.Notify += AccountActionsOnNotify;
            BankAccount.Notify += AccountActionsLogging;
        }

        private async void AccountActionsLogging(object sender, AccountEventArgs e)
        {
            string str = $"{DateTime.Now} {e.Message}";
            using (StreamWriter sw = new StreamWriter(path, true))
            {
                await sw.WriteLineAsync(str);
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
                ClientsInDepartment = selectedDepartament.Clients;
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
                    ClientBankAccounts = selectedClient.Accounts;
                }
                OnPropertyChanged();
            }
        }

        public ObservableCollection<BankAccount> ClientBankAccounts
        {
            get => clientBankAccounts;
            set
            {
                clientBankAccounts = value;
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

        public BankAccount SelectedBankAccount
        {
            get => selectedBankAccount;
            set => selectedBankAccount = value;
        }

        private RelayCommand personDepartamentCommand;
        private RelayCommand vipDepartamentCommand;
        private RelayCommand legalDepartamentCommand;


        private RelayCommand addClientCommand;
        private RelayCommand editClientCommand;
        private RelayCommand delClientCommand;
        private RelayCommand addAccountCommand;
        private RelayCommand editAccountCommand;
        private RelayCommand delAccountCommand;
        private RelayCommand logCommand;

        private RelayCommand minusMonthCommand;
        private RelayCommand plusMonthCommand;
        private RelayCommand resetMonthCommand;



        public RelayCommand PersonDepartamentCommand
        {
            get
            {
                return personDepartamentCommand ??
                       (personDepartamentCommand = new RelayCommand(obj =>
                       {
                           SelectedDepartament = Bank1.PersonDepartament;
                           SelectedClient = ClientsInDepartment.First();
                       }));
            }
        }

        public RelayCommand VipDepartamentCommand
        {
            get
            {
                return vipDepartamentCommand ??
                       (vipDepartamentCommand = new RelayCommand(obj =>
                       {
                           SelectedDepartament = Bank1.VipBankDepartament;
                           SelectedClient = ClientsInDepartment.First();
                       }));
            }
        }

        public RelayCommand LegalDepartamentCommand
        {
            get
            {
                return legalDepartamentCommand ??
                       (legalDepartamentCommand = new RelayCommand(obj =>
                       {
                           SelectedDepartament = Bank1.LegalDepartament;
                           SelectedClient = ClientsInDepartment.First();
                       }));
            }
        }


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
                               DataContext = new ClientViewModel(Bank1, newClient),
                               Owner = obj as Window,
                               WindowStartupLocation = WindowStartupLocation.CenterOwner
                           };

                           clientWindow.ShowDialog();

                           if (clientWindow.DialogResult.Value)
                           {
                               SelectedDepartament = newClient.Departament;
                               SelectedDepartament.Clients.Add(newClient);
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
                                   DataContext = new ClientViewModel(Bank1, updatedClient)
                               };

                               clientWindow.Owner = obj as Window;
                               clientWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                               clientWindow.ShowDialog();

                               if (clientWindow.DialogResult.Value)
                               {
                                   SelectedDepartament = updatedClient.Departament;
                                   SelectedClient = updatedClient;
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
                               SelectedDepartament.Clients.Remove(SelectedClient);
                               SelectedClient = ClientsInDepartment.First();
                           },
                           obj => SelectedClient != null));
            }
        }

        public RelayCommand AddAccountCommand
        {
            get
            {
                return addAccountCommand ??
                       (addAccountCommand = new RelayCommand(obj =>
                           {
                               AccountAddWindow accountAddWindow = new AccountAddWindow()
                               {
                                   DataContext = new AccountAddViewModel(SelectedClient)
                               };

                               accountAddWindow.Owner = obj as Window;
                               accountAddWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                               accountAddWindow.ShowDialog();

                               if (accountAddWindow.DialogResult.Value)
                               {

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
                               }
                           },
                           obj => SelectedBankAccount != null));
            }
        }

        public RelayCommand DelAccountCommand
        {
            get
            {
                return delAccountCommand ??
                       (delAccountCommand = new RelayCommand(obj =>
                           {
                               SelectedClient.Accounts.Remove(SelectedBankAccount);
                           },
                           obj => SelectedBankAccount != null));
            }
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
                           ClientBankAccounts = selectedClient.Accounts;
                           foreach (var a in ClientBankAccounts)
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
                           ClientBankAccounts = selectedClient.Accounts;
                           foreach (var a in ClientBankAccounts)
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
                           LogWindow historyWindow = new LogWindow
                           {
                               DataContext = new LogViewModel(),
                               Owner = obj as Window,
                               WindowStartupLocation = WindowStartupLocation.CenterOwner
                           };

                           historyWindow.ShowDialog();
                       }));
            }
        }

        private void CreateLog()
        {
            int count = 11_000_000;

            string[] files = Directory.GetFiles("logs");
            foreach (var f in files)
            {
                File.Delete(f);
            }

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            int maxCountinFile = 2_000_000;
            int x = count / maxCountinFile;
            for (int i = 0; i < x; i++)
            {
                string p = $@"logs\log{DateTime.Now.AddYears(-(i + 1)).Year}.txt";

                using (StreamWriter sw = new StreamWriter(p, false))
                {
                    for (int j = 0; j < maxCountinFile; j++)
                    {
                        int indexI = i * maxCountinFile + j;
                        sw.WriteLine(DateTime.Now.AddHours(-indexI) + " " + indexI);
                    }
                }
            }

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds / 10:00}";
            StatusBarMsg = $"Файл лога содержащий {count:N0} записей создан за {elapsedTime}!";
        }
    }
}