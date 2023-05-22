using ETicaretAPI.Application.Features.Queries.ProductImageFile.GetProductImages;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.RequestParameters;
using ETicaretAPI.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.Product.GetAllProduct
{
    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQueryRequest, GetAllProductQueryResponse>
    {
        readonly IProductReadRepository _productReadRepository;
        readonly ILogger<GetAllProductQueryHandler> _logger;
        private readonly IConfiguration configuration;

        public GetAllProductQueryHandler(IProductReadRepository productReadRepository, ILogger<GetAllProductQueryHandler> logger, IConfiguration configuration)
        {
            this._productReadRepository = productReadRepository;
            _logger = logger;
            this.configuration = configuration;
        }
        public async Task<GetAllProductQueryResponse> Handle(GetAllProductQueryRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("get All products");
          
            var totalProductCount = _productReadRepository.GetAll(false).Count();
            var products = _productReadRepository.GetAll(false).Skip(request.Page * request.Size).Take(request.Size).Include(p=>p.ProductImageFiles)
                .Select(p => new
            {
                p.Id,
                p.Name,
                p.Stock,
                p.Price,
                p.CreatedDate,
                p.UpdatedDate,
                ProductImageFiles = p.ProductImageFiles.Select(p => new GetProductImagesQueryResponse
                {
                    Path = p.Storage == "LocalStorage" ? GetBase64($"{configuration["BaseStorageUrl"]}/{p.Path}") : $"{configuration["BaseStorageUrl"]}/{p.Path}",
                    Showcase = p.Showcase
                }).ToList()
                }).ToList();

            return new()
            {
                Products = products,
                TotalProductCount = totalProductCount
            };

                    
        }

        private static string GetBase64(string path)
        {
            var fileData = System.IO.File.ReadAllBytes(path);

            if (fileData == null)
                return String.Empty;

            string base64String = Convert.ToBase64String(fileData, 0, fileData.Length);
            return "data:image/png;base64," + base64String;
        }

    }
}
