using System.Collections.ObjectModel;
using BankLibrary;
using BankLibrary.BankAccount;
using BankLibrary.Client;

namespace hw_15.Utils
{
    interface IDB
    {
        ObservableCollection<BankDepartament> GetAllDepartments();
        BankDepartament SelectDepartament(int id);
        ObservableCollection<Client> GetClientsInDepartment(BankDepartament department);
        void InsertClient(Client client);
        void UpdateClient(Client client);
        void DeleteClient(Client client);
        ObservableCollection<BankRegularAccount> GetAccountsClients(Client client);
        ObservableCollection<DepositAccount> GetDepositAccountsClients(Client client);
        ObservableCollection<Credit> GetCreditsClients(Client client);
        void InsertAccount(BankRegularAccount account);
        void InsertDepositAccount(DepositAccount account);
        void InsertCredit(Credit account);
        void DeleteAccount(BankRegularAccount account);
        void DeleteDepositAccount(DepositAccount account);
        void DeleteCredit(Credit account);
        void UpdateAccount(BankRegularAccount account);
        void UpdateDepositAccount(DepositAccount account);
        void UpdateCredit(Credit account);
    }
}
