using System;
using System.Collections.Generic;

namespace SpeedRush.Models
{
    /// <summary>
    /// Manages race state, player actions, and game progression.
    /// </summary>
    public class RaceManager
    {
        public Car SelectedCar { get; private set; }
        public Track RaceTrack { get; private set; }
        public int CurrentLap { get; private set; }
        public double TimeElapsed { get; private set; }
        public double TimeLimit { get; private set; }
        public double LapProgress { get; private set; } // 0 to 100%
        public bool IsRaceOver { get; private set; }
        public Queue<string> RaceLog { get; private set; }

        public RaceManager(Car car, Track track, double timeLimit = 120.0)
        {
            SelectedCar = car;
            RaceTrack = track;
            TimeLimit = timeLimit;
            CurrentLap = 1;
            LapProgress = 0;
            TimeElapsed = 0;
            IsRaceOver = false;
            RaceLog = new Queue<string>();
        }

        /// <summary>
        /// Processes the player's action for the turn.
        /// </summary>
        /// <param name="action">The action chosen by the player.</param>
        public void ProcessTurn(PlayerAction action)
        {
            if (IsRaceOver)
                throw new InvalidOperationException("Race is already over.");

            double speed;
            double fuelRateMultiplier;
            double timeTaken;
            string status = "";

            switch (action)
            {
                case PlayerAction.SpeedUp:
                    speed = SelectedCar.MaxSpeed;
                    fuelRateMultiplier = 1.2;
                    timeTaken = 5;
                    status = "Speeding up!";
                    break;
                case PlayerAction.MaintainSpeed:
                    speed = SelectedCar.MaxSpeed * 0.7;
                    fuelRateMultiplier = 1.0;
                    timeTaken = 6;
                    status = "Maintaining speed.";
                    break;
                case PlayerAction.PitStop:
                    speed = 0;
                    fuelRateMultiplier = 0;
                    timeTaken = 8;
                    SelectedCar.Refuel(SelectedCar.FuelCapacity - SelectedCar.CurrentFuel);
                    status = "Pit stop. Refueled.";
                    break;
                default:
                    throw new ArgumentException("Invalid player action.");
            }

            if (action != PlayerAction.PitStop)
                SelectedCar.ConsumeFuel(fuelRateMultiplier);

            LapProgress += speed * timeTaken / (RaceTrack.LapLength * 100); // Simulate progress as %
            TimeElapsed += timeTaken;

            if (LapProgress >= 1.0)
            {
                LapProgress = 0;
                CurrentLap++;
                RaceLog.Enqueue($"Lap {CurrentLap - 1} completed.");
            }

            if (CurrentLap > RaceTrack.TotalLaps || SelectedCar.CurrentFuel <= 0 || TimeElapsed >= TimeLimit)
            {
                IsRaceOver = true;
                RaceLog.Enqueue("Race finished!");
            }
            else
            {
                RaceLog.Enqueue(status);
            }
        }
    }
}