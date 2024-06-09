using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;

namespace BusinessLogic.Repository
{
    public interface IAirlineRepository
    {
        IEnumerable<Airline> GetAirlines();
        Airline GetAirlineByID(int airlineId);
        void insertAirline(Airline airline);
        void updateAirline(Airline airline);
        void deleteAirline(Airline airline);
        IEnumerable<Airline> SearchByName(string search);
    }
}
