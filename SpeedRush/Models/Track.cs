using System;

namespace SpeedRush.Models
{
    /// <summary>
    /// Represents the race track.
    /// </summary>
    public class Track
    {
        /// <summary>
        /// The total number of laps for the race.
        /// </summary>
        public int TotalLaps { get; } = 5;
        /// <summary>
        /// The length of one lap (km).
        /// </summary>
        public double LapLength { get; } = 3.0;

        public Track() { }
    }
}