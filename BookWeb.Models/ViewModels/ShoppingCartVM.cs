using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookWeb.Models.ViewModels
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart> ShoppingCartList { get; set; }
        public OrderHeader OrderHeader { get; set; }
        
    }
}
