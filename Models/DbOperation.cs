using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Antlr.Runtime.Tree;
using LMS.Models;

namespace LMS.Models
{
    public class DbOperation
    {
        SqlConnection Con = new SqlConnection(ConfigurationManager.ConnectionStrings["LMS"].ConnectionString);



        /*admin register Dboperation*/
        /* public bool AdminRegisterDb(string name, string email, string password, string role)
         {
             string query = "INSERT INTO Admin_table (Admin_Name, Admin_Email, Admin_password, Admin_role) VALUES (@Name, @Email, @Password, @Role)";
             using (SqlCommand cmd = new SqlCommand(query, Con))
             {

                 cmd.Parameters.AddWithValue("@Name", name);
                 cmd.Parameters.AddWithValue("@Email", email);
                 cmd.Parameters.AddWithValue("@Password", password);
                 cmd.Parameters.AddWithValue("@Role", role);

                 try
                 {
                     Con.Open();
                     int m = cmd.ExecuteNonQuery();
                     return m > 0;
                 }
                 catch (Exception ex)
                 {
                     Console.WriteLine(ex.Message);
                     return false;
                 }
                 finally
                 {

                     Con.Close();
                 }
             }
         }
 */
        /*instructor register Dboperation*/
        public bool InstructorRegisterDb(string name, string email, string password)
        {
            string checkEmailQuery = "SELECT COUNT(*) FROM Instructor_table WHERE Instructor_Email = @Email";
            string insertQuery = "INSERT INTO Instructor_table (Instructor_Name, Instructor_Email, Instructor_password) VALUES (@Name, @Email, @Password)";

            using (SqlCommand checkCmd = new SqlCommand(checkEmailQuery, Con))
            {
                checkCmd.Parameters.AddWithValue("@Email", email);

                try
                {
                    Con.Open();
                    int emailCount = (int)checkCmd.ExecuteScalar();
                    if (emailCount > 0)
                    {
                        // Email already exists
                        return false;
                    }

                    // Email does not exist, proceed to insert
                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, Con))
                    {
                        insertCmd.Parameters.AddWithValue("@Name", name);
                        insertCmd.Parameters.AddWithValue("@Email", email);
                        insertCmd.Parameters.AddWithValue("@Password", password);

                        int rowsAffected = insertCmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    // Log exception
                    Console.WriteLine(ex.Message);
                    return false;
                }
                finally
                {
                    Con.Close();
                }
            }
        }



        /*learner register operation*/
        public bool LearnerRegisterDb(string name, string email, string password)
        {
            string checkEmailQuery = "SELECT COUNT(*) FROM Learner_table WHERE Learner_email = @Email";
            string query = "insert into Learner_table(Learner_name,Learner_email,Learner_password) values(@Name,@Email,@Password)";

            using (SqlCommand checkCmd = new SqlCommand(checkEmailQuery, Con))
            {
                checkCmd.Parameters.AddWithValue("@Email", email);

                try
                {


                    Con.Open();
                    int emailCount = (int)checkCmd.ExecuteScalar();
                    if (emailCount > 0)
                    {
                        // Email already exists
                        return false;
                    }

                    // Email does not exist, proceed to insert
                    using (SqlCommand insertCmd = new SqlCommand(query, Con))
                    {
                        insertCmd.Parameters.AddWithValue("@Name", name);
                        insertCmd.Parameters.AddWithValue("@Email", email);
                        insertCmd.Parameters.AddWithValue("@Password", password);

                        int rowsAffected = insertCmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }

                }
                catch (Exception ex)
                {
                    // Log exception
                    Console.WriteLine(ex.Message);
                    return false;
                }
                finally
                {
                    Con.Close();
                }
            }



        }

