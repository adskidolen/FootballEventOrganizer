namespace Footeo.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class FooteoDbContext : IdentityDbContext
    {
        public FooteoDbContext(DbContextOptions<FooteoDbContext> options)
            : base(options)
        {
        }
    }
}