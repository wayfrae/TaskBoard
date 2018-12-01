using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public ActionResult Index()
        {
            return View();
        }

        [NonAction]
        public void CreateBoard(string title, string body, int group)
        {
            
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string sql = "insert into boards values ('" + title + "', '" + body + "', " + group + ", 0)";
            SqlCommand command = new SqlCommand(sql, conn);
            command.ExecuteNonQuery();

            command.Dispose();
            conn.Close();
        }

        [NonAction]
        public void CreateOrEditBoard(Board board)
        {

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "IF EXISTS (SELECT * FROM boards WHERE id = @ID) BEGIN update boards set title="
                + "@title, body=@body where id=@ID END ELSE BEGIN insert into boards values ("
                + "@title, @body, @owner, @locked) END";
                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.Add("@ID", SqlDbType.Int);
                command.Parameters.Add("@title", SqlDbType.NVarChar);
                command.Parameters.Add("@body", SqlDbType.NVarChar);
                command.Parameters.Add("@owner", SqlDbType.Int);
                command.Parameters.Add("@locked", SqlDbType.Int);
                command.Parameters["@ID"].Value = board.ID;
                command.Parameters["@title"].Value = board.Title;
                command.Parameters["@body"].Value = board.Body;
                command.Parameters["@owner"].Value = board.Owner;
                command.Parameters["@locked"].Value = board.IsLocked.Equals("True") ? 1 : 0;
                command.ExecuteNonQuery();
            }

                
        }

        [NonAction]
        public void CreateGroup(string name)
        {
            
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
            
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string sql = "select title, body, owner, locked from board where id=" + id;
            SqlCommand command = new SqlCommand(sql, conn);
            SqlDataReader dataReader = command.ExecuteReader();

            Board board = null;
            //

            while (dataReader.Read())
            {
                board = new Board()
                {
                    ID = id,
                    Title = dataReader.GetString(0),
                    Body = dataReader.GetString(1),
                    Owner = dataReader.GetInt32(2),
                    IsLocked = dataReader.GetInt32(3) == 0 ? false : true
                };
            }

            dataReader.Close();
            command.Dispose();
            conn.Close();

            return board;
        }

        [NonAction]
        public ObservableCollection<Board> GetAllBoards()
        {
            string sql = "select * from boards";

            //Create a new DataSet
            DataSet ds = new DataSet();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter())
                {
                    SqlCommand cmd = new SqlCommand(sql, conn);

                    //Open the connection to the database
                    conn.Open();

                    //Add the information for the SelectCommand using the SQL statement and the connection object
                    adapter.SelectCommand = new SqlCommand(sql, conn);
                    adapter.SelectCommand.CommandTimeout = 0;

                    //Fill up the DataSet with data
                    adapter.Fill(ds);
                }
            }

            //Set the number of values returned
            int numRows = ds.Tables[0].Rows.Count;
            ObservableCollection<Board> boards = new ObservableCollection<Board>();

            for(int i = 0; i < numRows; i++)
            {
                boards.Add(new Board()
                {
                    ID = int.Parse(ds.Tables[0].Rows[i][0].ToString()),
                    Title = ds.Tables[0].Rows[i][1].ToString(),
                    Body = ds.Tables[0].Rows[i][2].ToString(),
                    Owner = int.Parse(ds.Tables[0].Rows[i][3].ToString()),
                    IsLocked = int.Parse(ds.Tables[0].Rows[i][4].ToString()) == 0 ? false : true
                });
            }          

            return boards;
        }

        [NonAction]
        public ObservableCollection<Group> GetAllGroups()
        {
            string sql = "select * from groups";

            //Create a new DataSet
            DataSet ds = new DataSet();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter())
                {
                    SqlCommand cmd = new SqlCommand(sql, conn);

                    //Open the connection to the database
                    conn.Open();

                    //Add the information for the SelectCommand using the SQL statement and the connection object
                    adapter.SelectCommand = new SqlCommand(sql, conn);
                    adapter.SelectCommand.CommandTimeout = 0;

                    //Fill up the DataSet with data
                    adapter.Fill(ds);
                }
            }

            //Set the number of values returned
            int numRows = ds.Tables[0].Rows.Count;
            ObservableCollection<Group> groups = new ObservableCollection<Group>();

            for (int i = 0; i < numRows; i++)
            {
                groups.Add(new Group()
                {
                    ID = int.Parse(ds.Tables[0].Rows[i][0].ToString()),
                    Name = ds.Tables[0].Rows[i][1].ToString()                    
                });
            }

            return groups;
        }




        [NonAction]
        public Group GetGroup(int id)
        {
            
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
                    IsLocked = dataReader.GetInt32(3) == 0 ? false : true
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
