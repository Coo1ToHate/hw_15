using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using BankLibrary.BankAccount;
using BankLibrary.Client;

namespace hw_15.ADO
{
    class AccountAdo
    {
        public static ObservableCollection<BankAccount> GetAccountsClients(Client client)
        {
            string sql = @"SELECT * FROM BankAccounts WHERE ClientId = @clientId";
            ObservableCollection<BankAccount> accounts = new ObservableCollection<BankAccount>();

            try
            {
                using (SqlConnection connection = new SqlConnection(MainAdo.connectionStringBuilder.ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.Add("@clientId", SqlDbType.Int).Value = client.Id;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                accounts.Add(new BankRegularAccount()
                                {
                                    Id = reader.GetInt32(0),
                                    ClientId = reader.GetInt32(1),
                                    Amount = reader.GetDecimal(2)
                                });
                            }
                        }
                    }

                    sql = @"SELECT * FROM DepositAccounts WHERE ClientId = @clientId";
                    command = new SqlCommand(sql, connection);
                    command.Parameters.Add("@clientId", SqlDbType.Int).Value = client.Id;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                accounts.Add(new DepositAccount()
                                {
                                    Id = reader.GetInt32(0),
                                    ClientId = reader.GetInt32(1),
                                    Amount = reader.GetDecimal(2),
                                    Percent = (double)reader.GetDecimal(3),
                                    Capitalization = reader.GetBoolean(4)
                                });
                            }
                        }
                    }

                    sql = @"SELECT * FROM Credits WHERE ClientId = @clientId";
                    command = new SqlCommand(sql, connection);
                    command.Parameters.Add("@clientId", SqlDbType.Int).Value = client.Id;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                accounts.Add(new Credit()
                                {
                                    Id = reader.GetInt32(0),
                                    ClientId = reader.GetInt32(1),
                                    Amount = reader.GetDecimal(2),
                                    Percent = (double)reader.GetDecimal(3)
                                });
                            }
                        }
                    }

                }

            }
            catch (Exception e)
            {
                throw;
            }

            return accounts;
        }
    }
}
