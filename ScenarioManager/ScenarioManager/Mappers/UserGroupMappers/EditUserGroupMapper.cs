using ScenarioManager.Model.DBModel;
using ScenarioManager.Model.DTO.UserGroupDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Mappers.UserGroupMappers
{
    public class EditUserGroupMapper : IMapper<UserGroup, EditUserGroup>
    {
        public UserGroup Map(EditUserGroup input)
        {
            return new UserGroup
            {
                Id = input.Id,
                Name = input.Name
            };
        }
    }
}
