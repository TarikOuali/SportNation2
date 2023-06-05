using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportNation2.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions opt): base(opt)
        {
            
        }
    }
}
