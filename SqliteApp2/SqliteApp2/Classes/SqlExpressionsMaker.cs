using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.Sqlite;
namespace SqliteApp2.Classes
{
    public enum Types { String, Int, Bool }
    class SqlExpressionsMaker
    {
        public static SqliteConnection Connection { get; set; }
        public static string MakeSqlExp(Variants chosenVar)
        {
            string sql_exp = "";
            switch (chosenVar)
            {
                case Variants.ViewAll:
                    SelectDataAndReWriteList();
                    break;
                case Variants.Insert:
                    sql_exp += $"INSERT INTO {Table.TableName} (";
                    sql_exp += MakeSrtringOfColumns();
                    sql_exp += ") VALUES (";
                    sql_exp += MakeStringOfValues();
                    sql_exp += ")";
                    break;
                case Variants.Delete:
                    sql_exp += $"DELETE FROM {Table.TableName} WHERE ";
                    sql_exp += ChooseManyValues(true);
                    break;
                case Variants.Update:
                    sql_exp += $"UPDATE {Table.TableName} SET ";
                    sql_exp+= ChooseManyValues(false);
                    sql_exp += " WHERE ";
                    sql_exp += ChooseManyValues(true);
                    break;
                case Variants.ChangeTableToExists:
                    Console.WriteLine("Enter table name:");
                    Table.TableName = Console.ReadLine();
                    Console.WriteLine("Enter number of avaible columns:");
                    Table.NumOfAvaibleColumns = EnterNumber();
                    AddColumns();
                    break;
                case Variants.CreateNewTable:

                    sql_exp += "CREATE TABLE \"";
                    Console.WriteLine("Enter table name:");
                    string name = Console.ReadLine();
                    sql_exp += $"{name}\" (\"id\" INTEGER NOT NULL UNIQUE,";
                    Console.WriteLine("Enter number of the columns:");
                    int num = EnterNumber();
                    for (int i = 0; i < num; i++)
                    {
                        Console.WriteLine("Enter column name:");
                        string col_name = Console.ReadLine();
                        sql_exp += $"\"{col_name}\" ";
                        Console.WriteLine("Enter column type:");
                        Types type = EnterColType();
                        switch (type)
                        {
                            case Types.String:
                                sql_exp += "TEXT";
                                break;
                            case Types.Int:
                                sql_exp += "INTEGER";
                                break;
                            case Types.Bool:
                                sql_exp += "INTEGER";
                                break;
                            default:
                                break;
                        }
                        sql_exp += ",";
                    }
                    sql_exp+= "PRIMARY KEY(\"id\" AUTOINCREMENT))";
                    break;
                default:
                    break;
            }
            return sql_exp;
        }
        private static void SelectDataAndReWriteList()
        {
            string sql_exp = $"SELECT * FROM {Table.TableName}";
            SqliteCommand command = new SqliteCommand(sql_exp, Connection);
            SqliteDataReader reader = command.ExecuteReader();
            TableItem.SetColumns();
            while (reader.Read())
            {
                List<object> values=new List<object>();
                object cur_val;
                reader.GetValue(0);
                for (int i = 1; i < Table.Columns.Length+1; i++)
                {
                    cur_val = reader.GetValue(i);
                    values.Add(cur_val);
                    Console.Write($"{cur_val}\t");
                }
                Console.WriteLine();
                TableItem p = new TableItem(values);
                TableItemContainer.tableItems.Add(p);
            }
        }
        private static string MakeSrtringOfColumns()
        {
            string res = "";
            for (int i = 0; i < Table.Columns.Length; i++)
            {
                res+= Table.Columns[i];
                if (i != Table.Columns.Length - 1)
                    res += ",";
            }
            return res;
        }
        private static string MakeStringOfValues()
        {
            string res = "";
            for (int i = 0; i < Table.Columns.Length; i++)
            {
                Console.WriteLine($"Enter {Table.Columns[i]}:");
                res+=EnterColVal(i);
                if (i != Table.Columns.Length - 1)
                    res += ",";
            }
            return res;
        }
        private static string EnterColVal(int indexOfCol)
        {
            switch (Table.TypesOfColumns[indexOfCol])
            {
                case Types.String:
                    return "\"" + Console.ReadLine() + "\"";
                case Types.Int:
                    return EnterNumber().ToString();
                case Types.Bool:
                    return EnterBool().ToString();
                default:
                    return "";
            }
        }
        private static int EnterNumber()
        {
            int num;
            while (true)
            {
                try
                {
                    num = int.Parse(Console.ReadLine());
                    return num;
                }
                catch
                {
                    Console.WriteLine("It's not a number. Try again");
                }
            }
            
        }
        private static int EnterBool()
        {
            string input;
            while (true)
            {
                Console.WriteLine("Enter \"true\" or \"false\"");
                input = Console.ReadLine();
                if (input == "true")
                    return 1;
                else if (input == "false")
                    return 0;
            }
        }
        private static int GetColIndex(string col)
        {
            for (int i = 0; i < Table.Columns.Length; i++)
            {
                if (Table.Columns[i] == col)
                    return i;
            }
            return -1;
        }
        private static void ShowColumns()
        {
            for (int i = 0; i < Table.Columns.Length; i++)
            {
                Console.WriteLine(Table.Columns[i]);
            }
        }
        private static string EnterColumn()
        {
            string col;
            while (true)
            {
                col = Console.ReadLine();
                for (int i = 0; i < Table.Columns.Length; i++)
                {
                    if (Table.Columns[i] == col)
                        return col;
                }
                Console.WriteLine("Incorrect input. Would you like to read columns again?" +
                    " Enter \"0\" if you would");
                if (Console.ReadLine() == "0")
                    ShowColumns();
                Console.WriteLine("Enter a column:");
            }
        }
        private static string ChooseManyValues(bool filters)
        {
            if(filters)
                Console.WriteLine("Enter the number of filters:");
            else
                Console.WriteLine("Enter number of parameters you want to change:");
            string res = "";
            int numOfValues=EnterNumOfParameters(Table.NumOfAvaibleColumns);
            for (int i = 0; i < numOfValues; i++)
            {
                ShowColumnInfo();
                string col = EnterColumn();
                res += col + " = ";
                Console.WriteLine("Enter value of column:");
                if (Table.TypesOfColumns[GetColIndex(col)] == Types.Int && !filters)
                    res += EnterAddSubOrValue(col);
                else
                    res += EnterColVal(GetColIndex(col));
                if (i<numOfValues-1)
                {
                    if (filters)
                    {
                        Console.WriteLine("Enter \"1\" for AND");
                        Console.WriteLine("Enter \"2\" for OR");
                        int num = EnterNumOfParameters(2);
                        if (num == 1)
                            res += " AND ";
                        else
                            res += " OR ";
                    }
                    else
                        res += ", ";
                }
            }
            return res;
        }
        private static string EnterAddSubOrValue(string col)
        {
            string res = "";
            Console.WriteLine("Enter \"1\" for add");
            Console.WriteLine("Enter \"2\" for sub");
            Console.WriteLine("Enter \"3\" for just num");
            int num = EnterNumOfParameters(3);
            if (num == 1)
                res += AddToExistVal(col);
            else if(num==2)
                res += SubFromExistVal(col);
            else
            {
                Console.WriteLine("Enter a number:");
                res += EnterColVal(GetColIndex(col));
            }
            return res;
        }
        private static string AddToExistVal(string col)
        {
            Console.WriteLine("Enter how much you want to add:");
            return col + " + " + EnterNumber();
        }
        private static string SubFromExistVal(string col)
        {
            Console.WriteLine("Enter how much you want to sub:");
            return col + " - " + EnterNumber();
        }
        private static void ShowColumnInfo()
        {
            Console.WriteLine("Choose one from columns. They are:");
            ShowColumns();
            Console.WriteLine("Enter your variant:");
        }
        private static int EnterNumOfParameters(int max)
        {
            while (true)
            {
                int num = EnterNumber();
                if (num < 1 || num > max)
                    Console.WriteLine($"Enter a value from 1 to {max}");
                else
                    return num;
            }
        }
        private static void AddColumns()
        {
            Table.Columns = new string[Table.NumOfAvaibleColumns];
            Table.TypesOfColumns = new Types[Table.NumOfAvaibleColumns];
            for (int i = 0; i < Table.NumOfAvaibleColumns; i++)
            {
                Console.WriteLine("Enter a column:");
                Table.Columns[i] = Console.ReadLine();
                Table.TypesOfColumns[i] = EnterColType();
            }
        }
        private static Types EnterColType()
        {
            Console.WriteLine("Enter \"1\" for string");
            Console.WriteLine("Enter \"2\" for int");
            Console.WriteLine("Enter \"3\" for bool");
            int num = EnterNumOfParameters(3);
            return (Types)(num - 1);
        }
    }
}
