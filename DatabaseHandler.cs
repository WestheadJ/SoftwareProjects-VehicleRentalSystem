using Microsoft.Data.Sqlite;
using ORM;

namespace DatabaseHandler{
    class Database{
        public string database = "Data Source=vrs.db";
        public bool Login(){
            List<Staff> staff = new List<Staff>();
            using(var connection = new SqliteConnection(database)){
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Staff";
                SqliteDataReader reader = command.ExecuteReader();
                while(reader.Read()){
                    staff.Add(new Staff(Convert.ToInt32(reader["staff_id"].ToString()),reader["staff_forename"].ToString(),reader["staff_surname"].ToString()));
                }
            }
            foreach(var item in staff){
                Console.WriteLine(item.Staff_Forename);
            }
            return true;
        }

        public bool Register(){
            return false;
        }

    }
}