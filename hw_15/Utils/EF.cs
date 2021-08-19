using System.Collections.ObjectModel;
using System.Linq;
using BankLibrary;
using BankLibrary.BankAccount;
using BankLibrary.Client;

namespace hw_15.Utils
{
    class EF : IDB
    {
        private readonly BankContext _context;

        public EF()
        {
            _context = new BankContext();
        }

        #region departament

        public ObservableCollection<BankDepartament> GetAllDepartments() =>
            new ObservableCollection<BankDepartament>(_context.Departments);

        public BankDepartament SelectDepartament(int id) =>
            _context.Departments.FirstOrDefault(d => d.Id == id);

        #endregion

        #region client

        public ObservableCollection<Client> GetClientsInDepartment(BankDepartament department) =>
            new ObservableCollection<Client>(_context.Clients.Where(c => c.DepartmentId == department.Id));

        public void InsertClient(Client client)
        {
            _context.Clients.Add(client);
            _context.SaveChanges();
        }

        public void UpdateClient(Client client)
        {
            var clientTemp = _context.Clients.FirstOrDefault(c => c.Id == client.Id);
            clientTemp.Name = client.Name;
            clientTemp.DepartmentId = client.DepartmentId;
            _context.SaveChanges();
        }

        public void DeleteClient(Client client)
        {
            _context.Clients.Attach(client);
            _context.Clients.Remove(client);
            _context.SaveChanges();
        }

        #endregion

        #region account

        public ObservableCollection<BankRegularAccount> GetAccountsClients(Client client) =>
            new ObservableCollection<BankRegularAccount>(_context.BankRegularAccounts.Where(a => a.ClientId == client.Id));

        public ObservableCollection<DepositAccount> GetDepositAccountsClients(Client client) =>
            new ObservableCollection<DepositAccount>(_context.DepositAccounts.Where(a => a.ClientId == client.Id));

        public ObservableCollection<Credit> GetCreditsClients(Client client) =>
            new ObservableCollection<Credit>(_context.Credits.Where(a => a.ClientId == client.Id));

        public void InsertAccount(BankRegularAccount account)
        {
            _context.BankRegularAccounts.Add(account);
            _context.SaveChanges();
        }

        public void InsertDepositAccount(DepositAccount account)
        {
            _context.DepositAccounts.Add(account);
            _context.SaveChanges();
        }

        public void InsertCredit(Credit account)
        {
            _context.Credits.Add(account);
            _context.SaveChanges();
        }

        public void DeleteAccount(BankRegularAccount account)
        {
            _context.BankRegularAccounts.Attach(account);
            _context.BankRegularAccounts.Remove(account);
            _context.SaveChanges();
        }

        public void DeleteDepositAccount(DepositAccount account)
        {
            _context.DepositAccounts.Attach(account);
            _context.DepositAccounts.Remove(account);
            _context.SaveChanges();
        }

        public void DeleteCredit(Credit account)
        {
            _context.Credits.Attach(account);
            _context.Credits.Remove(account);
            _context.SaveChanges();
        }

        public void UpdateAccount(BankRegularAccount account)
        {
            var accTemp = _context.BankRegularAccounts.FirstOrDefault(a => a.Id == account.Id);
            accTemp.Amount = account.Amount;
            _context.SaveChanges();
        }

        public void UpdateDepositAccount(DepositAccount account)
        {
            var accTemp = _context.DepositAccounts.FirstOrDefault(a => a.Id == account.Id);
            accTemp.Amount = account.Amount;
            _context.SaveChanges();
        }

        public void UpdateCredit(Credit account)
        {
            var accTemp = _context.Credits.FirstOrDefault(a => a.Id == account.Id);
            accTemp.Amount = account.Amount;
            _context.SaveChanges();
        }

        #endregion
    }
}
