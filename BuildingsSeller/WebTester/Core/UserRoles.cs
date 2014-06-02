
using System.Collections.Generic;

namespace BuildSeller.Core
{

public class UsersRoles
{
public const string Administrator = "Administrator";

public const string Seller = "Seller";

public const string User = "User";

public static readonly Dictionary<string, int> RolesRank = new Dictionary<string, int>
{
{ User, 3 },
{ Seller, 3 },
{ Administrator, 7 }
};
}
}
