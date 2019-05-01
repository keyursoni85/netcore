namespace Application.Data.Domain
{
    using Application.Entities;
    using Application.Framework.Utils;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;


    public class UserRepository : IUserRepository
    {
        private readonly ApplicationCPDbContext _dbContext;
        private readonly ILogger<ApplicationCPDbContext> _logger;

        public UserRepository(ApplicationCPDbContext dbContext, ILogger<ApplicationCPDbContext> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public List<User> GetUserListByRoleId(int rId)
        {
            List<User> user = null;
            try
            {
                user = _dbContext.User.Where(x => x.UserType == rId.ToSafeInt() && x.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToSafeString());
            }
            return user;
        }

        public Test GetTestTypeByTestId(int testId)
        {
            Test record = null;
            try
            {
                record = _dbContext.Test.Where(x => x.TestId == testId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToSafeString());
            }
            return record;
        }

        public List<Test> GetTestList()
        {
            List<Test> records = new List<Test>();
            try
            {
                records = _dbContext.Test.Where(x => x.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToSafeString());
            }
            return records;
        }

        public int AddTest(int testTypeId, string typeName, DateTime date)
        {
            int record = 0;
            try
            {
                int count = _dbContext.Test.Where(t => t.TestTypeId == testTypeId.ToSafeInt() && t.TestDate == date).Count();

                if (count == 0)
                {
                    Test test = new Test();
                    TestType TestType = new TestType();
                    TestType = _dbContext.TestType.Where(type => type.TypeId == testTypeId).FirstOrDefault();

                    test.TestTypeId = testTypeId.ToSafeInt();
                    test.TestName = TestType.TypeName.ToSafeString();
                    test.TestDate = date;
                    test.IsDeleted = false;
                    _dbContext.Add(test);
                    _dbContext.SaveChanges();
                    record = 1;
                }
                else
                {
                    record = -1;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToSafeString());
            }
            return record;
        }

        public List<AthleteTestMapping> GetAthletesByTypeId(int typeId)
        {
            List<AthleteTestMapping> athletesList = new List<AthleteTestMapping>();
            try
            {
                athletesList = _dbContext.AthleteTestMapping.Where(athletes => athletes.AthleteTestId == typeId && athletes.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToSafeString());
            }
            return athletesList;
        }

        public int AddAthlete(int athleteTestId, int athleteId, decimal distance, int editMode, int mapId)
        {
            int result = 0;
            int count = 0;
            try
            {
                AthleteTestMapping athleteTest = new AthleteTestMapping();
                User user = new User();

                count = (editMode == 0)
                    ? _dbContext.AthleteTestMapping.Where(a => a.AthleteId == athleteId && a.AthleteTestId == athleteTestId && a.IsDeleted == false).Count()
                    : _dbContext.AthleteTestMapping.Where(a => a.AthleteId == athleteId && a.AthleteTestId == athleteTestId && a.MapId != mapId.ToSafeInt() && a.IsDeleted == false).Count();

                if (count == 0)
                {
                    if (editMode == 0)
                    {
                        user = _dbContext.User.Where(u => u.UserId == athleteId).FirstOrDefault();
                        athleteTest.AthleteTestId = athleteTestId.ToSafeInt();
                        athleteTest.AthleteId = athleteId.ToSafeInt();
                        athleteTest.AthleteName = user.UserName.ToSafeString();
                        athleteTest.Distance = distance;
                        athleteTest.IsDeleted = false;
                        _dbContext.Add(athleteTest);

                        Test test = _dbContext.Test.Where(c => c.TestId == athleteTest.AthleteTestId).FirstOrDefault();
                        test.TotalParticipats = test.TotalParticipats.ToSafeInt() + 1;
                        _dbContext.Test.Update(test);
                        result = 1;
                    }
                    else
                    {
                        athleteTest = _dbContext.AthleteTestMapping.Find(mapId);
                        user = _dbContext.User.Where(u => u.UserId == athleteId).FirstOrDefault();
                        athleteTest.AthleteTestId = athleteTestId.ToSafeInt();
                        athleteTest.AthleteId = athleteId.ToSafeInt();
                        athleteTest.AthleteName = user.UserName.ToSafeString();
                        athleteTest.Distance = distance;
                        athleteTest.IsDeleted = false;
                        _dbContext.Update(athleteTest);
                        result = 2;
                    }

                    _dbContext.SaveChanges();
                }
                else
                {
                    result = -1;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToSafeString());
            }
            return result;
        }

        public AthleteTestMapping GetAthlete(int athleteId)
        {
            AthleteTestMapping record = null;
            try
            {
                record = _dbContext.AthleteTestMapping.Where(x => x.AthleteId == athleteId && x.IsDeleted == false).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToSafeString());
            }
            return record;
        }

        public int DeleteAthlete(int mapId)
        {
            int record = 0;
            AthleteTestMapping athleteTest = new AthleteTestMapping();
            try
            {
                athleteTest = _dbContext.AthleteTestMapping.Find(mapId);
                athleteTest.IsDeleted = true;
                _dbContext.Update(athleteTest);

                Test test = _dbContext.Test.Where(c => c.TestId == athleteTest.AthleteTestId).FirstOrDefault();
                test.TotalParticipats = test.TotalParticipats.ToSafeInt() - 1;
                _dbContext.Test.Update(test);

                _dbContext.SaveChanges();
                record = 1;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToSafeString());
            }
            return record;
        }

        public int DeleteTest(int testId)
        {
            int record = 0;
            List<AthleteTestMapping> athleteTestList = new List<AthleteTestMapping>();
            Test test = new Test();
            try
            {
                athleteTestList = _dbContext.AthleteTestMapping.Where(athlete => athlete.AthleteTestId == testId).ToList();
                test = _dbContext.Test.Where(t => t.TestId == testId).FirstOrDefault();
                test.TotalParticipats = 0;
                test.IsDeleted = true;
                _dbContext.Update(test);

                foreach (AthleteTestMapping athleteTestMapping in athleteTestList)
                {
                    athleteTestMapping.IsDeleted = true;
                    _dbContext.Update(athleteTestMapping);
                }
                _dbContext.SaveChanges();
                record = 1;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToSafeString());
            }
            return record;
        }
    }
}