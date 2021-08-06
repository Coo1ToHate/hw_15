using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using BankLibrary.Annotations;
using BankLibrary.Event;
using BankLibrary.Exceptions;

namespace BankLibrary.BankAccount
{
    /// <summary>
    /// Класс банковского счета
    /// </summary>
    public abstract class BankAccount : INotifyPropertyChanged
    {
        public delegate void AccountHandler(object sender, AccountEventArgs e);
        public static event AccountHandler Notify;

        protected BankAccount()
        {
        }

        protected BankAccount(decimal amount)
        {
            Amount = Math.Round(amount, 5);
            FutureAmount = Amount;
        }

        /// <summary>
        /// Id счета
        /// </summary>
        public int Id { get; set; }

        public int ClientId { get; set; }


        private decimal _amount;
        private decimal _futureAmount;

        /// <summary>
        /// Сумма на счету
        /// </summary>
        public decimal Amount
        {
            get => this._amount;
            set
            {
                this._amount = Math.Round(value, 5);
                OnPropertyChanged();
            }
        }

        public abstract override string ToString();

        /// <summary>
        /// Получить сумму на счету через monthCount месяцев
        /// </summary>
        /// <param name="monthCount">Кол-во месяцев</param>
        /// <returns></returns>
        public abstract void SetFutureAmount(int monthCount);

        /// <summary>
        /// Полная информаци о счете
        /// </summary>
        public string FullInfo => ToString();

        /// <summary>
        /// Планируемая сумма
        /// </summary>
        public decimal FutureAmount
        {
            get => _futureAmount;
            set
            {
                _futureAmount = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Название счета
        /// </summary>
        public abstract string TypeName { get; }

        protected void OnAccountChanged(AccountEventArgs e)
        {
            Notify?.Invoke(this, e);
        }

        /// <summary>
        /// Метод добавления денег на счет
        /// </summary>
        /// <param name="amount">Сумма</param>
        public virtual void PutAmount(int amount)
        {
            Amount += amount;
            OnAccountChanged(new AccountEventArgs($"На счет {Id} было положено {amount} руб. Баланс {Amount} руб.", amount));
        }

        /// <summary>
        /// Метод вывода средст со счета
        /// </summary>
        /// <param name="amount">Сумма</param>
        public void WithdrawAmount(int amount)
        {
            if (Amount >= amount)
            {
                Amount -= amount;
                OnAccountChanged(new AccountEventArgs($"Со счета {Id} было снято {amount} руб. Баланс {Amount} руб.", amount));
            }
            else
            {
                throw new AccountHasNoAmount();
            }
        }


        /// <summary>
        /// Метод перемещения с одного счета на другой
        /// </summary>
        /// <param name="newAccount">Новый счет</param>
        /// <param name="amount">Сумма</param>
        public void SendTo(BankAccount newAccount, int amount)
        {
            WithdrawAmount(amount);
            newAccount.PutAmount(amount);
            OnAccountChanged(new AccountEventArgs($"Со счета {Id} было переведено на счет {newAccount.Id} {amount} руб. " +
                                                  $"Баланс {Amount} руб. и {newAccount.Amount}", amount));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
