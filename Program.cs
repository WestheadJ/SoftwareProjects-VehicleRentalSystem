using DatabaseHandler;
using ORM;
namespace Program
{

    class Program
    {

        public static void Main(string[] args)
        {
            string help = @"HELP MENU:
    Command Formatting:
        vrs -command | [type] args 

    Commands:
        -help = help commands

        - help-admin = help commands for admins

        -version = version

        -login |[int] staff_id, [string] staff_password = Used for running multiple commands using an interface | Can be used by any member of staff.

        -register | [int] staff_id, [string] staff,password, [string] new_staff_forename, [string] new_staff_surname, [string] new_staff_email, [long] new_staff_phone_number, [string] is_admin(yes/no) = Used for registering a new staff member | Can only be used by admins

        -rent-car-new | [int] staff_id, [string] staff_password, [int] car_id, [string] customer_forename, [string] customer_surname, [string] customer_email, [long] customer_phone_number, [string] start_date, [string] end_date = Used to rent a car | Can be used by any member of staff

        -search-rented | [int] staff_id, [string] staff_password = Used to search for all rented cars | Can be used by by any member of staff

        -search-available | [int] staff_id, [string] staff_password = Used to search for all cars available to rent |  Can be used by any member of staff.

        -search-staff-details | [int] staff_id, [string] staff_password, [string] staff_email = Used to search for a member of staffs details using their email | Can only be used by an admins

        -search-customer-details | [int] staff_id, [string] staff_password, [string] customer_email = Used to search for a customers details | Can be used by any staff

        -search-car-detail-license | [int] staff_id, [string] staff_password, [string] car_license_plate = Used to search for a cars details with the license plate | Can be used by any member of staff 

        -search-car-detail-vin  | [int] staff_id, [string] staff_password, [string] car_vin = Used to search for a cars details with the VIN number | Can be used by any member of staff

        -search-car-detail-id  | [id] staff_id, [string] staff_password, [int] car_id = Used to search for a cars details with the cars database ID | Can be used by any member of staff

        -add-car | [int] staff_id, [string] staff_password, [string] car_model, [string] car_make, [string] car_vin, [string] car_plate_number = Used to add a new car to the database | Can only be used by admins

        -remove-car | [int] staff_id, [string] staff_password, [int] car_id = Used to remove a car from the database | Can only be used by admins

        -remove-staff | [int] staff_id, [string] staff_password, [int] staff_id = Used to remove a staff member from the database | Can only be used by admins

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

            Tuple<int, string> loggedInUser;

            Dictionary<int, Car> availableCars = new Dictionary<int, Car>();

            try
            {
                Database DB = new Database();
                // No arguments
                if (args.Length == 0) { Console.WriteLine("No arguments given!"); Help(); }
                else
                {
                    if (args[0] == "-help" || args[0] == "-h") { Help(); }
                    // login | staff_id, staff_password
                    else if (args[0] == "-login" || args[0] == "-l") { Login(DB, Convert.ToInt32(args[1]), args[2]); }
                    // register | staff_id, staff,password, new_staff_forename, new_staff_surname, new_staff_email, new_staff_phone_number, is_admin

                    else if (args[0] == "-register" || args[0] == "-r") { RegisterShorthand(DB, Convert.ToInt32(args[1]), args[2], args[3], args[4], args[5], Convert.ToInt64(args[6]), args[7], args[8]); }
                    // rent-car-new | staff_id, staff_password, car_id, customer_forename, customer_surname, customer_email, customer_phone_number, start_date, end_date

                    else if (args[0] == "-rent-car-new" || args[0] == "-rcn") { RentCarShorthand(DB, Convert.ToInt32(args[1]), args[2], Convert.ToInt32(args[3]), new Customer(args[4], args[5], args[6], Convert.ToInt64(args[7])), args[8], args[9]); }

                    // -search-rented | [int] staff_id, [string] staff_password = Used to search for all rented cars | Can be used by by any member of staff
                    else if (args[0] == "-search-rented" || args[0] == "-sr") { SearchRentedShorthand(DB, Convert.ToInt32(args[1]), args[2]); }

                    // -search-available | [int] staff_id, [string] staff_password = Used to search for all cars available to rent |  Can be used by any member of staff.
                    else if (args[0] == "-search-available" || args[0] == "-sa") { SearchAvailableShorthand(DB, Convert.ToInt32(args[1]), args[2]); }

                    else if (args[0] == "-search-staff-details" || args[0] == "-ssd") { SearchStaffDetailsShorthand(DB, Convert.ToInt32(args[1]), args[2], args[3]); }

                    // -search - customer - details | [int] staff_id, [string] staff_password, [string] customer_email = Used to search for a customers details | Can be used by any staff
                    else if (args[0] == "-search-customer-details" || args[0] == "-scd") { SearchCustomerDetailsByEmailShorthand(DB, Convert.ToInt32(args[1]), args[2], args[3]); }
                    else
                    {
                        ExitMessage($"Invalid Command :{args[0]} | use -help for a list of commands!");
                    }
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
                Tuple<bool, int, string> login = DB.Login(staff_id, staff_password);
                if (login.Item1)
                {
                    loggedInUser = new Tuple<int, string>(login.Item2, login.Item3);
                    Menu(new Dictionary<string, string> {
                        { "1", "Register a new user (Admin Only)" },
                        { "2", "Rent a car" },
                        { "3", "Search All Rented Cars"},
                        { "4", "Search All Available Cars"},
                        {"5","Search Staff Details (Admin Only)"},
                        { "6", "Help list"},
                        { "7", "Quit"}
                    },
                    new List<Action> {
                        // 1
                        new Action(() => { Register(DB,staff_id,staff_password); }),
                        // 2
                        new Action(()=>{ RentCar(DB);}),
                        // 3 
                        new Action(()=>{ SearchRentedShorthand(DB,staff_id,staff_password); }),
                        // 4
                        new Action(()=>{ SearchAvailableShorthand(DB,staff_id,staff_password); }),
                        // 5
                        new Action(()=>{ SearchStaffDetails(DB); }),

                        new Action(() => { Help(); }),
                        new Action(()=>{ System.Environment.Exit(0); })
                    });
                }
                // If login unsuccessful

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

                if (admin_staff_id == loggedInUser.Item1 && admin_staff_password == loggedInUser.Item2)
                {
                    DB.Register(admin_staff_id, admin_staff_password, new_staff_forename, new_staff_surname, new_staff_email, new_staff_phone_number, new_staff_password, is_admin);
                }
                else
                {
                    Console.WriteLine("Wrong details! Try again with the current logged in users details");
                }
            }

            // register | staff_id, staff,password, new_staff_forename, new_staff_surname, new_staff_email, new_staff_phone_number, is_admin
            void RegisterShorthand(Database DB, int staff_id, string staff_password, string new_staff_forename, string new_staff_surname, string new_staff_email, long new_staff_phone_number, string new_staff_password, string is_admin)
            {
                int new_staff_is_admin;

                if (is_admin == "yes")
                {
                    new_staff_is_admin = 1;
                }
                else
                {
                    new_staff_is_admin = 0;
                }
                DB.Register(staff_id, staff_password, new_staff_forename, new_staff_surname, new_staff_email, new_staff_phone_number, new_staff_password, new_staff_is_admin);

            }

            // ---- END Register ----

            // --- Rent Car ---

            // rent-car-new | staff_id, staff_password, car_id, customer_forename, customer_surname, customer_email, customer_phone_number, start_date[yyyy-mm-dd], end_date[yyyy-mm-dd]
            void RentCarShorthand(Database DB, int staff_id, string staff_password, int car_id, Customer customer, string start_date_string, string end_date_string)
            {
                Tuple<bool, int, string> login = DB.Login(staff_id, staff_password);
                // Login
                if (login.Item1)
                {
                    // Does the car exist
                    if (DB.GetCarByID(car_id).Item1)
                    {
                        // If the car is available
                        if (DB.GetRentalAvailability(car_id, start_date_string, end_date_string))
                        {
                            // Does the customer exist
                            Tuple<bool, Customer> customerDetails = DB.GetCustomerByEmail(customer.Customer_Email);
                            // If not
                            long customer_id;
                            if (!customerDetails.Item1)
                            {
                                customer_id = DB.CreateNewCustomer(customer);
                            }
                            else { customer_id = Convert.ToInt64(customerDetails.Item2.Customer_ID); }
                            DB.RentCar(car_id, customer_id, staff_id, start_date_string, end_date_string);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Car does not exist!");
                    }
                }
            }

            void RentCar(Database DB)
            {

                SearchAvailableShorthand(DB, loggedInUser.Item1, loggedInUser.Item2);

                Console.WriteLine("\nEnter the car id of the car you want to rent: ");
                Customer customer = new Customer();

                int car_id = Convert.ToInt32(Console.ReadLine());
                if (DB.GetCarByID(car_id).Item1)
                {
                    Console.WriteLine("Enter the start date in the format of: YYYY-MM-DD e.g 2023-12-25: ");
                    string start_date = Console.ReadLine();

                    Console.WriteLine("Enter the end date in the format of: YYYY-MM-DD e.g 2023-12-25: ");
                    string end_date = Console.ReadLine();

                    if (DB.GetRentalAvailability(car_id, start_date, end_date))
                    {
                        Console.WriteLine("Enter the customers forename: ");
                        customer.Customer_Forename = Console.ReadLine();

                        Console.WriteLine("Enter the customers surname: ");
                        customer.Customer_Surname = Console.ReadLine();

                        Console.WriteLine("Enter the customers email");
                        customer.Customer_Email = Console.ReadLine();

                        Console.WriteLine("Enter the customers phone number");
                        customer.Customer_Phone_Number = Convert.ToInt64(Console.ReadLine());

                        Console.WriteLine("Enter signed in account details:");

                        Console.WriteLine("Staff ID: ");
                        int admin_staff_id = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Staff Password: ");
                        string admin_staff_password = Console.ReadLine();

                        if (admin_staff_id == loggedInUser.Item1 && admin_staff_password == loggedInUser.Item2)
                        {
                            // Does the customer exist
                            Tuple<bool, Customer> customer_details = DB.GetCustomerByEmail(customer.Customer_Email);
                            // If not
                            long customer_id;
                            if (!customer_details.Item1)
                            {
                                customer_id = DB.CreateNewCustomer(customer);
                            }
                            else { customer_id = Convert.ToInt64(customer_details.Item2.Customer_ID); }
                            DB.RentCar(car_id, customer_id, admin_staff_id, start_date, end_date);
                        }
                    }
                }
                Console.WriteLine("Press enter to go back ->");
                Console.Read();
            }

            // ---  END Rent Car ---

            // --- Searches ---

            /// <summary>
            /// Searches for all cars being rented
            /// </summary>
            /// <param name="DB">The database instance.</param>
            /// <param name="staff_id">The staff ID for authentication.</param>
            /// <param name="staff_password">The staff password for authentication.</param>
            void SearchRentedShorthand(Database DB, int staff_id, string staff_password)
            {
                Tuple<bool, int, string> login = DB.Login(staff_id, staff_password);
                if (login.Item1)
                {
                    // Print a list of available cars
                    Console.WriteLine("List of cars being rented:");

                    // Get all available cars from the database
                    List<RentedCar> cars = DB.GetAllRentedCars();

                    // Print details of each available car
                    foreach (var car in cars)
                    {
                        Console.WriteLine("Car ID: {0}, Car Model: {1}, Car Make: {2}, Rental Period: {3} to {4}", car.Car_ID, car.Car_Model, car.Car_Make, car.Rental_Start_Date, car.Rental_End_Date);
                    }
                }
                Console.WriteLine("Enter any key to go back...");
                Console.Read();
            }

            /// <summary>
            /// Searches for available cars in the database and prints their details.
            /// </summary>
            /// <param name="DB">The database instance.</param>
            /// <param name="staff_id">The staff ID for authentication.</param>
            /// <param name="staff_password">The staff password for authentication.</param>
            void SearchAvailableShorthand(Database DB, int staff_id, string staff_password)
            {
                Tuple<bool, int, string> login = DB.Login(staff_id, staff_password);
                if (login.Item1)
                {
                    // Print a list of available cars
                    Console.WriteLine("List of available cars:");

                    // Get all available cars from the database
                    List<Car> cars = DB.GetAllAvailableCars();

                    // Print details of each available car
                    foreach (var car in cars)
                    {
                        Console.WriteLine("Car ID: {0}, Car Model: {1}, Car Make: {2}, Car Price Per Hour: {3}", car.Car_ID, car.Car_Model, car.Car_Make, car.Car_Price);
                    }
                }
                Console.WriteLine("Enter any key to go back...");
                Console.Read();
            }

            void SearchStaffDetailsShorthand(Database DB, int staff_id, string staff_password, string staff_email)
            {
                Tuple<bool, int, string> login = DB.Login(staff_id, staff_password);
                if (!login.Item1)
                {
                    ExitMessage("Incorrect Details!");
                }
                bool isAdmin = DB.IsAdmin(staff_id, staff_password);
                if (!isAdmin)
                {
                    ExitMessage("You need to be an admin to use this command!");
                }
                else
                {
                    Tuple<bool, Staff> result = DB.GetStaffDetails(staff_email);
                    if (result.Item1 == true)
                    {
                        Staff staffDetails = result.Item2;
                        Console.WriteLine($"{staffDetails.Staff_Forename} {staffDetails.Staff_Surname}'s Details:");
                        Console.WriteLine("ID: " + staffDetails.Staff_ID);
                        Console.WriteLine("Password: " + staffDetails.Staff_Password);
                        Console.WriteLine("Forename: " + staffDetails.Staff_Forename);
                        Console.WriteLine("Surname: " + staffDetails.Staff_Surname);
                        Console.WriteLine("Email: " + staffDetails.Staff_Email);
                        ExitMessage("Phone Number: " + staffDetails.Staff_Phone_Number);
                    }
                    else
                    {
                        ExitMessage("That user cannot be found!");

                    }
                }

            }

            void SearchStaffDetails(Database DB)
            {
                bool isAdmin = DB.IsAdmin(loggedInUser.Item1, loggedInUser.Item2);
                if (!isAdmin)
                {
                    ExitMessage("You need to be an admin to use this command!");
                }
                else
                {
                    Console.WriteLine("Enter the users email: ");
                    string email = Console.ReadLine();
                    Tuple<bool, Staff> result = DB.GetStaffDetails(email);
                    if (result.Item1 == true)
                    {
                        Staff staffDetails = result.Item2;
                        Console.WriteLine($"{staffDetails.Staff_Forename} {staffDetails.Staff_Surname}'s Details:");
                        Console.WriteLine("ID: " + staffDetails.Staff_ID);
                        Console.WriteLine("Password: " + staffDetails.Staff_Password);
                        Console.WriteLine("Forename: " + staffDetails.Staff_Forename);
                        Console.WriteLine("Surname: " + staffDetails.Staff_Surname);
                        Console.WriteLine("Email: " + staffDetails.Staff_Email);
                        ExitMessage("Phone Number: " + staffDetails.Staff_Phone_Number);
                    }
                    else
                    {
                        ExitMessage("That user cannot be found!");
                    }
                }
            }

            void SearchCustomerDetailsByEmailShorthand(Database DB, int staff_id, string staff_password, string customer_email)
            {
                bool loginResult = DB.Login(staff_id, staff_password).Item1;
                if (loginResult)
                {
                    Tuple<bool, Customer> customerDetailsResult = DB.GetCustomerByEmail(customer_email);
                    if (!customerDetailsResult.Item1)
                    {
                        ExitMessage($"No customer with the email: {customer_email}");
                    }
                    else
                    {
                        Customer customerDetails = customerDetailsResult.Item2;

                        Console.WriteLine($"{customerDetails.Customer_Forename} {customerDetails.Customer_Surname}'s details: ");
                        Console.WriteLine("ID: " + customerDetails.Customer_ID);
                        Console.WriteLine("Forename: " + customerDetails.Customer_Forename);
                        Console.WriteLine("Surname: " + customerDetails.Customer_Surname);
                        Console.WriteLine("Email: " + customerDetails.Customer_Email);
                        Console.WriteLine("Phone Number: " + customerDetails.Customer_Phone_Number);

                        List<RentedCar> customersRentals = new List<RentedCar>(5);

                        Console.WriteLine("Customers Rentals:");

                        if (customersRentals.Count != 0)
                        {
                            foreach (var car in customersRentals)
                            {
                                Console.WriteLine("Rental Information:");
                                Console.WriteLine($"Rental ID: {car.Rental_ID} | Rental Start Date: {car.Rental_Start_Date} | Rental End Date: {car.Rental_End_Date} | Rental Price: {car.Rental_Price}");
                                Console.WriteLine("Car Information:");
                                Console.WriteLine($"Car ID: {car.Car_ID} | Car Model: {car.Car_Model} | Car Make: {car.Car_Make} | Car VIN: {car.Car_Vin} | Car License Plate: {car.Car_License_Plate} \n");
                            }
                            ExitMessage("");
                        }
                        else
                        {
                            ExitMessage($"{customerDetails.Customer_Forename} {customerDetails.Customer_Surname} doesn't have any rentals on record.");
                        }
                    }
                }
                else
                {
                    ExitMessage("Incorrect Details! ");
                }
            }

            void SearchCustomerDetailsByEmail(Database DB, int staff_id, string staff_password, string customer_email)
            {

            }

            void Search(Database DB)
            {

            }

            // --- END Searches --- 

            void Help()
            {
                ExitMessage(help);
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
                        Console.Clear();
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
                        Console.Write("\n");
                    }
                    else
                    {
                        Console.WriteLine((count + 1) + ". " + options[(count + 1).ToString()]);
                        Console.ResetColor();
                    }
                }
            }

            void ExitMessage(string message)
            {
                if (message != "")
                {
                    Console.WriteLine(message);
                }
                Console.WriteLine("Press any key to escape...");
                Console.Read();
            }

            // --- END Menu Methods ---
        }

    }
}