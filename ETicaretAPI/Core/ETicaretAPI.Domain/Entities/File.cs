﻿using ETicaretAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Domain.Entities
{
    public class File: BaseEntity
    {
        public string FileName { get; set; }
        public string Path { get; set; }
        public string Storage { get; set; }

        [NotMapped]
        //burda bunu kullanmayacağımızı belirttik bunu durduk yere migrate etme dedik.tamam kalıtım aldık ama bunu almak istemiyorum :D
        public override DateTime UpdatedDate { get => base.UpdatedDate; set => base.UpdatedDate = value; }

    }
}
