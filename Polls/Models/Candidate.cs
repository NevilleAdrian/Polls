using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Polls.Models
{
    public class Candidate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string CandidateId { get; set; }
        public int TotalNumberOfVotes { get; set; }
        [DataType(DataType.Url)]
        public string MyImageUrl { get; set; }
        public string MyPromise { get; set; }

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