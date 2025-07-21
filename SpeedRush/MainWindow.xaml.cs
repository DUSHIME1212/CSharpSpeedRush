using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using SpeedRush.Models;

namespace SpeedRush
{
    public partial class MainWindow : Window
    {
        private List<Car> cars;
        private Track track;
        private RaceManager raceManager;

        public MainWindow()
        {
            InitializeComponent();

            // Initialize cars list with sample cars
            cars = new List<Car>
            {
                new Car("Speedster", 200, 0.1, 50),
                new Car("Thunderbolt", 180, 0.08, 60),
                new Car("Lightning", 220, 0.12, 45)
            };

            // Initialize track
            track = new Track();

            // Initialize race manager with the first car and track
            raceManager = new RaceManager(cars[0], track);

            // Populate CarSelector ComboBox
            CarSelector.ItemsSource = cars;
            CarSelector.DisplayMemberPath = "Name";
            CarSelector.SelectedIndex = 0;

            // Setup timer for continuous time progression
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

            UpdateUI();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!raceManager.IsRaceOver)
            {
                raceManager.AdvanceTime(0.01); // advance time by 0.01 seconds (10 ms)
                UpdateUI();
            }
        }

        private void CarSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CarSelector.SelectedItem is Car selectedCar)
            {
                raceManager = new RaceManager(selectedCar, track);
                RaceLogBox.Items.Clear();
                UpdateUI();
                EnableButtons(true);
            }
        }

        private void BtnSpeedUp_Click(object sender, RoutedEventArgs e)
        {
            ProcessPlayerAction(PlayerAction.SpeedUp);
        }

        private void BtnMaintain_Click(object sender, RoutedEventArgs e)
        {
            ProcessPlayerAction(PlayerAction.MaintainSpeed);
        }

        private void BtnPitStop_Click(object sender, RoutedEventArgs e)
        {
            ProcessPlayerAction(PlayerAction.PitStop);
        }

        private void ProcessPlayerAction(PlayerAction action)
        {
            try
            {
                raceManager.ProcessTurn(action);
                UpdateUI();

                if (raceManager.IsRaceOver)
                {
                    EnableButtons(false);
                    RaceLogBox.Items.Add("Race is over!");
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateUI()
        {
            LapDisplay.Text = raceManager.CurrentLap.ToString();
            FuelBar.Maximum = raceManager.SelectedCar.FuelCapacity;
            FuelBar.Value = raceManager.SelectedCar.CurrentFuel;
            FuelDisplay.Text = $"{raceManager.SelectedCar.CurrentFuel:F1} / {raceManager.SelectedCar.FuelCapacity}";

            TimeBar.Maximum = raceManager.TimeLimit;

            // Animate TimeBar.Value smoothly
            var animation = new System.Windows.Media.Animation.DoubleAnimation
            {
                To = raceManager.TimeElapsed,
                Duration = TimeSpan.FromMilliseconds(10),
                FillBehavior = System.Windows.Media.Animation.FillBehavior.HoldEnd
            };
            TimeBar.BeginAnimation(System.Windows.Controls.Primitives.RangeBase.ValueProperty, animation);

            double timeLeft = raceManager.TimeLimit - raceManager.TimeElapsed;
            if (timeLeft < 0) timeLeft = 0;
            TimeDisplay.Text = $"Time Left: {timeLeft:F1} s";

            SpeedDisplay.Text = $"{(raceManager.IsRaceOver ? 0 : (int)(raceManager.SelectedCar.MaxSpeed * (raceManager.LapProgress > 0 ? 1 : 0)))} km/h";

            ProgressIndicator.Text = $"Progress: {(raceManager.LapProgress * 100):F1}%";

            RaceLogBox.Items.Clear();
            foreach (var log in raceManager.RaceLog)
            {
                RaceLogBox.Items.Add(log);
            }
        }

        private void EnableButtons(bool enable)
        {
            BtnSpeedUp.IsEnabled = enable;
            BtnMaintain.IsEnabled = enable;
            BtnPitStop.IsEnabled = enable;
        }
    }
}
