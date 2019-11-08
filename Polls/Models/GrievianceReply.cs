using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Polls.Models
{
    public class GrievianceReply
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GrievianceReplyId { get; set; }
        public string Message { get; set; }

        [ForeignKey("GrievianceId")]
        public Grieviance Grieviance { get; set; }
        public int GrievianceId { get; set; }
    }
}