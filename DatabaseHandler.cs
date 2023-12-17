using Microsoft.Data.Sqlite;
using ORM;
using System.Runtime.InteropServices;

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

        public bool RentCar(int car_id, long customer_id, int staff_id, string start_date, string end_date)
        {
            Console.WriteLine("Renting Car");
            return false;
        }

        // --- GET Cars ---

        public Tuple<bool, List<Car>> GetCarByID(int car_id)
        {
            List<Car> cars = new List<Car>();
            using (var connection = new SqliteConnection(database))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Cars WHERE car_id= " + car_id;
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    cars.Add(new Car(Convert.ToInt32(reader["car_id"].ToString()), reader["car_make"].ToString(), reader["car_model"].ToString(), reader["car_vin"].ToString(), reader["car_license_plate"].ToString(), float.Parse(reader["car_price_per_hour"].ToString())));
                }
            }
            if (cars.Count() == 0)
            {
                return Tuple.Create(false, cars);
            }
            return Tuple.Create(true, cars);
        }

        public Tuple<bool, List<Car>> GetCarByVIN(string car_vin)
        {
            List<Car> cars = new List<Car>();
            using (var connection = new SqliteConnection(database))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Cars WHERE car_vin= " + car_vin;
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    cars.Add(new Car(Convert.ToInt32(reader["car_id"].ToString()), reader["car_make"].ToString(), reader["car_model"].ToString(), reader["car_vin"].ToString(), reader["car_license_plate"].ToString(), float.Parse(reader["car_price_per_hour"].ToString())));
                }
            }
            if (cars.Count() == 0)
            {
                return Tuple.Create(false, cars);
            }
            return Tuple.Create(true, cars);
        }

        public Tuple<bool, List<Car>> GetCarByLicensePlate(string car_license_plate)
        {
            List<Car> cars = new List<Car>();
            using (var connection = new SqliteConnection(database))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Cars WHERE car_license_plate= " + car_license_plate;
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    cars.Add(new Car(Convert.ToInt32(reader["car_id"].ToString()), reader["car_make"].ToString(), reader["car_model"].ToString(), reader["car_vin"].ToString(), reader["car_license_plate"].ToString(), float.Parse(reader["car_price_per_hour"].ToString())));
                }
            }
            if (cars.Count() == 0)
            {
                return Tuple.Create(false, cars);
            }
            return Tuple.Create(true, cars);
        }

        public bool GetRentalAvailability(int car_id, string start_date, string end_date)
        {
            List<int> results = new List<int>(1);
            using (var connection = new SqliteConnection(database))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"SELECT * FROM Rentals WHERE car_id={car_id} AND ({start_date} BETWEEN rental_start_date AND rental_end_date) OR ({end_date} BETWEEN rental_start_date AND rental_end_date) OR (rental_start_date BETWEEN {start_date} AND {end_date})";
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(Convert.ToInt32(reader["car_id"].ToString()));
                }
            }

            if (results.Count == 0)
            {
                return true;
            }

            return false;
        }



        // --- END GET Cars ---

        // --- GET Customers ---

        public Tuple<bool, List<Customer>> GetCustomerByID(int customer_id)
        {
            List<Customer> customers = new List<Customer>();
            using (var connection = new SqliteConnection(database))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Customers WHERE customer_id= " + customer_id;
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    customers.Add(new Customer(Convert.ToInt32(reader["customer_id"].ToString()), reader["customer_forename"].ToString(), reader["customer_surname"].ToString(), reader["customer_email"].ToString(), Convert.ToInt64(reader["customer_phone_number"].ToString())));
                }
            }
            if (customers.Count() == 0)
            {
                return Tuple.Create(false, customers);
            }
            return Tuple.Create(true, customers);
        }

        public Tuple<bool, List<Customer>> GetCustomerByEmail(string customer_email)
        {
            List<Customer> customers = new List<Customer>();
            using (var connection = new SqliteConnection(database))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"SELECT * FROM Customers WHERE customer_email= '{customer_email}';";
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    customers.Add(new Customer(Convert.ToInt32(reader["customer_id"].ToString()), reader["customer_forename"].ToString(), reader["customer_surname"].ToString(), reader["customer_email"].ToString(), Convert.ToInt64(reader["customer_phone_number"].ToString())));
                }
            }
            if (customers.Count() == 0)
            {
                return Tuple.Create(false, customers);
            }
            return Tuple.Create(true, customers);
        }

        public Tuple<bool, List<Customer>> GetCustomerByPhoneNumber(long customer_phone_number)
        {
            List<Customer> customers = new List<Customer>();
            using (var connection = new SqliteConnection(database))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Customers WHERE customer_phone_number= " + customer_phone_number;
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    customers.Add(new Customer(Convert.ToInt32(reader["customer_id"].ToString()), reader["customer_forename"].ToString(), reader["customer_surname"].ToString(), reader["customer_email"].ToString(), Convert.ToInt64(reader["customer_phone_number"].ToString())));
                }
            }
            if (customers.Count() == 0)
            {
                return Tuple.Create(false, customers);
            }
            return Tuple.Create(true, customers);
        }

        // --- END GET Customers ---

        public int CreateNewCustomer(Customer new_customer)
        {
            using (var connection = new SqliteConnection(database))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO Customers(customer_forename,customer_surname,customer_email,customer_phone_number) VALUES(@forename,@surname,@email,@phone_number)";
                command.Parameters.AddWithValue("@forename", new_customer.Customer_Forename);
                command.Parameters.AddWithValue("@surname", new_customer.Customer_Surname);
                command.Parameters.AddWithValue("@email", new_customer.Customer_Email);
                command.Parameters.AddWithValue("@phone_number", new_customer.Customer_Phone_Number);
                command.Prepare();
                command.ExecuteNonQuery();
            }
            int customer_id = 0;
            using (var connection = new SqliteConnection(database))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"SELECT * FROM Customers WHERE customer_email= '{new_customer.Customer_Email}';";
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    customer_id = Convert.ToInt32(reader["customer_id"].ToString());
                }
            }
            return customer_id;
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