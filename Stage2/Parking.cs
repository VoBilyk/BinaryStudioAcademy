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
            cars = new List<Car>(Settings.parkingSpace);
            transactions = new List<Transaction>();

            makingTransactionProcess = new Timer(new TimerCallback(MakeTransactions), null, 0, Settings.timeout * 1000);
            writingLogProcess = new Timer(new TimerCallback(WriteToLog), null, 60 * 1000, 60 * 1000);
        }

        private Timer makingTransactionProcess;
        private Timer writingLogProcess;


        private List<Car> cars;
        private List<Transaction> transactions;


        public decimal Balance { get; private set; }

        public decimal BalanceForLastMinute { get { return transactions.Where(t => t.DateTime >= DateTime.Now.AddMinutes(-1))
                                                                       .Sum(t => t.WrittenOffMoney); } }

        public int ParkingSpace { get { return Settings.parkingSpace; } }

        public int FreeParkingSpace { get { return ParkingSpace - cars.Count; } }

        public IEnumerable<Transaction> GetTransactionsForLastMinute { get { return transactions.Where(t => t.DateTime >= DateTime.Now.AddMinutes(-1)); } }

        public decimal RefillCarBalance(Guid id, decimal value)
        {
            if (!cars.Exists(c => c.Id == id))
            {
                throw new InvalidOperationException("Don`t have this car");
            }
            else
            {
                cars.Find(c => c.Id == id).AddMoney(value);

                return cars.Find(c => c.Id == id).Balance;
            }
        }

        public void AddCar(Car car)
        {
            if (cars.Count >= ParkingSpace)
            {
                throw new InvalidOperationException("Don`t have enough parking space");
            }
            else
            {
                cars.Add(car);
            }
        }

        public void RemoveCar(Guid carId)
        {
            if (!cars.Exists(c => c.Id == carId))
            {
                throw new InvalidOperationException("Here don`t have this car");
            }
           
            if(cars.Find(c => c.Id == carId).Balance < 0)
            {
                throw new InvalidOperationException("You must refill your balance");
            }
            else
            {
                cars.Remove(cars.Find(c => c.Id == carId));
            }
        }

        private void MakeTransactions(object obj)
        {
            foreach (var car in cars)
            {
                decimal price = Settings.parkingPrices[car.Type];

                if (car.Balance < price)
                {
                    price *= Settings.fine;
                }

                car.SubMoney(price);
                Balance += price;

                transactions.Add(new Transaction(car.Id, price));

                // Deleting transactions older than 1 minute
                transactions.RemoveAll(t => t.DateTime < DateTime.Now.AddMinutes(-1));
            }
        }

        private void WriteToLog(object obj)
        {
            using (StreamWriter file = new StreamWriter(Settings.logFile, true))
            {
                file.WriteLine($"Time: {DateTime.Now}, Earned: {BalanceForLastMinute}");
            }
        }
    }
}
