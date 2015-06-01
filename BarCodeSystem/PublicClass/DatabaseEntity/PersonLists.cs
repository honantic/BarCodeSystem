﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace BarCodeSystem
{
    public class PersonLists : StyleSelector
    {
        public bool IsSelected
        {
            get;
            set;
        }
        public bool isRightDepart
        {
            get;
            set;
        }
        public string name
        {
            get;
            set;
        }

        public string code
        {
            get;
            set;
        }
        public string departCode
        {
            get;
            set;
        }
        public string departName
        {
            get;
            set;
        }

        public Int64 departid
        {
            get;
            set;
        }


    }
}