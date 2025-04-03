using LearnNetCore.Application.Identities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnNetCore.Application.Interfaces;

public interface ITokenService
{
    string CreateToken(ApplicationUser user);

}
