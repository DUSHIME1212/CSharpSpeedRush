using System;

namespace SpeedRush.Models
{
    /// <summary>
    /// Represents a car in the game.
    /// </summary>
    public class Car
    {
        /// <summary>
        /// The name of the car.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Maximum speed of the car.
        /// </summary>
        public int MaxSpeed { get; set; }
        /// <summary>
        /// Fuel consumed per turn at max speed.
        /// </summary>
        public double FuelConsumptionRate { get; set; }
        /// <summary>
        /// Maximum fuel capacity of the car.
        /// </summary>
        public double FuelCapacity { get; set; }
        /// <summary>
        /// Current fuel level.
        /// </summary>
        public double CurrentFuel { get; set; }

        public Car(string name, int maxSpeed, double fuelConsumptionRate, double fuelCapacity)
        {
            Name = name;
            MaxSpeed = maxSpeed;
            FuelConsumptionRate = fuelConsumptionRate;
            FuelCapacity = fuelCapacity;
            CurrentFuel = fuelCapacity;
        }

        /// <summary>
        /// Refuels the car by the given amount. Throws exception if overfilled.
        /// </summary>
        /// <param name="amount">Amount of fuel to add.</param>
        public void Refuel(double amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Refuel amount must be positive.");
            if (CurrentFuel + amount > FuelCapacity)
                throw new InvalidOperationException("Cannot overfill fuel tank.");
            CurrentFuel += amount;
        }

        /// <summary>
        /// Consumes fuel for a turn. Throws exception if not enough fuel.
        /// </summary>
        /// <param name="rateMultiplier">Factor to adjust consumption based on action.</param>
        public void ConsumeFuel(double rateMultiplier = 1.0)
        {
            double consumption = FuelConsumptionRate * rateMultiplier;
            if (CurrentFuel < consumption)
                throw new InvalidOperationException("Not enough fuel to continue.");
            CurrentFuel -= consumption;
        }
    }
}