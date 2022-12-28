using ETicaretAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Repositories
{
    public interface IReadRepository<T> : IRepository<T> where T : BaseEntity
    {   //sorgu üzerinde çalışıyorsak
        IQueryable<T> GetAll(bool tracking = true);
        IQueryable<T> GetWhere(Expression<Func<T, bool>>method, bool tracking = true);
        //getwhere fonksiyonunda şartlı olan datalr getirilecek verdiğim şart uyuyor doğruysa onu getirecek
       Task<T> GetSingleAsync(Expression<Func<T, bool>>method, bool tracking = true);
        Task<T> GetByIdAsync(string id, bool tracking = true);//Id uygun olan hangisi ise onu getirecek
    }
}
