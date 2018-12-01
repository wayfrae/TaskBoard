using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace TaskBoard.Models
{
    public class Board : INotifyPropertyChanged
    {
        private int _id;
        private string _title;
        private string _body;
        private int _owner;
        private bool _isLocked;

        /// <summary>
        /// The ID of the Board in the database
        /// </summary>
        public int ID {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// The title of the board.
        /// </summary>
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// The body of the board
        /// </summary>
        public string Body
        {
            get
            {
                return _body;
            }
            set
            {
                _body = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// The id of the group that owns the board in the database
        /// </summary>
        public int Owner
        {
            get
            {
                return _owner;
            }
            set
            {
                _owner = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Keeps track of whether the board is locked
        /// </summary>
        public bool IsLocked
        {
            get
            {
                return _isLocked;
            }
            set
            {
                _isLocked = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Event handler for when the property is changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}