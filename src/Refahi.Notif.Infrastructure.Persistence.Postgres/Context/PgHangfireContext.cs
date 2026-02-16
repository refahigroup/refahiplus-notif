using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refahi.Notif.Infrastructure.Persistence.Postgres.Context
{
    public class PgHangfireContext: DbContext
    {
        public PgHangfireContext(DbContextOptions<PgHangfireContext> options) : base(options)
        {
            
        }
    }
}
