using System;
using System.Collections.Generic;
using System.Text;

namespace SqliteApp2.Classes
{
    /// <summary>
    /// Нужен для получения данных из таблицы
    /// </summary>
    class TableItem
    {
        public static List<string> columns = new List<string>();
        public static List<object> values = new List<object>();
        public static void SetColumns()
        {
            columns.Clear();
            values.Clear();
            for (int i = 0; i < Table.Columns.Length; i++)
            {
                columns.Add(Table.Columns[i]);
            }
        }
        public TableItem(List<object> val)
        {
            values = val;
        }
    }
}
