using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using BankLibrary;
using BankLibrary.BankAccount;
using BankLibrary.Client;

namespace hw_15.Utils
{
    class ADO
    {
        public static SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder
        {
            DataSource = @"(localdb)\MSSQLLocalDB",
            InitialCatalog = "MSSQLLocalBank",
            IntegratedSecurity = true
        };

        #region departament
        public static ObservableCollection<BankDepartament> GetAllDepartments()
        {
            string sql = "SELECT * FROM Departments";
            ObservableCollection<BankDepartament> departments = new ObservableCollection<BankDepartament>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStringBuilder.ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sql, connection);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                departments.Add(new BankDepartament()
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1)
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

            return departments;
        }

        public static BankDepartament SelectDepartament(int id)
        {
            string sql = @"SELECT * FROM Departments WHERE Id = @Id";
            BankDepartament departament = new BankDepartament();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStringBuilder.ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                departament.Id = reader.GetInt32(0);
                                departament.Name = reader.GetString(1);
                            }
                        }
                        else
                        {
                            departament = null;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return departament;
        }

        #endregion

        #region client
        public static ObservableCollection<Client> GetClientsInDepartment(BankDepartament department)
        {
            string sql = @"SELECT * FROM Clients WHERE DepartmentId = @DepartmentId";
            ObservableCollection<Client> clients = new ObservableCollection<Client>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStringBuilder.ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sql, connection);

                    command.Parameters.Add("@DepartmentId", SqlDbType.Int).Value = department.Id;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                clients.Add(new Client()
                                {
                                    Id = reader.GetInt32(0),
                                    DepartmentId = reader.GetInt32(1),
                                    Name = reader.GetString(2)
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

            return clients;
        }

        public static void InsertClient(Client client)
        {
            string sql = @"INSERT INTO Clients (DepartmentId,  Name) VALUES (@DepartmentId, @Name); SET @Id = @@IDENTITY;";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStringBuilder.ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sql, connection);

                    SqlParameter id = new SqlParameter
                    {
                        ParameterName = "@Id",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Output
                    };

                    command.Parameters.Add(id);
                    command.Parameters.Add("@DepartmentId", SqlDbType.Int).Value = client.DepartmentId;
                    command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = client.Name;

                    command.ExecuteNonQuery();

                    client.Id = (int)id.Value;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static void UpdateClient(Client client)
        {
            string sql = @"UPDATE Clients SET DepartmentId = @DepartmentId, Name = @Name WHERE Id = @Id";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStringBuilder.ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sql, connection);

                    command.Parameters.Add("@Id", SqlDbType.Int).Value = client.Id;
                    command.Parameters.Add("@DepartmentId", SqlDbType.Int).Value = client.DepartmentId;
                    command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = client.Name;

                    command.ExecuteNonQuery();
                }

            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static void DeleteClient(Client client)
        {
            string sql = @"DELETE FROM Clients WHERE Id = @Id";
            string sqlRegular = @"DELETE FROM BankAccounts WHERE ClientId = @clientId";
            string sqlDeposit = @"DELETE FROM DepositAccounts WHERE ClientId = @clientId";
            string sqlCredit = @"DELETE FROM Credits WHERE ClientId = @clientId";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStringBuilder.ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sql, connection);

                    command.Parameters.Add("@Id", SqlDbType.Int).Value = client.Id;

                    command.ExecuteNonQuery();

                    SqlCommand command1 = new SqlCommand(sqlRegular, connection);
                    command1.Parameters.Add("@clientId", SqlDbType.Int).Value = client.Id;
                    command1.ExecuteNonQuery();

                    SqlCommand command2 = new SqlCommand(sqlDeposit, connection);
                    command2.Parameters.Add("@clientId", SqlDbType.Int).Value = client.Id;
                    command2.ExecuteNonQuery();

                    SqlCommand command3 = new SqlCommand(sqlCredit, connection);
                    command3.Parameters.Add("@clientId", SqlDbType.Int).Value = client.Id;
                    command3.ExecuteNonQuery();
                }


            }
            catch (Exception e)
            {
                throw;
            }
        }

        #endregion

        #region account
        public static ObservableCollection<BankAccount> GetAccountsClients(Client client)
        {
            string sql = @"SELECT * FROM BankAccounts WHERE ClientId = @clientId";
            ObservableCollection<BankAccount> accounts = new ObservableCollection<BankAccount>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStringBuilder.ConnectionString))
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
                                    Percent = reader.GetDecimal(3),
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
                                    Percent = reader.GetDecimal(3),
                                    Period = reader.GetInt32(4)
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

        public static void InsertRegularAccount(BankRegularAccount account)
        {
            string sql = @"INSERT INTO BankAccounts (ClientId,  Amount) VALUES (@ClientId, @Amount);
                            SET @Id = @@IDENTITY;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStringBuilder.ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sql, connection);

                    SqlParameter id = new SqlParameter
                    {
                        ParameterName = "@Id",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Output
                    };

                    command.Parameters.Add(id);
                    command.Parameters.Add("@ClientId", SqlDbType.Int).Value = account.ClientId;
                    command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = account.Amount;

                    command.ExecuteNonQuery();

                    account.Id = (int)id.Value;
                }

            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static void InsertDepositAccount(DepositAccount account)
        {
            string sql = @"INSERT INTO DepositAccounts (ClientId,  Amount, [Percent], Capitalization) 
                                                VALUES (@ClientId, @Amount, @Percent, @Capitalization); 
                                                SET @Id = @@IDENTITY;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStringBuilder.ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sql, connection);

                    SqlParameter id = new SqlParameter
                    {
                        ParameterName = "@Id",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Output
                    };

                    command.Parameters.Add(id);
                    command.Parameters.Add("@ClientId", SqlDbType.Int).Value = account.ClientId;
                    command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = account.Amount;
                    command.Parameters.Add("@Percent", SqlDbType.Decimal).Value = account.Percent;
                    command.Parameters.Add("@Capitalization", SqlDbType.Bit).Value = account.Capitalization;

                    command.ExecuteNonQuery();

                    account.Id = (int)id.Value;
                }

            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static void InsertCredit(Credit account)
        {
            string sql = @"INSERT INTO Credits (ClientId,  Amount, [Percent], Period) 
                                        VALUES (@ClientId, @Amount, @Percent, @Period); 
                                        SET @Id = @@IDENTITY;";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStringBuilder.ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sql, connection);
                    
                    SqlParameter id = new SqlParameter
                    {
                        ParameterName = "@Id",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Output
                    };

                    command.Parameters.Add(id);
                    command.Parameters.Add("@ClientId", SqlDbType.Int).Value = account.ClientId;
                    command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = account.Amount;
                    command.Parameters.Add("@Percent", SqlDbType.Decimal).Value = account.Percent;
                    command.Parameters.Add("@Period", SqlDbType.Int).Value = account.Period;

                    command.ExecuteNonQuery();

                    account.Id = (int)id.Value;
                }

            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static void DeleteAccount(BankAccount account)
        {
            string sql;

            if (account.TypeName.Contains("Вклад"))
            {
                sql = @"DELETE FROM DepositAccounts WHERE Id = @Id";
            }
            else if (account.TypeName.Equals("Кредит"))
            {
                sql = @"DELETE FROM Credits WHERE Id = @Id";
            }
            else
            {
                sql = @"DELETE FROM BankAccounts WHERE Id = @Id";
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStringBuilder.ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sql, connection);

                    command.Parameters.Add("@Id", SqlDbType.Int).Value = account.Id;

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static void UpdateAccount(BankAccount account)
        {
            string sql;

            if (account.TypeName.Contains("Вклад"))
            {
                sql = @"UPDATE DepositAccounts SET
                        Amount = @Amount
                        WHERE Id = @Id";
            }
            else if (account.TypeName.Equals("Кредит"))
            {
                sql = @"UPDATE Credits SET
                        Amount = @Amount
                        WHERE Id = @Id";
            }
            else
            {
                sql = @"UPDATE BankAccounts SET 
                        Amount = @Amount
                        WHERE Id = @Id";
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStringBuilder.ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sql, connection);

                    command.Parameters.Add("@Id", SqlDbType.Int).Value = account.Id;
                    command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = account.Amount;

                    command.ExecuteNonQuery();
                }

            }
            catch (Exception e)
            {
                throw;
            }
        }

        #endregion
    }
}
