using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ucm.Domain.Entities;

namespace Ucm.Application.IServices
{

    public interface ITokenService
    {
        string GenerateToken(AppUser user, List<string> roles);
    }



}
