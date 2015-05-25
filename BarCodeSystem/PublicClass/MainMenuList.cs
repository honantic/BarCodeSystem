using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace BarCodeSystem
{
    public class MainMenuList
    {
        private  MenuItem menuitem;
        public  MenuItem menuItem
        {
            get { return menuitem; }
            set { menuitem=value;}
        }

        private  int fathernode = -1;
        public  int fatherNode
        {
            get { return fathernode;}
            set { fathernode=value;}
        }
        private int sonnumber=0;
        public int sonNumber
        {
            get { return sonnumber; }
            set { sonnumber = value; }
        }
    }
}
