using Microsoft.Data.Sqlite;
using ORM;

namespace DatabaseHandler{
    class Database{
        public string database = "Data Source=vrs.db";
        public bool Login(int staff_id,string staff_password){
            List<Staff> staff = new List<Staff>();
            using(var connection = new SqliteConnection(database)){
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Staff WHERE staff_id="+staff_id+ " AND staff_password=" +staff_password;
                SqliteDataReader reader = command.ExecuteReader();
                while(reader.Read()){
                    staff.Add(new Staff(Convert.ToInt32(reader["staff_id"].ToString()),reader["staff_forename"].ToString(),reader["staff_surname"].ToString()));
                }
            }
            if(staff.Count() == 0){
                return false;
            }
            return true;
        }

        public void Register(){
            
        }

    }
}