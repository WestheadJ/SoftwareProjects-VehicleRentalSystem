namespace ORM{
    class Car{
        // --- Getters ---
        protected int car_id;
        protected string car_make;
        protected string car_model;
        private string car_vin;
        private string car_license_plate;
        private float car_price;
        
        // --- End of Getters ---

        // --- Setters ----
        public int Car_ID{get{return car_id;}set{car_id=value;}}
        public string Car_Make{get{return car_make;}set{car_make=value;}}
        public string Car_Model{get{return car_model;}set{car_model=value;}}
        public string Car_Vin{get{return car_vin;}set{car_vin=value;}}
        public string Car_License_Plate{get{return car_license_plate;}set{car_license_plate=value;}}
        public float Car_Price{get{return car_price;}set{car_price=value;}}
    
        // --- End of Setters

        // --- Constructors ---

        // Existing Car Constructor
        public Car(int car_id,string car_make,string car_model,string car_vin,string car_license_plate,float car_price){
            this.car_id = car_id;
            this.car_make = car_make;
            this.car_model = car_model;
            this.car_vin = car_vin;
            this.car_license_plate = car_license_plate;
            this.car_price = car_price;
        }

        // New Car Constructor
        public Car(string car_make,string car_model,string car_vin,string car_license_plate,float car_price){
            this.car_make = car_make;
            this.car_model = car_model;
            this.car_vin = car_vin;
            this.car_license_plate = car_license_plate;
            this.car_price = car_price;
        }

        // Available Cars Constructor
        public Car(int car_id, string car_model, string car_make, float car_price){
            this.car_id = car_id;
            this.car_model = car_model;
            this.car_make = car_make;
            this.car_price = car_price;
        }

        public Car(){
            
        }
        // --- End of Constructors
    }

    class RentedCar : Car {
        private string rental_start_date;
        private string rental_end_date;

        public string Rental_Start_Date{get{return rental_start_date;}set{rental_start_date = value;}}
        public string Rental_End_Date{get{return rental_end_date;}set{rental_end_date=value;}}

        // --- Constructors --- 
            public RentedCar(int car_id, string car_model,string car_make, string rental_start_date, string rental_end_date){
            this.car_id = car_id;
            this.car_model = car_model;
            this.car_make = car_make;
            this.rental_start_date = rental_start_date;
            this.rental_end_date = rental_end_date;
        }
    }

    class Staff {
        // --- Getters ---
        protected int staff_id;
        private string staff_forename;
        private string staff_surname;
        private string staff_email;
        private string staff_password;
        private long staff_phone_number;
        private int staff_is_admin;

        // --- End of Getters --- 

        // --- Setters --- 
        public int Staff_ID{get{return staff_id;}set{staff_id=value;}}
        public string Staff_Forename{get{return staff_forename;}set{staff_forename=value;}}
        public string Staff_Surname{get{return staff_surname;}set{staff_surname=value;}}
        public string Staff_Email{get{return staff_email;}set{staff_email=value;}}
        public string Staff_Password{get{return staff_password;}set{staff_password=value;}}
        public long Staff_Phone_Number{get{return staff_phone_number;}set{staff_phone_number=value;}}
        public int Staff_Is_Admin{get{return staff_is_admin;}set{staff_is_admin=value;}}

        // --- End of Setters --- 

        // Constructors
        public Staff(int staff_id,string staff_forename,string staff_surname){
            this.staff_id=staff_id;
            this.staff_forename = staff_forename;
            this.staff_surname = staff_surname;
        }

        // New Staff Member Constructor
        public Staff(string staff_forename,string staff_surname,string staff_email, string staff_password, long staff_phone_number, int staff_is_admin){
            this.staff_id=staff_id;
            this.staff_forename = staff_forename;
            this.staff_surname = staff_surname;
            this.staff_email = staff_email;
            this.staff_password = staff_password;
            this.staff_phone_number = staff_phone_number;
            this.staff_is_admin = staff_is_admin;
        }

        // --- End of Constructors
    }

    class Customer{
        // Getters
        private int customer_id;
        private string customer_forename;
        private string customer_surname;
        private string customer_email;
        private long customer_phone_number;

        // End of Getters

        // Setters
        public int Customer_ID{get{return customer_id;}set{customer_id=value;}}
        public string Customer_Forename{get{return customer_forename;}set{customer_forename=value;}}
        public string Customer_Surname{get{return customer_surname;}set{customer_surname=value;}}
        public string Customer_Email{get{return customer_email;}set{customer_email=value;}}
        public long Customer_Phone_Number{get{return customer_phone_number;}set{customer_phone_number=value;}}

        // --- End of Setters ---

        // --- Constructors ---

        // Existing Customer Constructor
        public Customer(int customer_id,string customer_forename, string customer_surname, string customer_email, long customer_phone_number){
            this.customer_id = customer_id;
            this.customer_forename = customer_forename;
            this.customer_surname = customer_surname;
            this.customer_email = customer_email;
            this.customer_phone_number = customer_phone_number;
        }

        // New Customer 
        public Customer(string customer_forename, string customer_surname, string customer_email, long customer_phone_number){
            this.customer_forename = customer_forename;
            this.customer_surname = customer_surname;
            this.customer_email = customer_email;
            this.customer_phone_number = customer_phone_number;
        }

        public Customer(){
        }

        // --- End of Constructors
    }

}