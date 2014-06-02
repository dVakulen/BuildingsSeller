using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
namespace MvcApplication1.Controllers
{
    using Core;
    using Core.Interface;
    using Core.Model;

    using Newtonsoft.Json;

    using WebGrease.Activities;

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

        private ICrudService<Message> messageService;

        private void test()
        {
            var user = userService.Where(c => c.Id == 1).FirstOrDefault();
            var user2 = userService.Where(v => v.Id == 2).FirstOrDefault();

            messageService.Create(new Message
                                      {
                                          Sender = user,
                                          Reciever = user2,
                                          DateSend = DateTime.Now,
                                          Content = "zzzz",

                                      });
            messageService.Create(new Message
            {
                Sender = user2,
                Reciever = user,
                DateSend = DateTime.Now,
                Content = "cxzzx",

            });
        }
        public ValuesController(ICrudService<Users> userServ, ICrudService<Message> messageServic)
        {
            userService = userServ;
            messageService = messageServic;
            //      test();
            /*   userService.Create(new Users
                                   {
                                       Messages = new List<Message>
                                                      {
                                                          {
                                                              new Message
                                                                  {
                                                                      DateSend = DateTime.Now,
                                                                      Content = "adddd",
                                                                      
                                                                  }
                                                          } 
                                                      },
                                       Login = "As32d",
                                       Name = "svc2as",
                                       Password = "As1c",
                                       RegisterDateTime = DateTime.Now
                                   });
            var z = userService.GetAll().ToList();
            var b = z;*/
        }
        // GET api/values
        static List<RankTableEntry> entries = new List<RankTableEntry>
                                            {
                                                new RankTableEntry { Id = Guid.NewGuid(), TimePassed = "3421", UserName = "fsda" },
                                                new RankTableEntry { Id = Guid.NewGuid(), TimePassed = "12321", UserName = "AAA" }
                                            };
        public IEnumerable<RankTableEntry> Get()
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
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}