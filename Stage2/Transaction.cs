using System;

namespace Stage2
{
    class Transaction
    {
        public Transaction(int carId, decimal writtenOffMoney)
        {
            DateTime = DateTime.Now;
            CarId = carId;
            WrittenOffMoney = writtenOffMoney;
        }

        public DateTime DateTime { get; private set; }

        public int CarId { get; private set; }

        public decimal WrittenOffMoney { get; private set; }

        public override string ToString()
        {
            return String.Format($"Time: {DateTime}, " +
                                 $"CarId: {CarId}, " +
                                 $"WrittenOffMoney {WrittenOffMoney}" );
        }
    }
}
