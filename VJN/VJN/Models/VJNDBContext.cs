using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace VJN.Models
{
    public partial class VJNDBContext : DbContext
    {
        public VJNDBContext()
        {
        }

        public VJNDBContext(DbContextOptions<VJNDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ApplyJob> ApplyJobs { get; set; } = null!;
        public virtual DbSet<BanLogPostJob> BanLogPostJobs { get; set; } = null!;
        public virtual DbSet<BanUserLog> BanUserLogs { get; set; } = null!;
        public virtual DbSet<Blog> Blogs { get; set; } = null!;
        public virtual DbSet<Chat> Chats { get; set; } = null!;
        public virtual DbSet<CurrentJob> CurrentJobs { get; set; } = null!;
        public virtual DbSet<Cv> Cvs { get; set; } = null!;
        public virtual DbSet<FavoriteList> FavoriteLists { get; set; } = null!;
        public virtual DbSet<ImagePostJob> ImagePostJobs { get; set; } = null!;
        public virtual DbSet<ItemOfCv> ItemOfCvs { get; set; } = null!;
        public virtual DbSet<JobCategory> JobCategories { get; set; } = null!;
        public virtual DbSet<JobPostDate> JobPostDates { get; set; } = null!;
        public virtual DbSet<JobSchedule> JobSchedules { get; set; } = null!;
        public virtual DbSet<Log> Logs { get; set; } = null!;
        public virtual DbSet<MediaItem> MediaItems { get; set; } = null!;
        public virtual DbSet<Notification> Notifications { get; set; } = null!;
        public virtual DbSet<PostJob> PostJobs { get; set; } = null!;
        public virtual DbSet<RegisterEmployer> RegisterEmployers { get; set; } = null!;
        public virtual DbSet<RegisterEmployerMedium> RegisterEmployerMedia { get; set; } = null!;
        public virtual DbSet<Report> Reports { get; set; } = null!;
        public virtual DbSet<ReportMedium> ReportMedia { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<SalaryType> SalaryTypes { get; set; } = null!;
        public virtual DbSet<ServicePriceList> ServicePriceLists { get; set; } = null!;
        public virtual DbSet<ServicePriceLog> ServicePriceLogs { get; set; } = null!;
        public virtual DbSet<Slot> Slots { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<WishJob> WishJobs { get; set; } = null!;
        public virtual DbSet<WorkingHour> WorkingHours { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server =(local); database = VJNDB;uid=sa;pwd=123456;TrustServerCertificate =true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplyJob>(entity =>
            {
                entity.ToTable("ApplyJob");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ApplyDate).HasColumnType("datetime");

                entity.Property(e => e.CvId).HasColumnName("cv_ID");

                entity.Property(e => e.JobSeekerId).HasColumnName("JobSeeker_Id");

                entity.Property(e => e.PostId).HasColumnName("Post_Id");

                entity.HasOne(d => d.Cv)
                    .WithMany(p => p.ApplyJobs)
                    .HasForeignKey(d => d.CvId)
                    .HasConstraintName("FK__ApplyJob__cv_ID__5BE2A6F2");

                entity.HasOne(d => d.JobSeeker)
                    .WithMany(p => p.ApplyJobs)
                    .HasForeignKey(d => d.JobSeekerId)
                    .HasConstraintName("FK__ApplyJob__JobSee__5CD6CB2B");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.ApplyJobs)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK__ApplyJob__Post_I__5DCAEF64");
            });

            modelBuilder.Entity<BanLogPostJob>(entity =>
            {
                entity.HasKey(e => e.BanLogPostJob1)
                    .HasName("PK__BanLogPo__8801D8C9F098CBFF");

                entity.ToTable("BanLogPostJob");

                entity.Property(e => e.BanLogPostJob1).HasColumnName("BanLogPostJob");

                entity.Property(e => e.Reason).HasColumnName("reason");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.BanLogPostJobs)
                    .HasForeignKey(d => d.AdminId)
                    .HasConstraintName("FK__BanLogPos__Admin__5EBF139D");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.BanLogPostJobs)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK__BanLogPos__PostI__5FB337D6");
            });

            modelBuilder.Entity<BanUserLog>(entity =>
            {
                entity.HasKey(e => e.BanId)
                    .HasName("PK__BanUserL__FD9DEB4A930ABE18");

                entity.ToTable("BanUserLog");

                entity.Property(e => e.BanId).HasColumnName("Ban_Id");

                entity.Property(e => e.AdminId).HasColumnName("AdminID");

                entity.Property(e => e.BanDate).HasColumnType("datetime");

                entity.Property(e => e.UnbanDate).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.BanUserLogAdmins)
                    .HasForeignKey(d => d.AdminId)
                    .HasConstraintName("FK__BanUserLo__Admin__60A75C0F");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BanUserLogUsers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__BanUserLo__UserI__619B8048");
            });

            modelBuilder.Entity<Blog>(entity =>
            {
                entity.ToTable("Blog");

                entity.Property(e => e.BlogId).HasColumnName("Blog_Id");

                entity.Property(e => e.AuthorId).HasColumnName("Author_Id");

                entity.Property(e => e.BlogTitle).HasMaxLength(200);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Thumbnail).HasColumnName("thumbnail");

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.Blogs)
                    .HasForeignKey(d => d.AuthorId)
                    .HasConstraintName("FK__Blog__Author_Id__628FA481");

                entity.HasOne(d => d.ThumbnailNavigation)
                    .WithMany(p => p.Blogs)
                    .HasForeignKey(d => d.Thumbnail)
                    .HasConstraintName("FK__Blog__thumbnail__6383C8BA");
            });

            modelBuilder.Entity<Chat>(entity =>
            {
                entity.ToTable("Chat");

                entity.Property(e => e.ChatId).HasColumnName("Chat_Id");

                entity.Property(e => e.SendFromId).HasColumnName("SendFrom_Id");

                entity.Property(e => e.SendTime).HasColumnType("datetime");

                entity.Property(e => e.SendToId).HasColumnName("SendTo_Id");

                entity.HasOne(d => d.SendFrom)
                    .WithMany(p => p.ChatSendFroms)
                    .HasForeignKey(d => d.SendFromId)
                    .HasConstraintName("FK__Chat__SendFrom_I__656C112C");

                entity.HasOne(d => d.SendTo)
                    .WithMany(p => p.ChatSendTos)
                    .HasForeignKey(d => d.SendToId)
                    .HasConstraintName("FK__Chat__SendTo_Id__66603565");
            });

            modelBuilder.Entity<CurrentJob>(entity =>
            {
                entity.ToTable("CurrentJob");

                entity.Property(e => e.CurrentJobId).HasColumnName("Current_Job_Id");

                entity.Property(e => e.JobName)
                    .HasMaxLength(200)
                    .HasColumnName("Job_Name");
            });

            modelBuilder.Entity<Cv>(entity =>
            {
                entity.ToTable("Cv");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Cvs)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Cv__UserId__6477ECF3");
            });

            modelBuilder.Entity<FavoriteList>(entity =>
            {
                entity.ToTable("Favorite_List");

                entity.Property(e => e.FavoriteListId).HasColumnName("Favorite_List_Id");

                entity.HasOne(d => d.Employer)
                    .WithMany(p => p.FavoriteListEmployers)
                    .HasForeignKey(d => d.EmployerId)
                    .HasConstraintName("FK__Favorite___Emplo__6754599E");

                entity.HasOne(d => d.JobSeeker)
                    .WithMany(p => p.FavoriteListJobSeekers)
                    .HasForeignKey(d => d.JobSeekerId)
                    .HasConstraintName("FK__Favorite___JobSe__68487DD7");
            });

            modelBuilder.Entity<ImagePostJob>(entity =>
            {
                entity.HasKey(e => e.ImageJobId)
                    .HasName("PK__ImagePos__94211786A2ED28C2");

                entity.ToTable("ImagePostJob");

                entity.Property(e => e.ImageJobId).HasColumnName("ImageJob_Id");

                entity.Property(e => e.ImageId).HasColumnName("Image_Id");

                entity.Property(e => e.PostId).HasColumnName("Post_Id");

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.ImagePostJobs)
                    .HasForeignKey(d => d.ImageId)
                    .HasConstraintName("FK__ImagePost__Image__693CA210");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.ImagePostJobs)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK__ImagePost__Post___6A30C649");
            });

            modelBuilder.Entity<ItemOfCv>(entity =>
            {
                entity.ToTable("ItemOfCv");

                entity.Property(e => e.ItemOfCvId).HasColumnName("ItemOfCvID");

                entity.HasOne(d => d.Cv)
                    .WithMany(p => p.ItemOfCvs)
                    .HasForeignKey(d => d.CvId)
                    .HasConstraintName("FK__ItemOfCv__CvId__6B24EA82");
            });

            modelBuilder.Entity<JobCategory>(entity =>
            {
                entity.ToTable("JobCategory");

                entity.Property(e => e.JobCategoryId).HasColumnName("JobCategory_Id");

                entity.Property(e => e.JobCategoryName).HasMaxLength(200);
            });

            modelBuilder.Entity<JobPostDate>(entity =>
            {
                entity.HasKey(e => e.EventDateId)
                    .HasName("PK__JobPostD__C24CACCE0FC7AE4E");

                entity.Property(e => e.EventDateId).HasColumnName("EventDate_Id");

                entity.Property(e => e.EventDate).HasColumnType("date");

                entity.Property(e => e.PostId).HasColumnName("Post_Id");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.JobPostDates)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK__JobPostDa__Post___6C190EBB");
            });

            modelBuilder.Entity<JobSchedule>(entity =>
            {
                entity.HasKey(e => e.ScheduleId)
                    .HasName("PK__JobSched__8C4D3C5B82A444D7");

                entity.ToTable("JobSchedule");

                entity.Property(e => e.ScheduleId).HasColumnName("Schedule_Id");

                entity.HasOne(d => d.Slot)
                    .WithMany(p => p.JobSchedules)
                    .HasForeignKey(d => d.SlotId)
                    .HasConstraintName("FK__JobSchedu__SlotI__6D0D32F4");
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.ToTable("log");

                entity.Property(e => e.LogId).HasColumnName("Log_id");

                entity.Property(e => e.Action).HasColumnType("text");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Time)
                    .HasColumnType("datetime")
                    .HasColumnName("time");

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Logs)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__log__User_Id__6E01572D");
            });

            modelBuilder.Entity<MediaItem>(entity =>
            {
                entity.Property(e => e.Url)
                    .HasColumnType("text")
                    .HasColumnName("URL");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.NotifycationId)
                    .HasName("PK__Notifica__391E35C893919F5B");

                entity.ToTable("Notification");

                entity.Property(e => e.NotifycationId).HasColumnName("Notifycation_Id");

                entity.Property(e => e.NotifycationContent).HasMaxLength(255);

                entity.Property(e => e.UserId).HasColumnName("User_ID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Notificat__User___6EF57B66");
            });

            modelBuilder.Entity<PostJob>(entity =>
            {
                entity.HasKey(e => e.PostId)
                    .HasName("PK__PostJob__5875F7AD517FE5B4");

                entity.ToTable("PostJob");

                entity.Property(e => e.PostId).HasColumnName("Post_Id");

                entity.Property(e => e.CensorDate)
                    .HasColumnType("datetime")
                    .HasColumnName("censor_Date");

                entity.Property(e => e.CensorId).HasColumnName("censor_Id");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.ExpirationDate).HasColumnType("datetime");

                entity.Property(e => e.JobCategoryId).HasColumnName("JobCategory_Id");

                entity.Property(e => e.JobTitle).HasMaxLength(200);

                entity.Property(e => e.Latitude)
                    .HasColumnType("decimal(17, 14)")
                    .HasColumnName("latitude");

                entity.Property(e => e.Longitude)
                    .HasColumnType("decimal(17, 14)")
                    .HasColumnName("longitude");

                entity.Property(e => e.Salary).HasColumnType("money");

                entity.Property(e => e.SalaryTypesId).HasColumnName("salary_types_id");

                entity.Property(e => e.Time).HasColumnName("time");

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.PostJobAuthors)
                    .HasForeignKey(d => d.AuthorId)
                    .HasConstraintName("FK__PostJob__AuthorI__6FE99F9F");

                entity.HasOne(d => d.Censor)
                    .WithMany(p => p.PostJobCensors)
                    .HasForeignKey(d => d.CensorId)
                    .HasConstraintName("FK__PostJob__censor___70DDC3D8");

                entity.HasOne(d => d.JobCategory)
                    .WithMany(p => p.PostJobs)
                    .HasForeignKey(d => d.JobCategoryId)
                    .HasConstraintName("FK__PostJob__JobCate__71D1E811");

                entity.HasOne(d => d.SalaryTypes)
                    .WithMany(p => p.PostJobs)
                    .HasForeignKey(d => d.SalaryTypesId)
                    .HasConstraintName("FK__PostJob__salary___72C60C4A");
            });

            modelBuilder.Entity<RegisterEmployer>(entity =>
            {
                entity.ToTable("RegisterEmployer");

                entity.Property(e => e.RegisterEmployerId).HasColumnName("RegisterEmployer_Id");

                entity.Property(e => e.BussinessName).HasMaxLength(200);

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RegisterEmployers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__RegisterE__User___73BA3083");
            });

            modelBuilder.Entity<RegisterEmployerMedium>(entity =>
            {
                entity.HasKey(e => e.RegisterEmployerMedia)
                    .HasName("PK__Register__AA6A3626D85004FD");

                entity.Property(e => e.MediaId).HasColumnName("Media_Id");

                entity.Property(e => e.RegisterEmployerId).HasColumnName("RegisterEmployer_Id");

                entity.HasOne(d => d.Media)
                    .WithMany(p => p.RegisterEmployerMedia)
                    .HasForeignKey(d => d.MediaId)
                    .HasConstraintName("FK__RegisterE__Media__74AE54BC");

                entity.HasOne(d => d.RegisterEmployer)
                    .WithMany(p => p.RegisterEmployerMedia)
                    .HasForeignKey(d => d.RegisterEmployerId)
                    .HasConstraintName("FK__RegisterE__Regis__75A278F5");
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.ToTable("Report");

                entity.Property(e => e.ReportId).HasColumnName("Report_Id");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.JobSeekerId).HasColumnName("JobSeeker_Id");

                entity.Property(e => e.PostId).HasColumnName("Post_Id");

                entity.HasOne(d => d.JobSeeker)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.JobSeekerId)
                    .HasConstraintName("FK__Report__JobSeeke__76969D2E");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK__Report__Post_Id__778AC167");
            });

            modelBuilder.Entity<ReportMedium>(entity =>
            {
                entity.HasKey(e => e.ReportImageId)
                    .HasName("PK__ReportMe__585F554B1D99ED18");

                entity.Property(e => e.ReportImageId).HasColumnName("ReportImage_Id");

                entity.Property(e => e.ImageId).HasColumnName("Image_Id");

                entity.Property(e => e.ReportId).HasColumnName("Report_Id");

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.ReportMedia)
                    .HasForeignKey(d => d.ImageId)
                    .HasConstraintName("FK__ReportMed__Image__787EE5A0");

                entity.HasOne(d => d.Report)
                    .WithMany(p => p.ReportMedia)
                    .HasForeignKey(d => d.ReportId)
                    .HasConstraintName("FK__ReportMed__Repor__797309D9");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("role");

                entity.Property(e => e.RoleId).HasColumnName("Role_Id");

                entity.Property(e => e.RoleName).HasMaxLength(100);
            });

            modelBuilder.Entity<SalaryType>(entity =>
            {
                entity.HasKey(e => e.SalaryTypesId)
                    .HasName("PK__salary_t__0CA27F7857E73B78");

                entity.ToTable("salary_types");

                entity.Property(e => e.SalaryTypesId).HasColumnName("salary_types_id");

                entity.Property(e => e.TypeName)
                    .HasMaxLength(50)
                    .HasColumnName("type_name");
            });

            modelBuilder.Entity<ServicePriceList>(entity =>
            {
                entity.HasKey(e => e.ServicePriceId)
                    .HasName("PK__Service___59C7673B9CED2379");

                entity.ToTable("Service_price_list");

                entity.Property(e => e.ServicePriceId).HasColumnName("Service_price_id");

                entity.Property(e => e.DurationsMonth).HasColumnName("durationsMonth");

                entity.Property(e => e.Price).HasColumnType("money");
            });

            modelBuilder.Entity<ServicePriceLog>(entity =>
            {
                entity.ToTable("Service_price_Log");

                entity.Property(e => e.ServicePriceLogId).HasColumnName("Service_price_Log_Id");

                entity.Property(e => e.DurationsMonth).HasColumnName("durationsMonth");

                entity.Property(e => e.RegisterDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Register_Date");

                entity.Property(e => e.ServicePriceId).HasColumnName("Service_price_id");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.HasOne(d => d.ServicePrice)
                    .WithMany(p => p.ServicePriceLogs)
                    .HasForeignKey(d => d.ServicePriceId)
                    .HasConstraintName("FK__Service_p__Servi__7A672E12");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ServicePriceLogs)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Service_p__User___7B5B524B");
            });

            modelBuilder.Entity<Slot>(entity =>
            {
                entity.ToTable("Slot");

                entity.Property(e => e.SlotId).HasColumnName("Slot_Id");

                entity.Property(e => e.PostId).HasColumnName("Post_Id");

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Slots)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK__Slot__Post_Id__7C4F7684");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Slots)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Slot__User_Id__7D439ABD");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.Property(e => e.Balance).HasColumnType("money");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.FullName).HasMaxLength(100);

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Phonenumber)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.RoleId).HasColumnName("Role_Id");

                entity.Property(e => e.SendCodeTime).HasColumnType("datetime");

                entity.Property(e => e.VerifyCode)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.HasOne(d => d.AvatarNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.Avatar)
                    .HasConstraintName("FK__User__Avatar__7E37BEF6");

                entity.HasOne(d => d.CurrentJobNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.CurrentJob)
                    .HasConstraintName("FK__User__CurrentJob__7F2BE32F");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__User__Role_Id__00200768");
            });

            modelBuilder.Entity<WishJob>(entity =>
            {
                entity.ToTable("WishJob");

                entity.Property(e => e.WishJobId).HasColumnName("WishJob_Id");

                entity.Property(e => e.JobSeekerId).HasColumnName("JobSeeker_Id");

                entity.Property(e => e.PostJobId).HasColumnName("PostJob_Id");

                entity.HasOne(d => d.JobSeeker)
                    .WithMany(p => p.WishJobs)
                    .HasForeignKey(d => d.JobSeekerId)
                    .HasConstraintName("FK__WishJob__JobSeek__01142BA1");

                entity.HasOne(d => d.PostJob)
                    .WithMany(p => p.WishJobs)
                    .HasForeignKey(d => d.PostJobId)
                    .HasConstraintName("FK__WishJob__PostJob__02084FDA");
            });

            modelBuilder.Entity<WorkingHour>(entity =>
            {
                entity.ToTable("WorkingHour");

                entity.Property(e => e.WorkingHourId).HasColumnName("WorkingHour_Id");

                entity.Property(e => e.ScheduleId).HasColumnName("Schedule_Id");

                entity.HasOne(d => d.Schedule)
                    .WithMany(p => p.WorkingHours)
                    .HasForeignKey(d => d.ScheduleId)
                    .HasConstraintName("FK__WorkingHo__Sched__02FC7413");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
