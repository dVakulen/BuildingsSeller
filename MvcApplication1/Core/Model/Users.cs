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

    /// <summary>
    ///     The users.
    /// </summary>
    public class Users : Entity
    {
        public virtual string Name { get; set; }

        public List<Message> Messages { get; set; }
        /// <summary>
        ///     Gets or sets the last name.
    
        public virtual string Login { get; set; }


        /// <summary>
        ///     Gets or sets the password.
        /// </summary>
        public virtual string Password { get; set; }

        public virtual DateTime RegisterDateTime { get; set; }

        /// <summary>
        ///     Gets or sets the roles.
        /// </summary>
    }
}