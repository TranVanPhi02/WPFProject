using DataAccess.Models;
using BusinessLogic.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public class BaggageRepository : IBaggageRepository
    {
        public void delete(Baggage baggage)=>BaggageDAO.Instance.Remove(baggage);

        public IEnumerable<Baggage> GetAll() => BaggageDAO.Instance.GetAllList();

        public Baggage GetByID(int Id) => BaggageDAO.Instance.GetByID(Id);

        public void insert(Baggage baggage) => BaggageDAO.Instance.Add(baggage);

    

        public IEnumerable<Baggage> SearchByWeight(decimal weight) => BaggageDAO.Instance.SearchByWeight(weight);

        public void update(Baggage baggage) => BaggageDAO.Instance.Update(baggage);

        
    }
}
