using ScenarioManager.Model.DBModel;
using ScenarioManager.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Mappers.ScenarioMapper
{
    public class ScenarioMapper : IMapper<ScenarioDTO, Scenario>
    {
        public ScenarioDTO Map(Scenario input)
        {
            return new ScenarioDTO()
            {
                Id = input.Id,
                Description = input.Description,
                Name = input.Name,
                Text = input.Text,
                UserGroup = input.UserGroup
            };
        }
    }
}
