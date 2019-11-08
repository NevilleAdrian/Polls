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
    public class GrievianceRepliesRepository
    {
        private readonly ApplicationDbContext _ctx;
        private readonly ILogger<GrievianceRepliesRepository> _logger;
        public GrievianceRepliesRepository(ApplicationDbContext context,
            ILogger<GrievianceRepliesRepository> logger)
        {
            _ctx = context;
            _logger = logger;
        }

        public async Task ReplyAGrieviance(GrievianceReply grieviance)
        {
            try
            {
                Grieviance grievianceToReply = await _ctx.Grieviances.FindAsync(grieviance.GrievianceId);
                grievianceToReply.HasBeenSeen = true;
                

                await _ctx.GrievianceReplies.AddAsync(grieviance);
                _ctx.Grieviances.Update(grievianceToReply);
                Notification notification = new Notification
                {
                    HasBeenSeen = false,
                    Message = grieviance.Message,
                    UserId = grievianceToReply.UserId
                };
                await _ctx.Notifications.AddAsync(notification);

                await _ctx.SaveChangesAsync();
                return;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"There was an error {ex.Message}");
            }
        }

        public async Task<GrievianceReply> GetGrievianceReplyById(int id)
        {
            if (GrievianceReplyExists(id))
            {
                return await _ctx.GrievianceReplies.Include(g => g.Grieviance).ThenInclude(gr => gr.User).SingleOrDefaultAsync(g => g.GrievianceReplyId == id);
                
            }
            return null;
        }

        public async Task<List<GrievianceReply>> GetAllGrievianceReplies()
        {
            
            return await _ctx.GrievianceReplies.Include(g => g.Grieviance).ThenInclude(gr => gr.User).ToListAsync();
        }

        public async Task<bool> UpdateGrevianceReplyById(GrievianceReply grieviance)
        {
            if (GrievianceReplyExists(grieviance.GrievianceId))
            {
                try
                {

                    _ctx.GrievianceReplies.Update(grieviance);
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
            if (GrievianceReplyExists(id))
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

        public bool GrievianceReplyExists(int id) => _ctx.GrievianceReplies.Any(g => g.GrievianceReplyId == id);
    }
}
