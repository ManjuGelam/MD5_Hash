using MD5_Hash.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MD5_Hash.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(GetUsers());
        }

        [HttpPost]
        public ActionResult Index(string userName, string password)
        {
            string constr = ConfigurationManager.ConnectionStrings["MVCDBContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "INSERT INTO Users VALUES (@Username, @Password)";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@Username", userName);
                    cmd.Parameters.AddWithValue("@Password", encryption(password));
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            return View(GetUsers());
        }
        private static List<User> GetUsers()
        {
            List<User> users = new List<User>();
            string constr = ConfigurationManager.ConnectionStrings["MVCDBContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT Username, Password FROM Users"))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            users.Add(new User
                            {
                                Username = sdr["Username"].ToString(),
                                EncryptedPassword = sdr["Password"].ToString(),

                            });
                        }
                    }
                    con.Close();
                }
            }
            return users;
        }
      

        public string encryption(string password)
        {

            MD5Cng md5Hash = new MD5Cng();
            byte[] hashBytes = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
            }


            return sb.ToString();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}