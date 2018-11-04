using Microsoft.AspNetCore.Mvc;
using ScenarioManager.Model.DTO.UserInfoDTO;
using ScenarioManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScenarioManager.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController:Controller
    {
        private readonly AccountService _accountService;
        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpPost]
        public Token Login([FromBody]LoginPassword input)
        {
            return _accountService.LogIn(input);
        }
    }
}
