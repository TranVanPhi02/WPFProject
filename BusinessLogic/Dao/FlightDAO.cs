using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic.Dao
{
    public class FlightDAO
    {
        //-------------------------------------
        // Using Singleton Pattern
        private static FlightDAO instance = null;
        private static readonly object instanceLock = new object();
        private FlightDAO() { }
        public static FlightDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new FlightDAO();
                    }
                    return instance;
                }
            }
        }
        //-------------------------------------
        public IEnumerable<Flight> GetAllList()
        {
            List<Flight> flights;
            try
            {
                var flightManagement = new FlightManagementDBContext();
                flights = flightManagement.Flights.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return flights;
        }
        //-------------------------------------
        public Flight GetByID(int flightId)
        {
            Flight flight = null;
            try
            {
                var flightManagement = new FlightManagementDBContext();
                flight = flightManagement.Flights.SingleOrDefault(f => f.Id == flightId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return flight;
        }

        //-------------------------------------
        public void Add(Flight flight)
        {
            try
            {
                Flight _flight = GetByID(flight.Id);
                if (_flight == null)
                {
                    var flightManagement = new FlightManagementDBContext();
                    flightManagement.Flights.Add(flight);
                    flightManagement.SaveChanges();
                }
                else
                {
                    throw new Exception("The flight already exists.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //-------------------------------------
        public void Update(Flight flight)
        {
            try
            {
                var flightManagement = new FlightManagementDBContext();
                Flight existing = flightManagement.Flights.FirstOrDefault(f => f.Id == flight.Id);
                if (existing != null)
                {
                    flightManagement.Entry(existing).CurrentValues.SetValues(flight);
                    flightManagement.SaveChanges();
                }
                else
                {
                    throw new Exception("The flight does not already exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating flight: {ex.Message}");
            }
        }

        //-------------------------------------
        public void Remove(Flight flight)
        {
            try
            {
                using (var flightManagement = new FlightManagementDBContext())
                {
                    var flightToRemove = flightManagement.Flights
                        .Include(f => f.Bookings)
                            .ThenInclude(b => b.Baggages)
                        .FirstOrDefault(f => f.Id == flight.Id);

                    if (flightToRemove != null)
                    {
                        // Remove related entities first
                        flightManagement.Baggages.RemoveRange(flightToRemove.Bookings.SelectMany(b => b.Baggages));
                        flightManagement.Bookings.RemoveRange(flightToRemove.Bookings);

                        // Remove the flight
                        flightManagement.Flights.Remove(flightToRemove);
                        flightManagement.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("The flight does not already exist.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error removing flight: {ex.Message}");
            }
        }
        //----------------------------------------
        public IEnumerable<Flight> SearchByName(string search)
        {
            List<Flight> flights;
            try
            {
                var flightManagement = new FlightManagementDBContext();
                flights = flightManagement.Flights.Where(x => x.ArrivingGate.Contains(search)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return flights;
        }
    }
}
