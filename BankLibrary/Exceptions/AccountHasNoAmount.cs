using System;

namespace BankLibrary.Exceptions
{
    public class AccountHasNoAmount : Exception
    {
        public int ErrorCode { get; }

        public AccountHasNoAmount() : base("У клиента отсутствует запрошенная сумма!")
        {
            ErrorCode = 500;
        }
    }
}
