using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using hw_15.Utils;

namespace hw_15.View
{
    /// <summary>
    /// Логика взаимодействия для AllDataWindow.xaml
    /// </summary>
    public partial class AllDataWindow : Window
    {
        private SqlConnection con;
        private SqlDataAdapter da;
        private DataTable dt;
        private DataTable dt2;
        private DataRow dr;
        private DataRowView row;
        private SqlDataAdapter daAll;
        private DataTable dtAll;



        public AllDataWindow()
        {
            InitializeComponent();
            Preparing();
        }

        private void Preparing()
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

            gridView.DataContext = dt.DefaultView;

            dtAll = new DataTable();
            daAll = new SqlDataAdapter();
            sql = @"SELECT Clients.Id as 'ID',
                            Clients.Name as 'Имя клиента',
                            Departments.Name as 'Отдел',
                            BankAccounts.Amount as 'На счету'
                    FROM Clients, Departments, BankAccounts
                    WHERE Departments.Id = Clients.DepartmentId and
                            BankAccounts.ClientId = Clients.Id
                    Order By Clients.Id";
            daAll.SelectCommand = new SqlCommand(sql, con);
            daAll.Fill(dtAll);

            gridViewAll.DataContext = dtAll.DefaultView;
        }

        private void GridView_OnCurrentCellChanged(object sender, EventArgs e)
        {
            if (row == null) return;
            row.EndEdit();
            da.Update(dt);
        }

        private void GridView_OnCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            row = (DataRowView) gridView.SelectedItem;
            row.BeginEdit();
        }
    }
}
