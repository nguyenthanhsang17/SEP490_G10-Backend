using NuGet.Protocol.Core.Types;
using VJN.Models;
using VJN.ModelsDTO.JobSeekerDTOs;
using VJN.Repositories;

namespace UnitTests
{
    public class UserRepositoryTest
    {
        private UserRepository repository;
        private RegisterEmployerRepository registerEmployerRepository;
        private JobSeekerRespository jobSeekerRespository;
        [SetUp]
        public void Setup()
        {
            VJNDBContext testContext = new VJNDBContext();
            repository = new UserRepository(testContext);
            registerEmployerRepository = new RegisterEmployerRepository(testContext);
            jobSeekerRespository = new JobSeekerRespository(testContext);
        }

        [Test]
        public async Task TestLogin1()
        {
            var user = await repository.Login("sangnthe160447@fpt.edu.vn", "17102002");
            Assert.IsNotNull(user);
        }

        [Test]
        public async Task TestLogin2()
        {
            var user = await repository.Login("sangnthe160447@fpt.edu.vn", "1710200");
            Assert.IsNotNull(user);
        }

        [Test]
        public async Task TestSelectJobseekerall()
        {
            var s = new JobSeekerSearchDTO()
            {
                keyword = "đại học bách khoa hà nội",
                CurrentJob = 3,
                sort = 1,
                numberPage = 1,
                agemax = 30,
                agemin = 18,
                address="Hưng yên"
            };
            var id = await jobSeekerRespository.GetAllJobSeeker(s, 2);
            Assert.AreEqual(1, id.Count(), "result ");
        }

        
    }
}