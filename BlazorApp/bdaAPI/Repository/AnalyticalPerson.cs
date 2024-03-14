using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace bdaAPI.Repository
{
    public class AnalyticalPerson
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public required string FullName { get; set; }
        public DateTime Birth { get; set; }
        public int Type { get; set; }
        public int OwnerID { get; set; }
    }
}
