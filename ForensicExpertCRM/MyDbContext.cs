using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForensicExpertCRM
{
    public class MyDbContext : IdentityDbContext<MyUser>
    {
        public MyDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
