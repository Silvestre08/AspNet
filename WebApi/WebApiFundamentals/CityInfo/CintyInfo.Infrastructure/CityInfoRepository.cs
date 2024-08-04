using CityInfo.Domain.Entities;
using CityInfo.Domain.Services;
using CityInfo.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.Infrastructure
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CityInfoContext _context;

        public CityInfoRepository(CityInfoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await _context.Cities.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest)
        {
            if (includePointsOfInterest)
            {
                return await _context.Cities.Include(c => c.PointsOfInterest)
                    .Where(c => c.Id == cityId).FirstOrDefaultAsync();
            }

            return await _context.Cities
                  .Where(c => c.Id == cityId).FirstOrDefaultAsync();
        }

        public async Task<bool> CityExistsAsync(int cityId)
        {
            return await _context.Cities.AnyAsync(c => c.Id == cityId);
        }

        public async Task<PointOfInterest?> GetPointOfInterestForCityAsync(
            int cityId, 
            int pointOfInterestId)
        {
            return await _context.PointsOfInterest
               .Where(p => p.CityId == cityId && p.Id == pointOfInterestId)
               .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(
            int cityId)
        {
            return await _context.PointsOfInterest
                           .Where(p => p.CityId == cityId).ToListAsync();
        }

        public async Task AddPointOfInterestForCityAsync(int cityId, 
            PointOfInterest pointOfInterest)
        {
            var city = await GetCityAsync(cityId, false);
            if (city != null)
            {
                city.PointsOfInterest.Add(pointOfInterest);
            }
        }

        public void DeletePointOfInterest(PointOfInterest pointOfInterest)
        {
            _context.PointsOfInterest.Remove(pointOfInterest);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<(IEnumerable<City> Citites, PaginationMetadata PaginationMetadata)> GetCitiesAsync(string? name, string? searchQuery, int pagesize, int pageNumber)
        {
            var collection = _context.Cities.AsQueryable();
            if (!string.IsNullOrWhiteSpace(name)) 
            {
                name = name.Trim();
                collection = collection.Where(c => c.Name ==  name);
            }

            if (!string.IsNullOrWhiteSpace(searchQuery)) 
            {
                searchQuery = searchQuery.Trim();
                collection = collection.Where(c=> c.Name.Contains(searchQuery) || (c.Description!=null && c.Name.Contains(searchQuery)));
            }
            var itemCount = collection.Count();
            var paginationMetadate = new PaginationMetadata(itemCount, pagesize, pageNumber);
            var result = await collection.OrderBy(c => c.Name).Skip(pagesize * (pageNumber - 1)).Take(pagesize).ToListAsync();
            return (result, paginationMetadate);
                //return await _context.Cities.Where(c=> c.Name == name).OrderBy(c => c.Name).ToListAsync();
        }
    }
}
