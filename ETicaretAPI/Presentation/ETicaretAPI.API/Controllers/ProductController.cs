using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase

    {
        readonly private IProductWriteRepository _productWriteRepository;
        readonly private IProductReadRepository _productReadRepository;

        readonly private IOrderWriteRepository _orderWriteRepository;
        readonly private IOrderReadRepository _orderReadRepository;

        readonly private ICustomerWriteRepository _customerWriteRepository;
        readonly private ICustomerReadRepository _customerReadRepository;




        public ProductController(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository, IOrderWriteRepository orderWriteRepository,IOrderReadRepository orderReadRepository, ICustomerWriteRepository customerWriteRepository, ICustomerReadRepository customerReadRepository)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;

            _orderReadRepository = orderReadRepository;
            _orderWriteRepository= orderWriteRepository;

            _customerReadRepository = customerReadRepository;
            _customerWriteRepository = customerWriteRepository;

        }
        [HttpGet]
        public async Task Get() {

            //{
            //    await _productWriteRepository.AddRangeAsync(new()
            //    {
            //        new() {Id = Guid.NewGuid(), Name="Product 1", Price=100, CreateDate = DateTime.UtcNow, Stock=10 },
            //         new() {Id = Guid.NewGuid(), Name="Product 2", Price=200, CreateDate = DateTime.UtcNow, Stock=20 },
            //          new() {Id = Guid.NewGuid(), Name="Product 3", Price=300, CreateDate = DateTime.UtcNow, Stock=30 },
            //    });
            //    await _productWriteRepository.SaveAsync();


            //tracking deneme 
            //Product p = await _productReadRepository.GetByIdAsync("40ac979c-e990-41da-a89d-8970b1725edc");
            //p.Name = "Ali";
            //await _productWriteRepository.SaveAsync();
            //await _productWriteRepository.AddAsync(new()
            //{
            //    Name = "C product", Price =1.500F , Stock = 10 ,CreateDate = DateTime.UtcNow 

            //});
            //await _productWriteRepository.SaveAsync();
            //Product p = await _productReadRepository.GetByIdAsync("40ac979c-e990-41da-a89d-8970b1725edc",false);
            //p.Name = "ali";
            //p.UpdateDate = DateTime.UtcNow;
            //await _productWriteRepository.SaveAsync();


            //orderla iliişkili customer olduğu için bir tane mecbur customer üretmemiz lazım
            //var customerId = Guid.NewGuid();
            //await _customerWriteRepository.AddAsync(new()
            //{
            //    Id = customerId,
            //    Name = "ebru"
            //});

            //await _orderWriteRepository.AddAsync(new()
            //{

            //    Description = "bdfikdafgdaf ", Adress = "ankara çankaya", CustomerId = customerId 


            //});

            //await _orderWriteRepository.AddAsync(new()
            //{

            //    Description = "ıorgjsdkgjdsfkg ",
            //    Adress = "ankara pursaklar",
            //    CustomerId = customerId


            //}) ;
            //await _orderWriteRepository.SaveAsync();


            Order order = await _orderReadRepository.GetByIdAsync("2a60430b-82c0-4b29-a817-e5550e76fcb1", false);
                order.Adress = "çankaya dikmen ";

            _orderWriteRepository.Update(order);
            await _orderWriteRepository.SaveAsync();









        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
           Product product= await _productReadRepository.GetByIdAsync(id);
            return Ok(product);
        }
       
    }
}
