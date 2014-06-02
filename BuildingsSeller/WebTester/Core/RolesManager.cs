
using System;
using System.Linq;
using BuildSeller.Core.Model;
using BuildSeller.Core.Service;

namespace BuildSeller.Core
{

    public static class RolesManager
    {

        private static IUserService userServic;

        public static IUserService userService
        {
            get { return userServic; }
            set
            {
                if (userServic == null)
                {
                    userServic = value;
                }
            }
        }

        public static bool IsUserInRole(Users user, string roleString)
        {
            if (user.Roles == null)
            {
                return false;
            }

            foreach (Role role in user.Roles)
            {
                if (role.Name == roleString)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsUserInRole(string username, string roleString)
        {
            if (!string.IsNullOrEmpty(username))
            {
                if (userService == null) return true;
                Users user = userService.Get(username);
                if (user == null || user.Roles == null)
                {
                    return false;
                }

                foreach (Role role in user.Roles)
                {
                    if (role.Name == roleString)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static string[] GetRolesForUser(Users user)
        {
            var roles = new string[user.Roles.Count];
            for (int i = 0; i < user.Roles.Count; i++)
            {
                roles[i] = user.Roles.ElementAt(i).Name;
            }

            return roles;
        }

        public static string[] GetAllRoles()
        {
            var roles = new string[UsersRoles.RolesRank.Count];
            for (int i = 0; i < UsersRoles.RolesRank.Count; i++)
            {
                roles[i] = UsersRoles.RolesRank.ElementAt(i).Key;
            }

            return roles;
        }

        public static bool AddUserToRole(this Users user, string roleString)
        {
            if (user.Roles == null)
            {
                return false;
            }

            if (user.Roles.Where(x => x.Name == roleString).FirstOrDefault() != null)
            {
                return false;
            }

            user.Roles.Add(new Role
            {
                Name = roleString
            });

            return true;
        }

        public static bool IsUserPaid(string username)
        {
            if (userService == null) return true;
            if (String.IsNullOrEmpty(username))
            {
                return false;
            }
            Users user = userServic.Get(username);
            if (user == null)
            {
                return false;
            }

            return user.PaidUser;
        }

        public static bool IsSellerPaid(string username)
        {
            if (userService == null) return true;
            if (String.IsNullOrEmpty(username))
            {
                return false;
            }
            Users user = userServic.Get(username);
            if (user == null)
            {
                return false;
            }

            return user.PaidSeller;
        }

        public static bool RemoveUserFromRole(this Users user, string roleString)
        {
            if (user.Roles == null)
            {
                return false;
            }

            if (user.Roles.Where(x => x.Name == roleString).FirstOrDefault() == null)
            {
                return false;
            }

            user.Roles.Remove(user.Roles.Where(x => x.Name == roleString).FirstOrDefault());
            return true;
        }
    }
}
