﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Dao
{
    public class BookingPlatformDAO
    {
        //-------------------------------------
        //Using Singleton Pattern
        private static BookingPlatformDAO instance = null;
        private static readonly object instanceLock = new object();
        private BookingPlatformDAO() { }
        public static BookingPlatformDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new BookingPlatformDAO();
                    }
                    return instance;
                }
            }
        }
        //-------------------------------------
        public IEnumerable<BookingPlatform> GetBookingPlatformList()
        {
            List<BookingPlatform> bookingPlatforms;
            try
            {
                var flightManagement = new FlightManagementDBContext();
                bookingPlatforms = flightManagement.BookingPlatforms.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return bookingPlatforms;
        }
        //-------------------------------------
        public BookingPlatform GetBookingPlatformByID(int bookingPlatformId)
        {
            BookingPlatform bookingPlatform = null;
            try
            {
                var flightManagement = new FlightManagementDBContext();
                bookingPlatform = flightManagement.BookingPlatforms.SingleOrDefault(a => a.Id == bookingPlatformId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return bookingPlatform;
        }

        //-------------------------------------
        public void Add(BookingPlatform bookingPlatform)
        {
            try
            {
                BookingPlatform _bookingPlatform = GetBookingPlatformByID(bookingPlatform.Id);
                if (_bookingPlatform == null)
                {
                    var flightManagement = new FlightManagementDBContext();
                    flightManagement.BookingPlatforms.Add(bookingPlatform);
                    flightManagement.SaveChanges();
                }
                else
                {
                    throw new Exception("The bookingPlatform is already exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //-------------------------------------
        public void Update(BookingPlatform bookingPlatform)
        {
            try
            {
                var flightManagement = new FlightManagementDBContext();
                BookingPlatform existing = flightManagement.BookingPlatforms.FirstOrDefault(a => a.Id == bookingPlatform.Id);
                if (existing != null)
                {
                    flightManagement.Entry(existing).CurrentValues.SetValues(bookingPlatform);
                    flightManagement.SaveChanges();
                }
                else
                {
                    throw new Exception("The bookingPlatform does not already exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating bookingPlatform: {ex.Message}");
            }
        }

        //-------------------------------------
        public void Remove(BookingPlatform bookingPlatform)
        {
            try
            {
                using (var flightManagement = new FlightManagementDBContext())
                {
                    // Load the bookingPlatform with related entities
                    var bookingPlatformToRemove = flightManagement.BookingPlatforms
                        .Include(a => a.Bookings)
                           
                                .ThenInclude(b => b.Baggages)
                        .FirstOrDefault(a => a.Id == bookingPlatform.Id);

                    if (bookingPlatformToRemove != null)
                    {
                        // Remove related entities first
                        flightManagement.Baggages.RemoveRange(
                            bookingPlatformToRemove.Bookings.SelectMany(b => b.Baggages));
                      
                        flightManagement.Bookings.RemoveRange(bookingPlatformToRemove.Bookings);

                        // Remove the bookingPlatform
                        flightManagement.BookingPlatforms.Remove(bookingPlatformToRemove);
                        flightManagement.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("The bookingPlatform does not already exist.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error removing bookingPlatform: {ex.Message}");
            }
        }

    }
}
