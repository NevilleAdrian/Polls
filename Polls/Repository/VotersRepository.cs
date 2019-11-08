using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polls.Data;
using Polls.Models;
using Polls.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Polls.Repository
{
    public class VotersRepository
    {
        private readonly ApplicationDbContext _ctx;
        private ILogger<VotersRepository> _logger;
        public VotersRepository(ApplicationDbContext context,
            ILogger<VotersRepository> logger)
        {
            _ctx = context;
            _logger = logger;
        }

        public async Task AddVoter(Voter voter)
        {
            voter.HasVoted = true;
            Candidate choice = await _ctx.Candidates.FindAsync(voter.CandidateId);
            choice.TotalNumberOfVotes += 1;
            Voter intendedVotee = null;
            if (voter.VoterId == null)
            {
                _ctx.Voters.Add(voter);
            }
            else
            {
                intendedVotee = await _ctx.Voters.FindAsync(voter.VoterId);
                
            }
            if(intendedVotee != null)
            {
                if (intendedVotee.WillVoteAgainBecauseOfTie)
                {
                    intendedVotee.WillVoteAgainBecauseOfTie = false;
                }
                _ctx.Voters.Update(intendedVotee);
            }
            

            try
            {
                
                _ctx.Candidates.Update(choice);
                await _ctx.SaveChangesAsync();
                return;
            }
            catch(Exception ex)
            {
                _logger.LogInformation($"There was an error {ex.Message}");
            }
        }

        public async Task<Voter> GetVoterById(string id)
        {
            if(VoterExist(id))
            {
                return await _ctx.Voters.Include(v => v.Election).Include(v => v.User).SingleOrDefaultAsync(v => v.VoterId == id);
            }
            return null;
        }

        public async Task<List<Voter>> GetAllVoters()
        {
            return await _ctx.Voters.Include(v => v.Election).Include(v => v.User).ToListAsync();
        }

        public async Task<bool> UpdateVoterById(string id)
        {
            if (VoterExist(id))
            {
                try
                {
                    Voter voter = await _ctx.Voters.FindAsync(id);
                    voter.WillVoteAgainBecauseOfTie = true;
                    _ctx.Voters.Update(voter);
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

        public async Task<bool> DeleteVoter(string id)
        {
            if (VoterExist(id))
            {
                try
                {
                    Voter voter = await _ctx.Voters.FindAsync(id);
                    _ctx.Voters.Remove(voter);
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

        public bool VoterExist(string id) => _ctx.Voters.Any(v => v.VoterId == id);
    }
}
