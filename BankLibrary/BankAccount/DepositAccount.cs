using System;

namespace BankLibrary.BankAccount
{
    /// <summary>
    /// Вклад
    /// </summary>
    public class DepositAccount : BankAccount
    {
        /// <summary>
        /// Процентная ставка
        /// </summary>
        public double Percent { get; set; }

        public bool Capitalization { get; set; }

        public DepositAccount()
        {
        }

        public DepositAccount(decimal amount, double percent) : base(amount)
        {
            Percent = percent;
        }

        public override string ToString()
        {
            return $"{Id}: Вклад ({Percent:N2}%) - {Amount:N5}";
        }

        public override void SetFutureAmount(int monthCount)
        {
            decimal result = Amount;
            decimal monthProfit = Amount * ((decimal)Percent / 12 / 100);
            result += monthProfit * monthCount;
            FutureAmount = Math.Floor(result * (decimal)Math.Pow(10, 5)) / (decimal)Math.Pow(10, 5);
        }

        public override string TypeName => "Вклад";

    }
}
