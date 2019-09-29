using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.ModelBinding;
using Market.GateWayLayer.Rest.Common;
using Market.GateWayLayer.Rest.Models;
using Market.InfrastructureLayer.Common;
using Market.InfrastructureLayer.Models;

namespace Market.GateWayLayer.Rest.Controllers
{
    [RoutePrefix("api/products")]
    public class ProductController : ApiController
    {
        [HttpGet]
        [Authorize]
        [Route]
        public HttpResponseMessage Get(int? categoryId = null, int? page = null, int? pageSize = null)
        {
            if (categoryId == null)
            {
                if (page == null && pageSize == null)
                {
                    IEnumerable<Product> products = Repository.ProductRepository.GetAll();
                    return Request.CreateResponse(HttpStatusCode.OK, products);
                }
                PageResult<Product> productsPage = Repository.ProductRepository.GetProducts(
                    null, page, pageSize);
                return Request.CreateResponse(HttpStatusCode.OK, productsPage);
            }
            if (page == null && pageSize == null)
            {
                PageResult<Product> productsPage = Repository.ProductRepository.GetProducts(categoryId);
                return Request.CreateResponse(HttpStatusCode.OK, productsPage.Items);
            }
            else
            {
                PageResult<Product> productsPage = Repository.ProductRepository.GetProducts(categoryId,
                    page, pageSize);
                return Request.CreateResponse(HttpStatusCode.OK, productsPage);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("{id}")]
        public HttpResponseMessage Get(long id)
        {
            Product product = Repository.ProductRepository.FindById(id);
            return Request.CreateResponse(HttpStatusCode.OK, product);
        }

        [HttpPost]
        [Authorize(Roles = "administrator")]
        [Route]
        public HttpResponseMessage Create(ProductJson productJson)
        {

            try
            {
                byte[] imageData = Convert.FromBase64String(productJson.ImageData);

                var product = new Product
                {
                    Name = productJson.Name,
                    Count = productJson.Count,
                    Price = productJson.Price
                };
                Category category = Repository.CategoryRepository.FindById(productJson.CategoryId);
                product.Category = category;
                product = Repository.ProductRepository.Add(product);
                string subPath = "photos/product/" + product.Id + "." + productJson.ImageType;
                string imagePath = HttpContext.Current.Server.MapPath("~/" + subPath);
                File.WriteAllBytes(imagePath, imageData);
                product.ImageUrlRelative = subPath;
                product = Repository.ProductRepository.Update(product);
                return Request.CreateResponse(HttpStatusCode.OK, product);
            }
            catch (Exception ex)
            {
                FileLogger.Log(ex.ToString());
                throw;
            }
        }

        [HttpPut]
        [Authorize(Roles = "administrator")]
        [Route]
        public HttpResponseMessage Update(ProductJson productJson)
        {
            if (productJson.Id == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Parameter 'Id' cannot be null");

            var product = Repository.ProductRepository.FindById(productJson.Id.Value);
            if (product == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    "Product with id '" + productJson.Id.Value + "' not found!");

            product.Name = productJson.Name;
            product.Count = productJson.Count;
            product.Price = productJson.Price;
            Category category = Repository.CategoryRepository.FindById(productJson.CategoryId);
            product.Category = category;
            if (!string.IsNullOrEmpty(productJson.ImageData))
            {
                byte[] imageData = Convert.FromBase64String(productJson.ImageData);
                string subPath = "photos/product/" + product.Id + "." + productJson.ImageType;
                string imagePath = HttpContext.Current.Server.MapPath("~/" + subPath);
                File.WriteAllBytes(imagePath, imageData);
                product.ImageUrlRelative = subPath;
            }
            product = Repository.ProductRepository.Update(product);
            return Request.CreateResponse(HttpStatusCode.OK, product);
        }

        [HttpDelete]
        [Authorize(Roles = "administrator")]
        [Route("{id}")]
        public HttpResponseMessage Delete(int id)
        {
            bool succeed = Repository.ProductRepository.Remove(id);
            return Request.CreateResponse(HttpStatusCode.OK, succeed);
        }
    }
}