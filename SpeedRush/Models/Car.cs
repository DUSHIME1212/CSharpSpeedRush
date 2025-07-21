using System;

namespace SpeedRush.Models
{
    
    
    
    public class Car
    {
        
        
        
        public string Name { get; set; }
        
        
        
        public int MaxSpeed { get; set; }
        
        
        
        public double FuelConsumptionRate { get; set; }
        
        
        
        public double FuelCapacity { get; set; }
        
        
        
        public double CurrentFuel { get; set; }

        public Car(string name, int maxSpeed, double fuelConsumptionRate, double fuelCapacity)
        {
            Name = name;
            MaxSpeed = maxSpeed;
            FuelConsumptionRate = fuelConsumptionRate;
            FuelCapacity = fuelCapacity;
            CurrentFuel = fuelCapacity;
        }

        
        
        
        
        public void Refuel(double amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Refuel amount must be positive.");
            if (CurrentFuel + amount > FuelCapacity)
                throw new InvalidOperationException("Cannot overfill fuel tank.");
            CurrentFuel += amount;
        }

        
        
        
        
        public void ConsumeFuel(double rateMultiplier = 1.0)
        {
            double consumption = FuelConsumptionRate * rateMultiplier;
            if (CurrentFuel < consumption)
                throw new InvalidOperationException("Not enough fuel to continue.");
            CurrentFuel -= consumption;
        }
    }
}