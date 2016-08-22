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
    [Table("Rezervacija")]
    public class Rezervacija
    {
        [Key]
        public virtual long Id { get; set; }

        [Required]
        [DisplayName("Korisnik")]
        public virtual string UserId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Datum posudbe")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime DatumOd { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Datum vraćanja")]
        public virtual DateTime DatumDo { get; set; }

        public virtual int? Odobreno { get; set; }

        public virtual string Napomena { get; set; }

        [Required]
        [DisplayName("Cijena")]
        public virtual float UkupnaCijena { get; set; }

        public virtual Collection<StavkaRezervacije> StavkaRezervacije { get; set; }
    }
}