        /*admin login process db operation*/
        public AdminModel AdminLoginDb(string emailId, string password)
        {

            string query = "select * from Admin_table where Admin_Email=@Email and Admin_password=@Password";

            using (SqlCommand cmd = new SqlCommand(query, Con))
            {
                cmd.Parameters.AddWithValue("@Email", emailId);
                cmd.Parameters.AddWithValue("@Password", password);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    AdminModel admin = new AdminModel();

                    int adminId = Convert.ToInt32(dt.Rows[0]["Admin_id"]);
                    admin.Admin_id = adminId;
                    admin.Admin_Name = Convert.ToString(dt.Rows[0]["Admin_Name"]);
                    admin.Admin_Email = Convert.ToString(dt.Rows[0]["Admin_Email"]);
                    admin.Admin_password = Convert.ToString(dt.Rows[0]["Admin_password"]);
                    /* admin.Admin_role = Convert.ToString(dt.Rows[0]["Admin_role"]);*/

                    return admin;
                }
                else
                {
                    return null;
                }


            }

        }

        /*Instructor login db operation*/

        public InstructorModel InstructorLoginDb(string emailId, string password)
        {

            string query = "select * from Instructor_table where Instructor_Email=@Email and Instructor_password=@Password";

            using (SqlCommand cmd = new SqlCommand(query, Con))
            {
                cmd.Parameters.AddWithValue("@Email", emailId);
                cmd.Parameters.AddWithValue("@Password", password);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    InstructorModel instructor = new InstructorModel();

                    int instruct = Convert.ToInt32(dt.Rows[0]["Instructor_id"]);

                    instructor.InstructorId = instruct;
                    instructor.InstructorName = Convert.ToString(dt.Rows[0]["Instructor_Name"]);
                    instructor.InstructorEmail = Convert.ToString(dt.Rows[0]["Instructor_Email"]);
                    instructor.InstructorPassword = Convert.ToString(dt.Rows[0]["Instructor_password"]);
                    /*instructor.InstructorCourse = Convert.ToString(dt.Rows[0]["Instructor_course"]);*/
                    /* instructor.InstructorRole = Convert.ToString(dt.Rows[0]["Instructor_role"]);*/

                    return instructor;
                }
                else
                {
                    return null;
                }
            }

        }

        /*learner login dboperation*/

        public LearnerModel LearnerLogignDb(string emailId, string password)
        {
            string query = "select * from Learner_table where Learner_email=@Email and Learner_password=@Password";

            using (SqlCommand cmd = new SqlCommand(query, Con))
            {
                cmd.Parameters.AddWithValue("@Email", emailId);
                cmd.Parameters.AddWithValue("@Password", password);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    LearnerModel lear = new LearnerModel();

                    int learnerid = Convert.ToInt32(dt.Rows[0]["Learner_id"]);
                    lear.LearnerId = learnerid;
                    lear.LearnerName = Convert.ToString(dt.Rows[0]["Learner_name"]);
                    lear.LearnerEmail = Convert.ToString(dt.Rows[0]["Learner_email"]);
                    lear.LearnerPassword = Convert.ToString(dt.Rows[0]["Learner_password"]);

                    return lear;

                }
                else
                {
                    return null;
                }

            }


        }

        /*admin modal edit db operation*/

        public bool AdminEditDb(string name, string email, string password, string originalEmail, string originalPassword)
        {
            string query = "update Admin_table set Admin_Name=@Name, Admin_Email=@Email, Admin_password=@Password " +
                           "where Admin_Email=@OriginalEmail and Admin_password=@OriginalPassword";

            using (SqlCommand cmd = new SqlCommand(query, Con))
            {
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@OriginalEmail", originalEmail);
                cmd.Parameters.AddWithValue("@OriginalPassword", originalPassword);

                try
                {
                    Con.Open();
                    int m = cmd.ExecuteNonQuery();
                    return m > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
                finally
                {
                    Con.Close();
                }
            }
        }

        /*get instructor list*/

        public List<InstructorModel> GetInstructorList()
        {
            List<InstructorModel> list = new List<InstructorModel>();

            using (SqlCommand cmd = new SqlCommand("select * from Instructor_table", Con))
            {

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new InstructorModel()
                    {
                        InstructorId = Convert.ToInt32(dr[0]),
                        InstructorName = Convert.ToString(dr[1]),
                        InstructorEmail = Convert.ToString(dr[2]),
                        InstructorPassword = Convert.ToString(dr[3]),



                    });

                }

            }

            return list;
        }

        /* get learner list*/

        public List<LearnerModel> GetLearnerList()
        {
            List<LearnerModel> list = new List<LearnerModel>();

            using (SqlCommand cmd = new SqlCommand("select * from Learner_table", Con))
            {

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new LearnerModel()
                    {
                        LearnerId = Convert.ToInt32(dr[0]),
                        LearnerName = Convert.ToString(dr[1]),
                        LearnerEmail = Convert.ToString(dr[2]),
                        LearnerPassword = Convert.ToString(dr[3])

                    });

                }

            }

            return list;
        }


        /*instructor modal edit db operation*/

        public bool InstructorEditDb(string name, string email, string password, string originalEmail, string originalPassword)
        {
            string query = "UPDATE Instructor_table SET Instructor_Name = @Name, Instructor_Email = @Email, Instructor_password = @Password WHERE Instructor_Email = @OriginEmail AND Instructor_password = @OriginPassword";

            using (SqlCommand cmd = new SqlCommand(query, Con))
            {
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@OriginEmail", originalEmail);
                cmd.Parameters.AddWithValue("@OriginPassword", originalPassword);

                try
                {
                    Con.Open();
                    int m = cmd.ExecuteNonQuery();
                    return m > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
                finally
                {
                    Con.Close();
                }
            }
        }


        /*learner edit modal db operation*/

        public bool LearnerEditDb(string name, string email, string password, string originalemail, string originalpassword)
        {
            string query = "UPDATE Learner_table SET Learner_name = @Name, Learner_email = @Email, Learner_password = @Password WHERE Learner_email = @OriginEmail AND Learner_password = @OriginPassword";

            using (SqlCommand cmd = new SqlCommand(query, Con))
            {
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@OriginEmail", originalemail);
                cmd.Parameters.AddWithValue("@OriginPassword", originalpassword);  // Fixed this parameter name

                try
                {
                    Con.Open();
                    int m = cmd.ExecuteNonQuery();
                    return m > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
                finally
                {
                    Con.Close();
                }
            }
        }


        /*admin instruct edit db operation*/
        public bool AdminInstructUpdateDb(string name, string email, string password, string originalEmail, string originalPassword, string originalId)
        {
            string query = "UPDATE Instructor_table SET Instructor_Name = @Name, Instructor_Email = @Email, Instructor_password = @Password  WHERE Instructor_Email = @OriginalEmail AND Instructor_password = @OriginalPassword AND Instructor_id=@ID";

            using (SqlCommand cmd = new SqlCommand(query, Con))
            {
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@OriginalEmail", originalEmail);
                cmd.Parameters.AddWithValue("@OriginalPassword", originalPassword);
                cmd.Parameters.AddWithValue("@Id", originalId);


                try
                {
                    Con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
                finally
                {
                    Con.Close();
                }
            }
        }

        /*admin instruct delete db operation*/

        public bool DeleteAdminInstructorDb(string id, string email)
        {
            string query = "DELETE FROM Instructor_table WHERE Instructor_id = @Id AND Instructor_Email = @Email";
            using (SqlCommand cmd = new SqlCommand(query, Con))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Email", email);

                try
                {
                    Con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
                finally
                {
                    Con.Close();
                }
            }
        }



        /*admin learner update*/
        public bool UpdateLearnerDb(string id, string name, string email, string password)
        {
            string query = "UPDATE Learner_table SET Learner_name = @Name, Learner_email = @Email, Learner_password = @Password WHERE Learner_id = @Id";
            using (SqlCommand cmd = new SqlCommand(query, Con))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                try
                {
                    Con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
                finally
                {
                    Con.Close();
                }
            }
        }

        /*admin learner delete*/
        public bool DeleteLearnerDb(string id, string email)
        {
            string query = "DELETE FROM Learner_table WHERE Learner_id = @Id and Learner_email=@Email";
            using (SqlCommand cmd = new SqlCommand(query, Con))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Email", email);

                try
                {
                    Con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
                finally
                {
                    Con.Close();
                }
            }
        }

        /*instructor add course db operation*/

        public bool AddInstructorCourseDb(string courseName, string instructorId, Dictionary<string, string> courses, string coursecode)
        {
            string insertCourseModuleQuery = "INSERT INTO Course_table (course_name, course_instructorId, module_name, module_link, course_code) VALUES (@CourseName, @CourseInstructorId, @ModuleName, @ModuleLink, @CourseCode)";

            try
            {
                using (var con = Con) // Assuming Con is your SqlConnection
                {
                    con.Open();
                    using (var transaction = con.BeginTransaction())
                    {
                        using (var cmd = new SqlCommand(insertCourseModuleQuery, con, transaction))
                        {
                            cmd.Parameters.Add("@CourseName", SqlDbType.NVarChar);
                            cmd.Parameters.Add("@CourseInstructorId", SqlDbType.Int);
                            cmd.Parameters.Add("@ModuleName", SqlDbType.NVarChar);
                            cmd.Parameters.Add("@ModuleLink", SqlDbType.NVarChar);
                            cmd.Parameters.Add("@CourseCode", SqlDbType.NVarChar);

                            foreach (var module in courses)
                            {
                                cmd.Parameters["@CourseName"].Value = courseName;
                                cmd.Parameters["@CourseInstructorId"].Value = int.Parse(instructorId);
                                cmd.Parameters["@ModuleName"].Value = module.Key;
                                cmd.Parameters["@ModuleLink"].Value = module.Value;
                                cmd.Parameters["@CourseCode"].Value = coursecode;
                                cmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /*get course details for fetching respective modules and their link in instructor panel db operation*/



        public List<Course> GetInstructorCourseList()
        {
            var courses = new Dictionary<int, Course>();

            using (SqlCommand cmd = new SqlCommand("SELECT course_id, course_name, course_code, module_name, module_link FROM Course_table", Con))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    int courseId = Convert.ToInt32(dr["course_id"]);

                    if (!courses.ContainsKey(courseId))
                    {
                        courses[courseId] = new Course
                        {
                            CourseId = courseId,
                            CourseName = Convert.ToString(dr["course_name"]),
                            CourseCode = Convert.ToString(dr["course_code"]),
                            Modules = new List<Module>()
                        };
                    }

                    courses[courseId].Modules.Add(new Module
                    {
                        ModuleName = Convert.ToString(dr["module_name"]),
                        ModuleLink = Convert.ToString(dr["module_link"])
                    });
                }
            }

            return courses.Values.ToList();
        }



        /*get courselist with course name and course code useed in admin panel*/

        public List<Identity> GetInstructorIdentityList()
        {
            List<Identity> result = new List<Identity>();
            string query = "select distinct course_name,course_code from Course_table";
            using (SqlCommand cmd = new SqlCommand(query, Con))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    result.Add(new Identity()
                    {
                        CourseCode = Convert.ToString(dr["course_code"]),
                        CourseName = Convert.ToString(dr["course_name"]),


                    });

                }

            }
            return result;

        }

        /*get course code and name of the particular instructor in their instructor course panel*/
        public List<Identity> GetInstructorIdentityById(int instructorId)
        {
            List<Identity> PInsresult = new List<Identity>();
            string query = "select distinct course_name,course_code from Course_table where course_instructorId=@RespectiveInsId";
            using (SqlCommand cmd = new SqlCommand(query, Con))
            {
                cmd.Parameters.AddWithValue("@RespectiveInsId", instructorId);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    PInsresult.Add(new Identity()
                    {
                        CourseCode = Convert.ToString(dr["course_code"]),
                        CourseName = Convert.ToString(dr["course_name"]),


                    });

                }

            }
            return PInsresult;

        }

        /*get module list name and respective link using course name code for admin panel*/

        public List<Module> GetInstructorModuleList(string coursename, string coursecode)
        {
            List<Module> result = new List<Module>();

            string query = "select course_id, module_name ,module_link from Course_table where course_name=@Name and course_code=@CODE";

            using (SqlCommand cmd = new SqlCommand(query, Con))
            {
                cmd.Parameters.AddWithValue("@Name", coursename);
                cmd.Parameters.AddWithValue("@CODE", coursecode);

                SqlDataAdapter adaptor = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adaptor.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    result.Add(new Module()
                    {
                        ModuleId = Convert.ToInt32(dr["course_id"]),
                        ModuleName = Convert.ToString(dr["module_name"]),
                        ModuleLink = Convert.ToString(dr["module_link"]),
                    });
                }
            }

            return result;

        }

        // Method to get video URL by ModuleId
        public string GetModuleLinkById(int moduleId)
        {
            string moduleLink = null;

            string query = "SELECT module_link FROM Course_table WHERE course_id = @ModuleId";

            using (SqlCommand cmd = new SqlCommand(query, Con))
            {
                cmd.Parameters.AddWithValue("@ModuleId", moduleId);

                moduleLink = Convert.ToString(cmd.ExecuteScalar());
            }

            return moduleLink;
        }

        /*grant access records in Learner_course table   */                      /*May it will be not be used*/

        public bool GrantAccessDb(int Ins_id, string Ins_name, string courseCode, string courseName, int Learner_id, int flag, string learnerName)
        {
            string checkQuery = "SELECT COUNT(*) FROM Learner_Courses WHERE Ins_Id = @InsID AND CourseCode = @COURCode AND LearnerId = @LId";

            string insertQuery = @"
        INSERT INTO Learner_Courses (Ins_Id, Ins_Name, CourseCode, CourseName, LearnerId, AccessFlag,LearnerName)
        VALUES (@InsID, @InsNAME, @COURCode, @COURName, @LId, @FLAG,@LerName)";

            string updateQuery = @"
        UPDATE Learner_Courses
        SET Ins_Name = @InsNAME, CourseName = @COURName, AccessFlag = @FLAG,LearnerName=@lerName
        WHERE Ins_Id = @InsID AND CourseCode = @COURCode AND LearnerId = @LId";

            using (SqlCommand checkCmd = new SqlCommand(checkQuery, Con))
            {
                checkCmd.Parameters.AddWithValue("@InsID", Ins_id);
                checkCmd.Parameters.AddWithValue("@COURCode", courseCode);
                checkCmd.Parameters.AddWithValue("@LId", Learner_id);
                checkCmd.Parameters.AddWithValue("@LerName", learnerName);

                try
                {
                    Con.Open();
                    int recordCount = (int)checkCmd.ExecuteScalar();

                    string query;
                    SqlCommand cmd;

                    if (recordCount > 0)
                    {
                        // Record exists, perform update
                        query = updateQuery;
                        cmd = new SqlCommand(query, Con);
                        cmd.Parameters.AddWithValue("@InsID", Ins_id);
                        cmd.Parameters.AddWithValue("@InsNAME", Ins_name);
                        cmd.Parameters.AddWithValue("@COURCode", courseCode);
                        cmd.Parameters.AddWithValue("@COURName", courseName);
                        cmd.Parameters.AddWithValue("@LId", Learner_id);
                        cmd.Parameters.AddWithValue("@FLAG", flag);
                        cmd.Parameters.AddWithValue("@LerName", learnerName);
                    }
                    else
                    {
                        // Record does not exist, perform insert
                        query = insertQuery;
                        cmd = new SqlCommand(query, Con);
                        cmd.Parameters.AddWithValue("@InsID", Ins_id);
                        cmd.Parameters.AddWithValue("@InsNAME", Ins_name);
                        cmd.Parameters.AddWithValue("@COURCode", courseCode);
                        cmd.Parameters.AddWithValue("@COURName", courseName);
                        cmd.Parameters.AddWithValue("@LId", Learner_id);
                        cmd.Parameters.AddWithValue("@FLAG", flag);
                        cmd.Parameters.AddWithValue("@LerName", learnerName);
                    }

                    // Execute the appropriate command
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
                finally
                {
                    Con.Close();
                }
            }
        }

        /*get allowed course list in learner panel to choose which course data will be populated in table*/
        public List<Course> GetAllowedCourseList(int learnerId)
        {
            List<Course> result = new List<Course>();
            string query = "select CourseCode,CourseName from Learner_Courses where AccessFlag=1 and LearnerId=@LID";
            using (SqlCommand cmd = new SqlCommand(query, Con))
            {

                cmd.Parameters.AddWithValue("@LID", learnerId);

                using (SqlDataAdapter Adapter = new SqlDataAdapter(cmd))
                {

                    DataTable dt = new DataTable();
                    Adapter.Fill(dt);

                    foreach (DataRow dr in dt.Rows)
                    {
                        result.Add(new Course()
                        {
                            CourseCode = Convert.ToString(dr["CourseCode"]),
                            CourseName = Convert.ToString(dr["CourseName"]),

                        });
                    }

                }



            }
            return result;


        }

        /*get Master Record in Admin  panel*/

        public List<MasterRecord> MasterRecordDb()
        {
            List<MasterRecord> record = new List<MasterRecord>();
            string query = "select Ins_Id,Ins_Name,LearnerId,LearnerName,CourseCode,CourseName from Learner_Courses ";

            Con.Open();
            using (SqlCommand cmd = new SqlCommand(query, Con))
            {

                SqlDataAdapter adapter = new SqlDataAdapter(cmd); // Pass the command to the SqlDataAdapter
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    record.Add(new MasterRecord()
                    {
                        InstructorId = dr["Ins_Id"] != DBNull.Value ? Convert.ToInt32(dr["Ins_Id"]) : 0,
                        InstructorName = dr["Ins_Name"] != DBNull.Value ? Convert.ToString(dr["Ins_Name"]) : string.Empty,
                        LearnerId = dr["LearnerId"] != DBNull.Value ? Convert.ToInt32(dr["LearnerId"]) : 0,
                        LearnerName = dr["LearnerName"] != DBNull.Value ? Convert.ToString(dr["LearnerName"]) : string.Empty,
                        Coursecode = dr["CourseCode"] != DBNull.Value ? Convert.ToString(dr["CourseCode"]) : string.Empty,
                        Coursename = dr["CourseName"] != DBNull.Value ? Convert.ToString(dr["CourseName"]) : string.Empty
                    });
                }
            }

            return record;
        }

        /*delete module by id from admin panel */

        public bool DeleteModuleById(int moduleId)
        {
            string query = "Delete from Course_table where course_id=@MId";
            using (SqlCommand cmd = new SqlCommand(query, Con))
            {
                cmd.Parameters.AddWithValue("@MId", moduleId);

                try
                {
                    Con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
                finally
                {
                    Con.Close();
                }
            }

        }

        //edit admin panel modules db operation

        public bool UpdateAdminPanelModule(int moduleId, string moduleName, string moduleLink)
        {
            string query = "update course_table set module_name=@Mname,module_link=@Mlink where course_id=@Mid";
            using (SqlCommand cmd = new SqlCommand(query, Con))
            {
                cmd.Parameters.AddWithValue("@Mname", moduleName);
                cmd.Parameters.AddWithValue("@Mlink", moduleLink);
                cmd.Parameters.AddWithValue("@Mid", moduleId);

                try
                {
                    Con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
                finally { Con.Close(); }
            }
        }

        /*total instructor in admin panel*/
        public int TotalInstructorDb()
        {
            string query = "select count(*) from Instructor_table";
            using (SqlCommand cmd = new SqlCommand(query, Con))
            {
                try
                {
                    Con.Open();
                    int result = (int)cmd.ExecuteScalar();
                    return result;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return 0;
                }
                finally
                {
                    Con.Close();
                }

            }
        }

        /*toal learner in admin panel and instructor panel*/
        public int TotalLearnerDb()
        {
            string query = "select Count(*) from Learner_table";
            using (SqlCommand cmd = new SqlCommand(query, Con))
            {
                try
                {
                    Con.Open();
                    int result = (int)cmd.ExecuteScalar();
                    return result;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return 0;
                }
                finally
                {
                    Con.Close();
                }

            }
        }

        /*total course count in admin panel*/
        public int TotalCourseDb()
        {
            string query = "select count (distinct course_code) from Course_table";
            using (SqlCommand sql = new SqlCommand(query, Con))
            {
                try
                {
                    Con.Open();
                    int result = (int)sql.ExecuteScalar();
                    return result;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return 0;
                }
                finally
                {
                    Con.Close();
                }

            }
        }

        /*total active learner in instructor panel*/

        public int TotalActiveLearnerInsPanel(int InstructorId)
        {
            string query = "select count (distinct LearnerId) from Learner_Courses where Ins_Id=@InsId";
            using (SqlCommand cmd = new SqlCommand(query, Con))
            {
                cmd.Parameters.AddWithValue("@InsId", InstructorId);

                try
                {
                    Con.Open();

                    int result = (int)cmd.ExecuteScalar();

                    return result;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return 0;
                }
                finally
                {
                    Con.Close();
                }

            }
        }

        /*total added courses of a instructor */
        public int TotalAddedCourseIns(int InstructorId)
        {
            string query = "select count (distinct course_code) from Course_table where course_instructorId=@InsId";

            using (SqlCommand cmd = new SqlCommand(query, Con))
            {
                cmd.Parameters.AddWithValue("@InsId", InstructorId);

                try
                {
                    Con.Open();
                    int result = (int)cmd.ExecuteScalar();
                    return result;
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                    return 0;
                }
                finally
                {
                    Con.Close();
                }
            }
        }

        /*get allowed learner list who has  access to the particular  course allowed by a particular instructor*/

        public List<Details> DetailsDb(int InstructorId)
        {
            List<Details> result = new List<Details>();
            string query = "select LearnerId,LearnerName,LearnerEmail,CourseCode,Ins_Id from Learner_Courses where Ins_Id=@InsID and AccessFlag=1";


            using (SqlCommand cmd = new SqlCommand(query, Con))
            {
                cmd.Parameters.AddWithValue("@InsID", InstructorId);



                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    foreach (DataRow dr in dt.Rows)
                    {
                        result.Add(new Details()
                        {
                            D_LearnerId = Convert.ToInt32(dr["LearnerId"]),
                            D_LearnerName = Convert.ToString(dr["LearnerName"]),
                            D_LearnerMail = Convert.ToString(dr["LearnerEmail"]),
                            D_courseCode = Convert.ToString(dr["CourseCode"]),
                            D_InstructorId = Convert.ToInt32(dr["Ins_Id"]),

                        });
                    }

                }

            }

            return result;
        }

        /*giving acces of course  to the learner by the instructor */

        public bool GrantLearnerByInstructor(int InstructorId, string InstructorName, string coursecode, string coursename, int learnerid, string learnername, string learneremail)
        {
            bool isSuccess = false;

            string updateQuery = "UPDATE Learner_Courses " +
                                 "SET Ins_Id = @InsId, Ins_Name = @InsName, CourseCode = @Ccode, CourseName = @CName, " +
                                 "LearnerId = @LId, LearnerName = @LName, LearnerEmail = @LMail, AccessFlag = 1 " +
                                 "WHERE Ins_Id = @OrInsId AND CourseCode = @OrCode AND LearnerEmail = @OrLMail";

            string insertQuery = "INSERT INTO Learner_Courses (Ins_Id, Ins_Name, CourseCode, CourseName, LearnerId, LearnerName, LearnerEmail, AccessFlag) " +
                                 "VALUES (@InsId, @InsName, @Ccode, @CName, @LId, @LName, @LMail, 1)";

            // Ensure that the connection is open
            if (Con.State != System.Data.ConnectionState.Open)
            {
                Con.Open();
            }

            using (SqlTransaction transaction = Con.BeginTransaction())
            {
                try
                {
                    // Update operation
                    using (SqlCommand cmd = new SqlCommand(updateQuery, Con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@InsId", InstructorId);
                        cmd.Parameters.AddWithValue("@InsName", InstructorName);
                        cmd.Parameters.AddWithValue("@Ccode", coursecode);
                        cmd.Parameters.AddWithValue("@CName", coursename);
                        cmd.Parameters.AddWithValue("@LId", learnerid);
                        cmd.Parameters.AddWithValue("@LName", learnername);
                        cmd.Parameters.AddWithValue("@LMail", learneremail);
                        cmd.Parameters.AddWithValue("@OrInsId", InstructorId);
                        cmd.Parameters.AddWithValue("@OrCode", coursecode);
                        cmd.Parameters.AddWithValue("@OrLMail", learneremail);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected == 0) // If no rows were updated
                        {
                            // Insert operation
                            using (SqlCommand insertCmd = new SqlCommand(insertQuery, Con, transaction))
                            {
                                insertCmd.Parameters.AddWithValue("@InsId", InstructorId);
                                insertCmd.Parameters.AddWithValue("@InsName", InstructorName);
                                insertCmd.Parameters.AddWithValue("@Ccode", coursecode);
                                insertCmd.Parameters.AddWithValue("@CName", coursename);
                                insertCmd.Parameters.AddWithValue("@LId", learnerid);
                                insertCmd.Parameters.AddWithValue("@LName", learnername);
                                insertCmd.Parameters.AddWithValue("@LMail", learneremail);

                                insertCmd.ExecuteNonQuery();
                            }
                        }

                        // Commit transaction
                        transaction.Commit();
                        isSuccess = true;
                    }
                }
                catch (Exception ex)
                {
                    // Rollback transaction in case of error
                    transaction.Rollback();
                    // Log or handle the exception
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    // Ensure that the connection is closed
                    if (Con.State == System.Data.ConnectionState.Open)
                    {
                        Con.Close();
                    }
                }
            }

            return isSuccess;
        }



        /*get learner name ,id*/

        public CustomLearner GetCustomLearner(string learneremail)
        {
            CustomLearner ret = new CustomLearner();
            string query = "select Learner_id,Learner_name from Learner_table where Learner_email=@LMail";
            using (SqlCommand cmd = new SqlCommand(query, Con))
            {
                cmd.Parameters.AddWithValue("@LMail", learneremail);

                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {


                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0]; // Assuming there's only one result
                        ret.CL_Id = Convert.ToInt32(dr["Learner_id"]);
                        ret.CL_Name = Convert.ToString(dr["Learner_name"]);
                    }

                }


            }

            return ret;
        }

        /*deactive the learner by the particular instructor from a particular course*/
        public bool DeactivateLearnerDb(int InstructorId, int LearnerId, string courseCode)
        {
            string query = "Delete from Learner_Courses where Ins_Id=@InsId and LearnerId=@LId and CourseCode=@Ccode";
            using (SqlCommand cmd = new SqlCommand(query, Con))
            {
                cmd.Parameters.AddWithValue("@InsId", InstructorId);
                cmd.Parameters.AddWithValue("@LId", LearnerId);
                cmd.Parameters.AddWithValue("@Ccode", courseCode);

                try
                {
                    Con.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
                finally
                {
                    Con.Close();
                }
            }
        }

        /*get identity list i.e course name and code in learner panel*/

        public List<Identity> LearnerAllowedCourseList(int LearnerId)
        {
            List<Identity> list = new List<Identity>();
            string query = "SELECT CourseCode,CourseName FROM Learner_Courses WHERE LearnerId = @ALId AND AccessFlag = 1";

            // Ensure the connection is properly managed
            using (SqlCommand cmd = new SqlCommand(query, Con))
            {
                cmd.Parameters.AddWithValue("@ALId", LearnerId);

                try
                {
                    // Open the connection if it's not already open
                    if (Con.State != System.Data.ConnectionState.Open)
                    {
                        Con.Open();
                    }

                    // Set the SelectCommand property of SqlDataAdapter
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    foreach (DataRow dr in dt.Rows)
                    {
                        list.Add(new Identity()
                        {
                            CourseCode = dr["CourseCode"].ToString(),
                            CourseName = dr["CourseName"].ToString()
                        });
                    }
                }
                catch (Exception ex)
                {
                    // Handle or log the exception as needed
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    // Ensure the connection is closed
                    if (Con.State == System.Data.ConnectionState.Open)
                    {
                        Con.Close();
                    }
                }
            }

            return list;
        }


    }
}