﻿using ETicaretAPI.Application.Abstractions.Storage;
using ETicaretAPI.Application.Features.Commands.Product.CreateProduct;
using ETicaretAPI.Application.Features.Commands.Product.RemoveProduct;
using ETicaretAPI.Application.Features.Commands.Product.UpdateProduct;
using ETicaretAPI.Application.Features.Commands.ProductImageFile.RemoveProductImage;
using ETicaretAPI.Application.Features.Commands.ProductImageFile.UploadProductImage;
using ETicaretAPI.Application.Features.Queries.Product.GetAllProduct;
using ETicaretAPI.Application.Features.Queries.Product.GetByIdProduct;
using ETicaretAPI.Application.Features.Queries.ProductImageFile.GetProductImages;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.RequestParameters;
//using ETicaretAPI.Application.Services;
using ETicaretAPI.Application.ViewModels.Products;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Persistence.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(AuthenticationSchemes="Admin")]
    public class ProductsController : ControllerBase
     {       
        readonly IMediator _mediator;
        public ProductsController(         
         
            IMediator mediator){
           
            _mediator = mediator;
        }



        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllProductQueryRequest getAllProductQueryRequest)
            => Ok(await _mediator.Send(getAllProductQueryRequest));

        [HttpGet("{Id}")]

        public async Task<IActionResult> Get([FromRoute]GetByIdProductQueryRequest getByIdProductQueryRequest)
        {
          GetByIdProductQueryResponse response= await  _mediator.Send(getByIdProductQueryRequest);
            return Ok(response);
        }



        [HttpPost]
        public async Task<IActionResult> Post(CreateProductCommandRequest createProductCommandRequest)
        {
            CreateProductCommandResponse response = await _mediator.Send(createProductCommandRequest);
            return StatusCode((int)HttpStatusCode.Created);
        }


        [HttpPut]
        public async Task<IActionResult> Put([FromBody]UpdateProductCommandRequest updateProductCommandRequest) 
        {
            UpdateProductCommandResponse response = await _mediator.Send(updateProductCommandRequest);
            return Ok();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute]RemoveProductCommandRequest removeProductCommandRequest)
        {
            RemoveProductCommandResponse response =await _mediator.Send(removeProductCommandRequest);
            return Ok();
        }

        //https://...api/controller/action
        [HttpPost("[action]")]      
        public async Task<IActionResult> Upload([FromQuery] UploadProductImageCommandRequest uploadProductImageCommandRequest)
        {
            uploadProductImageCommandRequest.Files = Request.Form.Files;
          UploadProductImageCommandResponse response= await _mediator.Send(uploadProductImageCommandRequest);
            return Ok();
        }

        [HttpGet("[action]/{Id}")]
        public async Task<IActionResult> GetProductImages([FromRoute]GetProductImagesQueryRequest getProductImagesQueryRequest)
        {
        List<GetProductImagesQueryResponse> response = await  _mediator.Send(getProductImagesQueryRequest);
            return Ok(response);
        }
        

        [HttpDelete("[action]/{Id}")]
        public async Task<IActionResult> DeleteProductImage([FromRoute] RemoveProductImageCommandRequest  removeProductImageCommandRequest, [FromQuery] string imageId)
        {
            removeProductImageCommandRequest.ImageId = imageId;
            RemoveProductImageCommandResponse response = await _mediator.Send(removeProductImageCommandRequest);
            return Ok();
        }

    }
}