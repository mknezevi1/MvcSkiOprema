using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcSkiOprema.Models
{
    [Table("Oprema")]
    public class Oprema
    {
        [Key]
        public virtual long Id { get; set; }

        [Required]
        public virtual string Naziv { get; set; }

        public virtual string Opis { get; set; }

        [Required]
        public virtual float Cijena { get; set; }

        [Required]
        public virtual int Godina { get; set; }

        [Required]
        public virtual long KategorijaId { get; set; }
        public virtual Kategorija Kategorija { get; set; }

        [Required]
        public virtual int Dostupno { get; set; }

        public virtual string Slika { get; set; }

        [Required]
        public virtual long ProizvodacId { get; set; }
        public virtual Proizvodac Proizvodac { get; set; }

        [Required]
        [DisplayName("Veličina")]
        public virtual string Velicina { get; set; }

        public virtual Collection<StavkaRezervacije> StavkaRezervacije { get; set; }
    }
}