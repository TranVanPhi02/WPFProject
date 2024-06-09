using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public interface IPassengerRepository
    {
        IEnumerable<Passenger> GetAll();
        Passenger GetByID(int Id);
        void insert(Passenger passenger);
        void update(Passenger passenger);
        void delete(Passenger passenger);
    }
}
