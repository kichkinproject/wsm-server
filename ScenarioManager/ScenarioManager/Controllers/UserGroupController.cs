using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScenarioManager.Mappers;
using ScenarioManager.Model.DBModel;
using ScenarioManager.Model.DTO.UserGroupDTO;
using ScenarioManager.Repositories;

namespace ScenarioManager.Controllers
{
    [Produces("application/json")]
    [Route("api/UserGroup")]
    [Authorize]
    public class UserGroupController : Controller
    {
        private readonly IMapper<UserGroup, EditUserGroup> _editUserGroupMapper;
        private readonly UserGroupRepository _repository;
        private readonly IMapper<UserGroup, CreateUserGroup> _createUserGroupMapper;
        public UserGroupController(UserGroupRepository repository, IMapper<UserGroup, CreateUserGroup> createUserGroupMapper, 
            IMapper<UserGroup, EditUserGroup> editUserGroupMapper)
        {
            _repository = repository;
            _createUserGroupMapper = createUserGroupMapper;
            _editUserGroupMapper = editUserGroupMapper;
        }

        [HttpGet]
        public UserGroup GetUserGroup()
        {
            long id = GetUserGroupId();
            var returningValue = _repository.UserGroups.Include(x => x.ParentGroup).Include(x => x.ChildrenGroups).Where(x => x.Id == id).FirstOrDefault();
            if (returningValue == null)
                throw new Exception("элемент не найден");
            return returningValue;
        }



        [HttpGet("All")]
        [Authorize(Roles=Constants.RoleNames.Admin)]
        public IEnumerable<UserGroup> GetUserGroups()
        {
            return _repository.UserGroups;
        }

        [HttpGet("ById/{id}")]
        [Authorize(Roles = Constants.RoleNames.Admin)]
        public UserGroup GetUserGroupById(long id)
        {
            var returning = _repository[id];

            if (returning == null)
                throw new Exception("Группа с таким Id не существует");

            return returning;
        }


        [HttpPost]
        public long CreateUserGroup([FromBody] CreateUserGroup input)
        {
            var userGroup = _createUserGroupMapper.Map(input);
            _repository.Create(userGroup);
            _repository.SaveChanges();
            return userGroup.Id;
        }

        [HttpPut]
        public void EditUserGroup([FromBody] EditUserGroup input)
        {
            var userGroup = _editUserGroupMapper.Map(input);
            _repository.Edit(userGroup);
            _repository.SaveChanges();
        }

        [HttpDelete("{id}")]
        public void DeleteUserGroup(long id)
        {
            _repository.Delete(id);
            _repository.SaveChanges();
        }


        private long GetUserGroupId()
        {
            return Convert.ToInt64(User.Claims.Where(x => x.Type == Constants.ClaimTypeNames.UserGroupId).FirstOrDefault().Value);
        }
    }
}