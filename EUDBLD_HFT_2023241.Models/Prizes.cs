﻿using System;
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
        [Column("id", TypeName = "int")]
        public int Id { get; set; }

        [NotMapped]
        public virtual Championship Championship { get; set; }

        [ForeignKey(nameof(Championship))]
        public int ChampionshipId { get; set; }

        [MaxLength(3)]
        [Required]
        public int Place {  get; set; }

        [MaxLength(100)]
        [Required]
        public int Price { get; set; }
    }
}
