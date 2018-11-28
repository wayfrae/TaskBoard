using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskBoard.Models;

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
        public Board GetBoard(int id)
        {
            string connectionString = "";
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string sql = "select title, body, owner, locked from board where id=" + id;
            SqlCommand command = new SqlCommand(sql, conn);
            SqlDataReader dataReader = command.ExecuteReader();

            Board board = null;

            while (dataReader.Read())
            {
                board = new Board()
                {
                    ID = id,
                    Title = dataReader.GetString(0),
                    Body = dataReader.GetString(1),
                    Owner = dataReader.GetInt32(2),
                    IsLocked = dataReader.GetInt32(3)
                };
            }

            dataReader.Close();
            command.Dispose();
            conn.Close();

            return board;
        }

        [NonAction]
        public Group GetGroup(int id)
        {
            string connectionString = "";
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string sql = "select id, title, body, locked from board where owner=" + id;
            SqlCommand command = new SqlCommand(sql, conn);
            SqlDataReader dataReader = command.ExecuteReader();

            List<Board> boards = new List<Board>();

            while (dataReader.Read())
            {
                boards.Add(new Board()
                {
                    ID = dataReader.GetInt32(0),
                    Title = dataReader.GetString(1),
                    Body = dataReader.GetString(2),
                    Owner = id,
                    IsLocked = dataReader.GetInt32(3)
                });
            }

            sql = "select name from groups where id=" + id;
            command = new SqlCommand(sql, conn);
            dataReader = command.ExecuteReader();

            string name = "";

            while (dataReader.Read())
            {
                name = dataReader.GetString(0);
            }

            dataReader.Close();
            command.Dispose();
            conn.Close();

            return new Group(){ID = id, Name = name, Boards = boards};
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
