using Microsoft.Data.Sqlite;
using ORM;
using System.Runtime.InteropServices;

namespace DatabaseHandler
{

    class Database
    {
        public string database = "Data Source=vrs.db";

        public Tuple<bool, int, string> Login(int staff_id, string staff_password)
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
                connection.Close();

            }
            if (staff.Count() == 0)
            {
                Console.WriteLine("Credentials were incorrect!");
                return Tuple.Create(false, 0, "");
            }

            return Tuple.Create(true, staff_id, staff_password);
        }

        public bool Register(int staff_id, string staff_password, string new_staff_forename, string new_staff_surname, string new_staff_email, long new_staff_phone_number, string new_staff_password, int is_admin)
        {

            if (!IsAdmin(staff_id, staff_password))
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
                connection.Close();

            }
            return true;
        }

        public bool RentCar(int car_id, long customer_id, int staff_id, string start_date_string, string end_date_string)
        {
            float price = 0;
            DateTime start_date;
            DateTime end_date;
            // GET THE DATE TIME TO CHECK THE HOURS
            if (DateTime.TryParseExact(start_date_string, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out start_date)
            && DateTime.TryParseExact(end_date_string, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out end_date))
            {
                TimeSpan difference = end_date - start_date;
                int total_hours = (int)difference.TotalHours;
                price = (float)(total_hours * GetCarPrice(car_id));
                Console.WriteLine("Price: " + price);
            }
            else
            {
                return false;
            }

            Console.WriteLine("Still want to go through the rental request?");
            if (Console.ReadLine().ToLower() != "yes")
            {
                return false;
            }

            Console.WriteLine("Renting:");
            using (var connection = new SqliteConnection(database))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO Rentals(car_id,customer_id,staff_id,rental_start_date,rental_end_date,rental_cost) VALUES(@car_id,@customer_id,@staff_id,@start_date,@end_date,@cost)";
                command.Parameters.AddWithValue("@car_id", car_id);
                command.Parameters.AddWithValue("@customer_id", customer_id);
                command.Parameters.AddWithValue("@staff_id", staff_id);
                command.Parameters.AddWithValue("@start_date", start_date_string);
                command.Parameters.AddWithValue("@end_date", end_date_string);
                command.Parameters.AddWithValue("@cost", Math.Round(price, 2));
                command.Prepare();
                command.ExecuteNonQuery();
                connection.Close();
            }
            return true;
        }

        public void AddCar(int car_id, string car_model, string car_make, string car_vin, string car_license_plate, float car_price_per_hour)
        {
            using (var connection = new SqliteConnection(database))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO Cars(car_id,car_model,car_make,car_vin,car_license_plate,car_price_per_hour) VALUES(@car_id,@car_model,@car_make,@car_vin,@car_license_plate,@car_price_per_hour)";
                command.Parameters.AddWithValue("@car_id", car_id);
                command.Parameters.AddWithValue("@car_model", car_model);
                command.Parameters.AddWithValue("@car_make", car_make);
                command.Parameters.AddWithValue("@car_vin", car_vin);
                command.Parameters.AddWithValue("@car_license_plate", car_license_plate);
                command.Parameters.AddWithValue("@car_price_per_hour", Math.Round(car_price_per_hour, 2));
                command.Prepare();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void RemoveCar(int car_id)
        {
            using (var connection = new SqliteConnection(database))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"DELETE FROM Cars WHERE car_id={car_id};";
                command.Prepare();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void RemoveStaff(int staff_id)
        {
            using (var connection = new SqliteConnection(database))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"DELETE FROM Staff WHERE car_id={staff_id};";
                command.Prepare();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        // --- GET Cars ---

        /// <summary>
        /// Get a cars details by it's ID
        /// <param name="car_id"></param>
        /// <para>Returns: <c>Tuple</c></para>
        /// <list type="bullet">
        /// <item>
        /// <term>bool</term>
        /// <description>Returns true if the car was found, returns false if the car wasn't found</description>
        /// </item>
        /// <item>
        /// <term>List</term>
        /// <description>A list of Car objects</description>
        /// </item>
        /// </list>
        /// </summary>
        public Tuple<bool, Car> GetCarByID(int car_id)
        {
            Car car = new Car();
            using (var connection = new SqliteConnection(database))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Cars WHERE car_id= " + car_id;
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    car = new Car(Convert.ToInt32(reader["car_id"].ToString()), reader["car_make"].ToString(), reader["car_model"].ToString(), reader["car_vin"].ToString(), reader["car_license_plate"].ToString(), float.Parse(reader["car_price_per_hour"].ToString()));
                }
                connection.Close();

            }
            if (car.Car_Model == "")
            {
                return Tuple.Create(false, car);
            }
            return Tuple.Create(true, car);
        }

        /// <summary>
        /// Get a cars details by it's VIN number
        /// <param name="car_vin"></param>
        /// <para>Returns: Tuple</para>
        /// <list type="bullet">
        /// <item>
        /// <term>bool</term>
        /// <description>Returns true if the car was found, returns false if the car wasn't found</description>
        /// </item>
        /// <item>
        /// <term>List</term>
        /// <description>A list of Car objects</description>
        /// </item>
        /// </list>
        /// </summary>
        public Tuple<bool, Car> GetCarByVIN(string car_vin)
        {
            Car car = new Car();
            using (var connection = new SqliteConnection(database))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"SELECT * FROM Cars WHERE car_vin= '{car_vin}'";
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    car = new Car(Convert.ToInt32(reader["car_id"].ToString()), reader["car_make"].ToString(), reader["car_model"].ToString(), reader["car_vin"].ToString(), reader["car_license_plate"].ToString(), float.Parse(reader["car_price_per_hour"].ToString()));
                }
                connection.Close();

            }
            if (car.Car_Model == "")
            {
                return Tuple.Create(false, car);
            }
            return Tuple.Create(true, car);
        }

        /// <summary>
        /// Get a cars details by it's license plate
        /// <param name="car_license_plate"></param>
        /// <para>Returns: <c>Tuple</c></para>
        /// <list type="bullet">
        /// <item>
        /// <term>bool</term>
        /// <description>Returns true if the car was found, returns false if the car wasn't found</description>
        /// </item>
        /// <item>
        /// <term>List</term>
        /// <description>A list of Car objects</description>
        /// </item>
        /// </list>
        /// </summary>
        public Tuple<bool, Car> GetCarByLicensePlate(string car_license_plate)
        {
            Car car = new Car();
            using (var connection = new SqliteConnection(database))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"SELECT * FROM Cars WHERE Cars.car_license_plate = '{car_license_plate}'";
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    car = new Car(Convert.ToInt32(reader["car_id"].ToString()), reader["car_make"].ToString(), reader["car_model"].ToString(), reader["car_vin"].ToString(), reader["car_license_plate"].ToString(), float.Parse(reader["car_price_per_hour"].ToString()));
                }
                connection.Close();

            }
            if (car.Car_Model == "")
            {
                return Tuple.Create(false, car);
            }
            return Tuple.Create(true, car);
        }

        /// <summary>
        /// Get a cars availability for rental
        /// <param name="car_id"></param>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <para>Returns: <c>bool</c></para>
        /// <list type="bullet">
        /// <item>
        /// <term>bool</term>
        /// <description>Returns true if the car is available, returns false if the car isn't available</description>
        /// </item>
        /// </list>
        /// </summary>
        public bool GetRentalAvailability(int car_id, string start_date, string end_date)
        {
            List<int> results = new List<int>(1);

            using (var connection = new SqliteConnection(database))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"SELECT * FROM Rentals WHERE car_id = {car_id} AND (('{start_date}' BETWEEN rental_start_date AND rental_end_date) OR ('{end_date}' BETWEEN rental_start_date AND rental_end_date) OR (rental_start_date BETWEEN '{start_date}' AND '{end_date}'));";
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(Convert.ToInt32(reader["car_id"].ToString()));
                }
                connection.Close();
            }

            if (results.Count == 0)
            {
                Console.WriteLine("This car is available");
                return true;
            }
            Console.WriteLine("This car is unavailable");
            return false;
        }

        /// <summary>
        /// Get a cars price per hour
        /// <param name="car_id"></param>
        /// <para>Returns: <c>float</c></para>
        /// <list type="bullet">
        /// <item>
        /// <term>float</term>
        /// <description>The price of the car per hour</description>
        /// </item>
        /// </list>
        /// </summary>
        public float GetCarPrice(int car_id)
        {
            float price = 0;
            using (var connection = new SqliteConnection(database))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT car_price_per_hour FROM Cars WHERE car_id=" + car_id;
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    price = float.Parse(reader["car_price_per_hour"].ToString());
                }
                connection.Close();

            }
            return price;
        }

        /// <summary>
        ///  Gets all the cars being rented currently
        ///  <para>Returns: <c>List</c></para>
        /// <list type="bullet">
        /// <item>
        /// <term>List</term>
        /// <description> A list of RentedCar objects</description>
        /// </item>
        /// </list>
        /// </summary>
        public List<RentedCar> GetAllRentedCars()
        {
            List<RentedCar> rentedCars = new List<RentedCar>();
            using (var connection = new SqliteConnection(database))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT cars.car_id, cars.car_model, cars.car_make, rental_start_date, rental_end_date FROM cars JOIN rentals ON cars.car_id = rentals.car_id WHERE date('now') BETWEEN rentals.rental_start_date AND rentals.rental_end_date;";
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    rentedCars.Add(new RentedCar(Convert.ToInt32(reader["car_id"]), reader["car_model"].ToString(), reader["car_make"].ToString(), reader["rental_start_date"].ToString(), reader["rental_end_date"].ToString()));
                }
                connection.Close();

            }
            return rentedCars;
        }

        /// <summary>
        /// Get a list of cars that are available to rent
        /// <para>Returns: <c>List</c></para>
        /// <list type="bullet">
        /// <item>
        /// <term>List</term>
        /// <description>Returns a list of Car objects</description>
        /// </item>
        /// </list>
        /// </summary>
        public List<Car> GetAllAvailableCars()
        {
            List<Car> availableCars = new List<Car>();
            using (var connection = new SqliteConnection(database))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Cars.car_id, Cars.car_model, Cars.car_make, Cars.car_price_per_hour FROM cars LEFT JOIN rentals ON cars.car_id = rentals.car_id WHERE (rentals.rental_start_date > date('now') OR rentals.rental_start_date IS NULL) OR (rentals.rental_end_date < date('now') OR rentals.rental_end_date IS NULL);";
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    availableCars.Add(new Car(Convert.ToInt32(reader["car_id"]), reader["car_model"].ToString(), reader["car_make"].ToString(), float.Parse(reader["car_price_per_hour"].ToString())));
                }
                connection.Close();

            }
            return availableCars;
        }

        public List<Rental> GetRentedCarsByID(int car_id)
        {
            List<Rental> rentedCars = new List<Rental>(5);
            using (var connection = new SqliteConnection(database))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Rentals WHERE car_id=" + car_id + ";";
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    rentedCars.Add(new Rental(
                        Convert.ToInt32(reader["rental_id"].ToString()),
                        Convert.ToInt32(reader["customer_id"].ToString()),
                        Convert.ToInt32(reader["staff_id"].ToString()),
                        reader["rental_start_date"].ToString(),
                        reader["rental_end_date"].ToString(),
                        float.Parse(reader["rental_cost"].ToString())
                        ));
                }
            }
            return rentedCars;
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
                connection.Close();

            }
            if (customers.Count() == 0)
            {
                return Tuple.Create(false, customers);
            }
            return Tuple.Create(true, customers);
        }

        public Tuple<bool, Customer> GetCustomerByEmail(string customer_email)
        {
            Customer customerDetails = new Customer();
            using (var connection = new SqliteConnection(database))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"SELECT * FROM Customers WHERE customer_email= '{customer_email}';";
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    customerDetails = new Customer(Convert.ToInt32(reader["customer_id"].ToString()), reader["customer_forename"].ToString(), reader["customer_surname"].ToString(), reader["customer_email"].ToString(), Convert.ToInt64(reader["customer_phone_number"].ToString()));
                }
                connection.Close();

            }
            if (customerDetails.Customer_Email == "")
            {
                return Tuple.Create(false, customerDetails);
            }
            return Tuple.Create(true, customerDetails);
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
                connection.Close();

            }
            if (customers.Count() == 0)
            {
                return Tuple.Create(false, customers);
            }
            return Tuple.Create(true, customers);
        }

        public List<RentedCar> GetCustomersRentals(int customer_id)
        {
            List<RentedCar> cars = new List<RentedCar>(5);
            using (var connection = new SqliteConnection(database))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Cars JOIN Rentals ON Cars.car_id = Rentals.car_id JOIN Customers ON Rentals.customer_id = Customers.customer_id WHERE Customers.customer_id = 2;";

                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    cars.Add(new(Convert.ToInt32(reader["rental_id"]), Convert.ToInt32(reader["car_id"]), reader["car_model"].ToString(), reader["car_make"].ToString(), reader["car_vin"].ToString(), reader["car_license_plate"].ToString(), reader["rental_start_date"].ToString(), reader["rental_end_date"].ToString(), float.Parse(reader["rental_cost"].ToString())));
                }
                return cars;
            }
        }

        // --- END GET Customers ---

        // --- GET Staff ---

        public Tuple<bool, Staff> GetStaffDetails(string staff_email)
        {
            Staff staff_member = new Staff();

            using (var connection = new SqliteConnection(database))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"SELECT * FROM Staff WHERE staff_email= '{staff_email}';";
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    staff_member = new Staff(Convert.ToInt32(reader["staff_id"].ToString()), reader["staff_forename"].ToString(), reader["staff_surname"].ToString(), reader["staff_email"].ToString(), reader["staff_password"].ToString(), Convert.ToInt64(reader["staff_phone_number"]), Convert.ToInt32(reader["is_admin"].ToString()));
                }

                if (staff_member.Staff_Forename == "")
                {
                    return Tuple.Create(false, staff_member);
                }
                return Tuple.Create(true, staff_member);
            }
        }

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
                connection.Close();
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
                connection.Close();

            }
            return customer_id;
        }

        public bool IsAdmin(int staff_id, string staff_password)
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
                connection.Close();
            }

            if (staff.Count == 0)
            {
                return false;
            }
            return true;
        }

    }
}