﻿using System;
using System.Collections.Generic;

namespace VJN.Models
{
    public partial class PostJob
    {
        public PostJob()
        {
            ApplyJobs = new HashSet<ApplyJob>();
            ImagePostJobs = new HashSet<ImagePostJob>();
            JobPostDates = new HashSet<JobPostDate>();
            Reports = new HashSet<Report>();
            Slots = new HashSet<Slot>();
            WishJobs = new HashSet<WishJob>();
        }

        public int PostId { get; set; }
        public string? JobTitle { get; set; }
        public string? JobDescription { get; set; }
        public int? SalaryTypesId { get; set; }
        public decimal? Salary { get; set; }
        public int? NumberPeople { get; set; }
        public string? Address { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public int? AuthorId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int? Status { get; set; }
        public int? CensorId { get; set; }
        public DateTime? CensorDate { get; set; }
        public string? Reason { get; set; }
        public bool? IsUrgentRecruitment { get; set; }
        public int? JobCategoryId { get; set; }
        public int? Time { get; set; }

        public virtual User? Author { get; set; }
        public virtual User? Censor { get; set; }
        public virtual JobCategory? JobCategory { get; set; }
        public virtual SalaryType? SalaryTypes { get; set; }
        public virtual ICollection<ApplyJob> ApplyJobs { get; set; }
        public virtual ICollection<ImagePostJob> ImagePostJobs { get; set; }
        public virtual ICollection<JobPostDate> JobPostDates { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<Slot> Slots { get; set; }
        public virtual ICollection<WishJob> WishJobs { get; set; }
    }
}
