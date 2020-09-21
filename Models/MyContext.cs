using Microsoft.EntityFrameworkCore;

namespace Models
{
    public partial class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            



            
        }


        // partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}