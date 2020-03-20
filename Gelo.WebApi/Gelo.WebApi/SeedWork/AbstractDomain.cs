using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gelo.WebApi.SeedWork
{
    public class AbstractDomain
    {
        public virtual long Id { get; protected set; }
    }
}
