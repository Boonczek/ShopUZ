using System.Data.Entity;


namespace ShopUZ.Models.Data
{
    public class Db :DbContext
    {
        public DbSet<PageDTO> Pages { get; set; }
    }
}