// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Users.cs" company="nixsolutions">
//   (c) by nix
// </copyright>
// <summary>
//   The users.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Core.Model
{
    using System;
    using System.Collections.Generic;

    public class Users : Entity
    {
        public virtual string Name { get; set; }

        public List<Message> Messages { get; set; }
    
        public virtual string Login { get; set; }


        public virtual string Password { get; set; }

        public virtual DateTime RegisterDateTime { get; set; }
    
    }
}