using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskBoard.Models
{
    public class Group
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public List<Board> Boards { get; set; }
    }
}