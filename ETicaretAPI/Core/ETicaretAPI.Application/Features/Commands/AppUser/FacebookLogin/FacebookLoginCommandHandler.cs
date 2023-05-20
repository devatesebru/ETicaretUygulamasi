using ETicaretAPI.Application.Abstractions;
using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Application.DTOs.Facebook;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace ETicaretAPI.Application.Features.Commands.AppUser.FacebookLogin
{
    public class FacebookLoginCommandHandler : IRequestHandler<FacebookLoginCommandRequest, FacebookLoginCommandResponse>
    {
      
       readonly IAuthService _authService;

        public FacebookLoginCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        async Task<FacebookLoginCommandResponse> IRequestHandler<FacebookLoginCommandRequest, FacebookLoginCommandResponse>.Handle(FacebookLoginCommandRequest request, CancellationToken cancellationToken)
        {

          var token =  await _authService.FacebookLoginAsync(request.AuthToken,900);
            return new()
            {
                Token = token
            };

        }
    }
}
