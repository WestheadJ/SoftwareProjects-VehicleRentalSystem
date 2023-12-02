namespace ORM{
    public class Car{
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

        public Car(int car_id,string car_make,string car_model,string car_vin,string car_license_plate){
            this.car_id = car_id;
            this.car_make = car_make;
            this.car_model = car_model;
            this.car_vin = car_vin;
            this.car_license_plate = car_license_plate;
        }
    }


}