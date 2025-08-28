using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ucm.Infrastructure.Common.Mappers
{
    public interface IEntityEfMapper<TEntity, TEf>
    {
        TEntity ToEntity(TEf ef);
        TEf ToEf(TEntity entity);
    }
}
