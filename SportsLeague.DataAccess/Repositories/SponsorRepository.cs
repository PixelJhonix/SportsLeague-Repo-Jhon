using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace SportsLeague.DataAccess.Repositories
{
    public class SponsorRepository : GenericRepository<Sponsor>, ISponsorRepository
    {
        public SponsorRepository(LeagueDbContext context) : base(context)
        {
        }

        public async Task<Sponsor?> GetByNameAsync(string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(s => s.Name.ToLower() == name.ToLower());
        }
    }
}
