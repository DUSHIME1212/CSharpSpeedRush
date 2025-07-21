using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpeedRush.Models;
using System;

namespace SpeedRush.Tests
{
    [TestClass]
    public class RaceManagerTests
    {
        [TestMethod]
        public void TestRaceManagerInitialization()
        {
            var car = new Car("TestCar", 200, 0.1, 50);
            var track = new Track();
            var manager = new RaceManager(car, track);

            Assert.AreEqual(car, manager.SelectedCar);
            Assert.AreEqual(track, manager.RaceTrack);
            Assert.AreEqual(1, manager.CurrentLap);
            Assert.IsFalse(manager.IsRaceOver);
        }

        [TestMethod]
        public void TestProcessTurn_SpeedUp_ConsumesFuelAndAdvances()
        {
            var car = new Car("TestCar", 200, 0.1, 50);
            var track = new Track();
            var manager = new RaceManager(car, track);
            double initialFuel = car.CurrentFuel;

            manager.ProcessTurn(PlayerAction.SpeedUp);

            Assert.IsTrue(car.CurrentFuel < initialFuel);
            Assert.IsFalse(manager.IsRaceOver);
        }

        [TestMethod]
        public void TestProcessTurn_PitStop_RefuelsCar()
        {
            var car = new Car("TestCar", 200, 0.1, 50);
            car.CurrentFuel = 10;
            var track = new Track();
            var manager = new RaceManager(car, track);

            manager.ProcessTurn(PlayerAction.PitStop);

            Assert.AreEqual(car.FuelCapacity, car.CurrentFuel);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestProcessTurn_WhenRaceOver_ThrowsException()
        {
            var car = new Car("TestCar", 200, 0.1, 50);
            var track = new Track();
            var manager = new RaceManager(car, track);

            // End the race by advancing time beyond the limit
            manager.AdvanceTime(200.0);
            manager.ProcessTurn(PlayerAction.MaintainSpeed);
        }
    }
}