using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows;
using System.Data;

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

        /// <summary>
        /// 检查该料品编号是否存在
        /// </summary>
        /// <param name="_itemCode"></param>
        /// <returns></returns>
        public static bool CheckForCode(string _itemCode)
        {
            bool flag = false;
            int count;
            string SQl = string.Format(@"select count(*) from [ItemInfo] where [II_Code]='{0}'", _itemCode);
            MyDBController.GetConnection();
            try
            {
                count = Convert.ToInt32(MyDBController.ExecuteScalar(SQl));
                if (count > 0)
                {
                    flag = true;
                }
            }
            catch (Exception)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("该料号不存在！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            MyDBController.CloseConnection();

            return flag;
        }

        /// <summary>
        /// 根据指定车间，返回拥有工艺路线的料品信息列表
        /// </summary>
        /// <param name="_wcID"></param>
        /// <returns></returns>
        public static List<ItemInfoLists> FetchItemInfoByTechAndWC(params Int64[] _wcID)
        {
            List<ItemInfoLists> iilList = new List<ItemInfoLists>();
            DataSet ds = new DataSet();
            string SQl = "";
            if (_wcID.Length == 0)
            {
                SQl = string.Format(@"select * from ItemInfo  where [ID] in (select distinct [TR_ItemID] from [TechRoute] )");
            }
            else
            {
                string idList = "";
                foreach (Int64 item in _wcID)
                {
                    idList += idList.Length == 0 ? item.ToString() : "," + item.ToString();
                }
                SQl = string.Format(@"select * from ItemInfo  where [ID] in (select distinct [TR_ItemID] from [TechRoute] where [TR_WorkCenterID] in ({0}))", idList);
            }
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "ItemInfo");
            MyDBController.CloseConnection();

            foreach (DataRow row in ds.Tables["ItemInfo"].Rows)
            {
                ItemInfoLists iil = new ItemInfoLists();
                iil.ID = Convert.ToInt64(row["ID"]);
                iil.II_Code = row["II_Code"].ToString();
                iil.II_Name = row["II_Name"].ToString();
                iil.II_Spec = row["II_Spec"].ToString();
                iil.II_UnitName = row["II_UnitName"].ToString();
                iil.II_UnitCode = row["II_UnitCode"].ToString();
                iil.II_UnitID = Convert.ToInt64(row["II_UnitID"]);
                iilList.Add(iil);
            }
            return iilList;
        }
    }
}
