namespace Application.Data
{
    using Application.Entities;
    using System;
    using System.Collections.Generic;

    public interface IUserRepository
    {
        Test GetTestTypeByTestId(int testId);

        List<User> GetUserListByRoleId(int uId);

        List<Test> GetTestList();

        int AddTest(int typeId, string testName, DateTime date);

        List<AthleteTestMapping> GetAthletesByTypeId(int typeId);

        int AddAthlete(int athleteTestId, int athleteId, decimal distance, int editMode, int mapId);

        AthleteTestMapping GetAthlete(int athleteId);

        int DeleteAthlete(int mapId);

        int DeleteTest(int testId);
    }
}