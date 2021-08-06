using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using BankLibrary;
using BankLibrary.Client;

namespace hw_15.ADO
{
    class ClientAdo
    {
        public static ObservableCollection<Client> GetClientsInDepartment(BankDepartament department)
        {
            string sql = @"SELECT * FROM Clients WHERE DepartmentId = @DepartmentId";
            ObservableCollection<Client> clients = new ObservableCollection<Client>();

            try
            {
                using (SqlConnection connection = new SqlConnection(MainAdo.connectionStringBuilder.ConnectionString))
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
    }
}
