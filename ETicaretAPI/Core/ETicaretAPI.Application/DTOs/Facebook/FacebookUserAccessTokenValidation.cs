using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.DTOs.Facebook
{
    public class FacebookUserAccessTokenValidation
    {
        public Data data { get; set; }

        public class Data
        {
            public bool is_valid { get; set; }
            public string user_id { get; set; }
        }
    }
}
