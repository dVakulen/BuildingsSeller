using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MvcApplication1.Controllers
{
    using Core.Interface;
    using Core.Model;

    using Newtonsoft.Json;

    public class RankTableEntry
    {
        #region Fields

        private Guid id;

        #endregion

        #region Public Properties

       public Guid Id
        {
            get
            {
                return this.id;
            }

            set
            {
                this.id = value;
            }
        }

        public string UserName { get; set; }
       public string TimePassed { get; set; }
        #endregion
    }
    public class ValuesController : ApiController
    {
        private ICrudService<Users> userService;
        public ValuesController(ICrudService<Users> userServ)
        {
            userService = userServ;
            userService.Create(new Users
                                   {
                                       Messages = new List<Message>(),
                                       Login = "Asd",
                                       Name = "svcas",
                                       Password = "Asc",
                                       RegisterDateTime = DateTime.Now
                                   });
            var z = userService.GetAll();
            var b = z;
        }
        // GET api/values
      static  List<RankTableEntry>  entries = new List<RankTableEntry>
                                            {
                                                new RankTableEntry { Id = Guid.NewGuid(), TimePassed = "3421", UserName = "fsda" },
                                                new RankTableEntry { Id = Guid.NewGuid(), TimePassed = "12321", UserName = "AAA" }
                                            };
        public IEnumerable<RankTableEntry> Get()
        {
            return entries.ToArray();
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
      //  [HttpPost]
        public HttpResponseMessage Post([FromBody]string value)
        {
            if (value != null)
            {
                var v = JsonConvert.DeserializeObject<RankTableEntry>(value);
       
                entries.Add(v);
                return Request.CreateResponse(HttpStatusCode.Accepted);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}