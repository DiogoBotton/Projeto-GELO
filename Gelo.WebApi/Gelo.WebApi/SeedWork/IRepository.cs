using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gelo.WebApi.SeedWork
{
    public interface IRepository<T> where T : AbstractDomain
    {
        IUnitOfWork UnitOfWork { get; }
        T Create(T objeto);
    }
}
