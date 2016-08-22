using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcSkiOprema.Models
{
    [Table("Cart")]
    public class Cart
    {
        [Key]
        [Column(Order=0)]
        public string Id { get; set; }
        [Key]
        [Column(Order = 1)]
        public long OpremaId { get; set; }
        public virtual Oprema Oprema { get; set; }
    }
}