using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Model.DTO.UserGroupDTO
{
    public class CreateUserGroup
    {        
        public string Name { get; set; }

        public long? ParentGroupId { get; set; }        
    }
}
