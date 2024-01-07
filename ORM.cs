namespace ORM
{
    class Car
    {
        // --- Getters ---
        protected int car_id = 0;
        protected string car_make = "";
        protected string car_model = "";
        protected string car_vin = "";
        protected string car_license_plate = "";
        protected float car_price = 0;

        // --- End of Getters ---

        // --- Setters ----
        public int Car_ID { get { return car_id; } set { car_id = value; } }
        public string Car_Make { get { return car_make; } set { car_make = value; } }
        public string Car_Model { get { return car_model; } set { car_model = value; } }
        public string Car_Vin { get { return car_vin; } set { car_vin = value; } }
        public string Car_License_Plate { get { return car_license_plate; } set { car_license_plate = value; } }
        public float Car_Price { get { return car_price; } set { car_price = value; } }

        // --- End of Setters

        // --- Constructors ---

        // Existing Car Constructor
        public Car(int car_id, string car_make, string car_model, string car_vin, string car_license_plate, float car_price)
        {
            this.car_id = car_id;
            this.car_make = car_make;
            this.car_model = car_model;
            this.car_vin = car_vin;
            this.car_license_plate = car_license_plate;
            this.car_price = car_price;
        }

        // New Car Constructor
        public Car(string car_make, string car_model, string car_vin, string car_license_plate, float car_price)
        {
            this.car_make = car_make;
            this.car_model = car_model;
            this.car_vin = car_vin;
            this.car_license_plate = car_license_plate;
            this.car_price = car_price;
        }

        // Available Cars Constructor
        public Car(int car_id, string car_model, string car_make, float car_price)
        {
            this.car_id = car_id;
            this.car_model = car_model;
            this.car_make = car_make;
            this.car_price = car_price;
        }

        public Car()
        {

        }
        // --- End of Constructors
    }


    class RentedCar : Car
    {
        private string rental_start_date;
        private string rental_end_date;
        private float rental_price;
        private int rental_id;

        public string Rental_Start_Date { get { return rental_start_date; } set { rental_start_date = value; } }
        public string Rental_End_Date { get { return rental_end_date; } set { rental_end_date = value; } }
        public float Rental_Price { get { return rental_price; } set { rental_price = value; } }

        public int Rental_ID { get { return rental_id; } set { rental_id = value; } }

        // --- Constructors --- 
        public RentedCar(int car_id, string car_model, string car_make, string rental_start_date, string rental_end_date)
        {
            this.car_id = car_id;
            this.car_model = car_model;
            this.car_make = car_make;
            this.rental_start_date = rental_start_date;
            this.rental_end_date = rental_end_date;
        }

        public RentedCar(int rental_id, int car_id, string car_model, string car_make, string car_vin, string car_license_plate, string rental_start_date, string rental_end_date, float rental_price)
        {
            this.rental_id = rental_id;
            this.car_id = car_id;
            this.car_model = car_model;
            this.car_make = car_make;
            this.car_vin = car_vin;
            this.car_license_plate = car_license_plate;
            this.rental_start_date = rental_start_date;
            this.rental_end_date = rental_end_date;
            this.rental_price = rental_price;
        }
    }

    class Staff
    {
        // --- Getters ---
        protected int staff_id = 0;
        private string staff_forename = "";
        private string staff_surname = "";
        private string staff_email = "";
        private string staff_password = "";
        private long staff_phone_number = 0;
        private int staff_is_admin = 0;

        // --- End of Getters --- 

        // --- Setters --- 
        public int Staff_ID { get { return staff_id; } set { staff_id = value; } }
        public string Staff_Forename { get { return staff_forename; } set { staff_forename = value; } }
        public string Staff_Surname { get { return staff_surname; } set { staff_surname = value; } }
        public string Staff_Email { get { return staff_email; } set { staff_email = value; } }
        public string Staff_Password { get { return staff_password; } set { staff_password = value; } }
        public long Staff_Phone_Number { get { return staff_phone_number; } set { staff_phone_number = value; } }
        public int Staff_Is_Admin { get { return staff_is_admin; } set { staff_is_admin = value; } }

        // --- End of Setters --- 

        // Constructors
        public Staff(int staff_id, string staff_forename, string staff_surname)
        {
            this.staff_id = staff_id;
            this.staff_forename = staff_forename;
            this.staff_surname = staff_surname;
        }

        // New Staff Member Constructor
        public Staff(string staff_forename, string staff_surname, string staff_email, string staff_password, long staff_phone_number, int staff_is_admin)
        {
            this.staff_forename = staff_forename;
            this.staff_surname = staff_surname;
            this.staff_email = staff_email;
            this.staff_password = staff_password;
            this.staff_phone_number = staff_phone_number;
            this.staff_is_admin = staff_is_admin;
        }

        // GET Staff Details
        public Staff(int staff_id, string staff_forename, string staff_surname, string staff_email, string staff_password, long staff_phone_number, int staff_is_admin)
        {
            this.staff_id = staff_id;
            this.staff_forename = staff_forename;
            this.staff_surname = staff_surname;
            this.staff_email = staff_email;
            this.staff_password = staff_password;
            this.staff_phone_number = staff_phone_number;
            this.staff_is_admin = staff_is_admin;
        }

        public Staff() { }

        // --- End of Constructors
    }

    class Customer
    {
        // Getters
        private int customer_id = 0;
        private string customer_forename = "";
        private string customer_surname = "";
        private string customer_email = "";
        private long customer_phone_number = 0;

        // End of Getters

        // Setters
        public int Customer_ID { get { return customer_id; } set { customer_id = value; } }
        public string Customer_Forename { get { return customer_forename; } set { customer_forename = value; } }
        public string Customer_Surname { get { return customer_surname; } set { customer_surname = value; } }
        public string Customer_Email { get { return customer_email; } set { customer_email = value; } }
        public long Customer_Phone_Number { get { return customer_phone_number; } set { customer_phone_number = value; } }

        // --- End of Setters ---

        // --- Constructors ---

        // Existing Customer Constructor
        public Customer(int customer_id, string customer_forename, string customer_surname, string customer_email, long customer_phone_number)
        {
            this.customer_id = customer_id;
            this.customer_forename = customer_forename;
            this.customer_surname = customer_surname;
            this.customer_email = customer_email;
            this.customer_phone_number = customer_phone_number;
        }

        // New Customer 
        public Customer(string customer_forename, string customer_surname, string customer_email, long customer_phone_number)
        {
            this.customer_forename = customer_forename;
            this.customer_surname = customer_surname;
            this.customer_email = customer_email;
            this.customer_phone_number = customer_phone_number;
        }

        public Customer() { }

        // --- End of Constructors
    }

    class Rental
    {
        private int rental_id;
        private int car_id;
        private int customer_id;
        private int staff_id;
        private string rental_start_date;
        private string rental_end_date;
        private float rental_cost;

        public int Rental_ID { get { return rental_id; } set { rental_id = value; } }
        public int Car_ID { get { return car_id; } set { car_id = value; } }
        public int Customer_ID { get { return customer_id; } set { customer_id = value; } }
        public int Staff_ID { get { return staff_id; } set { staff_id = value; } }
        public string Rental_Start_Date { get { return rental_start_date; } set { rental_start_date = value; } }
        public string Rental_End_Date { get { return rental_end_date; } set { rental_end_date = value; } }
        public float Rental_Cost { get { return rental_cost; } set { rental_cost = value; } }

        public Rental(int rental_id, int customer_id, int staff_id, string rental_start_date, string rental_end_date, float rental_cost)
        {
            this.rental_id = rental_id;
            this.customer_id = customer_id;
            this.staff_id = staff_id;
            this.rental_start_date = rental_start_date;
            this.rental_end_date = rental_end_date;
            this.rental_cost = rental_cost;
        }

        public Rental() { }
    }
}
