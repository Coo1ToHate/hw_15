using System.Collections.Generic;

namespace hw_15.Actions
{
    class AccountActions
    {
        /// <summary>
        /// Список типов счетов
        /// </summary>
        public IEnumerable<string> AccountTypeList = new List<string>()
        {
            "Лицевой счет",
            "Вклад",
            "Вклад+",
            "Кредит"
        };
    }
}
