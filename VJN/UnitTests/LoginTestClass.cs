using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VJN.Map;
using VJN.Models;
using VJN.Repositories;
using VJN.Services;

namespace UnitTests
{
    public class LoginTestClass
    {
        private IUserRepository repository;
        private IRegisterEmployerRepository registerEmployerRepository;
        private IJobSeekerRespository jobSeekerRespository;
        private UserService userServcie;
        private IMapper _mapper;
        [SetUp]
        public void Setup()
        {
            VJNDBContext testContext = new VJNDBContext();
            repository = new UserRepository(testContext);
            registerEmployerRepository = new RegisterEmployerRepository(testContext);
            jobSeekerRespository = new JobSeekerRespository(testContext);
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MapperConfig>(); // Thêm các profile đã định nghĩa
            });
            _mapper = new Mapper(configuration);
            userServcie = new UserService(repository, _mapper, registerEmployerRepository);
        }

        [Test]
        public async Task TestLogin1()
        {
            var user = await userServcie.Login("sangnthe160447@fpt.edu.vn", "17102002");
            Assert.IsNotNull(user);
        }
    }
}
