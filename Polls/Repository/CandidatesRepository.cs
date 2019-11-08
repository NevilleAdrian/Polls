using Microsoft.AspNetCore.Hosting;
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
    public class CandidatesRepository
    {
        private readonly ApplicationDbContext _ctx;
        private readonly IImageService _img;
        private readonly ILogger<CandidatesRepository> _logger;
        private readonly IHostingEnvironment _env;
        public CandidatesRepository(ApplicationDbContext context,
            IImageService imageService,
            IHostingEnvironment env,
            ILogger<CandidatesRepository> logger)
        {
            _ctx = context;
            _img = imageService;
            _logger = logger;
            _env = env;
        }

        public async Task AddCandidate(CandidateViewModel candidateViewModel)
        {
            if (string.IsNullOrEmpty(candidateViewModel.ElectionId))
                throw new Exception();
            if (string.IsNullOrEmpty(candidateViewModel.UserId))
                throw new Exception();
            string url = "";
            if(candidateViewModel.File != null)
            {
                url = _img.CreateImage(candidateViewModel.File, _env);
            }
            Candidate candidate = new Candidate
            {
                ElectionId = candidateViewModel.ElectionId,
                MyImageUrl = url,
                MyPromise = candidateViewModel.MyPromise,
                TotalNumberOfVotes = candidateViewModel.TotalNumberOfVotes,
                UserId = candidateViewModel.UserId
            };
            try
            {
                await _ctx.Candidates.AddAsync(candidate);
                await _ctx.SaveChangesAsync();
                return;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"There was an error {ex.Message}");
            }
        }

        public async Task<Candidate> GetCandidateById(string id)
        {
            if (CandidateExist(id))
            {
                return await _ctx.Candidates.Include(c => c.User).Include(c => c.Election).SingleOrDefaultAsync(v => v.CandidateId == id);
            }
            return null;
        }

        public async Task<List<Candidate>> GetAllCandidates() => await _ctx.Candidates.Include(c => c.User).Include(c => c.Election).ToListAsync();

        public async Task<bool> UpdateCandidateById(CandidateViewModel candidateViewModel)
        {
            if (CandidateExist(candidateViewModel.CandidateId))
            {
                string url = null;
                if (candidateViewModel.File != null)
                {
                    url = _img.EditImage(candidateViewModel.File, candidateViewModel.MyImageUrl, _env);
                }

                Candidate candidate = await _ctx.Candidates.Where(c => c.CandidateId == candidateViewModel.CandidateId).SingleOrDefaultAsync();
                candidate.ElectionId = candidateViewModel.ElectionId;
                candidate.MyImageUrl = !string.IsNullOrEmpty(url) ? url : candidateViewModel.MyImageUrl;
                candidate.MyPromise = candidateViewModel.MyPromise;
                candidate.TotalNumberOfVotes = candidateViewModel.TotalNumberOfVotes;
                candidate.UserId = candidateViewModel.UserId;
                try
                {
                    _ctx.Candidates.Update(candidate);
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


        public async Task<bool> DeleteCandidate(string id)
        {
            if (CandidateExist(id))
            {
                try
                {
                    Candidate candidate = await _ctx.Candidates.FindAsync(id);
                    _img.DeleteImage(candidate.MyImageUrl, _env);
                    _ctx.Candidates.Remove(candidate);
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

        public bool CandidateExist(string id) => _ctx.Candidates.Any(c => c.CandidateId == id);
    }
}
