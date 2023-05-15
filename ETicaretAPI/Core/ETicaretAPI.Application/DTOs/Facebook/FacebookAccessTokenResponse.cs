using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.DTOs.Facebook
{
    public class FacebookAccessTokenResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
    }
}
