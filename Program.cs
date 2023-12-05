using DatabaseHandler;
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
        add-car | staff_id, staff_password, car_id, car_model, car_make, car_vin, car_plate_number, used by any member of staff
        remove-car | staff_id, staff_password, car_id, can only be used by an admin
        remove-staff | staff_id, staff_password, staff_id, can only be used by an admin

    Shorthands:
        -h = help 
        -v = version
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
        -scdid = search-car-details-id
        -ac = add-car
        -rc = remove-car
        -rs = remove-staff";
        
            try
            {
                Database DB = new Database();

                if (args.Length == 0)
                {
                    Console.WriteLine("No arguments given!");
                    Help(help);
                }
                else
                {
                    if (args[0] == "help" || args[0] == "-h")
                    {
                        Help(help);
                    }
                    else if (args[0] == "login" || args[0] == "-l")
                    {
                        Login(DB, Convert.ToInt32(args[1]), args[2]);
                    }
                    else if(args[0] == "register"){
                        Register(DB,Convert.ToInt32(args[1]),args[2]);
                    } 
                    else if(args[0] == "-r"){
                        RegisterShorthand(DB,Convert.ToInt32(args[1]),args[2]);
                    }
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err);

            }

            void Login(Database DB, int staff_id, string staff_password)
            {
                if (DB.Login(staff_id, staff_password))
                {
                    Console.WriteLine("Logged In!");
                    Menu(new Dictionary<string, string> { 
                        { "1", "Register a new user" },
                        { "2", "Help list" } 
                    },
                    new List<Action> { 
                        new Action(() => { Register(DB,staff_id,staff_password); }), 
                        new Action(() => { Help(help); }) 
                    });
                }
                else
                {
                    Console.WriteLine("Could not login, details are incorrect!");
                }

            }

            void Register(Database DB, int staff_id ,string staff_password){
                Console.WriteLine("Registering a new member of staff, make sure the new member of staff fills this out:");
                Console.WriteLine("Enter new staff members' forename: ");
                string new_staff_forename = Console.ReadLine();
                Console.WriteLine("Enter new staff members' surname: ");
                string new_staff_surname = Console.ReadLine();
                Console.WriteLine("Enter new staff members' email: ");
                string new_staff_email = Console.ReadLine();
                Console.WriteLine("Enter new staff members' phone number: ");
                int new_staff_phone_number = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter new staff members' password: ");
                string new_staff_password = Console.ReadLine();
                int is_admin = 0;
                while(true){
                    Console.WriteLine("Is new member an admin (answer yes or no): ");
                    string answer = Console.ReadLine();
                    if(answer == "yes"){
                        is_admin = 1;
                        break;
                    }
                    break;
                }

            


            }

            void RegisterShorthand(Database DB,int staff_id,string staff_password){

            }

            void Help(string help)
            {
                Console.WriteLine(help);
            }

            void Menu(Dictionary<string, string> options, List<Action> actions)
            {
                int position = 0;
                MenuWrite(options,position);
                while (true)
                {
                    var key = Console.ReadKey();
                    if (key.KeyChar == 'q') { break; }
                    else if (key.Key == ConsoleKey.UpArrow)
                    {
                        if (position == 0)
                        {
                            position = options.Count-1;
                        }
                        else { position--; }
                                            Console.Clear();


                    }
                    else if(key.Key == ConsoleKey.DownArrow){
                        if(position == options.Count-1){
                            position = 0;
                        }
                        else{position ++;}
                                            Console.Clear();

                    }
                    else if(key.Key == ConsoleKey.Enter){
                        actions[position]();
                    }
                    MenuWrite(options,position);
                }

                void MenuWrite(Dictionary<string, string> options,int position)
                {
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
                            Console.WriteLine("\n"+(count + 1) + ". " + options[(count + 1).ToString()]);
                            Console.ResetColor();
                        }
                    }
                }

            }
        }
    }

}