using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScenarioManager.Mappers;
using ScenarioManager.Model.DBModel;
using ScenarioManager.Model.DTO;
using ScenarioManager.Model.DTO.AdminDTO;
using ScenarioManager.Repositories;

namespace ScenarioManager.Controllers
{
    [Produces("application/json")]
    [Route("api/Admin")]
    [Authorize(Roles = Constants.RoleNames.Admin)]
    public class AdminController : Controller
    {
        private readonly AdminRepository _repository;
        private readonly IMapper<Admin, AdminWithPassword> _mapper;
        public AdminController(AdminRepository repository, IMapper<Admin, AdminWithPassword> mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<Admin> GetAdmins()
        {
            return _repository.Admins;
        }

        [HttpGet("{login}")]
        public Admin GetAdmin(string login)
        {
            return _repository.Admins.Where(x=>x.Login==login).FirstOrDefault();
        }

        [HttpPost]
        public void CreateAdmin([FromBody] AdminWithPassword input)
        {
            //zaregat'
            _repository.Create(_mapper.Map(input));
            _repository.SaveChanges();
        }

        [HttpPut]
        public void EditAdmin([FromBody] AdminWithPassword input)
        {
            //checklogin
            _repository.Edit(_mapper.Map(input));
            _repository.SaveChanges();
        }

        [HttpPost("Delete")]
        public void DeleteAdmin([FromBody] LoginPassword input)
        {
            //checklogin
            _repository.Delete(input.Login);
            _repository.SaveChanges();
        }

    }
}