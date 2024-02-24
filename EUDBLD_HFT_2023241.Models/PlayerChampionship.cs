using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EUDBLD_HFT_2023241.Models
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
        [Required]
        public int PlayerId { get; set; }

        [JsonIgnore]
        [NotMapped]
        public virtual Championship Championship { get; set; }

        [Range(0, int.MaxValue)]
        [Required]
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

        public override bool Equals(object obj)
        {
            PlayerChampionship other = obj as PlayerChampionship;
                if (other == null) return false;
            
            PlayerChampionship current = this;

            return current.Id == other.Id &&
                current.PlayerId == other.PlayerId &&
                current.ChampionshipId == other.ChampionshipId &&
                current.Place == other.Place;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, PlayerId, ChampionshipId, Place);
        }
    }
}
