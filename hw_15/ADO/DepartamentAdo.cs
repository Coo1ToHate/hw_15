using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using BankLibrary;

namespace hw_15.ADO
{
    class DepartamentAdo
    {
        public static ObservableCollection<BankDepartament> GetAllDepartments()
        {
            string sql = "SELECT * FROM Departments";
            ObservableCollection<BankDepartament> departments = new ObservableCollection<BankDepartament>();

            try
            {
                using (SqlConnection connection = new SqlConnection(MainAdo.connectionStringBuilder.ConnectionString))
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
                using (SqlConnection connection = new SqlConnection(MainAdo.connectionStringBuilder.ConnectionString))
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
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return departament;
        }

    }
}
