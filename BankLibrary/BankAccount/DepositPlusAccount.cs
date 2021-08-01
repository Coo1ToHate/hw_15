using System;

namespace BankLibrary.BankAccount
{
    /// <summary>
    /// Вклад с капитализацией
    /// </summary>
    public class DepositPlusAccount : DepositAccount
    {
        public DepositPlusAccount()
        {
        }

        public DepositPlusAccount(ulong id, decimal amount, double percent) : base(id, amount, percent)
        {
        }

        public override string ToString()
        {
            return $"{Id}: Вклад+ ({Percent:N2}%) - {Amount:N5}";
        }

        public override void SetFutureAmount(int monthCount)
        {
            decimal result = Amount;
            for (int i = 0; i < monthCount; i++)
            {
                result += result * ((decimal)Percent / 12 / 100);
            }
            FutureAmount = Math.Floor(result * (decimal)Math.Pow(10, 5)) / (decimal)Math.Pow(10, 5);
        }

        public override string TypeName => "Вклад+";


    }
}
