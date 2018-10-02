using Microsoft.EntityFrameworkCore;
using ScenarioManager.Model.DBModel;
using ScenarioManager.Model.DBModel.DBContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Repositories
{
    public class ScenarioRepository
    {
        private readonly MainDbContext _context;
        public ScenarioRepository(MainDbContext context)
        {
            _context = context;
        }

        public DbSet<Scenario> Scenarios { get => _context.Scenarios; }


        public Scenario CreateScenario(Scenario input)
        {
            _context.Add(input);
            return input;
        }


        public Scenario EditScenario(Scenario input)
        {
            var scenario = Scenarios.Where(x => x.Id == input.Id).FirstOrDefault();
            if (scenario == null)
                throw new Exception("Сценарий с таким Id не обнаружен");
            if (input.Name != null)
                scenario.Name = input.Name;
            if (input.Text != null)
                scenario.Text = input.Text;
            if (input.Description != null)
                scenario.Description = input.Description;
            if (input.UserGroup != null)
                scenario.UserGroupId = input.UserGroup.Id;

            return input;
        }


        public void Delete(long id)
        {
            var scenario = Scenarios.Where(x => x.Id == id).FirstOrDefault();
            if (scenario == null)
                throw new Exception("Сценарий с таким Id не обнаружен");
            _context.Remove(scenario);
        }


        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
