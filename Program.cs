using DatabaseHandler;
using ORM;
namespace Program
{

    class Program
    {


        public static void Main(string[] args)
        {
            string help = @"HELP MENU:
    Args Formatting:
        vrs command | args 

    Commands:
        -help, help commands
        -version, version
        login | staff_id, staff_password = Used for logging in, used by any member of staff.
        register | staff_id, staff,password, new_staff_forename, new_staff_surname, new_staff_email, new_staff_phone_number, is_admin= Used for registering, can only be used by an admin.
        rent-car-new | staff_id, staff_password, car_id, customer_forename, customer_surname, customer_email, customer_phone_number, start_date, end_date = Rent a car with a new customer, used by any member of staff.
        search-rented | staff_id, staff_password = Search for all rented cars = used by any member of staff. 
        search-available | staff_id, staff_password = Search for all available to rent car = used by any member of staff.
        search-staff-details | staff_id, staff_password, staff_id = Search for a member of staffs details = can only be used by an admin.
        search-customer-details | staff_id, staff_password, customer_email = 
        search-car-detail-license  | staff_id, staff_password, car_license_plate =
        search-car-detail-vin  | staff_id, staff_password, car_vin = 
        search-car-detail-id  | staff_id, staff_password, car_id =
        add-car | staff_id, staff_password, car_id, car_model, car_make, car_vin, car_plate_number = used by any member of staff
        remove-car | staff_id, staff_password, car_id = can only be used by an admin
        remove-staff | staff_id, staff_password, staff_id = can only be used by an admin

    Shorthands:
        -h = help 
        -v = version
        -l = login
        -r = register
        -rcn = rent-car-new
        -sr = search-rented
        -sa = search-available
        -ssd = search-staff-details
        -scd = search-customer-details
        -scdl = search-car-details-license
        -scdv = search-car-details-vin
        -scdid = search-car-details-id
        -ac = add-car
        -rc = remove-car
        -rs = remove-staff";

            try
            {
                Database DB = new Database();
                // No arguments
                if (args.Length == 0) { Console.WriteLine("No arguments given!"); Help(); }
                else
                {
                    if (args[0] == "help" || args[0] == "-h") { Help(); }
                    // login | staff_id, staff_password
                    else if (args[0] == "login" || args[0] == "-l") { 
                        Login(DB, Convert.ToInt32(args[1]), args[2]);}
                    // register | staff_id, staff,password, new_staff_forename, new_staff_surname, new_staff_email, new_staff_phone_number, is_admin
                    else if (args[0] == "register" || args[0] == "-r") { RegisterShorthand(DB, Convert.ToInt32(args[1]), args[2],args[3],args[4],args[5],Convert.ToInt64(args[6]),args[7],args[8]); }
                    // rent-car-new | staff_id, staff_password, car_id, customer_forename, customer_surname, customer_email, customer_phone_number, start_date, end_date 
                    else if (args[0] == "rent-car-new" || args[0] == "-rcn"){}
                    
                }
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Not enough arguments given - staff id or password was supplied"); Help();
            }
            catch (Exception err) { Console.WriteLine(err); }

            // --- Login ---

            void Login(Database DB, int staff_id, string staff_password)
            {
                // If login successful
                if (DB.Login(staff_id, staff_password))
                {
                    Console.WriteLine("Logged In!");
                    Menu(new Dictionary<string, string> {
                        { "1", "Register a new user" },
                        { "2", "Rent a car" },
                        { "3", "Help list"}
                    },
                    new List<Action> {
                        new Action(() => { Register(DB,staff_id,staff_password);}),
                        new Action(()=>{Console.WriteLine("Rent a new car");}),
                        new Action(() => { Help();})
                    });
                    File.Delete("info.dat");
                }
                // If login unsuccessful
                else{Console.WriteLine("Could not login, details are incorrect!");}
            }

            // --- END Login ---

            // --- Register ----

            void Register(Database DB, int staff_id, string staff_password)
            {
                // Enter new staff details
                Console.WriteLine("Registering a new member of staff, make sure the new member of staff fills this out:");

                Console.WriteLine("Enter new staff members' forename: ");
                string new_staff_forename = Console.ReadLine();

                Console.WriteLine("Enter new staff members' surname: ");
                string new_staff_surname = Console.ReadLine();

                Console.WriteLine("Enter new staff members' email: ");
                string new_staff_email = Console.ReadLine();

                Console.WriteLine("Enter new staff members' phone number: ");
                long new_staff_phone_number = Convert.ToInt64(Console.ReadLine());

                Console.WriteLine("Enter new staff members' password: ");
                string new_staff_password = Console.ReadLine();

                int is_admin = 0;
                while (true)
                {
                    Console.WriteLine("Is new member an admin (answer yes or no): ");
                    string answer = Console.ReadLine();
                    if (answer == "yes")
                    {
                        is_admin = 1;
                        break;
                    }
                    break;
                }

                Console.WriteLine("Enter signed in account details:");
                Console.WriteLine("Staff ID: ");
                int admin_staff_id = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Staff Password: ");
                string admin_staff_password = Console.ReadLine();

                Tuple<int,string> account_details = ReadStaffDetails();

                if(admin_staff_id == account_details.Item1 && admin_staff_password == account_details.Item2){
                    DB.Register(admin_staff_id,admin_staff_password,new_staff_forename,new_staff_surname,new_staff_email,new_staff_phone_number,new_staff_password,is_admin);
                }

            }

            // register | staff_id, staff,password, new_staff_forename, new_staff_surname, new_staff_email, new_staff_phone_number, is_admin
            void RegisterShorthand(Database DB, int staff_id, string staff_password, string new_staff_forename, string new_staff_surname, string new_staff_email, long new_staff_phone_number,string new_staff_password, string is_admin ){
            int new_staff_is_admin;
            
                if(is_admin == "yes"){
                    new_staff_is_admin = 1;
                }
                else{
                    new_staff_is_admin = 0;
                }
                DB.Register(staff_id,staff_password,new_staff_forename,new_staff_surname,new_staff_email,new_staff_phone_number,new_staff_password,new_staff_is_admin);
            
            }

            // ---- END Register ----
        
            // --- Rent Car ---

            // rent-car-new | staff_id, staff_password, car_id, customer_forename, customer_surname, customer_email, customer_phone_number, start_date, end_date
            void RentCarShorthand(Database DB, int staff_id,string staff_password,int car_id,Customer customer,DateTime start_date,DateTime end_date){
                // Login
                if(DB.Login(staff_id,staff_password)){
                    // Does the car exist
                    if(DB.GetCarByID(car_id=car_id).Item1){
                        if(!DB.GetRentalAvailability(car_id,start_date,end_date)){
                            
                        }
                        Console.WriteLine("Car is unavailable for this time period!")
                        // if(!DB.GetCustomerByEmail(customer.Customer_Email).Item1){
                        //     DB.CreateNewCustomer(customer);
                        // }
                    }
                }
            }

            // ---  END Rent Car ---

            void Help()
            {
                Console.WriteLine(help);
                Console.WriteLine("Press a key to go back to the menu ->");
                Console.Read();
            }

            Tuple<int, string> ReadStaffDetails()
            {
                var fs = File.Open("info.dat", FileMode.Open);
                BinaryReader br = new BinaryReader(fs);
                int staff_id;
                string staff_password;
                staff_id = br.ReadInt32();
                staff_password = br.ReadString();
                br.Close();
                fs.Close();
                return new Tuple<int, string>(staff_id, staff_password);
            }

            // --- Menu Methods --- 
            void Menu(Dictionary<string, string> options, List<Action> actions)
            {
                int position = 0;
                Render(options, position);
                while (true)
                {
                    var key = Console.ReadKey();
                    if (key.KeyChar == 'q') { break; }
                    else if (key.Key == ConsoleKey.UpArrow)
                    {
                        if (position == 0)
                        {
                            position = options.Count - 1;
                        }
                        else { position--; }
                        Console.Clear();
                    }
                    else if (key.Key == ConsoleKey.DownArrow)
                    {
                        if (position == options.Count - 1)
                        {
                            position = 0;
                        }
                        else { position++; }
                        Console.Clear();

                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        actions[position]();
                    }
                    Render(options, position);
                }
            }

            void Render(Dictionary<string, string> options, int position)
                {
                    Console.Clear();
                    for (int count = 0; count < options.Count(); count++)
                    {
                        if (count == position)
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.BackgroundColor = ConsoleColor.White;

                            Console.Write(count + 1 + ". " + options[(count + 1).ToString()]);
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.WriteLine("\n" + (count + 1) + ". " + options[(count + 1).ToString()]);
                            Console.ResetColor();
                        }
                    }
                }

            // --- END Menu Methods ---
        }
    }
}