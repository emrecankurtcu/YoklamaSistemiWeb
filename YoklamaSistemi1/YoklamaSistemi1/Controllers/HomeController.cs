using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;
using YoklamaSistemi1.Models;
using Excel = Microsoft.Office.Interop.Excel;

namespace YoklamaSistemi1.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        [HttpPost]
        public ActionResult Index(string username,string password)
        {
            MySqlConnection mysqlcon = new MySqlConnection("server=192.168.2.56;uid=root;pwd='pass';database=yoklama");
            string query = "Select * from instructors where ino='"+username+"' and ipassword='"+password+"'";
            MySqlCommand mysqlcomm = new MySqlCommand(query);
            mysqlcomm.Connection = mysqlcon;
            mysqlcon.Open();
            MySqlDataReader dr = mysqlcomm.ExecuteReader();
            while(dr.Read())
            {
                /*user.Add(new Models.User
                {
                    ino=dr["ino"].ToString(),
                    iname=dr["iname"].ToString(),
                    isurname=dr["isurname"].ToString(),
                    ipassword=dr["ipassword"].ToString(),
                    imail=dr["imail"].ToString(),
                    imacadr=dr["imacadr"].ToString()
                });*/

                Session["user"] = username;
                return RedirectToAction("MyHomePage","Home");


            }
            mysqlcon.Close();
            ViewBag.Message = "Kullanıcı adı veya şifre yanlış";
            return View();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MyHomePage()
        {
            string user = Session["user"].ToString();
            List<Lessons> lessons = new List<Lessons>();
            MySqlConnection mysqlcon = new MySqlConnection("server=192.168.2.56;uid=root;pwd='pass';database=yoklama");
            string query = "SELECT * FROM instructors_lessons as a,instructors as b,lessons as c WHERE b.ino=a.ino AND a.lno=c.lno AND a.ino='"+user+"' ";
            MySqlCommand mysqlcomm = new MySqlCommand(query);
            mysqlcomm.Connection = mysqlcon;
            mysqlcon.Open();
            MySqlDataReader dr = mysqlcomm.ExecuteReader();
            while (dr.Read())
            {
                lessons.Add(new Models.Lessons
                {
                    lessonName=dr["lname"].ToString()
                    
                });


            

            }
            mysqlcon.Close();
            return View(lessons);
        }
        [HttpPost]
        public ActionResult Students(string lessonName)
        {
            int discontinuitycount = 0;
            string discontinuityday = "";
            List<string> lessonsweeks = new List<string>();
            lessonsweeks.Add("-");

            MySqlConnection myyconnection = new MySqlConnection("server=192.168.2.56;uid=root;pwd='pass';database=yoklama");
            string myyquery = "SELECT * FROM lessons WHERE lname='" + lessonName + "'";
            MySqlCommand myycommand = new MySqlCommand(myyquery);
            myycommand.Connection = myyconnection;
            myyconnection.Open();
            MySqlDataReader myydr = myycommand.ExecuteReader();
            while (myydr.Read())
            {
                for (int i = 1; i < 17; i++)
                {
                    for (int j = 1; j < 4; j++)
                    {
                        lessonsweeks.Add(myydr["hafta" + i + "_" + j + ""].ToString());

                    }

                }

            }
            List<Students> students = new List<Students>();
            MySqlConnection mysqlcon = new MySqlConnection("server=192.168.2.56;uid=root;pwd='pass';database=yoklama");
            string query = "SELECT * FROM students_lessons as a,students as b,lessons as c WHERE b.sno=a.sno AND a.lno=c.lno AND c.lname='"+lessonName+"' ";
            MySqlCommand mysqlcomm = new MySqlCommand(query);
            mysqlcomm.Connection = mysqlcon;
            mysqlcon.Open();
            MySqlDataReader dr = mysqlcomm.ExecuteReader();
            while (dr.Read())
            {
                List<string> studentsweeks = new List<string>();
                studentsweeks.Add("-");

                string sno = dr["sno"].ToString();
                MySqlConnection myconnection = new MySqlConnection("server=192.168.2.56;uid=root;pwd='pass';database=yoklama");
                string myquery = "SELECT * FROM students_lessons WHERE sno='" + sno + "'";
                MySqlCommand mycommand = new MySqlCommand(myquery);
                mycommand.Connection = myconnection;
                myconnection.Open();
                MySqlDataReader mydr = mycommand.ExecuteReader();
                while (mydr.Read())
                {
                    for (int i = 1; i < 17; i++)
                    {
                        for (int j = 1; j < 4; j++)
                        {
                            studentsweeks.Add(mydr["hafta" + i + "_" + j + ""].ToString()) ;

                        }

                    }

                }
                for (int i = 1; i < 49; i++)
                {
                    if(lessonsweeks[i]=="True" && studentsweeks[i]=="False")
                    {
                        discontinuitycount += 1;
                        if(i%3!=0)
                        {
                            discontinuityday = discontinuityday + "hafta" + ((i  / 3)+1) + "_" + (i % 3) + "-";
                        }
                        else
                        {
                            discontinuityday = discontinuityday + "hafta" + ((i  / 3)) + "_3-";
                        }
                        
                    }

                }












                students.Add(new Models.Students
                {
                    no = dr["sno"].ToString(),
                    name = dr["sname"].ToString(),
                    surname = dr["ssurname"].ToString(),
                    discontinuitycount=discontinuitycount.ToString(),
                    discontinuitydays=discontinuityday.ToString()

                });
                discontinuitycount =0;
                discontinuityday = "";


                
                /*string newquery = "SELECT a.hafta1_1,a.hafta1_2,a.hafta1_3,a.hafta2_1,a.hafta2_2,a.hafta2_3,a.hafta3_1,a.hafta3_2,a.hafta3_3,a.hafta4_1,a.hafta4_2,a.hafta4_3,a.hafta5_1,a.hafta5_2,a.hafta5_3,a.hafta6_1,a.hafta6_2,a.hafta6_3,a.hafta7_1,a.hafta7_2,a.hafta7_3,a.hafta8_1,a.hafta8_2,a.hafta8_3,a.hafta9_1,a.hafta9_2,a.hafta9_3,a.hafta10_1,a.hafta10_2,a.hafta10_3,a.hafta11_1,a.hafta11_2,a.hafta11_3,a.hafta12_1,a.hafta12_2,a.hafta12_3,a.hafta13_1,a.hafta13_2,a.hafta13_3,a.hafta14_1,a.hafta14_2,a.hafta14_3,a.hafta15_1,a.hafta15_2,a.hafta15_3,a.hafta16_1,a.hafta16_2,a.hafta16_3 FROM students_lessons as a,students as b,lessons as c WHERE b.sno=a.sno AND a.lno=c.lno AND c.lname='"+lessonName+"' AND b.sno='"+dr["sno"].ToString()+"'";
                MySqlCommand mysqlc = new MySqlCommand(newquery);
                mysqlcon.Close();
                mysqlc.Connection = mysqlcon;
                mysqlcon.Open();
                MySqlDataReader dr1 = mysqlc.ExecuteReader();
                while(dr1.Read())
                {
                    string newquery1 = "SELECT * FROM lessons WHERE lname='"+lessonName+"'";
                    MySqlCommand mcc = new MySqlCommand(newquery1);
                    mysqlcon.Close();
                    mcc.Connection = mysqlcon;
                    mysqlcon.Open();
                    MySqlDataReader dr2 = mcc.ExecuteReader();
                    while(dr2.Read())
                    {
                        int discontinuitycount = 0;
                        string discontinuityday = "";
                        for (int sayi = 1; sayi < 17; sayi++) {
                            for (int saaa = 1;saaa < 4;saaa++)
											{
                                if (dr1["hafta"+sayi+"_"+saaa] == "0" && dr2["hafta"+sayi+"_"+saaa] == "1")
												{
													 discontinuitycount =discontinuitycount + 1;
													 discontinuityday =discontinuityday+"hafta"+sayi+"_"+saaa+",";
                                }
                            }

                        }
                        
                        
                    }

                }

                


               */
            }
            mysqlcon.Close();
            

            return View(students);       
        }

        [HttpGet]
        public ActionResult addLesson()
        {
            return View();
        }
        [HttpPost]
        public ActionResult addLesson(string lessoncode,string lessonname,HttpPostedFileBase excelFile)
        {
            if (excelFile == null || excelFile.ContentLength == 0)
            {
                ViewBag.Error = "Lütfen dosya seçimi yapınız.";

                return View();
            }
            else
            {
                //Dosyanın uzantısı xls ya da xlsx ise;
                if (excelFile.FileName.EndsWith("xls")
                || excelFile.FileName.EndsWith("xlsx"))
                {
                    string path = Server.MapPath("~/Content/" + excelFile.FileName);                 
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    excelFile.SaveAs(path);

                    Excel.Application application = new Excel.Application();
                    Excel.Workbook workbook = application.Workbooks.Open(path);
                    Excel.Worksheet worksheet = workbook.ActiveSheet;
                    Excel.Range range = worksheet.UsedRange;
                    
                    MySqlConnection newcon1 = new MySqlConnection("server=192.168.2.56;uid=root;pwd='pass';database=yoklama");
                    string newq1 = "INSERT INTO `lessons` (`id`, `lno`, `lname`, `lonline`, `lhafta`, `hafta1_1`, `hafta1_2`, `hafta1_3`, `hafta2_1`, `hafta2_2`, `hafta2_3`, `hafta3_1`, `hafta3_2`, `hafta3_3`, `hafta4_1`, `hafta4_2`, `hafta4_3`, `hafta5_1`, `hafta5_2`, `hafta5_3`, `hafta6_1`, `hafta6_2`, `hafta6_3`, `hafta7_1`, `hafta7_2`, `hafta7_3`, `hafta8_1`, `hafta8_2`, `hafta8_3`, `hafta9_1`, `hafta9_2`, `hafta9_3`, `hafta10_1`, `hafta10_2`, `hafta10_3`, `hafta11_1`, `hafta11_2`, `hafta11_3`, `hafta12_1`, `hafta12_2`, `hafta12_3`, `hafta13_1`, `hafta13_2`, `hafta13_3`, `hafta14_1`, `hafta14_2`, `hafta14_3`, `hafta15_1`, `hafta15_2`, `hafta15_3`, `hafta16_1`, `hafta16_2`, `hafta16_3`) VALUES (NULL, '" + lessoncode + "', '" + lessonname + "', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '');";
                    string newq11="";
                    if (Session["user"] != null)
                    {
                        newq11 = "INSERT INTO `instructors_lessons` (`id`, `ino`, `lno`) VALUES (NULL, '" + Session["user"].ToString() + "', '" + lessoncode + "');";

                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    MySqlCommand newcommand1 = new MySqlCommand(newq1);
                    MySqlCommand newcommand11 = new MySqlCommand(newq11);
                    newcommand1.Connection = newcon1;
                    
                    newcon1.Open();
                    newcommand1.ExecuteNonQuery();
                    newcon1.Close();

                    newcommand11.Connection = newcon1;
                    newcon1.Open();
                    newcommand11.ExecuteNonQuery();
                    newcon1.Close();

                   
                    
                    for (int i = 2; i <= range.Rows.Count; i++)
                    {
                        MySqlConnection newcon = new MySqlConnection("server=192.168.2.56;uid=root;pwd='pass';database=yoklama");
                        string newq = "INSERT INTO `students_lessons` (`id`, `sno`, `lno`, `hafta1_1`, `hafta1_2`, `hafta1_3`, `hafta2_1`, `hafta2_2`, `hafta2_3`, `hafta3_1`, `hafta3_2`, `hafta3_3`, `hafta4_1`, `hafta4_2`, `hafta4_3`, `hafta5_1`, `hafta5_2`, `hafta5_3`, `hafta6_1`, `hafta6_2`, `hafta6_3`, `hafta7_1`, `hafta7_2`, `hafta7_3`, `hafta8_1`, `hafta8_2`, `hafta8_3`, `hafta9_1`, `hafta9_2`, `hafta9_3`, `hafta10_1`, `hafta10_2`, `hafta10_3`, `hafta11_1`, `hafta11_2`, `hafta11_3`, `hafta12_1`, `hafta12_2`, `hafta12_3`, `hafta13_1`, `hafta13_2`, `hafta13_3`, `hafta14_1`, `hafta14_2`, `hafta14_3`, `hafta15_1`, `hafta15_2`, `hafta15_3`, `hafta16_1`, `hafta16_2`, `hafta16_3`) VALUES (NULL, '"+ ((Excel.Range)range.Cells[i, 1]).Text +"', '"+lessoncode+"', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '');";
                        MySqlCommand newcommand = new MySqlCommand(newq);
                        newcommand.Connection = newcon;
                        newcon.Open();
                        newcommand.ExecuteNonQuery();
                        newcon.Close();
                    }

                    application.Quit();
                    return RedirectToAction("MyHomePage","Home");
                }
                else
                {
                    return RedirectToAction("Index","Home");

                    
                }
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

    }
}