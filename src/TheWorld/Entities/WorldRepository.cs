using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheWorld.Entities
{
    public class WorldRepository : IWorldRepository
    {
        #region Fields
        private WorldContext context;
        private ILogger<WorldRepository> logger;

        public WorldRepository(WorldContext context, ILogger<WorldRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }
        #endregion

        #region Methods

        public void AddStop(string tripName, Stop newStop, string userName)
        {
            var trip = GetTripsByUserName(tripName, userName);   
            if (trip != null)
            {
                trip.Stops.Add(newStop);
            }
        }

        public void AddTrip(Trip trip)
        {
            context.Add(trip);
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            logger.LogInformation("Getting all trips form the Database");
            return this.context.Trips.ToList();
        }

        public IEnumerable<Trip> GetTripsByUserName(string userName)
        {
            return this.context.Trips.Include(t => t.Stops)
                               .Where(t => t.UserName == userName).ToList();
        }

        public Trip GetTripByName(string tripName)
        {
            return context.Trips.Include(t => t.Stops).Where(t => t.Name == tripName).FirstOrDefault();
        }

        public Trip GetTripsByUserName(string tripName, string userName)
        {
            return this.context.Trips.Include(t => t.Stops)
                                     .Where(t => t.Name == tripName && t.UserName == userName)
                                     .FirstOrDefault();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await this.context.SaveChangesAsync()) > 0;
        }
        #endregion
    }
}
