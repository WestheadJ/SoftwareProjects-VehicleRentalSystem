using Microsoft.Data.Sqlite;

namespace DatabaseHandler{
    class Database{
        public string database = "Data Source=vrs.db";
        public bool Login(){
            List<string> staff = new List<string>();
            using(var connection = new SqliteConnection(database)){
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Staff";
                SqliteDataReader reader = command.ExecuteReader();
                while(reader.Read()){
                    staff.Add(Convert.ToString(reader["staff_id"]));
                }
            }
            foreach(var item in staff){
                Console.WriteLine(item);
            }
            return true;
        }

        public bool Register(){
            return false;
        }

    }
}