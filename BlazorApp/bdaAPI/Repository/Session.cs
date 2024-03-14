using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace bdaAPI.Repository
{
    public class Session
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int accountID { get; set; }
        public DateTime loginTime { get; set; }
        public DateTime sessionExpirationDate { get; set; }
    }
}
