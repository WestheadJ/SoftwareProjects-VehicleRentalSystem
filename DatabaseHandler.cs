using Microsoft.Data.Sqlite;
using ORM;

namespace DatabaseHandler
{
    class Database
    {
        public string database = "Data Source=vrs.db";
        public bool Login(int staff_id, string staff_password)
        {
            List<Staff> staff = new List<Staff>();
            using (var connection = new SqliteConnection(database))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Staff WHERE staff_id=" + staff_id + " AND staff_password=" + staff_password;
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    staff.Add(new Staff(Convert.ToInt32(reader["staff_id"].ToString()), reader["staff_forename"].ToString(), reader["staff_surname"].ToString()));
                }
            }
            if (staff.Count() == 0)
            {
                return false;
            }

            SaveDetails(staff_id, staff_password);
            return true;
        }

        public bool Register(int staff_id, string staff_password, string new_staff_forename, string new_staff_surname, string new_staff_email, long new_staff_phone_number, string new_staff_password, int is_admin)
        {
            Console.WriteLine("Register Function");
            List<Staff> staff = new List<Staff>();
            using (var connection = new SqliteConnection(database))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Staff WHERE staff_id=" + staff_id + " AND staff_password=" + staff_password;
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    staff.Add(new Staff(Convert.ToInt32(reader["staff_id"].ToString()), reader["staff_forename"].ToString(), reader["staff_surname"].ToString()));
                }
            }
            if (staff.Count() == 0)
            {
                return false;
            }

            using (var connection = new SqliteConnection(database))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO Staff(staff_forename,staff_surname,staff_password,staff_email,staff_phone_number,is_admin) VALUES(@forename,@surname,@password,@email,@phone_number,@is_admin)";
                command.Parameters.AddWithValue("@forename", new_staff_forename);
                command.Parameters.AddWithValue("@surname", new_staff_surname);
                command.Parameters.AddWithValue("@email", new_staff_email);
                command.Parameters.AddWithValue("@password", new_staff_password);
                command.Parameters.AddWithValue("@phone_number", new_staff_phone_number);
                command.Parameters.AddWithValue("@is_admin", is_admin);

                command.Prepare();
                command.ExecuteNonQuery();
            }
            return true;
        }

        // Writes login details of current user
        void SaveDetails(int staff_id, string staff_password)
        {
            FileStream fs = File.Open("info.dat", FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(staff_id);
            bw.Write(staff_password);
            bw.Close();
            fs.Close();
        }
        
    }
}