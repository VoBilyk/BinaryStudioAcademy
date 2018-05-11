namespace Stage2
{
    class Car
    {
        public Car(CarType type, decimal balance = 0)
        {
            Id = idNumber++;
            Type = type;
            Balance = balance;
        }

        static private int idNumber = 0;

        public int Id { get; private set; }

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
