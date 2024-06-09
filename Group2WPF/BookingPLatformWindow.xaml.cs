﻿using BusinessLogic.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BusinessLogic.Repository;
using DataAccess.Models;
using BusinessLogic.Dao;

namespace Group2WPF
{
    /// <summary>
    /// Interaction logic for BookingPLatformWindow.xaml
    /// </summary>
    public partial class BookingPLatformWindow : Window
    {
        IBookingPLatformRepository bookingPlatformRepository;
        public string UserRole { get; set; }
        public BookingPLatformWindow(string role)
        {
            InitializeComponent();
            bookingPlatformRepository = new BookingPLatformRepository();
            UserRole = role;
            SetupRoleBasedUI();
            LoadList();
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
        private void LoadList()
        {
            IEnumerable<BookingPlatform> bookingPlatforms = bookingPlatformRepository.GetAll();
            DataGridBookingPlatform.ItemsSource = bookingPlatforms;
        }

        private BookingPlatform GetObject()
        {
            return new BookingPlatform
            {
                Id = int.TryParse(txtId.Text, out int id) ? id : 0,
                Name = txtName.Text,
                Url = txtUrl.Text
            };
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BookingPlatform bookingPlatform = GetObject();
                bookingPlatformRepository.insert(bookingPlatform);
                LoadList();
                MessageBox.Show($"{bookingPlatform.Name} added successfully", "Added");
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Add");
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BookingPlatform bookingPlatform = GetObject();
                bookingPlatformRepository.update(bookingPlatform);
                LoadList();
                MessageBox.Show($"{bookingPlatform.Name} updated successfully", "Updated");
                ClearForm();
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
                BookingPlatform bookingPlatform = GetObject();
                MessageBoxResult result = MessageBox.Show($"Do you want to delete {bookingPlatform.Id}?",
                    "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    bookingPlatformRepository.delete(bookingPlatform);
                    LoadList();
                    MessageBox.Show($"{bookingPlatform.Id} deleted successfully", "Delete");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Delete");
            }
        }
        private void ClearForm()
        {
            txtId.Text = string.Empty;
            txtName.Text = string.Empty;
            txtUrl.Text = string.Empty;
        }
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void OpenAirlineWindow_Click(object sender, RoutedEventArgs e)
        {
            AirlineWindow airlineWindow = new AirlineWindow(UserRole);
            airlineWindow.ShowDialog();
            this.Close();
        }

        private void OpenAirportWindow_Click(object sender, RoutedEventArgs e)
        {
            AirportWindow airportWindow = new AirportWindow(UserRole);
            airportWindow.ShowDialog();
            this.Close();
        }

        private void OpenPassengerWindow_Click(object sender, RoutedEventArgs e)
        {
            PassengerWindow passengerWindow = new PassengerWindow(UserRole);
            passengerWindow.ShowDialog();
            this.Close();
        }

        private void OpenBookingWindow_Click(object sender, RoutedEventArgs e)
        {
            BookingWindow bookingWindow = new BookingWindow(UserRole);
            bookingWindow.ShowDialog();
            this.Close();
        }

        private void OpenBookingPlatformWindow_Click(object sender, RoutedEventArgs e)
        {
            BookingPLatformWindow bookingPlatformWindow = new BookingPLatformWindow(UserRole);
            bookingPlatformWindow.ShowDialog();
            this.Close();
        }

        private void OpenFlightWindow_Click(object sender, RoutedEventArgs e)
        {
            FlightWindow flightWindow = new FlightWindow(UserRole);
            flightWindow.ShowDialog();
            this.Close();
        }

        private void OpenBaggageWindow_Click(object sender, RoutedEventArgs e)
        {
            BaggageWindow baggageWindow = new BaggageWindow(UserRole);
            baggageWindow.ShowDialog();
            this.Close();
        }
    }
}
