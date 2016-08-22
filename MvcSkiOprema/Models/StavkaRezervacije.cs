using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcSkiOprema.Models
{
    [Table("StavkaRezervacije")]
    public class StavkaRezervacije
    {
        [Key]
        [Column(Order = 0)]
        public virtual long RezervacijaId { get; set; }
        public virtual Rezervacija Rezervacija { get; set; }

        [Key]
        [Column(Order = 1)]
        public virtual long OpremaId { get; set; }
        public virtual Oprema Oprema { get; set; }
    }
}