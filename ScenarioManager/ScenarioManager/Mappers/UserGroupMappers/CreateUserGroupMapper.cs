using ScenarioManager.Model.DBModel;
using ScenarioManager.Model.DTO;
using ScenarioManager.Model.DTO.UserGroupDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Mappers.UserGroupMappers
{
    public class CreateUserGroupMapper : IMapper<UserGroup, CreateUserGroup>
    {
        public UserGroup Map(CreateUserGroup input)
        {
            return new UserGroup
            {
                Name = input.Name,
                ParentGroupId = input.ParentGroupId
            };
        }
    }
}
