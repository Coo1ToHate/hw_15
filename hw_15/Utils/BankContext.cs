using System.Data.Entity;
using BankLibrary;
using BankLibrary.BankAccount;
using BankLibrary.Client;

namespace hw_15.Utils
{
    class BankContext : DbContext
    {
        public BankContext() : base("DbConnection") { }

        public virtual DbSet<BankDepartament> Departments { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<BankRegularAccount> BankRegularAccounts { get; set; }
        public virtual DbSet<DepositAccount> DepositAccounts { get; set; }
        public virtual DbSet<Credit> Credits { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BankDepartament>().ToTable("Departments");
            modelBuilder.Entity<Client>().ToTable("Clients");
            modelBuilder.Entity<BankRegularAccount>().ToTable("BankAccounts");
            modelBuilder.Entity<DepositAccount>().ToTable("DepositAccounts");
            modelBuilder.Entity<Credit>().ToTable("Credits");
            modelBuilder.Ignore<BankAccount>();
            modelBuilder.Entity<BankAccount>().Ignore(p => p.FutureAmount);
            base.OnModelCreating(modelBuilder);
        }
    }
}
