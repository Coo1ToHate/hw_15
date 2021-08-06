using System.Data.SqlClient;

namespace hw_15.ADO
{
    class MainAdo
    {
        public static SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder
        {
            DataSource = @"(localdb)\MSSQLLocalDB",
            InitialCatalog = "MSSQLLocalBank",
            IntegratedSecurity = true
        };
    }
}
