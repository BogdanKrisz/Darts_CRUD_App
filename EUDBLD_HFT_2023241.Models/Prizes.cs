using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUDBLD_HFT_2023241.Models
{
    [Table("prizes")]
    public class Prizes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("prizes_id", TypeName = "int")]
        public int Id { get; set; }

        [NotMapped]
        public virtual Championship Championship { get; set; }

        [ForeignKey(nameof(Championship))]
        public int ChampionshipId { get; set; }

        [Range(0,1000)]
        [Required]
        public int Place {  get; set; }

        [Required]
        public int Price { get; set; }
    }
}
