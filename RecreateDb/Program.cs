using System;
using Npgsql;

class Program
{
    static void Main()
    {
        string connStr = "postgresql://neondb_owner:npg_Z9xUTAMtv0wm@ep-lingering-cell-azunw5me-pooler.c-3.ap-southeast-1.aws.neon.tech/postgres?sslmode=require";
        try
        {
            using var conn = new NpgsqlConnection(connStr);
            conn.Open();
            using var cmd = new NpgsqlCommand("CREATE DATABASE neondb", conn);
            cmd.ExecuteNonQuery();
            Console.WriteLine("Database neondb created.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
