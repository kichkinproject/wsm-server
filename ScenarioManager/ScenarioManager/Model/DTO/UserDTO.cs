using ScenarioManager.Model.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Model.DTO
{
    public class UserDTO
    {
        public string Login { get; set; }
        public UserType UserType { get; set; }
        public UserGroup UserGroup { get; set; }
    }
}
