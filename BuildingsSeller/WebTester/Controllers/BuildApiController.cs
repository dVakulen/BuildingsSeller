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

namespace BuildSeller.Controllers
{
    using Newtonsoft.Json;

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

        public BuildApiController()//IRealtyService realtyServic
        {
            this.realtyService =  new RealtyService(IoC.Resolve<IRepo<Realty>>());//realtyServic;
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
     
        public IEnumerable<Realty> GetBooks()
       {
           return realtyService.GetAll();
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

        // POST api/buildapi
        public void Post([FromBody]string value)
        {
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
