using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Stage2
{
    class Parking
    {
        private static readonly Lazy<Parking> lazy = new Lazy<Parking>(() => new Parking());

        public static Parking Instance { get { return lazy.Value; } }

        private Parking()
        {
            Cars = new List<Car>(Settings.parkingSpace);
            Transactions = new List<Transaction>();

            makingTransactionProcess = new Timer(new TimerCallback(MakeTransactions), null, 0, Settings.timeout * 1000);
            writingLogProcess = new Timer(new TimerCallback(WriteToLog), null, 60 * 1000, 60 * 1000);
        }

        private Timer makingTransactionProcess;
        private Timer writingLogProcess;


        public List<Car> Cars { get; private set; }

        public List<Transaction> Transactions { get; private set; }


        public decimal Balance { get; private set; }

        public decimal BalanceForLastMinute { get { return Transactions.Where(t => t.DateTime >= DateTime.Now.AddMinutes(-1)).Sum(t => t.WrittenOffMoney); } }


        public int ParkingSpace { get { return Settings.parkingSpace; } }

        public int FreeParkingSpace { get { return ParkingSpace - Cars.Count; } }

        public IEnumerable<Transaction> GetHistoryForLastMinute { get { return Transactions.Where(t => t.DateTime >= DateTime.Now.AddMinutes(-1)); } }



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
            if (!Cars.Exists(c => c.Id == carId))
            {
                throw new InvalidOperationException("Here don`t have this car");
            }
            else
            {
                Cars.Remove(Cars.First(c => c.Id == carId));
            }
        }

        private void MakeTransactions(object obj)
        {
            foreach (var car in Cars)
            {
                decimal price = Settings.parkingPrices[car.Type];

                if (car.Balance < price)
                {
                    price *= Settings.fine;
                }

                car.SubMoney(price);
                Balance += price;

                Transactions.Add(new Transaction(car.Id, price));

                // Deleting transactions older than 1 minute
                Transactions.RemoveAll(t => t.DateTime < DateTime.Now.AddMinutes(-1));
            }
        }

        private void WriteToLog(object obj)
        {
            using (StreamWriter file = new StreamWriter("Transactions.log", true))
            {
                file.WriteLine($"Time: {DateTime.Now}, Earned: {BalanceForLastMinute}");
            }
        }
    }
}
