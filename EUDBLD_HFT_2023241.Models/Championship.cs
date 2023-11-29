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
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public int MaxAttender;

        [NotMapped]
        public ICollection<Player> Attenders { get; set; }

        public int PrizePool { get; set; }

        public Championship()
        {
            
        }

        // 1#GrandSlamOfDarts23#2023.11.12.#2023.11.20.#32#650,000
        public Championship(string line)
        {
            string[] split = line.Split('#');
            Id = int.Parse(split[0]);
            Name = split[1];
            StartDate = DateTime.Parse(split[2]);
            EndDate = DateTime.Parse(split[3]);
            MaxAttender = int.Parse(split[4]);
            PrizePool = int.Parse(split[5]);
        }
    }
}
