﻿using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic.Dao
{
    public class BookingDAO
    {
        //-------------------------------------
        // Using Singleton Pattern
        private static BookingDAO instance = null;
        private static readonly object instanceLock = new object();
        private BookingDAO() { }
        public static BookingDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new BookingDAO();
                    }
                    return instance;
                }
            }
        }
        //-------------------------------------
        public IEnumerable<Booking> GetAllList()
        {
            List<Booking> bookings;
            try
            {
                var flightManagement = new FlightManagementDBContext();
                bookings = flightManagement.Bookings.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return bookings;
        }
        //-------------------------------------
        public Booking GetByID(int bookingId)
        {
            Booking booking = null;
            try
            {
                var flightManagement = new FlightManagementDBContext();
                booking = flightManagement.Bookings.SingleOrDefault(b => b.Id == bookingId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return booking;
        }

        //-------------------------------------
        public void Add(Booking booking)
        {
            try
            {
                Booking _booking = GetByID(booking.Id);
                if (_booking == null)
                {
                    var flightManagement = new FlightManagementDBContext();
                    flightManagement.Bookings.Add(booking);
                    flightManagement.SaveChanges();
                }
                else
                {
                    throw new Exception("The booking already exists.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //-------------------------------------
        public void Update(Booking booking)
        {
            try
            {
                var flightManagement = new FlightManagementDBContext();
                Booking existing = flightManagement.Bookings.FirstOrDefault(b => b.Id == booking.Id);
                if (existing != null)
                {
                    flightManagement.Entry(existing).CurrentValues.SetValues(booking);
                    flightManagement.SaveChanges();
                }
                else
                {
                    throw new Exception("The booking does not already exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating booking: {ex.Message}");
            }
        }

        //-------------------------------------
        public void Remove(Booking booking)
        {
            try
            {
                using (var flightManagement = new FlightManagementDBContext())
                {
                    var bookingToRemove = flightManagement.Bookings
                        .Include(b => b.Baggages)
                        .FirstOrDefault(b => b.Id == booking.Id);

                    if (bookingToRemove != null)
                    {
                        flightManagement.Baggages.RemoveRange(bookingToRemove.Baggages);

                        flightManagement.Bookings.Remove(bookingToRemove);
                        flightManagement.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("The booking does not already exist.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error removing booking: {ex.Message}");
            }
        }
        public IEnumerable<Booking> SearchByTime(DateTime searchTime)
        {
            List<Booking> bookings;
            try
            {
                using (var flightManagement = new FlightManagementDBContext())
                {
                    bookings = flightManagement.Bookings
                        .Where(x => x.BookingTime.HasValue && x.BookingTime.Value.Date == searchTime.Date)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return bookings;
        }

    }
}
