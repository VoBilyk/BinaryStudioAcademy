using System;

namespace Stage2
{
    class Car
    {
        public Car(CarType type, decimal balance = 0)
        {
            Id = Guid.NewGuid();
            Type = type;
            Balance = balance;
        }

        public Guid Id { get; private set; }

        public decimal Balance { get; private set; }

        public CarType Type { get; private set; }

        public void AddMoney(decimal value)
        {
            Balance += value;
        }

        public void SubMoney(decimal value)
        {
            Balance -= value;
        }
    }
}
