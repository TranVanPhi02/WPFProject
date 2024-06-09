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
using DataAccess.Models;

using BusinessLogic.Repository;
using System.Data;

namespace Group2WPF
{
    /// <summary>
    /// Interaction logic for BookingWindow.xaml
    /// </summary>
    public partial class BookingWindow : Window
    {
        private readonly IBookingRepository bookingRepository;
        public string UserRole { get; set; }
        public BookingWindow(string role)
        {
            InitializeComponent();
            bookingRepository = new BookingRepository();
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
        public void LoadList()
        {
            var bookings = bookingRepository.GetAll();
            DataGridBooking.ItemsSource = bookings;
        }

        private Booking GetObject()
        {
            return new Booking
            {
                Id = int.Parse(txtId.Text),
                PassengerId = int.Parse(txtPassengerId.Text),
                FlightId = int.Parse(txtFlightId.Text),
                BookingPlatformId = int.Parse(txtBookingPlatformId.Text),
                BookingTime = DateTime.Parse(txtBookingTime.Text)
            };
           
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var booking = GetObject();
                bookingRepository.insert(booking);
                LoadList();
                MessageBox.Show($"{booking.Id} inserted successfully ", "Insert");
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
                var booking = GetObject();
                bookingRepository.update(booking);
                LoadList();
                MessageBox.Show($"{booking.Id} updated successfully ", "Updated");
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
                var booking = GetObject();
                MessageBoxResult result = MessageBox.Show($"Do you want to delete {booking.Id}?",
                    "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    bookingRepository.delete(booking);
                    LoadList();
                    MessageBox.Show($"{booking.Id} deleted successfully", "Delete");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Delete");
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            txtId.Text = "";
            txtPassengerId.Text = "";
            txtFlightId.Text = "";
            txtBookingPlatformId.Text = "";
            txtBookingTime.Text = "";
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


        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = txtSearch.Text.Trim();
            if (!string.IsNullOrEmpty(searchText) && DateTime.TryParse(searchText, out DateTime searchTime))
            {
                try
                {
                    IEnumerable<Booking> searchResult = bookingRepository.SearchByTime(searchTime);

                    DataGridBooking.ItemsSource = searchResult;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while searching: {ex.Message}");
                }
            }
            else
            {
                LoadList();
            }
        }

    }
}
