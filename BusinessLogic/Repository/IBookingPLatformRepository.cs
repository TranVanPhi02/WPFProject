using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public interface IBookingPLatformRepository
    {
        IEnumerable<BookingPlatform> GetAll();
        BookingPlatform GetBookingPlatformByID(int Id);
        void insert(BookingPlatform bookingPlatform);
        void update(BookingPlatform bookingPlatform);
        void delete(BookingPlatform bookingPlatform);
    /*    IEnumerable<BookingPlatform> SearchByName(string search);*/
    }
}
