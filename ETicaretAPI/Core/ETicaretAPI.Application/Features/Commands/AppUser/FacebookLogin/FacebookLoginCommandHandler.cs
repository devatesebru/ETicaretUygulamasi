using ETicaretAPI.Application.Abstractions;
using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Application.DTOs.Facebook;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace ETicaretAPI.Application.Features.Commands.AppUser.FacebookLogin
{
    public class FacebookLoginCommandHandler : IRequestHandler<FacebookLoginCommandRequest, FacebookLoginCommandResponse>
    {
        readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        readonly ITokenHandler _tokenHandler;
        readonly HttpClient _httpClient;

        public FacebookLoginCommandHandler(UserManager<Domain.Entities.Identity.AppUser> userManager, ITokenHandler tokenHandler, System.Net.Http.IHttpClientFactory httpClientFactory)
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _httpClient = httpClientFactory.CreateClient();

        }

        async Task<FacebookLoginCommandResponse> IRequestHandler<FacebookLoginCommandRequest, FacebookLoginCommandResponse>.Handle(FacebookLoginCommandRequest request, CancellationToken cancellationToken)
        {
            string accesTokenResponse
                = await _httpClient.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id=168121676204063&client_secret=d4c47f4cc1b33c9595e7885f9baa00fe&grant_type=client_credentials");



            FacebookAccessTokenResponse facebookAccessTokenResponse = JsonConvert.DeserializeObject<FacebookAccessTokenResponse>(accesTokenResponse)
                ?? throw new NullReferenceException(nameof(accesTokenResponse));

            string userAccessTokenValidation =
                await _httpClient.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={request.AuthToken}&access_token={facebookAccessTokenResponse.access_token}");

            FacebookUserAccessTokenValidation validation = JsonConvert.DeserializeObject<FacebookUserAccessTokenValidation>(userAccessTokenValidation) ?? throw new NullReferenceException(nameof(userAccessTokenValidation));

            if (validation.data.is_valid)
            {
                string userInfoResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/me?fields=email,name&access_token={request.AuthToken}");
                FacebookUserInfoResponse userInfo = JsonConvert.DeserializeObject<FacebookUserInfoResponse>(userInfoResponse) ?? throw new NullReferenceException(nameof(userInfoResponse));

                var info = new UserLoginInfo("FACEBOOK", validation.data.user_id, "FACEBOOK");
                Domain.Entities.Identity.AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

                bool result = user != null;
                if (user == null)
                {
                    user = await _userManager.FindByEmailAsync(userInfo.Email);
                    if (user == null)
                    {
                        user = new()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Email = userInfo.Email,
                            UserName = userInfo.Email,
                            NameSurname = userInfo.Name
                        };
                        var identityResult = await _userManager.CreateAsync(user);
                        result = identityResult.Succeeded;
                    }
                }
                if (result)
                {
                    await _userManager.AddLoginAsync(user, info); //AspNetUserLogins
                    Token token = _tokenHandler.CreateAccessToken(5);
                    return new()
                    {
                        Token = token
                    };

                }
            }

            throw new Exception("Invalid external authentication");

        }
    }
}
