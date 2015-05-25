using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BarCodeSystem
{
    public class DeviceLists : StyleSelector
    {
        /// <summary>
        /// Excel导入的时候检验部门编码是否正确
        /// </summary>
        public bool isRightDepart
        {
            get;
            set;
        }

        /// <summary>
        /// 设备数量
        /// </summary>
        public int D_Amount
        {
            get;
            set;
        }

        /// <summary>
        /// 在列表中是否选中
        /// </summary>
        public bool IsSelected
        {
            get;
            set;
        }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string D_Code
        {
            get;
            set;
        }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string D_Name
        {
            get;
            set;
        }

        /// <summary>
        /// 设备ID
        /// </summary>
        public Int64 D_ID
        {
            get;
            set;
        }

        /// <summary>
        /// 设备所属工作中心编码
        /// </summary>
        public string D_Department_Code
        {
            get;
            set;
        }

        /// <summary>
        /// 设备所属工作中心名称
        /// </summary>
        public string D_Department_Name
        {
            get;
            set;
        }

        /// <summary>
        /// 设备所属工作中心ID
        /// </summary>
        public Int64 D_Department_ID
        {
            get;
            set;
        }

        /// <summary>
        /// 设备数据来源，0:表示U9系统录入，1:表示手工录入。该字段在数据库中为int型，
        /// 这个属性转换成字符型，为了方便展示
        /// </summary>
        public string D_SourceType_Show
        {
            get;
            set;
        }

        /// <summary>
        /// 设备数据来源，0:表示U9系统录入，1:表示手工录入。该字段在数据库中为int型，
        /// 这个属性用来对数据库进行读写
        /// </summary>
        public int D_SourceType
        {
            get;
            set;
        }

        /// <summary>
        /// 设备数据来源，0:表示未启用，1:表示启用。该字段在数据库中为bool型，
        /// 这个属性转换成字符型，为了方便展示
        /// </summary>
        public string D_IsValidated_Show
        {
            get;
            set;
        }


        /// <summary>
        /// 设备数据来源，0:表示未启用，1:表示启用。该字段在数据库中为bool型，
        /// 这个属性转换成字符型，为了方便展示
        /// </summary>
        public bool D_IsValidated
        {
            get;
            set;
        }

        /// <summary>
        /// 设备条码号
        /// </summary>
        public string D_BarCode
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
        //public override Style SelectStyle(object item, DependencyObject container)
        //{
        //    Style st = new Style();
        //    st.TargetType = typeof(ListViewItem);
        //    Setter backGroundSetter = new Setter();
        //    backGroundSetter.Property = ListViewItem.BackgroundProperty;

        //    DeviceLists dl = item as DeviceLists;

        //    if (!dl.isRightDepart)
        //    {
        //        backGroundSetter.Value = Brushes.LightSalmon;
        //    }
        //    else
        //    {
        //        backGroundSetter.Value = Brushes.Transparent;
        //    }

        //    st.Setters.Add(backGroundSetter);
        //    return st;
        //}
    }
}
