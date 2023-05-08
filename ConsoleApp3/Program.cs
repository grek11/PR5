using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
            int carCount = 5;
            Random random = new Random();
            Console.WriteLine("\tGO!!!");
            var fuelStation = new FuelStationSemaphore(2, 300);
            var tasks = new Task[carCount];
            for (int i = 0; i < carCount; i++)
            {
                var car = new Car("car" + (i + 1), fuelStation, 100, random.Next(1, 5));
                tasks[i] = new Task(car.Run);
            }
            tasks.ToList().ForEach(x => x.Start());
            Thread.Sleep(30000);
            fuelStation.Fill(2000);
            Task.WaitAll(tasks);
            Console.WriteLine("\tFINISH!!!");
        }

        abstract class AbstractFuelStation
        {
            public int Reserve { get; protected set; }
            public int Capacity { get; } = 1000;
            public int ColumnCout { get; }

            public AbstractFuelStation(int columnCout, int reserve = 1000)
            {
                if (columnCout <= 0) throw new Exception("Invalid column cout");
                if (reserve > Capacity || reserve < 0) throw new Exception("Invalid reserve");
                ColumnCout = columnCout;
                Reserve = reserve;
            }

            public abstract void Fill(int amount);
            public abstract bool TryRefuel(Car car, int volume);
        }

        class Car
        {
            private static Random random = new Random();
            public string Name { get; set; }
            public int TankVolume { get; }
            public AbstractFuelStation Station { get; }
            public int RefuelCount { get; }

            public Car(string name, AbstractFuelStation station, int tankVolume, int refuelCount)
            {
                Name = name;
                TankVolume = tankVolume;
                Station = station;
                RefuelCount = refuelCount;
            }
            public void Run()
            {
                Console.WriteLine($"{Name} STARTED; refuel count is {RefuelCount}");
                for (int i = 0; i < RefuelCount; i++)
                {
                    var volume = random.Next(TankVolume) + 1;
                    Console.WriteLine($"{Name} tries to get {volume} lit. for {i + 1} times");
                    while (!Station.TryRefuel(this, volume))
                    {
                        Console.WriteLine($"{Name} is waiting");
                        Thread.Sleep(random.Next(1, 5) * 1000);
                    }
                    Thread.Sleep(random.Next(1, 3) * 1000);
                    Console.WriteLine($"{Name} left the station for {i + 1} times");
                }
                Console.WriteLine($"{Name} SAID GOOD-BYE");
            }
        }
    
        class FuelStationSemaphore : AbstractFuelStation
        {
            private Mutex mutex = new Mutex();
            private SemaphoreSlim semaphore;

            public FuelStationSemaphore(int columnCout, int reserve = 1000) : base(columnCout, reserve)
            {
                semaphore = new SemaphoreSlim(ColumnCout, ColumnCout);
            }
            public override void Fill(int amount)
            {
                mutex.WaitOne();
                if (amount + Reserve > Capacity)
                    Reserve = Capacity;
                else
                    Reserve += Capacity;
                Console.WriteLine("STATION TANK REFUELED");
                mutex.ReleaseMutex();
            }

            public override bool TryRefuel(Car car, int volume)
            {
                semaphore.Wait();
                mutex.WaitOne();
                bool result = false;
                if (volume <= Reserve)
                {
                    Console.WriteLine($"Fuels reserve is {Reserve}. {volume} liters fueling began for {car.Name}");
                    Reserve -= volume;
                    mutex.ReleaseMutex();
                    Thread.Sleep(100 * volume);
                    Console.WriteLine($"{car.Name} fueling finished");
                }
                else
                {
                    Console.WriteLine($"Fuel reserve {Reserve} is insufficient for {car.Name}");
                    mutex.ReleaseMutex();
                }
                semaphore.Release();
                return result;
            }

        }
    }
}
