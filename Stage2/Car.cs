
using System;

namespace Stage2
{
    class Car
    {
        public Car(int id, CarType type, decimal balance = 0)
        {
            Id = id;
            Type = type;
            Balance = balance;
        }

        public int Id { get; private set; }

        public decimal Balance { get; private set; }

        public CarType Type { get; private set; }

        public void AddMoney(decimal value)
        {
            if (value < 0)
                throw new InvalidOperationException("Wrong value");
            else
                Balance += value;
        }

        public void SubMoney(decimal value)
        {
            if (Balance < value && value < 0)
                throw new InvalidOperationException("Wrong value");
            else
                Balance -= value;
        }
    }
}
