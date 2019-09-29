using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using Market.GateWayLayer.Rest.Common;
using Market.GateWayLayer.Rest.Models;
using Market.InfrastructureLayer.Models;
using Microsoft.Owin.Security.Provider;

namespace Market.GateWayLayer.Rest.Controllers
{
    [RoutePrefix("api/categories")]
    public class CategoryController : ApiController
    {
        [HttpGet]
        [Authorize]
        [Route]
        public HttpResponseMessage GetAll()
        {
            IEnumerable<Category> categories = Repository.CategoryRepository.GetAll();
            return Request.CreateResponse(HttpStatusCode.OK, categories);
        }

        [HttpGet]
        [Authorize]
        [Route("{id}")]
        public HttpResponseMessage Get(int id)
        {
            Category category = Repository.CategoryRepository.FindById(id);
            return Request.CreateResponse(HttpStatusCode.OK, category);
        }

        [HttpPost]
        [Authorize(Roles = "administrator")]
        [Route]
        public HttpResponseMessage Create(CategoryJson categoryJson)
        {
            var category = new Category
            {
                Name = categoryJson.Name
            };
            category = Repository.CategoryRepository.Add(category);

            byte[] imageData = Convert.FromBase64String(categoryJson.ImageData);
            string subPath = "photos/category/" + category.Id + "." + categoryJson.ImageType;
            string imagePath = HttpContext.Current.Server.MapPath("~/" + subPath);
            File.WriteAllBytes(imagePath, imageData);
            category.ImageUrlRelative = subPath;
            category = Repository.CategoryRepository.Update(category);
            return Request.CreateResponse(HttpStatusCode.OK, category);
        }

        [HttpPut]
        [Authorize(Roles = "administrator")]
        [Route]
        public HttpResponseMessage Update(CategoryJson categoryJson)
        {
            if (categoryJson.Id == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Parameter 'Id' cannot be null");

            Category category = Repository.CategoryRepository.FindById(categoryJson.Id.Value);
            if (category == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    "Category with id '" + category.Id.Value + "' not found!");

            category.Name = categoryJson.Name;
            if (!string.IsNullOrEmpty(categoryJson.ImageData))
            {
                byte[] imageData = Convert.FromBase64String(categoryJson.ImageData);
                string subPath = "photos/category/" + categoryJson.Id + "." + categoryJson.ImageType;
                string imagePath = HttpContext.Current.Server.MapPath("~/" + subPath);
                File.WriteAllBytes(imagePath, imageData);
                category.ImageUrlRelative = subPath;
            }
            category = Repository.CategoryRepository.Update(category);
            return Request.CreateResponse(HttpStatusCode.OK, category);
        }

        [HttpDelete]
        [Authorize(Roles = "administrator")]
        [Route("{id}")]
        public HttpResponseMessage Delete(int id)
        {
            bool succeed = Repository.CategoryRepository.Remove(id);
            return Request.CreateResponse(HttpStatusCode.OK, succeed);
        }
    }
}