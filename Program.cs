using DatabaseHandler;
namespace Program{

    class Program{
        
        
        public static void Main(string[] args){
            string help = @"HELP MENU:
    Args Formatting:
        vrs command | args 

    Commands:
        login | staff_id, staff_password = Used for logging in, used by any member of staff.
        register | staff_forename, staff_surname, staff_email, staff_phone_number, is_admin, staff_id = Used for registering, can only be used by an admin.
        rent-car-new | staff_id, staff_password, car_id, customer_forename, customer_surname, customer_email, customer_phone_number, start_date, end_date = Rent a car with a new customer, used by any member of staff.
        rent-car-old | staff_id, staff_password, car_id, customer_id, start_date, end_date = Rent a car with an existing customer, used by any member of staff.
        search-rented | staff_id, staff_password = Search for all rented cars, used by any member of staff. 
        search-available | staff_id, staff_password = Search for all available to rent cars, used by any member of staff.
        search-staff-details | staff_id, staff_password, staff_id = Search for a member of staffs details, can only be used by an admin.
        search-customer-details | staff_id, staff_password, customer_email = 
        search-car-detail-license  | staff_id, staff_password, car_license_plate
        search-car-detail-vin  | staff_id, staff_password, car_vin
        search-car-detail-id  | staff_id, staff_password, car_id
        
    Shorthands:
        -help = help commands
        -version = version
        -l = login
        -r = register
        -rcn = rent-car-new
        -rco = rent-car-old
        -sr = search-rented
        -sa = search-available
        -ssd = search-staff-details
        -scd = search-customer-details
        -scdl = search-car-details-license
        -scdv = search-car-details-vin
        -scdid = search-car-details-id";
        Database db = new Database();
        db.Login();
        
        }
    }
}