using ETicaretAPI.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUser.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {
        readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        readonly SignInManager<Domain.Entities.Identity.AppUser> _signInManager;

        public LoginUserCommandHandler(UserManager<Domain.Entities.Identity.AppUser> userManager, SignInManager<Domain.Entities.Identity.AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            //kullanıcı email yada Name girecek ona göre iki kere sorguluyoruz önce name e baktık var mı diye yoksa emaile baksık var mı diye ikiside yoksa false döneceğiz. kullanıcının ne gireceğini bilmiyoruz sonuçta email girer name alanında arayıp false dönebiliriz.
            Domain.Entities.Identity.AppUser user =await _userManager.FindByNameAsync(request.UsernameOrEmail);
            if(user == null)
                user= await _userManager.FindByEmailAsync(request.UsernameOrEmail);
            if (user == null)
                throw new NotFoundUserException("Kullanıcı veya şifre hatalı...");
            //yukarıdaki fonksiyonda aşağıdaki fonksiyona gerekli olan user ı getirmemiz lazım
            //bu fonksiyon ile user ile password ün doğrulanıp geri dönüş yapacak true veya false
            //user ile sendeki pasword a bak en son istediği karşılaştırma sonrası yanlış olursa kitleyelim mi şimdilik false daha sonra 3 kere girmeye kitleriz 
            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (result.Succeeded)//authentication başarılı oluyor
            {
                ///yetkileri belirleyeceğiz
            }
            return new();
        }
    }
}
