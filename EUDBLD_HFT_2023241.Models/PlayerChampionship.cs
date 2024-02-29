using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EUDBLD_HFT_2023242.Models
{
    [Table("playersChampionships")]
    public class PlayerChampionship : Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id", TypeName = "int")]
        public override int Id { get; set; }

        [JsonIgnore]
        [NotMapped]
        public virtual Player Player { get; set; }

        [Range(0, int.MaxValue)]
        public int PlayerId { get; set; }

        [JsonIgnore]
        [NotMapped]
        public virtual Championship Championship { get; set; }

        [Range(0, int.MaxValue)]
        public int ChampionshipId { get; set; }

        [Range(0, int.MaxValue)]
        [Required]
        public int Place { get; set; }

        public PlayerChampionship()
        {
            
        }
        // 1#1#1#1
        public PlayerChampionship(string line)
        {
            string[] split = line.Split('#');
            Id = int.Parse(split[0]);
            PlayerId = int.Parse(split[1]);
            ChampionshipId = int.Parse(split[2]);
            Place = int.Parse(split[3]);
        }
    }
}
