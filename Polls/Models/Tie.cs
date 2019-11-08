using System.ComponentModel.DataAnnotations.Schema;

namespace Polls.Models
{
    public class Tie
    {
        public int TieId { get; set; }
        [ForeignKey("ElectionId")]
        public Election Election { get; set; }
        public string ElectionId { get; set; }
    }
}