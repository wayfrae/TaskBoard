using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskBoard.Models
{
    public sealed class Singleton
    {
        private static readonly Singleton _instance = new Singleton();

        public static Singleton Instance
        {
            get
            {
                return _instance;
            }
        }

        public List<Board> Boards { get; set; }        
        public List<Group> Groups { get; set; }

        private Singleton()
        {
            this.Groups = new List<Group>();
            this.Boards = new List<Board>();
        }
    }
}