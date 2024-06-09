using System;
using System.Collections.Generic;
using System.Windows;
using DataAccess.Models;
using BusinessLogic.Repository;
using System.Data;

namespace Group2WPF
{
    public partial class PassengerWindow : Window
    {
        private readonly IPassengerRepository passengerRepository;
       
        public string UserRole { get; set; }
        public PassengerWindow(string role)
        {
            InitializeComponent();
            passengerRepository = new PassengerRepository();
            UserRole = role;
            LoadList();
            SetupRoleBasedUI();
        }

        private void LoadList()
        {
            IEnumerable<Passenger> passengers = passengerRepository.GetAll();
            DataGridPassenger.ItemsSource = passengers;
        }

        private void SetupRoleBasedUI()
        {
            btnOpenAirlineWindow.Visibility = Visibility.Collapsed;
            btnOpenAirportWindow.Visibility = Visibility.Collapsed;
            btnOpenBookingWindow.Visibility = Visibility.Collapsed;
            btnOpenBookingPlatformWindow.Visibility = Visibility.Collapsed;
            btnOpenFlightWindow.Visibility = Visibility.Collapsed;
            btnOpenBaggageWindow.Visibility = Visibility.Collapsed;
            btnOpenPassengerWindow.Visibility = Visibility.Collapsed;

            if (UserRole == "Admin")
            {
                btnOpenAirlineWindow.Visibility = Visibility.Visible;
                btnOpenAirportWindow.Visibility = Visibility.Visible;
                btnOpenBookingWindow.Visibility = Visibility.Visible;
                btnOpenBookingPlatformWindow.Visibility = Visibility.Visible;
                btnOpenFlightWindow.Visibility = Visibility.Visible;
                btnOpenBaggageWindow.Visibility = Visibility.Visible;
                btnOpenPassengerWindow.Visibility = Visibility.Visible;
            }
            else if (UserRole == "Staff")
            {
                btnOpenPassengerWindow.Visibility = Visibility.Visible;
                btnOpenBookingWindow.Visibility = Visibility.Visible;
                btnOpenBaggageWindow.Visibility = Visibility.Visible;
            }

        }

        private Passenger GetObject()
        {
            Passenger passenger = null;

            try
            {
                passenger = new Passenger
                {
                    Id = int.Parse(txtId.Text),
                    FirstName = txtFirstName.Text,
                    LastName = txtLastName.Text,
                    DateOfBirth = Convert.ToDateTime(txtDateOfBirth.Text),
                    Country = txtCountry.Text,
                    Email = txtEmail.Text,
                    Gender = txtGender.Text
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Get Passenger");
            }
            return passenger;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Passenger passenger = GetObject();
                passengerRepository.insert(passenger);
                LoadList();
                MessageBox.Show($"{passenger.Id} inserted successfully ", "Insert");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Insert");
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Passenger passenger = GetObject();
                passengerRepository.update(passenger);
                LoadList();
                MessageBox.Show($"{passenger.Id} updated successfully ", "Updated");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Update");
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Passenger passenger = GetObject();
                MessageBoxResult result = MessageBox.Show($"Do you want to delete {passenger.Id}?",
                    "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    passengerRepository.delete(passenger);
                    LoadList();
                    MessageBox.Show($"{passenger.Id} deleted successfully", "Delete");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Delete");
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            txtId.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtDateOfBirth.Text = "";
            txtCountry.Text = "";
            txtEmail.Text = "";
            txtGender.Text = "";
        }

        private void OpenAirlineWindow_Click(object sender, RoutedEventArgs e)
        {
            AirlineWindow airlineWindow = new AirlineWindow(UserRole);
            airlineWindow.ShowDialog();
        }

        private void OpenAirportWindow_Click(object sender, RoutedEventArgs e)
        {
            AirportWindow airportWindow = new AirportWindow(UserRole);
            airportWindow.ShowDialog();
        }

        private void OpenPassengerWindow_Click(object sender, RoutedEventArgs e)
        {
            PassengerWindow passengerWindow = new PassengerWindow(UserRole);
            passengerWindow.ShowDialog();
        }

        private void OpenBookingWindow_Click(object sender, RoutedEventArgs e)
        {
            BookingWindow bookingWindow = new BookingWindow(UserRole);
            bookingWindow.ShowDialog();
        }

        private void OpenBookingPlatformWindow_Click(object sender, RoutedEventArgs e)
        {
            BookingPLatformWindow bookingPlatformWindow = new BookingPLatformWindow(UserRole);
            bookingPlatformWindow.ShowDialog();
        }

        private void OpenFlightWindow_Click(object sender, RoutedEventArgs e)
        {
            FlightWindow flightWindow = new FlightWindow(UserRole);
            flightWindow.ShowDialog();
        }

        private void OpenBaggageWindow_Click(object sender, RoutedEventArgs e)
        {
            BaggageWindow baggageWindow = new BaggageWindow(UserRole);
            baggageWindow.ShowDialog();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // Search Functionality
        }
    }
}
