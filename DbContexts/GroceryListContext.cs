using AspnetCore.Jwt.Authentication.Models;
using Microsoft.EntityFrameworkCore;

namespace AspnetCore.Jwt.Authentication.DbContexts
{
    public class GroceryListContext: DbContext
    {
        public GroceryListContext(DbContextOptions<GroceryListContext> options): base(options)
        {
            
        }

        public DbSet<GroceryItem> GroceryList { get; set; }
    }
}
