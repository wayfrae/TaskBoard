using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskBoard.Models
{
    public class Board
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int Owner { get; set; }
        public Boolean IsLocked { get; set; }
    }
}