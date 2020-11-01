using CS_Web_Core_MVC_Northwind.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CS_Web_Core_MVC_Northwind.Data
{
    public class NorthwindContext : DbContext
    {
        public NorthwindContext(DbContextOptions<NorthwindContext> options)
            : base(options)
        {
        }

        public DbSet<CategoriesModel> Categories { get; set; }
        public DbSet<ProductsModel> Products { get; set; }
        public DbSet<SuppliersModel> Suppliers { get; set; }

    }
}
