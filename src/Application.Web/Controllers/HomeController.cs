namespace Application.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Application.Web.Models;
    using Application.Data;
    using Application.Framework.Utils;
    using System.Collections.Generic;
    using Application.Business.BAL;
    using Microsoft.Extensions.Configuration;
    using Application.Entities;

    public class HomeController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public HomeController(IUserRepository userRepository, IConfiguration config)
        {
            this._userRepository = userRepository;
            this._configuration = config;
        }

        public IActionResult Index()
        {
            List<Test> model = BALUser.GetTestList(_configuration["DomainName"].ToSafeString());
            string message = string.Empty;
            if (TempData["message"] != null)
            {
                message = TempData["message"].ToSafeString();
                ViewBag.Message = message;

                if (TempData["IsSuccess"].ToSafeInt() == 0)
                {
                    ViewBag.IsError = 1;
                }
                else
                {
                    ViewBag.IsForgotPwdSucc = 1;
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult CreateTest(int? id)
        {
            return PartialView("~/Views/Home/AddTest.cshtml", new Test());
        }

        [HttpPost]
        public ActionResult CreateTest(Test model)
        {
            int result = 0;
            string domainName = _configuration["DomainName"].ToSafeString();
            if (!string.IsNullOrEmpty(domainName))
            {
                result = BALUser.AddTest(model, domainName);
            }

            switch (result)
            {
                case -1:
                    TempData["message"] = "Test Name already exists for same date.";
                    TempData["IsSuccess"] = 0;
                    break;
                case 0:
                    TempData["message"] = "Something went wrong.";
                    TempData["IsSuccess"] = 0;
                    break;
                case 1:
                    TempData["message"] = "Record successfully inserted.";
                    TempData["IsSuccess"] = 1;
                    break;
                case 2:
                    TempData["message"] = "Record successfully updated.";
                    TempData["IsSuccess"] = 1;
                    break;
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AthleteList(int? id)
        {
            AthleteModel model = new AthleteModel();
            string message = string.Empty;
            try
            {
                model.athleteList = BALUser.GetAthletesByTypeId(id.ToSafeInt(), _configuration["DomainName"].ToSafeString());
                model.TestId = id.ToSafeInt();

                if (TempData["message"] != null)
                {
                    message = TempData["message"].ToSafeString();
                    ViewBag.Message = message;

                    if (TempData["IsSuccess"].ToSafeInt() == 0)
                    {
                        ViewBag.IsError = 1;
                    }
                    else
                    {
                        ViewBag.IsForgotPwdSucc = 1;
                    }
                }
            }
            catch (System.Exception)
            {
                throw;
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult CreateAthlete(int? testId, int? athleteId)
        {
            AthleteModel model = new AthleteModel();
            Test test = new Test();
            AthleteTestMapping athleteTestMapping = new AthleteTestMapping();
            string domainName = _configuration["DomainName"].ToSafeString();
            if (!string.IsNullOrEmpty(domainName))
            {
                if (testId.HasValue && athleteId.ToSafeInt() == 0)
                {
                    model.TestId = testId.ToSafeInt();
                    model.userList = BALUser.GetUserListByRoleId((int)Enums.UserRole.Athlete, domainName);
                    test = BALUser.GetTestTypeByTestId(testId.ToSafeInt(), domainName);
                    model.TestName = test.TestName.ToSafeString() + " " + test.TestDate.ToString("yyyy/MM/dd");
                    model.IsEditMode = false;
                }
                else
                {
                    model.TestId = testId.ToSafeInt();
                    model.userList = BALUser.GetUserListByRoleId((int)Enums.UserRole.Athlete, domainName);
                    test = BALUser.GetTestTypeByTestId(testId.ToSafeInt(), domainName);
                    model.TestName = test.TestName.ToSafeString() + " " + test.TestDate.ToString("yyyy/MM/dd");

                    athleteTestMapping = BALUser.GetAthlete(athleteId.ToSafeInt(), domainName);

                    if (athleteTestMapping != null)
                    {
                        model.AthleteId = athleteTestMapping.AthleteId.ToSafeInt();
                        model.Distance = athleteTestMapping.Distance;
                        model.IsEditMode = true;
                        model.MapId = athleteTestMapping.MapId.ToSafeInt();
                    }
                }
            }
            return PartialView("~/Views/Home/AddAthlete.cshtml", model);
        }

        [HttpPost]
        public IActionResult CreateNewAthlete(AthleteModel model)
        {
            int result = 0;
            string domainName = _configuration["DomainName"].ToSafeString();
            if (!string.IsNullOrEmpty(domainName))
            {
                AthleteTestMapping athleteTest = new AthleteTestMapping();
                athleteTest.AthleteTestId = model.TestId.ToSafeInt();
                athleteTest.AthleteId = model.AthleteId.ToSafeInt();
                athleteTest.Distance = model.Distance;

                if (model.IsEditMode)
                {
                    result = BALUser.AddAthlete(athleteTest, domainName, 1, model.MapId.ToSafeInt());
                }
                else
                {
                    result = BALUser.AddAthlete(athleteTest, domainName, 0, model.MapId.ToSafeInt());
                }

                switch (result)
                {
                    case -1:
                        TempData["message"] = "Athlete already exists in this test.";
                        TempData["IsSuccess"] = 0;
                        break;
                    case 0:
                        TempData["message"] = "Something went wrong.";
                        TempData["IsSuccess"] = 0;
                        break;
                    case 1:
                        TempData["message"] = "Record successfully inserted.";
                        TempData["IsSuccess"] = 1;
                        break;
                    case 2:
                        TempData["message"] = "Record successfully updated.";
                        TempData["IsSuccess"] = 1;
                        break;
                }
            }
            return RedirectToAction("AthleteList", new { id = model.TestId });
        }

        [HttpGet]
        public IActionResult DeleteAthlete(int mapId, int testId)
        {
            return PartialView("~/Views/Home/DeleteAthlete.cshtml", new AthleteModel
            {
                MapId = mapId.ToSafeInt(),
                TestId = testId.ToSafeInt()
            });
        }

        [HttpPost]
        public IActionResult DeleteAthlete(AthleteModel model)
        {
            int result = 0;
            string domainName = _configuration["DomainName"].ToSafeString();
            if (!string.IsNullOrEmpty(domainName))
            {
                result = BALUser.DeleteAthlete(model.MapId.ToSafeInt(), domainName);

                switch (result)
                {
                    case 0:
                        TempData["message"] = "Something went wrong.";
                        TempData["IsSuccess"] = 0;
                        break;
                    case 1:
                        TempData["message"] = "Record successfully deleted.";
                        TempData["IsSuccess"] = 1;
                        break;
                }

            }
            return RedirectToAction("AthleteList", new { id = model.TestId });
        }

        [HttpGet]
        public IActionResult DeleteTest(int? id)
        {
            return PartialView("~/Views/Home/DeleteTest.cshtml", new AthleteModel
            {
                TestId = id.ToSafeInt()
            });
        }

        [HttpPost]
        public IActionResult DeleteTest(AthleteModel model)
        {
            int result = 0;
            string domainName = _configuration["DomainName"].ToSafeString();
            if (!string.IsNullOrEmpty(domainName))
            {
                result = BALUser.DeleteTest(model.TestId.ToSafeInt(), domainName);

                switch (result)
                {
                    case 0:
                        TempData["message"] = "Something went wrong.";
                        TempData["IsSuccess"] = 0;
                        break;
                    case 1:
                        TempData["message"] = "Record successfully deleted.";
                        TempData["IsSuccess"] = 1;
                        break;
                }
            }
            return RedirectToAction("Index");
        }
    }
}
