using System.Collections.ObjectModel;
using System.Linq;
using BankLibrary;
using BankLibrary.BankAccount;
using BankLibrary.Client;

namespace hw_15.Utils
{
    class EF : IDB
    {
        #region departament

        public ObservableCollection<BankDepartament> GetAllDepartments()
        {
            using (var context = new BankContext())
            {
                return new ObservableCollection<BankDepartament>(context.Departments);
            }
        }

        public BankDepartament SelectDepartament(int id)
        {
            using (var context = new BankContext())
            {
                return context.Departments.FirstOrDefault(d => d.Id == id);
            }
        }

        #endregion

        #region client

        public ObservableCollection<Client> GetClientsInDepartment(BankDepartament department)
        {
            using (var context = new BankContext())
            {
                return new ObservableCollection<Client>(context.Clients.Where(c => c.DepartmentId == department.Id));
            }
        }

        public void InsertClient(Client client)
        {
            using (var context = new BankContext())
            {
                context.Clients.Add(client);
                context.SaveChanges();
            }
        }

        public void UpdateClient(Client client)
        {
            using (var context = new BankContext())
            {
                var clientTemp = context.Clients.FirstOrDefault(c => c.Id == client.Id);
                clientTemp.Name = client.Name;
                clientTemp.DepartmentId = client.DepartmentId;
                context.SaveChanges();
            }
        }

        public void DeleteClient(Client client)
        {
            using (var context = new BankContext())
            {
                context.Clients.Attach(client);
                context.Clients.Remove(client);
                context.SaveChanges();
            }
        }

        #endregion

        #region account

        public ObservableCollection<BankRegularAccount> GetAccountsClients(Client client)
        {
            using (var context = new BankContext())
            {
                return new ObservableCollection<BankRegularAccount>(context.BankRegularAccounts.Where(a => a.ClientId == client.Id));
            }
        }

        public ObservableCollection<DepositAccount> GetDepositAccountsClients(Client client)
        {
            using (var context = new BankContext())
            {
                return new ObservableCollection<DepositAccount>(context.DepositAccounts.Where(a => a.ClientId == client.Id));
            }
        }

        public ObservableCollection<Credit> GetCreditsClients(Client client)
        {
            using (var context = new BankContext())
            {
                return new ObservableCollection<Credit>(context.Credits.Where(a => a.ClientId == client.Id));
            }
        }

        public void InsertAccount(BankRegularAccount account)
        {
            using (var context = new BankContext())
            {
                context.BankRegularAccounts.Add(account);
                context.SaveChanges();
            }
        }

        public void InsertDepositAccount(DepositAccount account)
        {
            using (var context = new BankContext())
            {
                context.DepositAccounts.Add(account);
                context.SaveChanges();
            }
        }

        public void InsertCredit(Credit account)
        {
            using (var context = new BankContext())
            {
                context.Credits.Add(account);
                context.SaveChanges();
            }
        }

        public void DeleteAccount(BankRegularAccount account)
        {
            using (var context = new BankContext())
            {
                context.BankRegularAccounts.Attach(account);
                context.BankRegularAccounts.Remove(account);
                context.SaveChanges();
            }
        }

        public void DeleteDepositAccount(DepositAccount account)
        {
            using (var context = new BankContext())
            {
                context.DepositAccounts.Attach(account);
                context.DepositAccounts.Remove(account);
                context.SaveChanges();
            }
        }

        public void DeleteCredit(Credit account)
        {
            using (var context = new BankContext())
            {
                context.Credits.Attach(account);
                context.Credits.Remove(account);
                context.SaveChanges();
            }
        }

        public void UpdateAccount(BankRegularAccount account)
        {
            using (var context = new BankContext())
            {
                var accTemp = context.BankRegularAccounts.FirstOrDefault(a => a.Id == account.Id);
                accTemp.Amount = account.Amount;
                context.SaveChanges();
            }
        }

        public void UpdateDepositAccount(DepositAccount account)
        {
            using (var context = new BankContext())
            {
                var accTemp = context.DepositAccounts.FirstOrDefault(a => a.Id == account.Id);
                accTemp.Amount = account.Amount;
                context.SaveChanges();
            }
        }

        public void UpdateCredit(Credit account)
        {
            using (var context = new BankContext())
            {
                var accTemp = context.Credits.FirstOrDefault(a => a.Id == account.Id);
                accTemp.Amount = account.Amount;
                context.SaveChanges();
            }
        }

        #endregion
    }
}
