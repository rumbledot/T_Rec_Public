using SQLite;
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

        public static List<object[]> RunSQL(string query, bool include_column_name)
        {
            var result = new List<object[]>();
            SQLitePCL.sqlite3_stmt statement = null;

            try
            {
                SQLitePCL.sqlite3 db;
                SQLite3.Result connection_result;

                SQLitePCL.Batteries.Init();
                connection_result = SQLite3.Open(T_Rec_Controller.DatabasePath, out db);
                //stQuery = SQLite3.Prepare2(fieldStrikeDatabase.Connection.Handle, sqlString);

                //exit if connection result not OK
                if (connection_result != SQLite3.Result.OK) throw new Exception($"RunSQL connection failed ({connection_result})");

                statement = SQLite3.Prepare2(db, query);
                var cols = SQLite3.ColumnCount(statement);

                if (include_column_name)
                {
                    var obj = new object[cols];
                    result.Add(obj);
                    for (int i = 0; i < cols; i++)
                    {
                        obj[i] = SQLite3.ColumnName(statement, i);
                    }
                }

                string col_type = "";

                while (SQLite3.Step(statement) == SQLite3.Result.Row)
                {
                    var obj = new object[cols];
                    result.Add(obj);
                    //Console.WriteLine(">>>> new row");

                    for (int i = 0; i < cols; i++)
                    {
                        col_type = SQLitePCL.raw.sqlite3_column_decltype(statement, i).utf8_to_string();
                        //obj[i] = SQLite3.ColumnString(statement, i);

                        //Console.Write(col_type + " : ");

                        if (col_type.Contains("varchar"))
                        {
                            obj[i] = SQLite3.ColumnString(statement, i);
                        }
                        else if (col_type.Contains("bigint"))
                        {
                            Int64 value = SQLite3.ColumnInt64(statement, i);
                            obj[i] = DateTime.FromBinary(value);
                        }
                        else if (col_type.Contains("int"))
                        {
                            obj[i] = SQLite3.ColumnInt(statement, i);
                        }
                        else if (col_type.Contains("float") || col_type.Contains("decimal") || col_type.Contains("double"))
                        {
                            obj[i] = SQLite3.ColumnDouble(statement, i);
                        }
                        else if (col_type.Contains("byte"))
                        {
                            obj[i] = SQLite3.ColumnByteArray(statement, i);
                        }
                        else if (col_type.Contains("blob"))
                        {
                            obj[i] = SQLite3.ColumnBlob(statement, i);
                        }
                        else if (col_type.Contains("null")) 
                        {
                            obj[i] = null;
                        }
                        else
                        {
                            obj[i] = SQLite3.ColumnString(statement, i);
                        }

                        //switch (col_type)
                        //{
                        //    case "varchar":
                        //        obj[i] = SQLite3.ColumnString(statement, i);
                        //        break;
                        //    case "integer":
                        //        obj[i] = SQLite3.ColumnInt(statement, i);
                        //        break;
                        //    case "bigint":
                        //        obj[i] = SQLite3.ColumnDouble(statement, i);
                        //        break;
                        //    case "blob":
                        //        obj[i] = SQLite3.ColumnBlob(statement, i);
                        //        break;
                        //    case "null":
                        //        obj[i] = null;
                        //        break;
                        //}
                    }

                    //Console.WriteLine();
                    //Console.WriteLine(new string('-', 20));
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($">>>>>>>>>>>>>>>>>>>> EXCEPTION!!!!! \n {ex.Message} \n\n {ex.StackTrace}");
                throw new Exception($"RunSQL exeption \n {ex.Message} \n {ex.StackTrace}");
            }
            finally
            {
                if (statement != null)
                {
                    SQLite3.Finalize(statement);
                }
            }
        }
    }
}
