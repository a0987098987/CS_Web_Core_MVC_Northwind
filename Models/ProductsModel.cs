using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CS_Web_Core_MVC_Northwind.Models
{
    public class ProductsModel
    {
        [Key]
        public int ProductID { get; set; }

        [StringLength(40)]
        public string ProductName { get; set; }

        public int? SupplierID { get; set; }

        public int? CategoryID { get; set; }

        [StringLength(20)]
        public string QuantityPerUnit { get; set; }

        public decimal? UnitPrice { get; set; }

        public Int16? UnitsInStock { get; set; }

        public Int16? UnitsOnOrder { get; set; }

        public Int16? ReorderLevel { get; set; }

        public bool Discontinued { get; set; }

        public SuppliersModel Supplier { get; set; }
        public CategoriesModel Category { get; set; }
    }
}
