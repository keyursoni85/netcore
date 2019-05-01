namespace Application.WebAPI.Controllers
{
    using System;
    using System.Collections.Generic;
    using Application.Data;
    using Application.Entities;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;

    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserController(IUserRepository UserRepository, IConfiguration config)
        {
            _userRepository = UserRepository;
            _configuration = config;
        }

        [HttpGet("GetTestTypeByTestId")]
        public Test GetTestTypeByTestId(int testId)
        {
            return _userRepository.GetTestTypeByTestId(testId);
        }

        [HttpGet("GetUserListByRoleId")]
        public List<User> GetUserListByRoleId(int rId)
        {
            return _userRepository.GetUserListByRoleId(rId);
        }

        [HttpGet("GetTestList")]
        public List<Test> GetTestList()
        {
            return _userRepository.GetTestList();
        }

        [HttpPost("AddTest")]
        public int AddTest(int typeId, string testName, DateTime date)
        {
            return _userRepository.AddTest(typeId, testName, date);
        }

        [HttpGet("GetAthletesByTypeId")]
        public List<AthleteTestMapping> GetAthletesByTypeId(int typeId)
        {
            return _userRepository.GetAthletesByTypeId(typeId);
        }

        [HttpPost("AddAthlete")]
        public int AddAthlete(int athleteTestId, int athleteId, decimal distance, int editMode, int mapId)
        {
            return _userRepository.AddAthlete(athleteTestId, athleteId, distance, editMode, mapId);
        }

        [HttpGet("GetAthlete")]
        public AthleteTestMapping GetAthlete(int athleteId)
        {
            return _userRepository.GetAthlete(athleteId);
        }

        [HttpPost("DeleteAthlete")]
        public int DeleteAthlete(int mapId)
        {
            return _userRepository.DeleteAthlete(mapId);
        }

        [HttpPost("DeleteTest")]
        public int DeleteTest(int testId)
        {
            return _userRepository.DeleteTest(testId);
        }
    }
}