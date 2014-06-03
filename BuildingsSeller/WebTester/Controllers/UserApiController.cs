namespace BuildSeller.Controllers
{
    #region Using Directives

    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using BuildSeller.Core.Model;
    using BuildSeller.Core.Service;
    using BuildSeller.Service;

    using Castle.Core.Internal;

    using Newtonsoft.Json;

    #endregion

    public class UserApiController : ApiController
    {
        #region Fields

        private readonly IRealtyService realtyService;

        private readonly IUserService userService;

        #endregion

        #region Constructors and Destructors

        public UserApiController(IRealtyService realtyServic, IUserService userServic)
        {
            this.realtyService = realtyServic;
            this.userService = userServic;
        }

        #endregion

        #region Public Methods and Operators

        public IHttpActionResult Get(string login, string pass)
        {
            if (login.IsNullOrEmpty() || pass.IsNullOrEmpty())
            {
                return this.NotFound();
            }

            if (this.userService.Get(login, pass) != null)
            {
                return this.Ok();
            }

            return this.NotFound();
        }

        public HttpResponseMessage Post([FromBody] string value)
        {
            // register
            if (value != null)
            {
                var user = JsonConvert.DeserializeObject<Users>(value);

                if (user == null)
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest);
                }

                if (this.userService.GetAll().Any(v => v.Login == user.Login))
                {
                    return this.Request.CreateResponse(HttpStatusCode.Conflict);
                }

                this.userService.Create(user);
                return this.Request.CreateResponse(HttpStatusCode.Accepted);
            }

            return this.Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        #endregion
    }
}