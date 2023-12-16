namespace ORM{
    class Car{
        private int car_id;
        public int Card_ID{get{return car_id;}set{car_id=value;}}
        private string car_make;
        public string Car_Make{get{return car_make;}set{car_make=value;}}
        private string car_model;
        public string Car_Model{get{return car_model;}set{car_model=value;}}
        private string car_vin;
        public string Car_Vin{get{return car_vin;}set{car_vin=value;}}
        private string car_license_plate;
        public string Car_License_Plate{get{return car_license_plate;}set{car_license_plate=value;}}
        private float car_price;
        public float Car_Price{get{return car_price}set{car_price=value;}}

        public Car(int car_id,string car_make,string car_model,string car_vin,string car_license_plate,float car_price){
            this.car_id = car_id;
            this.car_make = car_make;
            this.car_model = car_model;
            this.car_vin = car_vin;
            this.car_license_plate = car_license_plate;
            this.car_price = car_price;
        }
    }

    class Staff {
        protected int staff_id;
        public int Staff_ID{get{return staff_id;}set{staff_id=value;}}
        private string staff_forename;
        public string Staff_Forename{get{return staff_forename;}set{staff_forename=value;}}
        private string staff_surname;
        public string Staff_Surname{get{return staff_surname;}set{staff_surname=value;}}
        public Staff(int staff_id,string staff_forename,string staff_surname){
            this.staff_id=staff_id;
            this.staff_forename = staff_forename;
            this.staff_surname = staff_surname;
        }
    }

}