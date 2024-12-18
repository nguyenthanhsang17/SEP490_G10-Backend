﻿using VJN.Models;
using VJN.ModelsDTO.JobPostDateDTOs;
using VJN.ModelsDTO.SlotDTOs;

namespace VJN.ModelsDTO.PostJobDTOs
{
    public class PostJobDTOReport
    {
        public int PostId { get; set; }
        public string? JobTitle { get; set; }
        public string? JobDescription { get; set; }
        public decimal? Salary { get; set; }
        public int? NumberPeople { get; set; }
        public string? Address { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int? Status { get; set; }
        public DateTime? CensorDate { get; set; }
        public bool? IsUrgentRecruitment { get; set; }
        public int? JobCategoryId { get; set; }
        public string reason { get; set; }

        public int? Time { get; set; }

        public  UserDTOs.UserDTOReport? Author { get; set; }
        public UserDTOs.UserDTOReport? Censor { get; set; }
        public  JobCategory? JobCategory { get; set; }
        public  SalaryType? SalaryTypes { get; set; }
        public ICollection<BanLogPostJob> BanLogPostJobs { get; set; }
        public ICollection<ImagePostJob> ImagePostJobs { get; set; }
        public IEnumerable<JobPostDateDTO> JobPostDates { get; set; }
        public IEnumerable<SlotDTO> Slots { get; set; }

        
    }
}
