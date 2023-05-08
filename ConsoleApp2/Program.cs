using System;
using System.Collections.Generic;

class Passenger
{
    public int CurrentFloor { get; set; }
    public int DestinationFloor { get; set; }
}

class Elevator
{
    private int currentFloor;
    private List<Passenger> passengers;
    private int maxPassengers;

    public Elevator(int maxPassengers)
    {
        this.currentFloor = 1;
        this.passengers = new List<Passenger>();
        this.maxPassengers = maxPassengers;
    }

    public void Call(int destinationFloor)
    {
        Passenger passenger = new Passenger { CurrentFloor = this.currentFloor, DestinationFloor = destinationFloor };
        if (this.passengers.Count < this.maxPassengers)
        {
            this.passengers.Add(passenger);
            Console.WriteLine($"Passenger enters elevator at floor {this.currentFloor} going to floor {destinationFloor}.");
        }
        else
        {
            Console.WriteLine($"Elevator is full. Passenger at floor {this.currentFloor} could not enter.");
        }
    }

    public void MoveTo(int destinationFloor)
    {
        Console.WriteLine($"Elevator moves from floor {this.currentFloor} to floor {destinationFloor}.");
        this.currentFloor = destinationFloor;
    }

    public void Disembark()
    {
        for (int i = this.passengers.Count - 1; i >= 0; i--)
        {
            if (this.passengers[i].DestinationFloor == this.currentFloor)
            {
                Console.WriteLine($"Passenger exits elevator at floor {this.currentFloor}.");
                this.passengers.RemoveAt(i);
            }
        }
    }
}

class Program
{
    static void Main()
    {
        Elevator elevator = new Elevator(4);
        elevator.Call(3);
        elevator.Call(2);
        elevator.MoveTo(2);
        elevator.Disembark();
        elevator.Call(4);
        elevator.MoveTo(4);
        elevator.Disembark();
        elevator.Call(1);
        elevator.MoveTo(1);
        elevator.Disembark();
    }
}
