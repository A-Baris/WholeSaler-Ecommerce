using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSaler.Business.TokenServices.Models;
using WholeSaler.Entity.Entities.MongoIdentity;

namespace WholeSaler.Business.TokenServices.Abstract
{
    public interface ITokenService
    {
       Task<TokenModel> CreateToken(AppUser appUser);
       Task<TokenModel> RequestTokenByRefresh(string token);



    }
}
