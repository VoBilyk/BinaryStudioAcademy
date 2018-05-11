using System;
using System.Collections.Generic;

namespace Stage2
{
    static class Settings
    {
        static public readonly int timeout = 3;
        static public readonly int parkingSpace = 5;
        static public readonly decimal fine = 1.5M;

        static public readonly Dictionary<CarType, decimal> parkingPrices = new Dictionary<CarType, decimal>()
        {
            {CarType.Motorcycle, 1},
            {CarType.Passenger, 3},
            {CarType.Truck, 5},
            {CarType.Bus, 2}
        };
    }
}
