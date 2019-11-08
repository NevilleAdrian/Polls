using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Polls.Models
{
    public class Voter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string VoterId { get; set; }
        public bool HasVoted { get; set; }
        public DateTime WhenIStartedVoting { get; set; }
        public DateTime WhenIFinished { get; set; }
        public bool WillVoteAgainBecauseOfTie { get; set; }

        [ForeignKey("CandidateId")]
        public Candidate Candidate { get; set; }
        public string CandidateId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }

        [ForeignKey("ElectionId")]
        public Election Election { get; set; }
        public string ElectionId { get; set; }

        public ICollection<Grieviance> Grieviances { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }
}
