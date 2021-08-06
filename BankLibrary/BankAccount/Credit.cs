using System;
using BankLibrary.Event;

namespace BankLibrary.BankAccount
{


    /// <summary>
    /// Кредитный счет
    /// </summary>
    public class Credit : BankAccount
    {
        /// <summary>
        /// Процентная ставка
        /// </summary>
        public double Percent { get; set; }

        /// <summary>
        /// Срок кредита (месяц)
        /// </summary>
        public int Period { get; set; }

        public Credit()
        {
        }

        public Credit(decimal amount, double percent, int period) : base(amount)
        {
            Percent = percent;
            Period = period;
            Amount += (amount * (decimal)Percent / 100) / 12 * Period;
            FutureAmount = Amount;
        }

        public override string ToString()
        {
            return $"{Id}: Кредит ({Percent:N2}% - {Period} мес.) - {Amount:N5}";
        }

        public override void SetFutureAmount(int monthCount)
        {
            decimal result = Amount;

            //result -= (Amount / Period)*monthCount;

            for (int i = 0; i < monthCount; i++)
            {
                result -= Amount / Period;
            }
            FutureAmount = result > 0 ? Math.Floor(result * (decimal)Math.Pow(10, 5)) / (decimal)Math.Pow(10, 5) : 0;
        }

        public override string TypeName => "Кредит";

        public override void PutAmount(int amount)
        {
            Amount -= amount;
            if (Amount < 0)
            {
                Amount = 0;
            }
            OnAccountChanged(new AccountEventArgs($"Кредитный счет {Id} был пополнен на {amount} руб. Баланс {Amount} руб.", amount));
            //Notify?.Invoke(this, new AccountEventArgs($"Кредитный счет {Id} был пополнен на {amount} руб. Баланс {Amount} руб.", amount));
        }
    }
}
