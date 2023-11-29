using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUDBLD_HFT_2023241.Models
{
    [Table("players")]
    public class Player
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("player_id", TypeName = "int")]
        public int Id { get; set; }

        [StringLength(240)]
        [Required]
        public string Name { get; set; }

        [NotMapped]
        public virtual ICollection<Championship> AttendedChampionships { get; set; } // Melyik bajnokságokon vett részt

        // Helyezés -> on the fly a logicból
        [NotMapped]
        public int RankInWorld => 1;

        public Player()
        {
            
        }

        // 1#Michael Smith
        public Player(string line)
        {
            string[] split = line.Split('#');
            Id = int.Parse(split[0]);
            Name = split[1];
        }
    }
}
