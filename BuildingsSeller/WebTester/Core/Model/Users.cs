
using System;
using System.Collections.Generic;

namespace BuildSeller.Core.Model
{

    public class Users : Entity
    {

        public virtual bool Activated { get; set; }

        public virtual bool PaidUser { get; set; }

        public virtual bool Banned { get; set; }

        public virtual bool PaidSeller { get; set; }

        public virtual string Email { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual string Login { get; set; }

        public virtual string LoginHash { get; set; }

        public virtual string Password { get; set; }

        public virtual string Patronymic { get; set; }

        public virtual string Adress { get; set; }

        public virtual string Phone { get; set; }

        public virtual int Likes { get; set; }

        public virtual int Dislikes { get; set; }

        public virtual string Comments { get; set; }

        public virtual IList<UserInteraction> UsersLiked { get; set; }

        public virtual DateTime RegisterDateTime { get; set; }

        public virtual IList<Role> Roles { get; set; }
    }
}
