using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUDBLD_HFT_2023241.Models
{
    [Table("championships")]
    public class Championship
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("championship_id", TypeName = "int")]
        public int Id { get; set; }

        [StringLength(240)]
        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public int PrizePool { get; set; }
    }
}
