using System.Data;
using System.Data.SqlClient;
using hw_15.Utils;

namespace hw_15.ViewModel
{
    class AllDataViewModel : ApplicationViewModel
    {
        private SqlConnection con;
        private SqlDataAdapter da;
        private DataTable dt;
        private DataTable dt2;
        private DataRow dr;

        private DataRowView selectedItem;

        public DataRowView SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value;
                OnPropertyChanged();
            }
        }

        public DataTable Dt
        {
            get => dt;
            set
            {
                dt = value;
                OnPropertyChanged();
            }
        }

        public DataTable Dt2
        {
            get => dt2;
            set
            {
                dt2 = value;
                OnPropertyChanged();
            }
        }

        public AllDataViewModel()
        {
            con = new SqlConnection(ADO.connectionStringBuilder.ConnectionString);
            dt = new DataTable();
            da = new SqlDataAdapter();

            var sql = @"SELECT * FROM Clients Order by Clients.Id";
            da.SelectCommand = new SqlCommand(sql, con);

            sql = @"INSERT INTO Clients (Name, DepartmentId) VALUES (@Name, @DepartmentId);
                    SET @id = @@IDENTITY;";
            da.InsertCommand = new SqlCommand(sql, con);
            da.InsertCommand.Parameters.Add("@Id", SqlDbType.Int, 4, "Id").Direction = ParameterDirection.Output;
            da.InsertCommand.Parameters.Add("@Name", SqlDbType.NVarChar, 20, "Name");
            da.InsertCommand.Parameters.Add("@DepartmentId", SqlDbType.Int, 4, "DepartmentId");


            sql = @"UPDATE Clients SET Name = @Name, DepartmentId = @DepartmentId WHERE Id = @Id";
            da.UpdateCommand = new SqlCommand(sql, con);
            da.UpdateCommand.Parameters.Add("@Id", SqlDbType.Int, 0, "Id").SourceVersion = DataRowVersion.Original;
            da.UpdateCommand.Parameters.Add("@Name", SqlDbType.NVarChar, 20, "Name");
            da.UpdateCommand.Parameters.Add("@DepartmentId", SqlDbType.Int, 4, "DepartmentId");

            sql = @"DELETE FROM Users WHERE ID = @Id";
            da.DeleteCommand = new SqlCommand(sql, con);
            da.DeleteCommand.Parameters.Add("@Id", SqlDbType.Int, 4, "Id");

            da.Fill(dt);
        }
    }
}
