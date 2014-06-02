using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using BuildSeller.Core.Model;
using BuildSeller.Core.Repository;
using BuildSeller.Infra;
using BuildSeller.Service;

using Newtonsoft.Json;
namespace BuildSeller.Controllers
{
    using System.IO;
    using System.Runtime.Serialization.Json;
    using System.Text;

    using BuildSeller.Core.Service;

    using Castle.Core.Internal;

    public class RealtyTojson
    {
        public string Named { get; set; }

        /// <summary>
        /// Gets or sets the picture.
        /// </summary>
        public byte[] Picture { get; set; }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the square.
        /// </summary>
        public float Square { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is for rent.
        /// </summary>
        public bool IsForRent { get; set; } // otherwise - for Sale

        /// <summary>
        /// Gets or sets a value indicating whether is sold.
        /// </summary>
        public bool IsSold { get; set; }

        /// <summary>
        /// Gets or sets the created.
        /// </summary>
        public DateTime Created { get; set; }
    }
    /*
     *     public IEnumerable<RankTableEntry> Get()
        {
            return entries.ToArray();
        }

        // GET api/values/5
        public string Get(int userId, int authorId, int skip, int take)//IEnumerable<Message>
        {

            var cv =
                JsonConvert.SerializeObject(
                    messageService.Where(
                        v => v.Reciever.Id == userId
                        && v.Sender.Id == authorId
                        || v.Reciever.Id == authorId
                        && v.Sender.Id == userId)
                        .OrderByDescending(v => v.DateSend)
                        .Skip(skip)
                        .Take(take));

            return cv;
        }

        // POST api/values
        //  [HttpPost]
        public HttpResponseMessage Post([FromBody]string value)
        {
            if (value != null)
            {
                var v = JsonConvert.DeserializeObject<Message>(value);

                if (v == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
                messageService.Create(v);
                return Request.CreateResponse(HttpStatusCode.Accepted);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
        public HttpResponseMessage Register([FromBody]string login, [FromBody]string pass )
        {
            if (login != null && pass!= null)
            {

                Users user = new Users
                                 {
                                     Messages = new List<Message>(),
                                     Login = login,
                                     Password = Encryption.Encrypt(pass),
                                     Name = login,
                                     RegisterDateTime = DateTime.Now,
                                 };
             //   var v = JsonConvert.DeserializeObject<Message>(value);

           //     if (v == null)
                {
             //       return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
             //   messageService.Create(v);
                userService.Create(user);

                return Request.CreateResponse(HttpStatusCode.Accepted);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // PUT api/values/5
     * */
    public class BuildApiController : ApiController
    {

        private readonly IRealtyService realtyService;
        private readonly IUserService userService;
        public BuildApiController(IRealtyService realtyServic , IUserService userServic)
        {
            this.realtyService = realtyServic;
            this.userService = userServic; // new RealtyService(IoC.Resolve<IRepo<Realty>>());//realtyServic;
        }
        public string Get(int userId, int authorId, int skip, int take)//IEnumerable<Message>
        {

         /*   var cv =
                JsonConvert.SerializeObject(
                    realtyService.Where(
                        v => v.Reciever.Id == userId
                        && v.Sender.Id == authorId
                        || v.Reciever.Id == authorId
                        && v.Sender.Id == userId)
                        .OrderByDescending(v => v.DateSend)
                        .Skip(skip)
                        .Take(take));

            return cv;*/
            return null;
        }
     
        public IEnumerable<Realty> GetRealties()//IEnumerable<Realty>
        {
         //   var sdf =
        //       JsonConvert.DeserializeObject<Realty>(
           //         "{\"Named\":\"realt\",\"Picture\":\"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\",\"Price\":123.00,\"Square\":123.0,\"MapPosX\":0.0,\"MapPosY\":0.0,\"Address\":\"123\",\"Description\":\"123\",\"IsForRent\":true,\"IsSold\":false,\"Created\":\"2014-06-02T15:03:35.857\",\"BuildCategory\":null,\"Owner\":null,\"Photos\":null,\"Id\":1}");
         //   var g = sdf;
            return realtyService.GetAll().ToArray();
            var b = realtyService.GetAll().FirstOrDefault();
            b.Picture = new byte[1];
           // Realty
            var v = JsonConvert.SerializeObject(b); //, Formatting.Indented





          //  return v;
       }
        // GET api/Books/5
        [Route("{id:int}")]
        [ResponseType(typeof(Realty))]
        public async Task<IHttpActionResult> GetRealty(int id)
        {
            Realty realt =  realtyService.Where(c => c.Id == id).FirstOrDefault();
               
            if (realt == null)
            {
                return NotFound();
            }

            return Ok(realt);
        }


        // GET api/buildapi/5
        public Realty Get(int id)
        {
            return  realtyService.Where(c => c.Id == id).FirstOrDefault();
        }
        public HttpResponseMessage Get(string login, string pass)
        {
            if(login.IsNullOrEmpty() || pass.IsNullOrEmpty())
                return Request.CreateResponse(HttpStatusCode.OK);
            if (userService.Get(login, pass) != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }
        public HttpResponseMessage Post([FromBody]string value)
        {
            if (value != null)
            {
                var user =JsonConvert.DeserializeObject<Users>(value);
                var sdf =
                    JsonConvert.DeserializeObject<Users>(
                        "{\"Activated\":false,\"PaidUser\":true,\"Banned\":true,\"PaidSeller\":true,\"Email\":\"Asd\",\"FirstName\":\"dsccd\",\"LastName\":\"asdfcvf\",\"Login\":\"Addddddddd\",\"LoginHash\":null,\"Password\":\"Asdddddd\",\"Patronymic\":\"scasca\",\"Adress\":\"dsa\",\"Phone\":\"Asd\",\"Likes\":2,\"Dislikes\":1,\"Comments\":\"Asdcdf\",\"UsersLiked\":[],\"RegisterDateTime\":\"2014-06-02T12:03:57.0732605+03:00\",\"Roles\":[],\"Id\":0}");
                var g = sdf;

                if (user == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }  
                if(userService.GetAll().Any(v=>v.Login == user.Login))
                    return  Request.CreateResponse(HttpStatusCode.Conflict);

                userService.Create(user);
              //  messageService.Create(v);
                return Request.CreateResponse(HttpStatusCode.Accepted);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // PUT api/buildapi/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/buildapi/5
        public void Delete(int id)
        {
        }
    }
}
