using Microsoft.EntityFrameworkCore;
using ScenarioManager.Model.DBModel;
using ScenarioManager.Model.DBModel.DBContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Repositories
{
    public class AdminRepository
    {
        private readonly MainDbContext _context;
        public AdminRepository(MainDbContext context)
        {
            _context = context;
        }

        public DbSet<Admin> Admins { get => _context.Admins; }

      
        public Admin this[string login]
        {
            get { return Admins.Where(x => x.Login == login).FirstOrDefault(); }
        }

        public void Create(Admin input)
        {
            _context.Add(input);
        }
        public void Edit(Admin input)
        {
            var admin = Admins.Where(x => x.Login == input.Login).FirstOrDefault();
            CheckAdmin(admin);
            // some changes
        }



        public void Delete(string login)
        {
            
            var admin = Admins.Where(x => x.Login == login).FirstOrDefault();
            CheckAdmin(admin);
            _context.Remove(admin);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
        private static void CheckAdmin(Admin admin)
        {
            if (admin == null)
                throw new Exception("Администратор с таким Id не найден");
            if (admin.IsMainAdmin)
                throw new Exception("Нельзя удалять главного администратора");
        }
    }
}
