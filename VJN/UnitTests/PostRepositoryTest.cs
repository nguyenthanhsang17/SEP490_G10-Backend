using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VJN.Models;
using VJN.ModelsDTO.PostJobDTOs;
using VJN.Repositories;

namespace UnitTests
{
    public class PostRepositoryTest
    {
        private PostJobRepository repository;
        [SetUp]
        public void Setup()
        {
            VJNDBContext testContext = new VJNDBContext();
            repository = new PostJobRepository(testContext);
        }

        [Test]
        public async Task TestLogin()
        {
            var c = new PostJobSearch
            {
                JobTitle=null,
                JobDescription=null,
                SalaryTypesId=0,
                RangeSalaryMin=null,
                RangeSalaryMax=null,
                Address=null,
                Latitude=null,
                Longitude=null,
                distance=null,
                IsUrgentRecruitment=null,
                JobCategoryId=0,
            };
            var user = await repository.SearchJobPopular(c);
            Assert.AreEqual(user.Count(), 5);
        }
    }
}
