using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Runtime.Caching;
using TFIP.Common.Helpers;
using TFIP.Common.Logging;

namespace TFIP.Business.Services.ActiveDirectory
{
    public static class ActiveDirectoryHelper
    {
        #region Implementation of ActiveDirectoryService

        private static List<ActiveDirectoryUser> GetAllMembers(this GroupPrincipal groupPrincipal)
        {
#if DEBUG
            var users = groupPrincipal.Members
                .Where(it => !string.IsNullOrEmpty(it.Name))
                .Select(CreateActiveDirectoryUser)
                .ToList();

            groupPrincipal.Dispose();
            return users;
#else
            var users = new List<ActiveDirectoryUser>();

            foreach (var group in groupPrincipal.GetMembers(false))
            {
                if (group.StructuralObjectClass.Equals("group"))
                {
                    users.AddRange(((GroupPrincipal)group).GetAllMembers());
                }
                else
                {
                    users.Add(CreateActiveDirectoryUser(group));
                }
            }
            groupPrincipal.Dispose();
            return users;
#endif
        }

        private static List<ActiveDirectoryUser> GetMembers(GroupPrincipal groupPrincipal)
        {
            var members = groupPrincipal.GetAllMembers();
            var result = new List<ActiveDirectoryUser>();
            foreach (var principal in members.Where(principal => !result.Exists(p => p.Sid.Equals(principal.Sid))))
            {
                result.Add(principal);
            }

            return result;
        }

        public static bool IsUserInRole(string userAccount, string groupName)
        {
            CheckCacheForGroup(groupName);
            List<ActiveDirectoryUser> userAccounts = GetUserAccounts(groupName);
            string userName = userAccount.Split('\\').Last();
            var isUserInRole = userAccounts.Any(user => user.UserAccount.Equals(userName));
            CommonLogger.Warn(string.Format("Group: {0}, User: {1}, IsInGroup: {2}, UsersINGroup: {3}", groupName, userAccount, isUserInRole, string.Join(", ", (userAccounts.Select(it => it.UserAccount)))));
            return isUserInRole;
        }

        private static List<ActiveDirectoryUser> GetUserAccounts(string groupName)
        {
            var cache = MemoryCache.Default;
            if (cache[groupName] != null)
            {
                return (List<ActiveDirectoryUser>) cache[groupName];
            }
            else
            {
                throw new Exception(string.Format("cache for group {0} doesn't exist", groupName));
            }
        }

        private static void CheckCacheForGroup(string groupName)
        {
            var cache = MemoryCache.Default;
            if (cache[groupName] == null)
            {
                CreateCacheForGroup(groupName);
            }
        }

        private static void CreateCacheForGroup(string groupName)
        {
            using (var principalContext = CreatePrincipalContext())
            {
                var cache = MemoryCache.Default;
                var policy = new CacheItemPolicy
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(90)
                };

                var groupPrincipal = GroupPrincipal.FindByIdentity(principalContext, IdentityType.SamAccountName,
                                                           groupName);
                if (groupPrincipal == null)
                {
                    cache.Add(new CacheItem(groupName, new List<ActiveDirectoryUser>()), policy);
                    return;
                }
                var accounts = GetMembers(groupPrincipal)
                    .ToList();
                cache.Add(new CacheItem(groupName, accounts), policy);
            }
        }

        private static ActiveDirectoryUser CreateActiveDirectoryUser(Principal principal)
        {
            using (var userPrincipal = principal as UserPrincipal)
            {
                return new ActiveDirectoryUser
                    {
                        UserAccount = userPrincipal.SamAccountName,
                        DisplayName = userPrincipal.DisplayName,
                        Sid = userPrincipal.Sid,
                        Email = userPrincipal.Description // TODO: If use AD, than map from EmailAddress field.
                    };
            }
        }

        public static IEnumerable<ActiveDirectoryUser> GetGroupUsers(string groupName)
        {
            CheckCacheForGroup(groupName);
            List<ActiveDirectoryUser> userAccounts = GetUserAccounts(groupName);
            return userAccounts;
        }

        public static IEnumerable<string> GetGroupEmails(string groupName)
        {
            return GetGroupUsers(groupName).Select(it => it.Email);
        } 

        public static string GetEmailByUser(string userAccount)
        {
            var userPrincipal = GetUserPrincipal(userAccount);

            return userPrincipal != null ? userPrincipal.Description : null;
        }

        public static ActiveDirectoryUser GetActiveDirectoryUser(string userAccount)
        {
            var userPrincipal = GetUserPrincipal(userAccount);
            if (userPrincipal == null)
            {
                return null;
            }
            return CreateActiveDirectoryUser(userPrincipal);
        }
        #endregion

        private static UserPrincipal GetUserPrincipal(string userAccount)
        {
            try
            {
                using (var principalContext = CreatePrincipalContext())
                {
                    return !string.IsNullOrEmpty(userAccount)
                               ? UserPrincipal.FindByIdentity(principalContext, userAccount)
                               : null;
                }
            }
            catch (Exception exception)
            {
                return null;
            }

        }

        private static PrincipalContext CreatePrincipalContext()
        {
#if DEBUG
            return new PrincipalContext(ContextType.Machine);
#else
            return new PrincipalContext(ContextType.Domain, Domain.GetCurrentDomain().Name);
#endif
        }

        public static void ClearCache()
        {
            foreach (var element in MemoryCache.Default)
            {
                MemoryCache.Default.Remove(element.Key);
            }
        }
    }

}