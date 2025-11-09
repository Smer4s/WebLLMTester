using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uply.Domain.Abstractions.Services
{
    public interface IDatabaseMigrator
    {
        void Migrate();
    }

}
