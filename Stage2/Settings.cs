using System;
using System.Collections.Generic;

namespace Stage2
{
    static class Settings
    {
        static Dictionary<CarType, decimal> parkingPrices = new Dictionary<CarType, decimal>()
        {
            {CarType.Motorcycle, 1},
            {CarType.Passenger, 3},
            {CarType.Truck, 5},
            {CarType.Bus, 2}
        };

        static public int Timeout { get; private set; }

        static public int ParkingSpace { get; private set; }

        static public int Fine { get; private set; }

    }
}
