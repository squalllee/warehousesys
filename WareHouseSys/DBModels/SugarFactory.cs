using SqlSugar;

namespace WareHouseSys.DBModels
{
    public class SugarFactory
    {
        private SugarFactory()
        {

        }

        public static SqlSugarClient GetInstance(string connectionString)
        {

            var db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = connectionString,
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true,
               
            });

            return db;
        }
    }
}