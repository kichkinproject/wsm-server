using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using ScenarioManager.Model.DBModel;
using ScenarioManager.Model.DTO.UserInfoDTO;
using ScenarioManager.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace ScenarioManager.Services
{
    public class LoginService
    {
        private readonly UserLoginInfoRepository _userLoginInfoRepository;
        public  LoginService(UserLoginInfoRepository userLoginInfoRepository)
        {
            _userLoginInfoRepository = userLoginInfoRepository;
        }
        public UserInfo Login(LoginPassword input)
        {
            var info = _userLoginInfoRepository[input.Login];
            if (info == null)
                return null;
            //check password
            byte[] salt = info.Salt;
            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: input.Password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            if (hashed != info.HashedPassword)
                throw new Exception("Пароль не верен");
            return new UserInfo()
            {
                Login = info.Login,
                Role = info.Role
            };
        }

        public void Delete(string login)
        {
            _userLoginInfoRepository.Delete(login);
        }

        public void Register(LoginPassword input, string role)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: input.Password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            _userLoginInfoRepository.Add(new UserLoginInfo()
            {
                Login = input.Login,
                HashedPassword = hashed,//get hashed password
                Salt = salt,//get salt
                Role = role
            });
        }
    }
}
