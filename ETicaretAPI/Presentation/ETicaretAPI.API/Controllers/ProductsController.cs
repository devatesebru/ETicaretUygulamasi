using ETicaretAPI.Application.Abstractions.Storage;
using ETicaretAPI.Application.Features.Commands.CreateProduct;
using ETicaretAPI.Application.Features.Queries.GetAllProduct;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.RequestParameters;
//using ETicaretAPI.Application.Services;
using ETicaretAPI.Application.ViewModels.Products;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Persistence.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase

    {
        readonly private IProductWriteRepository _productWriteRepository;
        readonly private IProductReadRepository _productReadRepository;
        //readonly IFileService _fileService;
        readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
        readonly IInvoiceFileWriteRepository _invoiceFileWriteRepository;
        readonly IStorageService _storageService;
        readonly IConfiguration configuration;



        readonly IMediator _mediator;
        public ProductsController(
            IProductWriteRepository productWriteRepository,
            IProductReadRepository productReadRepository,
            //IFileService fileService,
            IProductImageFileWriteRepository productImageFileWriteRepository,
            IInvoiceFileWriteRepository invoiceFileWriteRepository,
            IStorageService storageService,
            IConfiguration configuration,
            IMediator mediator)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            //this._fileService = fileService;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _invoiceFileWriteRepository = invoiceFileWriteRepository;
            _storageService = storageService;
            this.configuration = configuration;
            _mediator = mediator;
        }



        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllProductQueryRequest getAllProductQueryRequest)
            => Ok(await _mediator.Send(getAllProductQueryRequest));

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return Ok(await _productReadRepository.GetByIdAsync(id, false));
        }



        [HttpPost]
        public async Task<IActionResult> Post(CreateProductCommandRequest createProductCommandRequest)
        {
            CreateProductCommandResponse response = await _mediator.Send(createProductCommandRequest);
            return StatusCode((int)HttpStatusCode.Created);
        }


        [HttpPut]
        public async Task<IActionResult> Put(VM_Update_Product model)
        {
            Product product = await _productReadRepository.GetByIdAsync(model.Id);
            product.Stock = model.Stock;
            product.Name = model.Name;
            product.Price = model.Price;
            await _productWriteRepository.SaveAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _productWriteRepository.RemoveAsync(id);
            await _productWriteRepository.SaveAsync();
            return Ok();
        }

        [HttpPost("[action]")]
        //https://...api/controller/action
        public async Task<IActionResult> Upload(string id)
        {         
          List<(string fileName, string pathOrContainerName)> result= await  _storageService.UploadAsync("photo-images", Request.Form.Files);

            Product product = await _productReadRepository.GetByIdAsync(id);

            //foreach (var r in result)
            //{
            //    product.ProductImageFiles.Add(new()
            //    {
            //        FileName = r.fileName,
            //        Path = r.pathOrContainerName,
            //        Storage=_storageService.StorageName,
            //        Products=new List<Product>() { product }
            //    });
            //}

            await _productImageFileWriteRepository.AddRangeAsync(result.Select(r => new ProductImageFile
            {
                FileName = r.fileName,
                Path = r.pathOrContainerName,
                Storage = _storageService.StorageName,
                Products = new List<Product>() { product }

            }).ToList());
            await _productImageFileWriteRepository.SaveAsync();
            return Ok();
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetProductImages(string id)
        {
          Product? product = await  _productReadRepository.Table.Include(p => p.ProductImageFiles).FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));
            return Ok(product.ProductImageFiles.Select(p => new
            {
              Path= p.Storage == "LocalStorage" ? GetBase64($"{configuration["BaseStorageUrl"]}/{p.Path}") : $"{configuration["BaseStorageUrl"]}/{p.Path}",
                p.FileName,
                p.Id
            }));
        }

        private string GetBase64(string path)
        {
            var fileData = System.IO.File.ReadAllBytes(path);

            if (fileData == null)
                return String.Empty;

            string base64String = Convert.ToBase64String(fileData, 0, fileData.Length);
            return "data:image/png;base64," + base64String;
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteProductImage(string id, string imageId)
        {

            Product? product = await _productReadRepository.Table.Include(p => p.ProductImageFiles).FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));
          ProductImageFile  productImageFile= product.ProductImageFiles.FirstOrDefault(p => p.Id == Guid.Parse(imageId));
            product.ProductImageFiles.Remove(productImageFile);
          await  _productWriteRepository.SaveAsync();
            return Ok();
        }

    }
}