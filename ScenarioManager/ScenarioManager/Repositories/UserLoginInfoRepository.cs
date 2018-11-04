using ScenarioManager.Model.DBModel;
using ScenarioManager.Model.DBModel.DBContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Repositories
{
    public class UserLoginInfoRepository
    {
        private readonly UserDbContext _context;
        public UserLoginInfoRepository(UserDbContext context)
        {
            _context = context;
        }

        public UserLoginInfo this[string login]
        {
            get => _context.Users.Where(x => x.Login == login).FirstOrDefault();
        }
        public void Add(UserLoginInfo input)
        {
            _context.Add(input);
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Delete(string login)
        {
            var info = this[login];
            if (info != null)
                _context.Remove(info);
        }
    }
}
