using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace T_Rec
{
    public static class T_Rec_Controller
    {
        public const string DatabaseFilename = "T_Rec_DB.db3";

        public const SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath
        {
            get
            {
                var base_path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

                return Path.Combine(base_path, DatabaseFilename);
            }
        }
    }
}
