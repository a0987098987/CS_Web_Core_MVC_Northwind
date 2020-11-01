using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CS_Web_Core_MVC_Northwind.Models
{
    public class CategoriesModel
    {
        [Key]
        public int CategoryID { get; set; }

        [StringLength(15)]
        public string CategoryName { get; set; }

        public string Description { get; set; }

        public byte[] Picture { get; set; }

    }
}
