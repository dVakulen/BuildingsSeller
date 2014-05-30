using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
  public  class Message : Entity
    {
        public virtual string Content { get; set; }

        public List<string> Messages { get; set; }
        public virtual Users Sender { get; set; }
        public virtual Users Reciever { get; set; }
        public virtual DateTime DateSend { get; set; }

    }
}
