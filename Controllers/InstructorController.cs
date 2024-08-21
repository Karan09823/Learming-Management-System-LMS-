using LMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS.Controllers
{
    public class InstructorController : Controller
    {
        DbOperation instructOperation = new DbOperation();

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        /*instructor panel*/
        public ActionResult InstructorPanel()
        {
            if (Session["InstructorModel"] == null)
            {
                return RedirectToAction("Welcome", "Home");
            }

            var instructorModel = Session["InstructorModel"] as InstructorModel;
            int totalLearner = instructOperation.TotalLearnerDb();
            int totalActiveLearner = instructOperation.TotalActiveLearnerInsPanel(instructorModel.InstructorId);
            int totalInsCourse = instructOperation.TotalAddedCourseIns(instructorModel.InstructorId);

            ViewBag.TotalLearner = totalLearner;
            ViewBag.TotalActiveLearner = totalActiveLearner;
            ViewBag.TotalInsCourse = totalInsCourse;
            ViewBag.Instructor = instructorModel;
            return View();
        }
        /*instructor learner*/
        public ActionResult InstructorLearner()
        {
            var instructModel = Session["InstructorModel"] as InstructorModel;
            if (instructModel == null)
            {
                return RedirectToAction("Welcome", "Home");
            }
            List<Identity> AddedCourseList = instructOperation.GetInstructorIdentityById(instructModel.InstructorId);

            List<Details> activelearner = instructOperation.DetailsDb(instructModel.InstructorId);

            ViewBag.ActiveLearner = activelearner;
            ViewBag.Instructor = instructModel;
            ViewBag.AddedCourse = AddedCourseList;
            return View();
        }

        /*instructor courses*/
        public ActionResult InstructorCourses()
        {


            var instructModel = Session["InstructorModel"] as InstructorModel;
            if (instructModel == null)
            {
                return RedirectToAction("Welcome", "Home");
            }
            List<Identity> AddedCourseList = instructOperation.GetInstructorIdentityById(instructModel.InstructorId);
            ViewBag.AddedCourse = AddedCourseList;
            ViewBag.Instructor = instructModel;
            return View();


        }

        /*instructor logout*/
        [HttpPost]
        public JsonResult Instructorlogout()
        {
            Session.Clear();
            Session.Abandon();

            return Json(new
            {
                success = true,
                message = "Logout successful"
            });

        }

        /* instructor update*/

        public JsonResult UpdateInstructor(string InstructorNameEdit, string InstructorEmailEdit, string InstructorPasswordEdit)
        {
            var instructModel = Session["InstructorModel"] as InstructorModel;
            if (instructModel == null)
            {
                return Json(new
                {
                    success = false, // Changed to false to indicate failure
                    message = "Instructor session not found"
                });
            }

            bool check = instructOperation.InstructorEditDb(InstructorNameEdit, InstructorEmailEdit, InstructorPasswordEdit, instructModel.InstructorEmail, instructModel.InstructorPassword);

            if (check)
            {
                instructModel.InstructorName = InstructorNameEdit;
                instructModel.InstructorEmail = InstructorEmailEdit;
                instructModel.InstructorPassword = InstructorPasswordEdit;

                Session["InstructorModel"] = instructModel;

                return Json(new
                {
                    success = true,
                    message = "Data Updated Successfully"
                });
            }
            else
            {
                return Json(new
                {
                    success = false,
                    message = "Something went wrong!"
                });
            }
        }

        /*instructor add course controller*/
        [HttpPost]
        public JsonResult AddInsCourse(string courseName, string coursecode, string instructorId, Dictionary<string, string> courses)
        {
            var instructModel = Session["InstructorModel"] as InstructorModel;
            if (instructModel == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Instructor session not found"
                });
            }

            bool check = instructOperation.AddInstructorCourseDb(courseName, instructorId, courses, coursecode);

            if (check)
            {
                return Json(new
                {
                    success = true,
                    message = "Course Added Successfully"
                });
            }
            else
            {
                return Json(new
                {
                    success = false,
                    message = "Failed to add course"
                });
            }
        }

        /*get course modules by course code and name logic version 2*/

        public JsonResult GetModuleListByCode(string courseCode, string courseName)
        {
            var instructModel = Session["InstructorModel"] as InstructorModel;
            if (instructModel == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Instructor session not found"
                });
            }

            List<Module> mList = instructOperation.GetInstructorModuleList(courseName, courseCode);

            if (mList != null)
            {
                return Json(new
                {
                    success = true,
                    message = "Success"
                });
            }

            else
            {
                return Json(new
                {
                    success = false,
                    message = "Failed "
                });
            }

        }

        /*instructor course access controller*/

        public JsonResult GrantAccessController(int InstructorId, string InstructorName, string courseCode, string courseName, int Learner_Id, int flag, string LearnerName)
        {
            var instructModel = Session["InstructorModel"] as InstructorModel;
            if (instructModel == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Instructor session not found"
                });
            }
            bool check = instructOperation.GrantAccessDb(InstructorId, InstructorName, courseCode, courseName, Learner_Id, flag, LearnerName);

            if (check)
            {
                return Json(new
                {
                    success = true,
                    message = "Access Granted"
                });
            }
            else
            {
                return Json(new
                {
                    success = false,
                    message = "Something went wrong !"
                });
            }

        }

        /*===================================================================================================*/

        /*instructor course panel module list w.r.t course code and coursename*/
        public JsonResult InsCourseModuleList(string courseCode, string courseName)
        {
            if (Session["InstructorModel"] == null)
            {
                return Json(new { success = false, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            }


            List<Module> modules = instructOperation.GetInstructorModuleList(courseName, courseCode);

            // Map modules to a more suitable format
            var moduleList = modules.Select(m => new
            {
                m.ModuleId,
                m.ModuleName,
                m.ModuleLink
            }).ToList();


            var result = new
            {
                modules = moduleList,

            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // Action to get module link by Id (if needed separately)
        public JsonResult GetModuleLink(int moduleId)
        {
            var moduleLink = instructOperation.GetModuleLinkById(moduleId);

            return Json(new { moduleLink }, JsonRequestBehavior.AllowGet);
        }



        /*delete modules in instructor course panel*/
        [HttpPost]
        public JsonResult InsCourseModuleDelete(int moduleId)
        {
            if (Session["InstructorModel"] == null)
            {
                return Json(new { success = false, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            }

            bool check = instructOperation.DeleteModuleById(moduleId);

            if (check)
            {
                return Json(new
                {
                    success = true,
                    message = "Module Deleted Successfully"
                });
            }
            else
            {
                return Json(new
                {
                    success = false, // Corrected to `false` on failure
                    message = "Something went wrong!"
                });
            }
        }


        //Edit Module in  AdminPanel
        [HttpPost]
        public JsonResult InsCourseModuleUpdate(int moduleId, string moduleName, string moduleLink)
        {
            if (Session["InstructorModel"] == null)
            {
                return Json(new { success = false, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            }

            bool check = instructOperation.UpdateAdminPanelModule(moduleId, moduleName, moduleLink);

            if (check)
            {
                return Json(new
                {
                    success = true,
                    message = "Module Updated Successfully "
                });
            }
            else
            {
                return Json(new
                {
                    success = true,
                    message = "Something went wrong ! "

                });

            }
        }

        /*updating the learner_courses table*/

        public JsonResult ActivateLearner(int instructorId, string instructorName, string learnerEmail, string courseCode, string courseName)
        {

            if (Session["InstructorModel"] == null)
            {
                return Json(new { success = false, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            }

            CustomLearner le = instructOperation.GetCustomLearner(learnerEmail);

            bool check = instructOperation.GrantLearnerByInstructor(instructorId, instructorName, courseCode, courseName, le.CL_Id, le.CL_Name, learnerEmail);

            if (check)
            {
                return Json(new
                {
                    success = true,
                    message = "Access Granted Successfully"
                });
            }
            else
            {
                return Json(new
                {
                    success = true,
                    message = "Something Went Wrong !"
                });
            }

        }

        /*deactive learner */
        public JsonResult DeactivateLearner(int instructorId, int learnerId, string courseCode)
        {
            if (Session["InstructorModel"] == null)
            {
                return Json(new { success = false, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            }

            bool check = instructOperation.DeactivateLearnerDb(instructorId, learnerId, courseCode);

            if (check)
            {
                return Json(new
                {
                    success = true,
                    message = "Deactivation Successful"
                });
            }
            else
            {
                return Json(new
                {
                    success = true,
                    message = "Something Went Wrong !"
                });
            }

        }






    }
}