using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcSkiOprema.ViewModels
{
    public class ShoppingCartRemoveViewModel
    {
        public string Message { get; set; }
        public decimal CartTotal { get; set; }
        public int DeleteId { get; set; }
    }
}