using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskBoard.Models
{
    public class BoardViewModel
    {
        public List<Group> Groups { get; set; }
        public List<Board> Boards { get; set; }
    }
}