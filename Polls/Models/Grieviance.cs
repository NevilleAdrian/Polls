using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Polls.Models
{
    public class Grieviance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GrievianceId { get; set; }
        public bool HasBeenSeen { get; set; }
        [DataType(DataType.Text)]
        public string Message { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }

        public ICollection<GrievianceReply> GrievianceReplies { get; set; }
    }
}