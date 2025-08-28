using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ucm.Domain.IRepositories
{
    public interface IRoleRepository
    {
        Task<bool> RoleExistsAsync(string roleName);
    }

}
