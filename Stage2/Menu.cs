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
            Console.WriteLine("1. Add/Remove car");
            Console.WriteLine("2. Add car balance(by id)");
            Console.WriteLine("3. Get history transactions for last minute");
            Console.WriteLine("4. Show parking balance");
            Console.WriteLine("5. Show parking balance for last minute");
            Console.WriteLine("6. Show number of places"); // free and all
            Console.WriteLine("7. Show file: Transactions.log");
            Console.WriteLine("8. Exit\n");
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
                    Console.WriteLine("What do you want: " +
                        "1. Add car\n" +
                        "2. Remove car\n");
                    Int32.Parse(Console.ReadLine());

                    break;

                case 2:
                    AddCarBalance();
                    break;

                case 3:
                    GetTransacrionForLastMinute();
                    break;

                case 4:
                    ShowParkingBalance();
                    break;
                    
                case 5:
                    ShowParkingBalanceForLastMinute();
                    break;

                case 6:
                    ShowParkingSpace();
                    break;

                case 7:
                    ShowLog();
                    break;

                case 8:
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

        static private void AddCarBalance()
        {
            Console.Write("Enter your CarID: ");
            var id = Int32.Parse(Console.ReadLine());

            if (!Parking.Instance.Cars.Exists(c => c.Id == id))
            {
                Console.WriteLine("Wrong ID");
                return;
            }

            Console.Write("Enter money: ");
            var value = Decimal.Parse(Console.ReadLine());

            var balance = Parking.Instance.RefillCarBalanceById(id, value);

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
            Console.WriteLine("---Transactions.log---");

            using (var log = File.OpenText(Settings.logFile))
            {
                Console.WriteLine(log.ReadToEnd());
            }
        }
    }
}
