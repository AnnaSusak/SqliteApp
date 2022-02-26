using System;
using System.Collections.Generic;
using System.Text;

namespace SqliteApp2.Classes
{
    static class Table
    {
        private static string tableName = "Players";
        private static int numOfAvaibleColumns = 3;
        private static string[] columns = { "name", "money", "level" };
        private static Types[] typesOfColums = { Types.String, Types.Int, Types.Int };
        public static string TableName { get { return tableName; } set { tableName = value; } }
        public static int NumOfAvaibleColumns { get { return numOfAvaibleColumns; } 
            set { numOfAvaibleColumns = value; } }
        public static string[] Columns { get { return columns; } set { columns = value; } }
        public static Types[] TypesOfColumns { get { return typesOfColums; }
            set { typesOfColums = value; } }
    }
}
