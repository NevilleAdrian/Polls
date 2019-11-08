using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Polls.Models
{
    public class ElectionWinner
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ElectionWinnerId { get; set; }

        [ForeignKey("ElectionId")]
        public Election Election { get; set; }
        public string ElectionId { get; set; }

        [ForeignKey("CandidateId")]
        public Candidate Candidate { get; set; }
        public string CandidateId { get; set; }
    }
}
