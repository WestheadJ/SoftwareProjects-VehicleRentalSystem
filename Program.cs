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
        -scdid = search-car-details-id";
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
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err);

            }

            void Login(Database DB, int staff_id, string password)
            {
                if (DB.Login(staff_id, password))
                {
                    Console.WriteLine("Logged In!");
                    Menu(new Dictionary<string, string> { { "1", "Register" }, { "2", "Help" } },
                    new List<Action> { new Action(() => { Help(help); }), new Action(() => { Help(help); }) });
                }
                else
                {
                    Console.WriteLine("Could not login, details are incorrect!");
                }

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

                // for (int count = 0; count < options.Count(); count++)
                // {
                //     if (count == position)
                //     {
                //         Console.ForegroundColor = ConsoleColor.Black;
                //         Console.BackgroundColor = ConsoleColor.White;

                //         Console.Write(count + 1 + ". " + options[(count + 1).ToString()]);
                //         Console.ResetColor();
                //         Console.Write("\n");
                //     }
                //     else
                //     {
                //         Console.WriteLine(count + 1 + ". " + options[(count + 1).ToString()]);
                //     }


                // }

                // foreach(var i in options){
                //     Console.WriteLine(i.Key + ", "+ i.Value);
                // }

                // foreach(Action action in actions){
                //     action();
                // }
                void MenuWrite(Dictionary<string, string> options,int position)
                {
                    for (int count = 0; count < options.Count(); count++)
                    {
                        if (count == position)
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.BackgroundColor = ConsoleColor.White;

                            Console.WriteLine(count + 1 + ". " + options[(count + 1).ToString()]);
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.WriteLine(count + 1 + ". " + options[(count + 1).ToString()]);
                            Console.ResetColor();
                        }
                    }
                }

            }
        }
    }

}