using ScenarioManager.Model.DTO;
using ScenarioManager.Model.DTO.UserInfoDTO;
using ScenarioManager.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ScenarioManager.Services
{
    public class AccountService
    {
        private readonly LoginService _loginService;
        private readonly TokenService _tokenService;
        private readonly UserRepository _userRepository;
        private readonly TokenRepository _tokenRepository;
        public AccountService(LoginService loginService, TokenService tokenService, UserRepository userRepository, TokenRepository tokenRepository)
        {
            _loginService = loginService;
            _tokenService = tokenService;
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
        }
        public void Delete(string login)
        {
            _loginService.Delete(login);
            _tokenRepository.SetGuid(login);
            _tokenRepository.SaveChanges();
        }

        public Token LogIn(LoginPassword input)
        {
            var info = _loginService.Login(input);
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, info.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, info.Role)
            };
            if (info.Role != Constants.RoleNames.Admin) {
                var user = _userRepository.Users.Where(x => x.Login == info.Login).FirstOrDefault();
               
                claims.Add(new Claim(Constants.ClaimTypeNames.UserGroupId,user.UserGroupId.ToString() ));
            }

           
            var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            return _tokenService.GetFullTokenAsync(claimsIdentity).Result;
        }

        public Token RegisterUser(LoginPassword input, long userGroupId)
        {
            _loginService.Register(input, Constants.RoleNames.SimpleUser);
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, input.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, Constants.RoleNames.SimpleUser)
            };
          

            claims.Add(new Claim(Constants.ClaimTypeNames.UserGroupId, userGroupId.ToString()));


            var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            return _tokenService.GetFullTokenAsync(claimsIdentity).Result;
        }
        public Token RegisterIntegrator(LoginPassword input, long userGroupId)
        {
            _loginService.Register(input, Constants.RoleNames.Integrator);
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, input.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, Constants.RoleNames.Integrator)
            };


            claims.Add(new Claim(Constants.ClaimTypeNames.UserGroupId, userGroupId.ToString()));


            var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            return _tokenService.GetFullTokenAsync(claimsIdentity).Result;
        }
        public Token RegisterAdmin(LoginPassword input)
        {
            _loginService.Register(input, Constants.RoleNames.Admin);

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, input.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, Constants.RoleNames.Admin)
            };
           


            var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            return _tokenService.GetFullTokenAsync(claimsIdentity).Result;
        }

    }
}
