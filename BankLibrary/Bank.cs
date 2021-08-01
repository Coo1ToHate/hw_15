namespace BankLibrary
{
    /// <summary>
    /// Класс банк
    /// </summary>
    public class Bank
    {
        public Bank(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Название банка
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Отделы банка
        /// </summary>
        //public ObservableCollection<BankDepartament> Departaments { get; set; } = new ObservableCollection<BankDepartament>();

        public BankDepartament PersonDepartament = new BankDepartament("Обычный отдел");
        public BankDepartament VipBankDepartament = new BankDepartament("VIP отдел");
        public BankDepartament LegalDepartament = new BankDepartament("Юридический отдел");
    }
}
