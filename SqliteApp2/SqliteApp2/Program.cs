using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using SqliteApp2.Classes;

namespace SqliteApp2
{
    class Program
    {
        static SqliteConnection connection = new SqliteConnection("Data Source=game.db");
        static void Main(string[] args)
        {
            connection.Open();
            SqlExpressionsMaker.Connection = connection;
            while (true)
            {
                Menu.ShowVariants();
                string sql_exp = SqlExpressionsMaker.MakeSqlExp(Menu.ReturnChosenVar(Menu.InputRequest()));
                if (sql_exp != "")
                {
                    try
                    {
                        SqliteCommand command = new SqliteCommand(sql_exp, connection);
                        try{
                            Console.WriteLine(command.ExecuteNonQuery().ToString());
                            Console.WriteLine("Succes");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }

                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
            }
        }
        
    }

    
}
