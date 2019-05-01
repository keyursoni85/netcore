/*
Application - The Multi server multi operating system control panel © copyright 2018.

Application is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License 3.0 as published by the Free Software Foundation.

This file is part of the Application project and is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License along with this file; if not, please contact Application at https://www.Application.com/contact

https://www.gnu.org/licenses/lgpl-3.0.txt
*/

using Application.Entities;
using Application.Framework.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Application.Business.BAL
{
    public class BALUser
    {
        #region Find

        public static Test GetTestTypeByTestId(int id, string domainName)
        {
            Test record = null;
            string strJson = string.Empty;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(domainName);
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/User/GetTestTypeByTestId?testId=" + id).Result;
                strJson = response.Content.ReadAsStringAsync().Result.ToSafeString();
                record = JsonConvert.DeserializeObject<Test>(strJson);
            }
            return record;
        }

        public static List<User> GetUserListByRoleId(int? rId, string domainName)
        {
            List<User> users = new List<User>();
            string strJson = string.Empty;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(domainName);
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/User/GetUserListByRoleId?rId=" + rId).Result;
                strJson = response.Content.ReadAsStringAsync().Result.ToSafeString();
                users = JsonConvert.DeserializeObject<List<User>>(strJson);
            }
            return users;
        }

        #endregion

        public static List<Test> GetTestList(string domainName)
        {
            List<Test> tests = new List<Test>();
            string strJson = string.Empty;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(domainName);
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/User/GetTestList").Result;
                strJson = response.Content.ReadAsStringAsync().Result.ToSafeString();
                tests = JsonConvert.DeserializeObject<List<Test>>(strJson);
            }
            return tests;
        }

        public static int AddTest(Test test, string domainName)
        {
            int result = 0;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(domainName);
                HttpResponseMessage response = client.PostAsync(client.BaseAddress + "/User/AddTest?typeId=" + test.TestTypeId.ToSafeInt() + "&testName=" + test.TestName.ToSafeString() + "&date=" + test.TestDate, null).Result;
                result = response.Content.ReadAsStringAsync().Result.ToSafeInt();
            }
            return result;
        }

        public static List<AthleteTestMapping> GetAthletesByTypeId(int typeId, string domainName)
        {
            List<AthleteTestMapping> athletesList = new List<AthleteTestMapping>();
            string strJson = string.Empty;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(domainName);
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/User/GetAthletesByTypeId?typeId=" + typeId.ToSafeInt()).Result;
                strJson = response.Content.ReadAsStringAsync().Result.ToSafeString();
                athletesList = JsonConvert.DeserializeObject<List<AthleteTestMapping>>(strJson);
            }
            return athletesList;
        }

        public static int AddAthlete(AthleteTestMapping athleteTestMapping, string domainName, int editMode, int mapId)
        {
            int result = 0;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(domainName);
                HttpResponseMessage response = client.PostAsync(client.BaseAddress + "/User/AddAthlete?athleteTestId=" + athleteTestMapping.AthleteTestId.ToSafeInt() + "&athleteId=" + athleteTestMapping.AthleteId.ToSafeInt() + "&distance=" + athleteTestMapping.Distance + "&editMode=" + editMode + "&MapId=" + mapId, null).Result;
                result = response.Content.ReadAsStringAsync().Result.ToSafeInt();
            }
            return result;
        }

        public static AthleteTestMapping GetAthlete(int athleteId, string domainName)
        {
            AthleteTestMapping record = null;
            string strJson = string.Empty;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(domainName);
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/User/GetAthlete?athleteId=" + athleteId).Result;
                strJson = response.Content.ReadAsStringAsync().Result.ToSafeString();
                record = JsonConvert.DeserializeObject<AthleteTestMapping>(strJson);
            }
            return record;
        }

        public static int DeleteAthlete(int mapId, string domainName)
        {
            int result = 0;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(domainName);
                HttpResponseMessage response = client.PostAsync(client.BaseAddress + "/User/DeleteAthlete?mapId=" + mapId.ToSafeInt(), null).Result;
                result = response.Content.ReadAsStringAsync().Result.ToSafeInt();
            }
            return result;
        }

        public static int DeleteTest(int testId, string domainName)
        {
            int result = 0;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(domainName);
                HttpResponseMessage response = client.PostAsync(client.BaseAddress + "/User/DeleteTest?testId=" + testId.ToSafeInt(), null).Result;
                result = response.Content.ReadAsStringAsync().Result.ToSafeInt();
            }
            return result;
        }
    }
}
