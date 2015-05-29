using System;
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


        /// <summary>
        /// 下面的示例演示如何定义一个为行定义 Style 的 StyleSelector。
        /// 此示例依据行索引定义 Background 颜色。
        /// </summary>
        /// <param name="item"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public override Style SelectStyle(object item, DependencyObject container)
        {
            Style st = new Style();
            st.TargetType = typeof(ListViewItem);
            Setter backGroundSetter = new Setter();
            backGroundSetter.Property = ListViewItem.BackgroundProperty;

            PersonLists pl = item as PersonLists;

            if (!pl.isRightDepart)
            {
                backGroundSetter.Value = Brushes.LightSalmon;
            }
            else
            {
                backGroundSetter.Value = Brushes.Transparent;
            }

            st.Setters.Add(backGroundSetter);
            return st;
        }
    }
}
