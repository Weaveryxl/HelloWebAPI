using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using HelloWebAPI.Models;

namespace HelloWebAPI.Controllers
{
    /* 
        1. ApiController is the base class used for all API Controllers in Web API
        2. The methods in the controller return raw objects rather than views (or other action results).
        3. MVC controllers always dispatch to actions by name, Web API controllers by default dis-
           patch to actions by HTTP verb. Although you can use verb override attributes such as  [HttpGet] or
           [HttpPost] , most of your verb-based actions will probably follow the pattern of starting the action
           name with the verb name. The action methods in the sample controller are named directly after the
           verb, but they could also have just started with the verb name (meaning  Get and  GetValues are
           both reachable with the  GET verb).
        4. The  ExecuteAsync method on  ApiController comes from  IHttpController , and as you would
           expect by its name, it means that all Web API controllers are asynchronous by design. You have no
           need for a separate class for sync versus async actions when you use Web API.  
        5. By default, Web API will assume that parameters that are simple types (that is, the intrinsic types,
           strings, dates, times, and anything with a type converter from strings) are taken from non-body values,
           and complex types (everything else) are taken from the body. An additional restriction exists as well:
           Only a single value can come from the body, and that value must represent the entirety of the body.
        6. Incoming parameters that are not part of the body are handled by a model binding system that is
           similar to the one included in MVC. Incoming and outgoing bodies, on the other hand, are handled
           by a brand-new concept called formatters. 
     */

    public class ProductsController : ApiController
    {
        private readonly Product[] _products = new Product[]
        {
            new Product {Id = 1, Name = "Tomato Soup", Category = "Groceries", Price = 1},
            new Product {Id = 2, Name = "Yo-yo", Category = "Toys", Price = 3.75M},
            new Product {Id = 3, Name = "Hammer", Category = "Hardware", Price = 16.99M},
            new Product {Id = 4, Name = "Hammer", Category = "Hardware", Price = 16.99M},
            new Product {Id = 5, Name = "Hammer", Category = "Hardware", Price = 16.99M},
            new Product {Id = 6, Name = "Hammer", Category = "Hardware", Price = 16.99M},
        };

        /*
          Accept header is used by HTTP clients to tell the server what content types they'll accept.
          The server will then send back a response, which will include a Content-Type header telling the client
          what the content type of the returned content actually is. 
         *
         HTTP requests can also contain Content-Type headers. Why? Well, think about POST or PUT requests.
         With those request types, the client is actually sending a bunch of data to the server as part of the request,
         and the Content-Type header tells the server what the data actually is (and thus determines how the server will parse it).
         */

        [HttpGet]
        [Route("api/Products")]
        public IEnumerable<Product> GetAllProducts()
        {
            return _products;
        }


        [HttpGet]
        [Route("api/HammerProducts")]
        public IEnumerable<Product> GetAllProductsWithHammerName()
        {
            return _products.Where(p => p.Name == "Hammer");
        }

        [Route("api/Products/{id}")]
        [HttpGet]
        public IHttpActionResult GetProduct(int id)
        {
            var product = _products.FirstOrDefault((p) => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [Route("api/Products/getall")]
        [HttpGet]
        public HttpResponseMessage Get()
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, "value");
            response.Content = new StringContent("hello", Encoding.Unicode);
            response.Headers.CacheControl = new CacheControlHeaderValue()
            {
                MaxAge = TimeSpan.FromMinutes(20)
            };
            return response;
        }

        [Route("api/Products/getdata")]
        [HttpGet]
        public HttpResponseMessage GetDataProducts()
        {
            // Get a list of products from a database.
            IEnumerable<Product> products = this._products;

            // Write the list to the response body.
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, products);
            return response;
        }

        /*
         * By default, Web API uses the following rules to bind parameters:

        If the parameter is a "simple" type, Web API tries to get the value from the URI. Simple types include
        the .NET primitive types (int, bool, double, and so forth), plus TimeSpan, DateTime, Guid, decimal, and string, 
        plus any type with a type converter that can convert from a string. (More about type converters later.)
       
        For complex types, Web API tries to read the value from the message body, using a media-type formatter.
         
         */

        [Route("api/SaveId/")]
        [HttpPost]
        public void Post([FromBody] string productId)
        {
        }


        //Can't bind multiple parameters ('productId' and 'name') to the request's content

        [Route("api/SaveMultiple/")]
        [HttpPost]
        public void PostMultiple([FromBody] string productId, [FromBody] string name)
        {
        }

        [Route("api/Product/")]
        [HttpPost]
        public HttpResponseMessage SaveProduct(Product prod)
        {
            return new HttpResponseMessage()
            {
                Content = new StringContent("POST: Test message")
            };
        }

        [Route("api/update/")]
        [HttpPut]
        public HttpResponseMessage Put(int id, Product item)
        {
            return new HttpResponseMessage()
            {
                Content = new StringContent("PUT: Test message")
            };
        }
    }
}