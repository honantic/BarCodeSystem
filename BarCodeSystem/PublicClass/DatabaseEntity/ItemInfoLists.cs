using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Controls;

namespace BarCodeSystem
{
    public class ItemInfoLists 
    {
        /// <summary>
        /// 料品Id
        /// </summary>
        public Int64 ID
        {
            get;
            set;
        }

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected
        {
            get;
            set;
        }
        /// <summary>
        /// 料品编码
        /// </summary>
        public string II_Code
        {
            get;
            set;
        }

        /// <summary>
        /// 料品规格
        /// </summary>
        public string II_Spec
        {
            get;
            set;
        }

        /// <summary>
        /// 料品名称
        /// </summary>
        public string II_Name
        {
            get;
            set;
        }

        /// <summary>
        /// 料品型号
        /// </summary>
        public string II_Version
        {
            get;
            set;
        }

        /// <summary>
        /// 计量单位ID
        /// </summary>
        public Int64 II_UnitID
        {
            get;
            set;
        }

        /// <summary>
        /// 计量单位Code
        /// </summary>
        public string II_UnitCode
        {
            get;
            set;
        }

        /// <summary>
        /// 计量单位名称
        /// </summary>
        public string II_UnitName
        {
            get;
            set;
        }

        /// <summary>
        /// 质检分类ID
        /// </summary>
        public Int64 II_QualitySortID
        {
            get;
            set;
        }
        /// <summary>
        /// 质检分类名称
        /// </summary>
        public string II_QualitySortName
        {
            get;
            set;
        }

        /// <summary>
        /// 料品的工艺路线版本中文描述
        /// </summary>
        public List<string> TRV_Version = new List<string> { };


        /// <summary>
        /// 料品工艺路线版本ID
        /// </summary>
        //public List<Int64> TR_VersionID = new List<Int64> { };

        public List<TechVersion> TechVersionList = new List<TechVersion> { };

        public ComboBox CB_TechVersion = new ComboBox();
    }
}
