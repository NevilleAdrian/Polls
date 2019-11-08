using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Polls.Models
{
    public class Election
    {
        public Election()
        {
            
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ElectionId { get; set; }
        public string ElectionName { get; set; }
        public string ExtraInformation { get; set; }
        public string ElectionPublishedMessage { get; set; }

        public DateTime ElectionStarts { get; set; }
        public DateTime ElectionEnds { get; set; }
        public bool Completed { get; set; }
        public bool Tie { get; set; }
        

        public ICollection<Candidate> Candidates { get; set; }
        public ICollection<Voter> Voters { get; set; }
    }
}