using Microsoft.AspNetCore.Identity;
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
    public class ElectionsRepository
    {
        private readonly ApplicationDbContext _ctx;
        private ILogger<ElectionsRepository> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        public ElectionsRepository(ApplicationDbContext context,
            ILogger<ElectionsRepository> logger,
            UserManager<ApplicationUser> userManager)
        {
            _ctx = context;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task AddElection(Election election)
        {
            
            try
            {
                await _ctx.Elections.AddAsync(election);
                await _ctx.SaveChangesAsync();
                return;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"There was an error {ex.Message}");
            }
        }

        public async Task<Election> GetElectionById(string id, bool includeCandidates = false)
        {
            if (ElectionExists(id))
            {
                Election election = null;
                if(includeCandidates)
                {
                    election = await _ctx.Elections.Include(e => e.Candidates).ThenInclude(u => u.User)
                                                .Include(e => e.Voters).ThenInclude(u => u.User)
                                                .SingleOrDefaultAsync(e => e.ElectionId == id);
                }
                else
                {
                    election = await _ctx.Elections.SingleOrDefaultAsync(e => e.ElectionId == id);
                }
                
                return election;
            }
            return null;
        }

        public async Task<List<Election>> GetAllElections(bool includeCandidates = false)
        {
            List<Election> elections = new List<Election>();
            if (includeCandidates)
            {
                elections = await _ctx.Elections.Include(e => e.Candidates).ThenInclude(u => u.User)
                                                .Include(e => e.Voters).ThenInclude(u => u.User)
                                                .ToListAsync();
            }
            else
            {
                elections =  await _ctx.Elections.ToListAsync();
               
            }
            
            return elections;


        }

        public async Task<bool> UpdateElectionById(Election election)
        {
            if (ElectionExists(election.ElectionId))
            {
                
                try
                {
                    _ctx.Elections.Update(election);
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

        public async Task<bool> PublishElectionResult(string electionId)
        {
            List<Candidate> winners = new List<Candidate>();
            if(ElectionExists(electionId))
            {
                Election election = await _ctx.Elections.Include(c => c.Candidates).ThenInclude(u => u.User).Where(e => e.ElectionId == electionId).SingleOrDefaultAsync();
                int maximumVote = election.Candidates.Max(c => c.TotalNumberOfVotes);
                winners = election.Candidates.Where(c => c.TotalNumberOfVotes == maximumVote).ToList();
                if (winners.Count() > 1)
                {
                    try
                    {
                        ApplicationUser user = await _userManager.Users.Where(u => u.FirstName == "Everyone").SingleOrDefaultAsync();
                        Notification notification = new Notification
                        {
                            HasBeenSeen = false,
                            Message = "There was a tie",
                            UserId = user.Id
                        };
                        election.ElectionPublishedMessage = "There was a tie.";
                        election.Tie = true;
                        _ctx.Notifications.Add(notification);
                        _ctx.Elections.Update(election);
                        await _ctx.SaveChangesAsync();


                        return true;
                        
                        
                    }
                    catch(Exception ex)
                    {
                        _logger.LogInformation($"There was an error {ex.Message}");
                    }
                }
                else
                {
                    if(winners.Count() == 1)
                    {
                        try
                        {
                            ApplicationUser user = await _userManager.Users.Where(u => u.FirstName == "Everyone").SingleOrDefaultAsync();
                            Notification notification = new Notification
                            {
                                HasBeenSeen = false,
                                Message = $"There is a winner {winners.FirstOrDefault().User.Name}",
                                UserId = user.Id
                            };
                            election.ElectionPublishedMessage = $"{winners.FirstOrDefault().User.Name} won the election";
                            election.Completed = true;
                            election.Tie = false;
                            _ctx.Notifications.Add(notification);
                            _ctx.Elections.Update(election);
                            await _ctx.SaveChangesAsync();


                            return true;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogInformation($"There was an error {ex.Message}");
                            return false;
                        }
                    }
                }
            }
            return false;
        }

        public async Task<bool> DeleteElection(string id)
        {
            if (ElectionExists(id))
            {
                try
                {
                    Election election = await _ctx.Elections.FindAsync(id);
                    _ctx.Elections.Remove(election);
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



        public bool ElectionExists(string id) => _ctx.Elections.Any(e => e.ElectionId == id);
    }
}
