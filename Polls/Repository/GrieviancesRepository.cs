using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polls.Data;
using Polls.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Polls.Repository
{
    public class GrieviancesRepository
    {
        private readonly ApplicationDbContext _ctx;
        private ILogger<GrieviancesRepository> _logger;
        public GrieviancesRepository(ApplicationDbContext context,
            ILogger<GrieviancesRepository> logger)
        {
            _ctx = context;
            _logger = logger;
        }

        public async Task AddGrieviance(Grieviance grieviance)
        {
            try
            {
                await _ctx.Grieviances.AddAsync(grieviance);
                await _ctx.SaveChangesAsync();
                return;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"There was an error {ex.Message}");
            }
        }

        public async Task<Grieviance> GetGrievianceById(int id, bool includeGrievianceReplies = false)
        {
            if (GrievianceExists(id))
            {
                if (includeGrievianceReplies)
                    return await _ctx.Grieviances.Include(g => g.GrievianceReplies).Include(g => g.User).SingleOrDefaultAsync(g => g.GrievianceId == id);
                return await _ctx.Grieviances.Include(g => g.User).SingleOrDefaultAsync(g => g.GrievianceId == id);
            }
            return null;
        }

        public async Task<List<Grieviance>> GetAllGrieviances(bool includeGrievianceReplies = false)
        {
            if (includeGrievianceReplies)
                return await _ctx.Grieviances.Include(g => g.GrievianceReplies).Include(g => g.User).ToListAsync();

            return await _ctx.Grieviances.Include(g => g.User).ToListAsync();
        }

        public async Task<bool> UpdateGrevianceById(Grieviance grieviance)
        {
            if (GrievianceExists(grieviance.GrievianceId))
            {
                try
                {

                    _ctx.Grieviances.Update(grieviance);
                    await _ctx.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"There was an error {ex.Message}");
                    return false;
                }
            }
            return false;
        }

        public async Task<bool> DeleteGreviance(int id)
        {
            if(GrievianceExists(id))
            {
                try
                {
                    Grieviance grieviance = await _ctx.Grieviances.FindAsync(id);
                    _ctx.Grieviances.Remove(grieviance);
                    await _ctx.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"There was an error {ex.Message}");
                    return false;
                }
            }
            return false;
        }

        public bool GrievianceExists(int id) => _ctx.Grieviances.Any(g => g.GrievianceId == id);
    }
}
