
using System;
using System.Collections.Generic;

namespace BuildSeller.Core.Model
{
    using Newtonsoft.Json;

    [JsonObject]
    public class Users : Entity
    {
        [JsonProperty]
        public virtual bool Activated { get; set; }

        [JsonProperty]
        public virtual bool PaidUser { get; set; }

        [JsonProperty]
        public virtual bool Banned { get; set; }

        [JsonProperty]
        public virtual bool PaidSeller { get; set; }

        [JsonProperty]
        public virtual string Email { get; set; }

        [JsonProperty]
        public virtual string FirstName { get; set; }

        [JsonProperty]
        public virtual string LastName { get; set; }

        [JsonProperty]
        public virtual string Login { get; set; }

        [JsonProperty]
        public virtual string LoginHash { get; set; }

        [JsonProperty]
        public virtual string Password { get; set; }

        [JsonProperty]
        public virtual string Patronymic { get; set; }

        [JsonProperty]
        public virtual string Adress { get; set; }

        [JsonProperty]
        public virtual string Phone { get; set; }

        [JsonProperty]
        public virtual int Likes { get; set; }

        [JsonProperty]
        public virtual int Dislikes { get; set; }

        [JsonProperty]
        public virtual string Comments { get; set; }

        public virtual IList<UserInteraction> UsersLiked { get; set; }

        [JsonProperty]
        public virtual DateTime RegisterDateTime { get; set; }

        public virtual IList<Role> Roles { get; set; }
    }
}
