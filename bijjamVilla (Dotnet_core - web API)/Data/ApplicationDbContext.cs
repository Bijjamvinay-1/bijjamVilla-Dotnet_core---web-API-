using Microsoft.EntityFrameworkCore;

namespace bijjamVilla__Dotnet_core___web_API_.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {

        //public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        //{

        //}
        
    }
}
