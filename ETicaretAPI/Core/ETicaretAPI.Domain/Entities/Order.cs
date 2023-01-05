using ETicaretAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Domain.Entities
{
    public class Order : BaseEntity
    {
        public Guid CustomerId { get; set; }
        public string Description { get; set; }
        public string Adress { get; set; }
        //Adress ileride geliştirilecek işte il ilçe mahalle felan


        public ICollection<Product> Products { get; set; }
        //burada bi order in birden fazla product u oldupunu ifade ediyor
        public Customer Customer { get; set; }
    }
}
