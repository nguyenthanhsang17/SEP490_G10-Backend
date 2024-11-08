using System;
using System.Collections.Generic;

namespace VJN.Models
{
    public partial class User
    {
        public User()
        {
            ApplyJobs = new HashSet<ApplyJob>();
            BanLogPostJobs = new HashSet<BanLogPostJob>();
            BanUserLogAdmins = new HashSet<BanUserLog>();
            BanUserLogUsers = new HashSet<BanUserLog>();
            Blogs = new HashSet<Blog>();
            ChatSendFroms = new HashSet<Chat>();
            ChatSendTos = new HashSet<Chat>();
            Cvs = new HashSet<Cv>();
            FavoriteListEmployers = new HashSet<FavoriteList>();
            FavoriteListJobSeekers = new HashSet<FavoriteList>();
            Logs = new HashSet<Log>();
            Notifications = new HashSet<Notification>();
            PostJobAuthors = new HashSet<PostJob>();
            PostJobCensors = new HashSet<PostJob>();
            RegisterEmployers = new HashSet<RegisterEmployer>();
            Reports = new HashSet<Report>();
            ServicePriceLogs = new HashSet<ServicePriceLog>();
            Slots = new HashSet<Slot>();
            WishJobs = new HashSet<WishJob>();
        }

        public int UserId { get; set; }
        public string? Email { get; set; }
        public int? Avatar { get; set; }
        public string? FullName { get; set; }
        public string? Password { get; set; }
        public int? Age { get; set; }
        public string? Phonenumber { get; set; }
        public int? CurrentJob { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public decimal? Balance { get; set; }
        public int? Status { get; set; }
        public bool? Gender { get; set; }
        public DateTime? SendCodeTime { get; set; }
        public string? VerifyCode { get; set; }
        public int? RoleId { get; set; }

        public virtual MediaItem? AvatarNavigation { get; set; }
        public virtual CurrentJob? CurrentJobNavigation { get; set; }
        public virtual Role? Role { get; set; }
        public virtual ICollection<ApplyJob> ApplyJobs { get; set; }
        public virtual ICollection<BanLogPostJob> BanLogPostJobs { get; set; }
        public virtual ICollection<BanUserLog> BanUserLogAdmins { get; set; }
        public virtual ICollection<BanUserLog> BanUserLogUsers { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<Chat> ChatSendFroms { get; set; }
        public virtual ICollection<Chat> ChatSendTos { get; set; }
        public virtual ICollection<Cv> Cvs { get; set; }
        public virtual ICollection<FavoriteList> FavoriteListEmployers { get; set; }
        public virtual ICollection<FavoriteList> FavoriteListJobSeekers { get; set; }
        public virtual ICollection<Log> Logs { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<PostJob> PostJobAuthors { get; set; }
        public virtual ICollection<PostJob> PostJobCensors { get; set; }
        public virtual ICollection<RegisterEmployer> RegisterEmployers { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<ServicePriceLog> ServicePriceLogs { get; set; }
        public virtual ICollection<Slot> Slots { get; set; }
        public virtual ICollection<WishJob> WishJobs { get; set; }
    }
}
