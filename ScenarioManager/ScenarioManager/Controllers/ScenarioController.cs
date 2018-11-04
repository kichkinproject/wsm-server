using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScenarioManager.Mappers;
using ScenarioManager.Model.DBModel;
using ScenarioManager.Model.DTO;
using ScenarioManager.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Controllers
{
    [Produces("application/json")]
    [Route("api/Scenario")]
    [Authorize]
    public class ScenarioController:Controller
    {
        private readonly ScenarioRepository _scenarioRepository;
        private readonly UserGroupRepository _userGroupRepository;
        private readonly IMapper<ScenarioDTO, Scenario> _mapper;
        public ScenarioController(ScenarioRepository scenarioRepository, UserGroupRepository userGroupRepository, IMapper<ScenarioDTO, Scenario> mapper)
        {
            _scenarioRepository = scenarioRepository;
            _userGroupRepository = userGroupRepository;
            _mapper = mapper;
        }

        [HttpGet("Available")]
        public IEnumerable<ScenarioDTO> ParentScenarios()
        {
            var parents = _userGroupRepository.GetParentGroups(GetUserGroupId());
            return _scenarioRepository.Scenarios.Include(x => x.UserGroup).Where(x => parents.Contains(x.UserGroupId)).Select(x => _mapper.Map(x));
        }
        [HttpGet("Children")]
        public IEnumerable<ScenarioDTO> ChildrenScenarios()
        {
            var children = _userGroupRepository.GetChildrenGroups(GetUserGroupId());
            return _scenarioRepository.Scenarios.Include(x => x.UserGroup).Where(x => children.Contains(x.UserGroupId)).Select(x => _mapper.Map(x));

        }

        [HttpGet("{id}")]
        public ScenarioDTO GetScenario(long id)
        {
            var parents = _userGroupRepository.GetParentGroups(GetUserGroupId());
            var children = _userGroupRepository.GetChildrenGroups(GetUserGroupId());
            var scenario = _scenarioRepository.Scenarios.Include(x=>x.UserGroup).Where(x => x.Id == id).FirstOrDefault();
            if (scenario == null)
                throw new Exception("Такого сценария не существует");
            if (parents.Contains(scenario.Id) || children.Contains(scenario.Id))
                return _mapper.Map(scenario);
            else
                throw new Exception("Этот сценарий вам не доступен");
        }

        [Authorize(Roles =Constants.RoleNames.Integrator)]
        [HttpPut]
        public void EditScenario([FromBody]Scenario input)
        {
            var currentScenario = _scenarioRepository.Scenarios.Where(x => x.Id == input.Id).FirstOrDefault();
            if (currentScenario == null)
                throw new Exception("Такого сценария не существует");
            var children = _userGroupRepository.GetChildrenGroups(GetUserGroupId());
            if (children.Contains(currentScenario.UserGroupId))
            {
                _scenarioRepository.EditScenario(input);
                _scenarioRepository.SaveChanges();
            }
            else
                throw new Exception("Этот сценарий вам не доступен");
        }

        [Authorize(Roles = Constants.RoleNames.Integrator)]
        [HttpDelete("{id}")]
        public void DeleteScenario(long id)
        {
            var currentScenario = _scenarioRepository.Scenarios.Where(x => x.Id == id).FirstOrDefault();
            if (currentScenario == null)
                throw new Exception("Такого сценария не существует");
            var children = _userGroupRepository.GetChildrenGroups(GetUserGroupId());
            if (children.Contains(currentScenario.UserGroupId))
            {
                _scenarioRepository.Delete(id);
                _scenarioRepository.SaveChanges();
            }
            else
                throw new Exception("Этот сценарий вам не доступен");
        }


        [Authorize(Roles = Constants.RoleNames.Integrator)]
        [HttpPost]
        public Scenario CreateScenario([FromBody]Scenario input)
        {
            var children = _userGroupRepository.GetChildrenGroups(GetUserGroupId());
            if (children.Contains(input.UserGroupId))
            {
                var returningScenario =_scenarioRepository.CreateScenario(input);
                _scenarioRepository.SaveChanges();
                return returningScenario;
            }
            else
                throw new Exception("эта группа пользователей вам недоступна");
        }


        private long GetUserGroupId()
        {
            return Convert.ToInt64(User.Claims.Where(x => x.Type == Constants.ClaimTypeNames.UserGroupId).FirstOrDefault().Value);
        }
    }
}
