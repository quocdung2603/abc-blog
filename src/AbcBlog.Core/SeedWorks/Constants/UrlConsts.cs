using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbcBlog.Core.SeedWorks.Constants
{
    public static class UrlConsts
    {
        public static string Posts = "/posts";
        public static string Home = "/";
        public static string About = "/";
        public static string Contact = "/contact";
        public static string PostsByCategorySlug = "/posts/{0}";
        public static string PostDetail = "/post/{0}";
        public static string PostsByTagSlug = "/tag/{0}";
        public static string Login = "/login";
        public static string Register = "/register";
        public static string Profile = "/profile";
        public static string Author = "/author/{0}";
        public static string ChangePassword = "/change-password";
    }
}
