using Microsoft.AspNetCore.Http;

namespace Polls.Models
{
    public class CandidateViewModel : Candidate
    {
        public IFormFile File { get; set; }
    }
}
