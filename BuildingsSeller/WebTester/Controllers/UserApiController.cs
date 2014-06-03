using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BuildSeller.Controllers
{
    using BuildSeller.Core.Model;
    using BuildSeller.Core.Service;
    using BuildSeller.Service;

    using Castle.Core.Internal;

    using Newtonsoft.Json;

    public class UserApiController : ApiController
    {
        private readonly IRealtyService realtyService;
        private readonly IUserService userService;
        public UserApiController(IRealtyService realtyServic, IUserService userServic)
        {
            this.realtyService = realtyServic;
            this.userService = userServic;
        }
        public IHttpActionResult Get(string login, string pass)
        {
            if (login.IsNullOrEmpty() || pass.IsNullOrEmpty())
                return NotFound();
            if (userService.Get(login, pass) != null)
            {
                return Ok();
            }

            return NotFound();
        }
        public HttpResponseMessage Post([FromBody]string value) //register
        {
            if (value != null)
            {
                var user = JsonConvert.DeserializeObject<Users>(value);


                if (user == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
                if (userService.GetAll().Any(v => v.Login == user.Login))
                    return Request.CreateResponse(HttpStatusCode.Conflict);

                userService.Create(user);
                return Request.CreateResponse(HttpStatusCode.Accepted);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}
