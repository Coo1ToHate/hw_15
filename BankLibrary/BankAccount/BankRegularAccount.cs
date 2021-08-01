namespace BankLibrary.BankAccount
{
    public class BankRegularAccount : BankAccount
    {
        public BankRegularAccount()
        {
        }

        public BankRegularAccount(ulong id, decimal amount) : base(id, amount)
        {
        }

        public override string ToString()
        {
            return $"{Id}: Обычный счет - {Amount:N5}";
        }

        public override void SetFutureAmount(int monthCount)
        {
            FutureAmount = Amount;
        }

        public override string TypeName => "Лицевой счет";
    }
}
