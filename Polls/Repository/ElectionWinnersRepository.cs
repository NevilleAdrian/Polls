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
    public class ElectionWinnersRepository
    {
        private readonly ApplicationDbContext _ctx;
        private readonly ILogger<ElectionWinnersRepository> _logger;
        public ElectionWinnersRepository(ApplicationDbContext context,
            ILogger<ElectionWinnersRepository> logger)
        {
            _ctx = context;
            _logger = logger;
        }

        public async Task AddElectionWinner(ElectionWinner winner)
        {
            try
            {
                _ctx.ElectionWinners.Update(winner);
                await _ctx.SaveChangesAsync();
                return;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"There was an error {ex.Message}");
            }
        }

        public async Task<ElectionWinner> GetElectionWinnerById(string id)
        {
            if (ElectionWinnerExists(id))
            {
                return await _ctx.ElectionWinners.Include(e => e.Election)
                                                .ThenInclude(c => c.Candidates)
                                                    .ThenInclude(u => u.User)
                                                .Include(e => e.Election)
                                                    .ThenInclude(c => c.Voters)
                                                        .ThenInclude(u => u.User)
                                             .Include(e => e.Candidate)
                                                .ThenInclude(c => c.User)
                                             .SingleOrDefaultAsync(e => e.ElectionWinnerId == id);

            }
            return null;
        }

        public async Task<List<ElectionWinner>> GetAllElectionWinners()
        {

            return await _ctx.ElectionWinners.Include(e => e.Election)
                                                .ThenInclude(c => c.Candidates)
                                                    .ThenInclude(u => u.User)
                                               .Include(e => e.Election)
                                                    .ThenInclude(c => c.Voters)
                                                        .ThenInclude(u => u.User)
                                             .Include(e => e.Candidate)
                                                .ThenInclude(c => c.User)
                                             .ToListAsync();
        }

        public async Task<bool> UpdateElectionWinnerById(ElectionWinner electionWinner)
        {
            if (ElectionWinnerExists(electionWinner.ElectionWinnerId))
            {
                try
                {

                    _ctx.ElectionWinners.Update(electionWinner);
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

        public async Task<bool> DeleteElectionWinner(string id)
        {
            if (ElectionWinnerExists(id))
            {
                try
                {
                    ElectionWinner electionWinner = await _ctx.ElectionWinners.FindAsync(id);
                    _ctx.ElectionWinners.Remove(electionWinner);
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

        public bool ElectionWinnerExists(string id) => _ctx.ElectionWinners.Any(e => e.ElectionWinnerId == id);
    }
}
