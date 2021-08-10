namespace BankLibrary.BankAccount
{
    public class BankRegularAccount : BankAccount
    {
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
