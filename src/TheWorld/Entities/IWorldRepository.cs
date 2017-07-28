using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheWorld.Entities
{
    public interface IWorldRepository
    {
        void AddStop(string tripName, Stop newStop, string userName);
        void AddTrip(Trip trip);

        IEnumerable<Trip> GetAllTrips();
        IEnumerable<Trip> GetTripsByUserName(string userName);

        Trip GetTripByName(string tripName);
        Trip GetTripsByUserName(string tripName, string userName);

        Task<bool> SaveChangesAsync();
    }
}