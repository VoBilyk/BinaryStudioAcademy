using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Stage2
{
    class Parking
    {
        private static readonly Lazy<Parking> lazy = new Lazy<Parking>(() => new Parking());

        public static Parking Instance { get { return lazy.Value; } }

        private Parking()
        {
            Cars = new List<Car>(ParkingSpace);
            Transactions = new List<Transaction>();

            timeout = Settings.Timeout;
            fine = Settings.Fine;
            ParkingSpace = Settings.ParkingSpace;
        }

        private int timeout;

        private decimal fine;


        public List<Car> Cars { get; private set; }

        public List<Transaction> Transactions { get; private set; }

        public decimal Balance { get; private set; }

        public decimal BalanceForLastMinute { get { return Transactions.Where(t => t.DateTime >= DateTime.Now.AddMinutes(-1)).Sum(t => t.WrittenOffMoney); } }

        public int ParkingSpace { get; private set; }

        public int FreeParkingSpace { get { return ParkingSpace - Cars.Count; } }


        public void AddCar(Car car)
        {
            if (Cars.Count >= ParkingSpace)
            {
                throw new InvalidOperationException("Don`t have enough parking space");
            }
            else
            {
                Cars.Add(car);
            }
        }

        public void DeleteCar(int carId)
        {
            Cars.Remove(Cars.First(item => item.Id == carId));
        }

        private void MakeTransactions()
        {
            foreach (var car in Cars)
            {
                decimal price = Settings.parkingPrices[car.Type];

                if (car.Balance < price)
                {
                    price *= fine;
                }

                car.SubMoney(price);
                Balance += price;

                Transactions.Add(new Transaction(car.Id, price));
            }
        }

        private void WriteToLog()
        {
            using (StreamWriter file = new StreamWriter("Transactions.log", true))
            {
                file.WriteLine($"Time: {DateTime.Now}, Sum: {BalanceForLastMinute}");
            }
        }
    }
}
