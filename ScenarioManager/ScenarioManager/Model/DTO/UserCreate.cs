using ScenarioManager.Model.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Model.DTO
{
    public class UserCreate
    {
        public string Login { get; set; }
        public UserType UserType { get; set; }
        public long UserGroupId { get; set; }
        public string Password { get; set; }
    }
}
