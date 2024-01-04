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
    public class Prizes : Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("prizes_id", TypeName = "int")]
        public override int Id { get; set; }

        [NotMapped]
        public virtual Championship Championship { get; set; }

        public int ChampionshipId { get; set; }

        [Range(0,1000)]
        [Required]
        public int Place {  get; set; }

        [Required]
        public int Price { get; set; }

        public Prizes()
        {
            
        }

        // 1#2#1#120000
        public Prizes(string line)
        {
            string[] split = line.Split('#');
            Id = int.Parse(split[0]);
            ChampionshipId = int.Parse(split[1]);
            Place = int.Parse(split[2]);
            Price = int.Parse(split[3]);
        }
    }
}
