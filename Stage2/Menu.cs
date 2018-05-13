using System;
using System.IO;

namespace Stage2
{
    static class Menu
    {
        static private bool run = true;

        public static void Run()
        {
            while (run)
            {
                ShowCommands();
                ChooseCommand();
            }
        }

        static private void ShowCommands()
        {
            Console.WriteLine("---Parking menu---");
            Console.WriteLine("1. Add car");
            Console.WriteLine("2. Remove car");
            Console.WriteLine("3. Add car balance");
            Console.WriteLine("4. Get history transactions for last minute");
            Console.WriteLine("5. Show parking balance");
            Console.WriteLine("6. Show parking balance for last minute");
            Console.WriteLine("7. Show number of places");
            Console.WriteLine("8. Show file: Transactions.log");
            Console.WriteLine("9. Exit\n");
        }


        static public void ChooseCommand()
        {
            int choice = 0;

            Console.Write("Enter your choice: ");
            Int32.TryParse(Console.ReadLine(), out choice);

            Console.WriteLine(new string('-', 35));

            switch (choice)
            {
                case 1:
                    AddCar();
                    break;

                case 2:
                    RemoveCar();
                    break;

                case 3:
                    AddCarBalance();
                    break;

                case 4:
                    GetTransacrionForLastMinute();
                    break;

                case 5:
                    ShowParkingBalance();
                    break;
                    
                case 6:
                    ShowParkingBalanceForLastMinute();
                    break;

                case 7:
                    ShowParkingSpace();
                    break;

                case 8:
                    ShowLog();
                    break;

                case 9:
                    run = false;
                    break;

                default:
                    Console.WriteLine("Error. Wrong command");
                    break;
            }
           
            Console.WriteLine(new string('-', 35));
            Console.WriteLine("\n\n");

            Console.ReadKey();
        }

        static private void AddCar()
        {
            Console.WriteLine("\nAvailable car type:");
            foreach (var carType in Enum.GetValues(typeof(CarType)))
            {
                Console.WriteLine($" {(int)carType}. {carType}");
            }

            int type = 0;
            Console.Write("Enter type(number): ");
            Int32.TryParse(Console.ReadLine(), out type);
            if(!Enum.IsDefined(typeof(CarType), type))
            {
                Console.WriteLine("---Error, Wrong car type---");
                return;
            }

            decimal value = 0;
            Console.Write("Enter money: ");
            Decimal.TryParse(Console.ReadLine(), out value);

            var car = new Car((CarType)type, value);

            try
            {
                Parking.Instance.AddCar(car);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("---Error---");
                Console.WriteLine(ex.Message);
                return;
            }

            Console.WriteLine($"---Success, your car [ID:{car.Id}]---");
        }

        static private void RemoveCar()
        {
            Console.Write("Enter your CarID: ");
                   
            try
            {
                var id = Guid.Parse(Console.ReadLine());
                Parking.Instance.RemoveCar(id);
            }
            catch (FormatException)
            {
                Console.WriteLine("---Error, Not correct CarID---");
                return;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("---Error---");
                Console.WriteLine(ex.Message);
                return;
            }
            

            Console.WriteLine($"---Success---");
        }

        static private void AddCarBalance()
        {
            var id = new Guid();
            Console.Write("Enter your CarID: ");
            Guid.TryParse(Console.ReadLine(), out id);

            decimal value = 0;
            Console.Write("Enter money: ");
            Decimal.TryParse(Console.ReadLine(), out value);

            decimal balance = 0;
            try
            {
                balance = Parking.Instance.RefillCarBalance(id, value);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("---Error---");
                Console.WriteLine(ex.Message);
                return;
            }

            Console.WriteLine($"---Success, your balance now: {balance}---");
        }

        static private void GetTransacrionForLastMinute()
        {
            Console.WriteLine("---Transaction for the last minute---");

            foreach (var transaction in Parking.Instance.GetTransactionsForLastMinute)
            {
                Console.WriteLine(transaction);
            }           
        }

        static private void ShowParkingBalance()
        {
            Console.WriteLine($"Parking balance: {Parking.Instance.Balance}");
        }

        static private void ShowParkingBalanceForLastMinute()
        {
            Console.WriteLine($"Parking balance for last minute: {Parking.Instance.BalanceForLastMinute}");
        }

        static private void ShowParkingSpace()
        {
            Console.WriteLine($"Free parking space: {Parking.Instance.FreeParkingSpace}");
            Console.WriteLine($"All parking space: {Parking.Instance.ParkingSpace}");
        }

        static private void ShowLog()
        {
            string log = String.Empty;

            try
            {
                log = File.ReadAllText(Settings.logFile);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("---Error, Transactions.log not found---");
                return;
            }

            Console.WriteLine("---Transactions.log---");
            Console.WriteLine(log);
        }
    }
}
