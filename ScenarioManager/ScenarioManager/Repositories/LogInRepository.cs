using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Repositories
{
    public class LogInRepository
    {
        public string Login { get; set; }
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
    }
}
