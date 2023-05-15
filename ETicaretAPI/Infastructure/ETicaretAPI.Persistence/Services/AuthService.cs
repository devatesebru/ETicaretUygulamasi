using ETicaretAPI.Application.Abstractions;
using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Application.DTOs.Facebook;
using ETicaretAPI.Application.Exceptions;
using ETicaretAPI.Application.Features.Commands.AppUser.LoginUser;
using ETicaretAPI.Domain.Entities.Identity;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Services
{
    public class AuthService : IAuthService

    {
        readonly HttpClient _httpClient;
        readonly IConfiguration _configuration;
        readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        readonly ITokenHandler _tokenHandler;

   
        readonly SignInManager<Domain.Entities.Identity.AppUser> _signInManager;

        public AuthService(IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            UserManager<Domain.Entities.Identity.AppUser> userManager,
            ITokenHandler tokenHandler,
            SignInManager<AppUser> signInManager)
        {
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _signInManager = signInManager;
        }

        async Task<Token> CreateUserExternalAsync(AppUser user, string email, string name, UserLoginInfo info, int accessTokenLifeTime)
        {
            bool result = user != null;
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    user = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = email,
                        UserName = email,
                        NameSurname = name
                    };
                    var identityResult = await _userManager.CreateAsync(user);
                    result = identityResult.Succeeded;
                }
            }
            if (result)
            {
                await _userManager.AddLoginAsync(user, info); //AspNetUserLogins
                Token token = _tokenHandler.CreateAccessToken(5);
                return token;
            }
            throw new Exception("Invalid external authentication");

        }


        public async Task<Token> FacebookLoginAsync(string authToken, int accessTokenLifeTime)
        {
            string accesTokenResponse
                 = await _httpClient.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={_configuration["ExternalLoginSettings:Facebook:Client_ID"]}&client_secret={_configuration["ExternalLoginSettings:Facebook:Client-Secret"]}&grant_type=client_credentials");



            FacebookAccessTokenResponse facebookAccessTokenResponse = JsonConvert.DeserializeObject<FacebookAccessTokenResponse>(accesTokenResponse)
                ?? throw new NullReferenceException(nameof(accesTokenResponse));
            string userAccessTokenValidation =
            await _httpClient.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={authToken}&access_token={facebookAccessTokenResponse.access_token}");

            FacebookUserAccessTokenValidation validation = JsonConvert.DeserializeObject<FacebookUserAccessTokenValidation>(userAccessTokenValidation) ?? throw new NullReferenceException(nameof(userAccessTokenValidation));

            if (validation.data.is_valid)
            {
                string userInfoResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/me?fields=email,name&access_token={authToken}");
                FacebookUserInfoResponse userInfo = JsonConvert.DeserializeObject<FacebookUserInfoResponse>(userInfoResponse) ?? throw new NullReferenceException(nameof(userInfoResponse));

                var info = new UserLoginInfo("FACEBOOK", validation.data.user_id, "FACEBOOK");
                Domain.Entities.Identity.AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

                return await CreateUserExternalAsync(user, userInfo.Email, userInfo.Name, info, accessTokenLifeTime);
            }
            throw new Exception("Invalid external authentication");

        }

        public async Task<Token> GoogleLoginAsync(string idToken, int accessTokenLifeTime)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { _configuration["ExternalLoginSettings:Google:Client_ID"] }

            };
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

            var info = new UserLoginInfo("GOOGLE", payload.Subject, "GOOGLE");
            Domain.Entities.Identity.AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);


            return await CreateUserExternalAsync(user, payload.Email, payload.Name, info, accessTokenLifeTime);


        }

        public async Task<Token> LoginAsync(string usernameOrEmail, string password, int accessTokenLifeTİme)
        {
            //kullanıcı email yada Name girecek ona göre iki kere sorguluyoruz önce name e baktık var mı diye yoksa emaile baksık var mı diye ikiside yoksa false döneceğiz. kullanıcının ne gireceğini bilmiyoruz sonuçta email girer name alanında arayıp false dönebiliriz.
            Domain.Entities.Identity.AppUser user = await _userManager.FindByNameAsync(usernameOrEmail);
            if (user == null)
                user = await _userManager.FindByEmailAsync(usernameOrEmail);
            if (user == null)
                throw new NotFoundUserException("Kullanıcı veya şifre hatalı...");
            //yukarıdaki fonksiyonda aşağıdaki fonksiyona gerekli olan user ı getirmemiz lazım
            //bu fonksiyon ile user ile password ün doğrulanıp geri dönüş yapacak true veya false
            //user ile sendeki pasword a bak en son istediği karşılaştırma sonrası yanlış olursa kitleyelim mi şimdilik false daha sonra 3 kere girmeye kitleriz 
            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (result.Succeeded)//authentication başarılı oluyor
            {
                ///yetkileri belirleyeceğiz
                Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTİme);
                return token;              
             
            }
              throw new AuthenticationErrorException();
        }
    }
}
