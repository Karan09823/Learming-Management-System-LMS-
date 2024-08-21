using LMS.Models;
using System.Collections.Generic;
using System.EnterpriseServices.CompensatingResourceManager;
using System.Linq;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace LMS.Controllers
{
    public class AdminController : Controller
    {
        DbOperation adminOperation = new DbOperation();

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        /*admin panel*/
        public ActionResult AdminPanel()
        {
            // Check if the session exists
            if (Session["AdminModel"] == null) // Ensure consistent session key usage
            {
                return RedirectToAction("Welcome", "Home");
            }

            // Load the admin panel view
            var adminModel = Session["AdminModel"] as AdminModel;

            List<MasterRecord> masterRecords = adminOperation.MasterRecordDb();

            int totalInstructor = adminOperation.TotalInstructorDb();
            int totalLearner=adminOperation.TotalLearnerDb();
            int totalCourses=adminOperation.TotalCourseDb();
            ViewBag.AdminModel = adminModel;
            ViewBag.TotalInstructorDb = totalInstructor;
            ViewBag.LearnerDb = totalLearner;
            ViewBag.CoursesDb = totalCourses;

            ViewBag.MasterRecord=masterRecords;
            return View();


            /*List<MasterRecord> master=adminOperation.MasterRecordDb();    --  use to populated the instructor and their related learner id and name*/
        }

        /*admin instructor*/
        public ActionResult Admin_Instructor()
        {
            if (Session["AdminModel"] == null)
            {
                return RedirectToAction("Welcome", "Home");
            }

            List<InstructorModel> instructLi = adminOperation.GetInstructorList();
            var adminModel = Session["AdminModel"] as AdminModel;
            ViewBag.AdminModel = adminModel;
            return View(instructLi);
        }

        /*admin learner*/
        public ActionResult Admin_Learner()
        {
            if (Session["AdminModel"] == null)
            {
                return RedirectToAction("Welcome", "Home");
            }

            List<LearnerModel> learnerLi = adminOperation.GetLearnerList();
            var adminModel = Session["AdminModel"] as AdminModel;
            ViewBag.AdminModel = adminModel;
            return View(learnerLi);
        }

        /*admin courses*/
        public ActionResult Admin_courses()
        {
            if (Session["AdminModel"] == null)
            {
                return RedirectToAction("Welcome", "Home");
            }

            // Get the data from the database or other sources
            List<Identity> identityList = adminOperation.GetInstructorIdentityList();



            // Create the model to pass to the view
            var adminModel = Session["AdminModel"] as AdminModel;

            // Pass data to the view
            ViewBag.AdminModel = adminModel;
            ViewBag.IdentityList = identityList;


            return View();
        }


        // Update admin details
        [HttpPost]
        public JsonResult UpdateAdmin(string AdminName, string AdminEmail, string AdminPassword)
        {
            var adminModel = Session["AdminModel"] as AdminModel;
            if (adminModel == null)
            {
                return Json(new { success = false, message = "Admin session not found" });
            }

            // Use the original email and password stored in session for the WHERE clause
            bool check = adminOperation.AdminEditDb(AdminName, AdminEmail, AdminPassword, adminModel.Admin_Email, adminModel.Admin_password);
            if (check)
            {
                // Update session with new values
                adminModel.Admin_Name = AdminName;
                adminModel.Admin_Email = AdminEmail;
                adminModel.Admin_password = AdminPassword;
                Session["AdminModel"] = adminModel;

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


        // Admin logout logic
        [HttpPost]
        public JsonResult Adminlogout()
        {
            Session.Clear();
            Session.Abandon();

            return Json(new
            {
                success = true,
                message = "Logout successful"
            });
        }

        /*admin instruct edit controller*/
        [HttpPost]
        public JsonResult AdminInstructUpdate(string AdminInstructName, string AdminInstructEmail, string AdminInstructPassword, string OriginalInstructorId, string OriginalInstructorEmail, string OriginalInstructorPassword)
        {
            var adminModel = Session["AdminModel"] as AdminModel;
            if (adminModel == null)
            {
                return Json(new { success = false, message = "Admin session not found" });
            }

            bool success = adminOperation.AdminInstructUpdateDb(AdminInstructName, AdminInstructEmail, AdminInstructPassword, OriginalInstructorEmail, OriginalInstructorPassword, OriginalInstructorId);

            if (success)
            {
                return Json(new { success = true, message = "Instructor updated successfully" });
            }
            else
            {
                return Json(new { success = false, message = "Failed to update instructor" });
            }
        }

        /*admin instruct delete*/
        [HttpPost]
        public JsonResult DeleteAdminInstructController(string InstructorId, string InstructorEmail)
        {
            var adminModel = Session["AdminModel"] as AdminModel;
            if (adminModel == null)
            {
                return Json(new { success = false, message = "Admin session not found" });
            }

            bool check = adminOperation.DeleteAdminInstructorDb(InstructorId, InstructorEmail);

            if (check)
            {
                return Json(new { success = true, message = "Instructor Deleted" });
            }
            else
            {
                return Json(new { success = false, message = "Failed to Delete Instructor!" });
            }
        }




        /*admin learner update*/
        [HttpPost]
        public JsonResult UpdateLearner(string OriginalLearnerId, string LearnerName, string LearnerEmail, string LearnerPassword)
        {
            if (Session["AdminModel"] == null)
            {
                return Json(new { success = false, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            }

            var result = adminOperation.UpdateLearnerDb(OriginalLearnerId, LearnerName, LearnerEmail, LearnerPassword);

            if (result)
            {
                return Json(new { success = true, message = "Learner updated successfully" });
            }
            else
            {
                return Json(new { success = false, message = "Failed to update learner" });
            }
        }



        /*admin learner delete*/
        [HttpPost]
        public JsonResult DeleteLearner(string id, string email)
        {
            if (Session["AdminModel"] == null)
            {
                return Json(new { success = false, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            }

            var result = adminOperation.DeleteLearnerDb(id, email);

            if (result)
            {
                return Json(new { success = true, message = "Learner deleted successfully" });
            }
            else
            {
                return Json(new { success = false, message = "Failed to delete learner" });
            }
        }

        /*admin course panel module list w.r.t course code and coursename*/
        public JsonResult AdminCourseModuleList(string courseCode, string courseName)
        {
            if (Session["AdminModel"] == null)
            {
                return Json(new { success = false, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            }


            List<Module> modules = adminOperation.GetInstructorModuleList(courseName, courseCode);

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
            var moduleLink = adminOperation.GetModuleLinkById(moduleId);

            return Json(new { moduleLink }, JsonRequestBehavior.AllowGet);
        }



        /*delete modules in admin course panel*/
        [HttpPost]
        public JsonResult DeleteModule(int moduleId)
        {
            if (Session["AdminModel"] == null)
            {
                return Json(new { success = false, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            }

            bool check = adminOperation.DeleteModuleById(moduleId);

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
        public JsonResult EditModuleAdminPanel(int moduleId,string moduleName,string moduleLink)
        {
            if (Session["AdminModel"] == null)
            {
                return Json(new { success = false, message = "Session expired" }, JsonRequestBehavior.AllowGet);
            }

            bool check=adminOperation.UpdateAdminPanelModule(moduleId, moduleName, moduleLink);

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





    }
}
