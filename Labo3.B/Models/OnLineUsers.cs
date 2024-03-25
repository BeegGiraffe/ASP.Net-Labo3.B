using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace MoviesDBManager.Models
{
    public static class OnlineUsers
    {
        public static List<int> ConnectedUsersId
        {
            get
            {
                if (HttpRuntime.Cache["OnLineUsers"] == null)
                    HttpRuntime.Cache["OnLineUsers"] = new List<int>();
                return (List<int>)HttpRuntime.Cache["OnLineUsers"];
            }
        }
        static Func<int,bool> IsOnline = n => n == (int)HttpContext.Current.Session["UserId"] ? true : false;
        public static void AddSessionUser(int userId)
        {
            HttpContext.Current.Session["UserId"] = userId;
            ConnectedUsersId.Add(userId);
          
        }
        public static void RemoveSessionUser()
        {
            ConnectedUsersId.Remove(GetSessionUser().Id);
            HttpContext.Current?.Session.Abandon();
        }
        public static User GetSessionUser()
        {
            if (HttpContext.Current.Session["UserId"] != null)
            {
                User currentUser = DB.Users.Get((int)HttpContext.Current.Session["UserId"]);
                return currentUser;
            }
            return null;
        }
        public static bool Write_Access()
        {
            User sessionUser = OnlineUsers.GetSessionUser();
            if (sessionUser != null)
            {
                return sessionUser.IsPowerUser || sessionUser.IsAdmin;
            }
            return false;
        }

        public class UserAccess : AuthorizeAttribute
        {
            protected override bool AuthorizeCore(HttpContextBase httpContext)
            {
                User sessionUser = OnlineUsers.GetSessionUser();
                if (sessionUser != null)
                {
                    if (sessionUser.Blocked)
                    {
                        RemoveSessionUser();
                        httpContext.Response.Redirect("~/Accounts/Login?message=Compte bloqué!");
                        return false;
                    }
                    return true;
                }
                httpContext.Response.Redirect("~/Accounts/Login?message=Accès non autorisé!");
                return false;

            }
        }
        public class PowerUserAccess : AuthorizeAttribute
        {
            protected override bool AuthorizeCore(HttpContextBase httpContext)
            {
                User sessionUser = OnlineUsers.GetSessionUser();
                if (sessionUser != null && (sessionUser.IsPowerUser || sessionUser.IsAdmin))
                    return true;
                else
                {
                    RemoveSessionUser();
                    httpContext.Response.Redirect("~/Accounts/Login?message=Accès non autorisé!", true);
                }
                return false;
            }
        }
        public class AdminAccess : AuthorizeAttribute
        {
            protected override bool AuthorizeCore(HttpContextBase httpContext)
            {
                User sessionUser = OnlineUsers.GetSessionUser();
                if (sessionUser != null && sessionUser.IsAdmin)
                    return true;
                else
                {
                    RemoveSessionUser();
                    httpContext.Response.Redirect("~/Accounts/Login?message=Accès non autorisé!");
                }
                return true;
            }
        }
    }
}