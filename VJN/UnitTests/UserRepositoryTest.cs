using NuGet.Protocol.Core.Types;
using VJN.Models;
using VJN.Repositories;

namespace UnitTests
{
    public class UserRepositoryTest
    {
        private UserRepository repository;
        [SetUp]
        public void Setup()
        {
            VJNDBContext testContext = new VJNDBContext();
            repository = new UserRepository(testContext);
        }

        [Test]
        public async Task TestLogin()
        {
            var user = await repository.Login("sangnthe160447@fpt.edu.vn", "17102002");
            Assert.IsNotNull(user);
        }
    }
}