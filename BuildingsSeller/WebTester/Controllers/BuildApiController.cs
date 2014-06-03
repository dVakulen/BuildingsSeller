namespace BuildSeller.Controllers
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;

    using BuildSeller.Core.Model;
    using BuildSeller.Core.Service;
    using BuildSeller.Service;

    using Castle.Core.Internal;

    using Newtonsoft.Json;

    #endregion

    public class BuildApiController : ApiController
    {
        #region Fields

        private readonly IRealtyService realtyService;

        private readonly IUserService userService;

        #endregion

        #region Constructors and Destructors

        public BuildApiController(IRealtyService realtyServic, IUserService userServic)
        {
            this.realtyService = realtyServic;
            this.userService = userServic;
        }

        #endregion

        #region Public Methods and Operators

        public void Delete(int id)
        {
        }

        // GET api/buildapi/5
        public Realty Get(int id)
        {
            return this.realtyService.Where(c => c.Id == id).FirstOrDefault();
        }

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

        public IEnumerable<Realty> GetPartOfRealties(int skip, int take)
        {
            return
                this.realtyService.GetAllIncluding(v => v.Owner)
                    .OrderByDescending(v => v.Created)
                    .Skip(skip)
                    .Take(take)
                    .ToArray();
        }

        public IEnumerable<Realty> GetRealties()
        {
            return this.realtyService.GetAllIncluding(v => v.Owner).OrderByDescending(v => v.Created).ToArray();
        }

        [Route("{id:int}")]
        [ResponseType(typeof(Realty))]
        public async Task<IHttpActionResult> GetRealty(int id)
        {
            Realty realt = this.realtyService.Where(c => c.Id == id).FirstOrDefault();

            if (realt == null)
            {
                return this.NotFound();
            }

            return this.Ok(realt);
        }

        public HttpResponseMessage Post([FromBody] string value)
        {
            // register
            if (value != null)
            {
                var realt = JsonConvert.DeserializeObject<Realty>(value);

                if (realt == null)
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest);
                }

                this.realtyService.Create(realt);
                return this.Request.CreateResponse(HttpStatusCode.Accepted);
            }

            return this.Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        // PUT api/buildapi/5
        public void Put(int id, [FromBody] string value)
        {
        }

        #endregion

        // DELETE api/buildapi/5
    }
}