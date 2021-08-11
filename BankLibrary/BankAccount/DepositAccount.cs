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
        public decimal Percent { get; set; }

        public bool Capitalization { get; set; }

        public override string ToString()
        {
            if (Capitalization)
            {
                return $"{Id}: Вклад+ ({Percent:N2}%) - {Amount:N5}";
            }
            return $"{Id}: Вклад ({Percent:N2}%) - {Amount:N5}";
        }

        public override void SetFutureAmount(int monthCount)
        {
            decimal result = Amount;
            if (Capitalization)
            {
                for (int i = 0; i < monthCount; i++)
                {
                    result += result * (Percent / 12 / 100);
                }
            }
            else
            {
                decimal monthProfit = Amount * (Percent / 12 / 100);
                result += monthProfit * monthCount;
            }
            FutureAmount = Math.Floor(result * (decimal)Math.Pow(10, 5)) / (decimal)Math.Pow(10, 5);
        }

        public override string TypeName => Capitalization ? "Вклад+" : "Вклад";
    }
}
