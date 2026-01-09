using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbcBlog.Core.SeedWorks.Constants
{
    public static class Permissions
    {
        public static class Dashboard
        {
            [Description("Xem Thống Kê")]
            public const string View = "Permissions.Dashboard.View";
        }

        public static class Roles
        {
            [Description("Xem Quyền")]
            public const string View = "Permissions.Roles.View";
            [Description("Tạo Quyền")]
            public const string Create = "Permissions.Roles.Create";
            [Description("Sửa Quyền")]
            public const string Edit = "Permissions.Roles.Edit";
            [Description("Xóa Quyền")]
            public const string Delete = "Permission.Roles.Delete";
        }

        public static class Users
        {
            [Description("Xem Người Dùng")]
            public const string View = "Permissions.Users.View";
            [Description("Tạo Người Dùng")]
            public const string Create = "Permissions.Users.Create";
            [Description("Sửa Người Dùng")]
            public const string Edit = "Permissions.Users.Edit";
            [Description("Xóa Người Dùng")]
            public const string Delete = "Permission.Users.Delete";
        }

        public static class PostCategories
        {
            [Description("Xem Danh Mục")]
            public const string View = "Permissions.PostCategories.View";
            [Description("Tạo Danh Mục")]
            public const string Create = "Permissions.PostCategories.Create";
            [Description("Sửa Danh Mục")]
            public const string Edit = "Permissions.PostCategories.Edit";
            [Description("Xóa Danh Mục")]
            public const string Delete = "Permission.PostCategories.Delete";
        }

        public static class Posts
        {
            [Description("Xem Bài Viết")]
            public const string View = "Permissions.Posts.View";
            [Description("Tạo Bài Viết")]
            public const string Create = "Permissions.Posts.Create";
            [Description("Sửa Bài Viết")]
            public const string Edit = "Permissions.Posts.Edit";
            [Description("Xóa Bài Viết")]
            public const string Delete = "Permission.Posts.Delete";
            [Description("Duyệt Bài Viết")]
            public const string Approve = "Permission.Posts.Approve";

        }

        public static class Series
        {
            [Description("Xem Loạt Bài")]
            public const string View = "Permissions.Series.View";
            [Description("Tạo Loạt Bài")]
            public const string Create = "Permissions.Series.Create";
            [Description("Sửa Loạt Bài")]
            public const string Edit = "Permissions.Series.Edit";
            [Description("Xóa Loạt Bài")]
            public const string Delete = "Permission.Series.Delete";
        }
    }
}
