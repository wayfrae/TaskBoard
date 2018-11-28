using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaskBoard.Controllers
{
    public class BoardController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [NonAction]
        public void CreateBoard(string title, string body, int group)
        {
            string connectionString = "";
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string sql = "insert into boards values ('" + title + "', '" + body + "', " + group + ", 0)";
            SqlCommand command = new SqlCommand(sql, conn);
            command.ExecuteNonQuery();

            command.Dispose();
            conn.Close();
        }

        [NonAction]
        public void CreateGroup(string name)
        {
            string connectionString = "";
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string sql = "insert into groups values ('" + name + "')";
            SqlCommand command = new SqlCommand(sql, conn);
            command.ExecuteNonQuery();

            command.Dispose();
            conn.Close();
        }

        [NonAction]
        public void EditBoard(int id, string title, string body)
        {
            string connectionString = "";
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string sql = "update board set title='" + title + "', body='" + body + "' where id=" + id;
            SqlCommand command = new SqlCommand(sql, conn);
            command.ExecuteNonQuery();

            command.Dispose();
            conn.Close();
        }

        [NonAction]
        public void EditGroup(int id, string name)
        {
            string connectionString = "";
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string sql = "update groups set name='" + name + "' where id=" + id;
            SqlCommand command = new SqlCommand(sql, conn);
            command.ExecuteNonQuery();

            command.Dispose();
            conn.Close();
        }

        [NonAction]
        public void RemoveBoard(int id)
        {
            string connectionString = "";
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string sql = "delete from board where id=" + id;
            SqlCommand command = new SqlCommand(sql, conn);
            command.ExecuteNonQuery();

            command.Dispose();
            conn.Close();
        }

        [NonAction]
        public void RemoveGroup(int id)
        {
            string connectionString = "";
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string sql = "delete from board where id=" + id;
            SqlCommand command = new SqlCommand(sql, conn);
            command.ExecuteNonQuery();

            sql = "delete from group where id=" + id;
            command = new SqlCommand(sql, conn);
            command.ExecuteNonQuery();

            command.Dispose();
            conn.Close();
        }

        [NonAction]
        public void RemoveLock(int id)
        {
            string connectionString = "";
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string sql = "update board set locked=1 where id=" + id;
            SqlCommand command = new SqlCommand(sql, conn);
            command.ExecuteNonQuery();

            command.Dispose();
            conn.Close();
        }

        [NonAction]
        public void SetLock(int id)
        {
            string connectionString = "";
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string sql = "update board set locked=0 where id=" + id;
            SqlCommand command = new SqlCommand(sql, conn);
            command.ExecuteNonQuery();

            command.Dispose();
            conn.Close();
        }
    }
}
