using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcSkiOprema.Models
{
    [Table("Kategorija")]
    public class Kategorija
    {
        [Key]
        public virtual long Id { get; set; }

        [Required]
        public virtual string Naziv { get; set; }

        public virtual Collection<Oprema> Oprema { get; set; }
    }
}