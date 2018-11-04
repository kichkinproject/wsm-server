using ScenarioManager.Model.DBModel;
using ScenarioManager.Model.DBModel.DBContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Repositories
{
    public class TokenRepository
    {
        private readonly UserDbContext _userDbContext; 
        public TokenRepository(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }
        public Guid SetGuid(string login)
        {
            var guid = Guid.NewGuid();
            var info = _userDbContext.TokenGuids.Where(x => x.Login == login).FirstOrDefault();
            if (info == null)
                _userDbContext.Add(new TokenGuid()
                {
                    Login = login,
                    Guid = guid
                });
            else
                info.Guid = guid;
            return guid;
        }
        public bool CheckGuid(string login, Guid guid)
        {
            var info = _userDbContext.TokenGuids.Where(x => x.Login == login).FirstOrDefault();
            if (info == null || info.Guid != guid)
                return false;
            return true;
        }
        public void SaveChanges()
        {
            _userDbContext.SaveChanges();
        }
    }
}
