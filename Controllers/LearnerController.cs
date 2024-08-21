using LMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS.Controllers
{
    public class LearnerController : Controller
    {
        DbOperation lerOperation = new DbOperation();

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]

        /*learner panel*/
        public ActionResult LearnerPanel()
        {
            if (Session["LearnerModel"] == null)
            {
                return RedirectToAction("Welcome", "Home");
            }

            /*// get course code and name from Learner_Courses Table
             List<Module> moduleList = lerOperation.GetModuleList(string courseCode, int instructorId);*/
            var learnerModel = Session["LearnerModel"] as LearnerModel;
            List<Identity> identitylist = lerOperation.LearnerAllowedCourseList(learnerModel.LearnerId);

            ViewBag.AllowedList = identitylist;
            ViewBag.Learner = learnerModel;
            return View();
        }

        [HttpPost]
        /*learner logout*/
        public JsonResult LearnerLogout()
        {
            Session.Clear();
            Session.Abandon();

            return Json(new
            {
                success = true,
                message = "Logout successful"
            });


        }

        /*learnr update*/

        public JsonResult UpdateLearner(string LearnerEditName, string LearnerEditEmail, string LearnerEditPassword)
        {
            var learnerModel = Session["LearnerModel"] as LearnerModel;

            if (learnerModel == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Learner Session Not Found!"
                });
            }

            bool check = lerOperation.LearnerEditDb(LearnerEditName, LearnerEditEmail, LearnerEditPassword, learnerModel.LearnerEmail, learnerModel.LearnerPassword);

            if (check)
            {
                learnerModel.LearnerName = LearnerEditName;
                learnerModel.LearnerEmail = LearnerEditEmail;
                learnerModel.LearnerPassword = LearnerEditPassword;

                Session["LearnerModel"] = learnerModel;

                return Json(new
                {
                    success = true,
                    message = "Updated Successfully"
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

        /*learner panel module list w.r.t course code and coursename*/
        public JsonResult InsCourseModuleList(string courseCode, string courseName)
        {
            if (Session["LearnerModel"] == null)
            {
                return Json(new { success = false, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            }


            List<Module> modules = lerOperation.GetInstructorModuleList(courseName, courseCode);

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
            var moduleLink = lerOperation.GetModuleLinkById(moduleId);

            return Json(new { moduleLink }, JsonRequestBehavior.AllowGet);
        }

    }
}