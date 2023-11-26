using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUDBLD_HFT_2023241.Models
{
    class PlayerChampionship
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [NotMapped]
        public virtual Player Player { get; set; }

        [ForeignKey(nameof(Player))]
        public int PlayerId { get; set; }

        [NotMapped]
        public virtual Championship Championship{ get; set; }

        [ForeignKey(nameof(Championship))]
        public int ChampionshipId { get; set; }

        [MaxLength(50)]
        [Required]
        public int Place { get; set; }
    }
}
