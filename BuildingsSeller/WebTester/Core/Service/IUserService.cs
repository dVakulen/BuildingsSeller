
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using BuildSeller.Core.Model;

namespace BuildSeller.Core.Service
{

    public interface IUserService : ICrudService<Users>
    {

        void ChangePassword(int id, string password);

        Users Get(string login, string password);

        Users Get(string login);

        bool IsUnique(string login, string email);

        void Update(int id, Users user);

        IList<Users> Where(Expression<Func<Users, bool>> predicate);
    }
}
