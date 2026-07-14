using System;
using Npgsql;

class Program
{
    static void Main()
    {
        string connStr = "Host=ep-lively-surf-ao0ayu4c-pooler.c-2.ap-southeast-1.aws.neon.tech;Database=neondb;Username=neondb_owner;Password='endpoint=ep-lively-surf-ao0ayu4c;npg_verN9s6jmbfi';SslMode=Require;Trust Server Certificate=true;";
        var builder = new NpgsqlConnectionStringBuilder(connStr);
        Console.WriteLine($"Host: {builder.Host}");
        Console.WriteLine($"Password: {builder.Password}");
    }
}
