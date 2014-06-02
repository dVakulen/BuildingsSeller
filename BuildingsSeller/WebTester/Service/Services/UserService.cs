
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using BuildSeller.Core;
using BuildSeller.Core.Model;
using BuildSeller.Core.Repository;
using BuildSeller.Core.Service;

namespace BuildSeller.Service
{

    public class UserService : CrudService<Users>, IUserService
    {

        public UserService(IRepo<Users> repo)
            : base(repo)
        {
        }

        public override int Create(Users user)
        {
            user.Password = Encryption.Encrypt(user.Password);

            user.LoginHash = Guid.NewGuid().ToString();
            user.RegisterDateTime = DateTime.Now;
            return base.Create(user);
        }

        public IList<Users> Where(Expression<Func<Users, bool>> predicate)
        {
            return this.Repo.Where(predicate);
        }

        public bool IsUnique(string login, string email)
        {
            return !this.Repo.Where(o => o.Login == login || o.Email == email).Any();
        }

        public Users Get(string login, string password)
        {
            Users user = this.Repo.Where(o => o.Login == login).SingleOrDefault();
            try
            {
                if (user == null || user.Password != Encryption.Encrypt(password))
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return null;
            }

            return user;
        }

        public Users Get(string login)
        {
            Users user = this.Repo.Where(o => o.Login == login).SingleOrDefault();
            if (user == null)
            {
                user = this.Repo.Where(o => o.Email == login).SingleOrDefault();
            }

            return user;
        }

        public void ChangePassword(int id, string password)
        {
            Users x = this.Repo.Get(id);
            x.Password = Encryption.Encrypt(password);
            this.Repo.Update(x);
            this.Repo.Save();
        }

        public void Update(Users o)
        {
            this.Repo.Update(o);
        }

        public void Update(int id, Users o)
        {
            this.Repo.Update(id, o);
        }
    }
}
