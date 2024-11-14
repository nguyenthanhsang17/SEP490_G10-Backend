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
        // test full
        [Test]
        public async Task TestSearchJob()
        {
            var c = new PostJobSearch
            {
                JobKeyWord="tân",
                SalaryTypesId=0,
                Address=null,
                Latitude=null,
                Longitude=null,
                distance=null,
                JobCategoryId=0,
                SortNumberApplied=0,
                pageNumber = 1
            };
            var user = await repository.SearchJobPopular(c);
            Assert.AreEqual(user.Count(), 1);
        }
    }
}
