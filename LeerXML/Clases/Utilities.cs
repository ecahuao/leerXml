using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace LeerXML
{
    public class Utilities
    {
        void Main(string[] args)
        {

        }
        public Boolean Conectar(String script)
        {
            //using (SqlConnection cn = new SqlConnection("Server=tcp:sfnetlab.database.windows.net,1433;Initial Catalog=dbSoftHard;Persist Security Info=False;User ID=testuser;Password=Factory2020;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;")) ;
            string sqlConnectionString = @"Server=tcp:sfnetlab.database.windows.net,1433;Initial Catalog=dbSoftHard;Persist Security Info=False;User ID=testuser;Password=Factory2020;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            //string script = File.ReadAllText(@"E:\Project Docs\MX462-PD\MX756_ModMappings1.sql");
            SqlConnection conn = new SqlConnection(sqlConnectionString);
            Server server = new Server(new ServerConnection(conn));
            server.ConnectionContext.ExecuteNonQuery(script);
            return true;
        }
    }
}
