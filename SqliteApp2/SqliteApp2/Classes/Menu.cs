using System;
using System.Collections.Generic;
using System.Text;

namespace SqliteApp2.Classes
{
    public enum Variants { ViewAll, Insert, Delete, Update, ChangeTableToExists , CreateNewTable};
    static class Menu
    {
        private const int NUMOFVARIANTS = 6;
        public static void ShowVariants()
        {
            Console.WriteLine("Enter 1 to view all data from the table and save changes.");
            Console.WriteLine("Enter 2 to insert data in the table.");
            Console.WriteLine("Enter 3 to delete data the table.");
            Console.WriteLine("Enter 4 to update the table.");
            Console.WriteLine("Enter 5 to change the table for another exists.");
            Console.WriteLine("Enter 6 to create a new table (id will be the first column).");
        }
        public static int InputRequest()
        {
            bool done = false;
            int command=0;
            while (!done)
            {
                try
                {
                     command= int.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Incorrect input. Would you like to read menu again? " +
                        "Enter 0, if you would.");
                    if (Console.ReadLine() == "0")
                        ShowVariants();
                }
                if (command >= 1 && command <= NUMOFVARIANTS)
                    done = true;
                else
                    Console.WriteLine($"Enter a number fro 1 to {NUMOFVARIANTS}");
            }
            return command;
        }
        public static Variants ReturnChosenVar(int num)
        {
            return ((Variants)(num-1));
        }
    }
}
