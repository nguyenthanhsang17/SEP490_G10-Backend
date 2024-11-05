using NuGet.Protocol.Core.Types;
using VJN.Models;
using VJN.Repositories;

namespace UnitTests
{
    public class UserRepositoryTest
    {
        private UserRepository repository;
        private RegisterEmployerRepository registerEmployerRepository;
        [SetUp]
        public void Setup()
        {
            VJNDBContext testContext = new VJNDBContext();
            repository = new UserRepository(testContext);
            registerEmployerRepository = new RegisterEmployerRepository(testContext);
        }

        [Test]
        public async Task TestLogin()
        {
            var user = await repository.Login("sangnthe160447@fpt.edu.vn", "17102002");
            Assert.IsNotNull(user);
        }

        [Test] 
        public async Task TestRegister()
        {
            var Register = new RegisterEmployer()
            {
                UserId = 1,
                BussinessAddress = "cổng trường đại học fpt",
                BussinessName = "Test",
                CreateDate = DateTime.Now,
            };
            var id = await registerEmployerRepository.RegisterEmployer(Register);
            Assert.AreEqual(2, id, "id : "+id);
        }
    }
}