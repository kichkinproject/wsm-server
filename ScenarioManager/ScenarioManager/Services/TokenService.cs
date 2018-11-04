using Microsoft.IdentityModel.Tokens;
using ScenarioManager.Model.DTO.UserInfoDTO;
using ScenarioManager.Repositories;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ScenarioManager.Services
{
    public static class AuthOptions
    {
        public static string Issuer { get; private set; } = "defaultIssuer"; // издатель токена
        public static string Audience { get; private set; } = "defauleAudience";  // потребитель токена
        private static string _accessKey = "mysupersecret_secretkey!123";   // ключ для шифрации, надо сгенерировать
        private static string _refreshKey = "mysupersecret_secretkey!123";
        public static int LifeTime = 10; // время жизни токена - 1 минута
        public static int RefreshLifeTime = 20;

      

        public static SymmetricSecurityKey GetSymmetricSecurityKeyForAccessTokens()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_accessKey));
        }

        public static SymmetricSecurityKey GetSymmetricSecurityKeyForRefreshTokens()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_refreshKey));
        }
    }
    public class TokenService
    {
        private readonly TokenRepository _tokenRepository;
        public TokenService(TokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }



        public async Task<Token> GetFullTokenAsync(ClaimsIdentity identity)
        {
            var now = DateTime.UtcNow;

            var guid =_tokenRepository.SetGuid(identity.Name);

            var acessedJwt = new JwtSecurityToken(
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LifeTime)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKeyForAccessTokens(), SecurityAlgorithms.HmacSha256));

            // создаем JWT-токен
            identity.AddClaim(new Claim("Guid", guid.ToString()));


            var jwt = new JwtSecurityToken(
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.RefreshLifeTime)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKeyForRefreshTokens(), SecurityAlgorithms.HmacSha256));


            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var acessedEncodedJwt = new JwtSecurityTokenHandler().WriteToken(acessedJwt);



            return await Task.FromResult(new Token()
            {
                AccessToken = acessedEncodedJwt,
                RefreshToken = encodedJwt

            });
        }


        private async Task<bool> CheckRefreshTokenValidationAsync(JwtSecurityToken token, string refreshToken)
        {
            try
            {
                var guidIsOk = _tokenRepository.CheckGuid(
                    token.Payload[ClaimsIdentity.DefaultNameClaimType] as string,
                    Guid.Parse(token.Payload["Guid"] as string));
                var to = token.Payload.ValidTo;
                var now = DateTime.UtcNow;
                if (now > to)
                    return await Task.FromResult(false);
                if (!guidIsOk)
                {
                    return await Task.FromResult(false);
                }

                var jwt = new JwtSecurityToken(
                    claims: token.Claims,
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKeyForRefreshTokens(), SecurityAlgorithms.HmacSha256));

                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                return await Task.FromResult(refreshToken.Equals(encodedJwt));
            }
            catch (Exception)
            {
                return await Task.FromResult(false);
            }

        }
        private async Task<bool> CheckAccessTokenValidationAsync(JwtSecurityToken token, string accessToken)
        {
            try
            {
                var to = token.Payload.ValidTo;
                var now = DateTime.UtcNow;
                if (now > to)
                    return await Task.FromResult(false);
                var acessedJwt = new JwtSecurityToken(
                    claims: token.Claims,
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKeyForAccessTokens(), SecurityAlgorithms.HmacSha256));
                var acessedEncodedJwt = new JwtSecurityTokenHandler().WriteToken(acessedJwt);
                return await Task.FromResult(accessToken.Equals(acessedEncodedJwt));
            }
            catch (Exception)
            {
                return await Task.FromResult(false);
            }

        }

        public async Task<Token> UpdateFullTokenAsync(string refreshToken)
        {


            var handler = new JwtSecurityTokenHandler();

            var token = handler.ReadJwtToken(refreshToken);
            if (!await CheckRefreshTokenValidationAsync(token, refreshToken))
                throw new Exception("Token is not valid");


            var now = DateTime.UtcNow;
            var acessedJwt = new JwtSecurityToken(
                claims: token.Claims.Where(x => (x.Type != "exp" && x.Type != "Guid")),
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LifeTime)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKeyForAccessTokens(), SecurityAlgorithms.HmacSha256));

            // создаем JWT-токен


            var jwt = new JwtSecurityToken(
                claims: token.Claims.Where(x => (x.Type != "exp")),
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.RefreshLifeTime)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKeyForRefreshTokens(), SecurityAlgorithms.HmacSha256));


            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var acessedEncodedJwt = new JwtSecurityTokenHandler().WriteToken(acessedJwt);



            return new Token()
            {
                AccessToken = acessedEncodedJwt,
                RefreshToken = encodedJwt
            };
        }


        public async Task DeleteTokenAsync(string login)
        {
            _tokenRepository.SetGuid(login);
        }

        public async Task<IEnumerable<Claim>> GetClaimsAsync(string accessToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(accessToken);
            if (!await CheckAccessTokenValidationAsync(token, accessToken))
                throw new Exception("Token is not valid");
            return await Task.FromResult(token.Claims);
        }
    }
}
