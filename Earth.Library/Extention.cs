using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace Earth.Library
{
    public static class Extention
    {
        public static int IndexOf(this StringBuilder sb, string value)
        {
            int ret = -1;
            for (int i = 0; i < sb.Length; i++)
            {
                if (sb[i] == value[0])
                {
                    int k = 1;
                    for (int j = 1; j < value.Length; j++)
                    {
                        if (sb[i + j] == value[j])
                            k++;
                        else
                            break;
                    }
                    if (value.Length == k)
                    {
                        ret = i;
                        break;
                    }
                    else
                        ret = -1;
                }
            }
            return ret;
        }

        public static string Substring(this StringBuilder sb, int startIndex, int length)
        {
            char[] cs = new char[length];
            sb.CopyTo(startIndex, cs, 0, length);
            return new string(cs);

        }

        public static DataTable ExecuteDataTable(this SqlCommand cmd)
        {
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dataTable = new DataTable();
            if (cmd.Connection == null)
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    cmd.Connection = conn;
                    adapter.Fill(dataTable);
                }
            }
            else
            {
                adapter.Fill(dataTable);
            }

            return dataTable;
        }
        public static int NonQuery(this SqlCommand cmd)
        {
            int ret;
            if (cmd.Connection == null)
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    cmd.Connection = conn;
                    ret = cmd.ExecuteNonQuery();
                }
            }
            else
            {
                ret = cmd.ExecuteNonQuery();
            }

            return ret;
        }
        public static object Scalar(this SqlCommand cmd)
        {
            object ret;
            if (cmd.Connection == null)
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    cmd.Connection = conn;
                    ret = cmd.ExecuteScalar();
                }
            }
            else
            {
                ret = cmd.ExecuteScalar();
            }

            return ret;
        }

    }
}
