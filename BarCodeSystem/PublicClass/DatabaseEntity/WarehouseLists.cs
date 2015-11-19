using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BarCodeSystem
{
    public class WarehouseLists
    {

        private bool isselected = false;
        public bool IsSelected
        {
            get { return isselected; }
            set { isselected = value; }
        }

        public string W_Code
        {
            get;
            set;
        }

        public Int64 W_ID
        {
            get;
            set;
        }

        public string W_Name
        {
            get;
            set;
        }

        /// <summary>
        /// 仓库信息录入来源，数据库中字段类型为int
        /// 0：U9导入，1：手工录入.这里为了方便展示，转换为字符型。这里是用来展示的属性
        /// </summary>
        public string W_SourceType_Show
        {
            get;
            set;
        }

        /// <summary>
        /// 仓库是否启用，数据库中字段类型为bool
        /// 0：否，1：是.这里为了方便展示，转换为字符型。这里是用来展示的属性
        /// </summary>
        public string W_IsValidated_Show
        {
            get;
            set;
        }


        /// <summary>
        /// 仓库信息录入来源，数据库中字段类型为int
        /// 0：U9导入，1：手工录入.这里是用来存入数据库的属性
        /// </summary>
        public int W_SourceType
        {
            get;
            set;
        }

        /// <summary>
        /// 仓库是否启用，数据库中字段类型为bool
        /// 0：否，1：是.这里是用来存入数据库的属性
        /// </summary>
        public bool W_IsValidated
        {
            get;
            set;
        }

        /// <summary>
        /// 是否默认仓库,数据库中字段类型为bool
        /// 0:否，1:是.这里为了方便展示，转换为字符型。这里用来展示的属性
        /// </summary>
        public string W_IsDefault_Show
        {
            get;
            set;
        }
        /// <summary>
        /// 仓库ID
        /// </summary>
        public Int64 ID
        {
            get;
            set;
        }


        /// <summary>
        /// 是否默认仓库
        /// </summary>
        public bool W_IsDefault
        {
            get;
            set;
        }

        /// <summary>
        /// 关联工作中心ID
        /// </summary>
        public Int64 W_WorkCenterID
        {
            get;
            set;
        }

        /// <summary>
        /// 工作中心名称
        /// </summary>
        public string WC_Department_Name
        {
            get;
            set;
        }

        /// <summary>
        /// 得到仓库所有信息
        /// </summary>
        /// <returns></returns>
        public static List<WarehouseLists> Fetch_WInfo()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            List<WarehouseLists> whList = new List<WarehouseLists>();
            string SQl = string.Format(@"select Warehouse.[ID],[W_ID],[W_Code],[W_Name],WorkCenter.WC_Department_Name,[W_WorkCenterID],[W_SourceType] ,[W_IsValidated],[W_IsDefault] FROM Warehouse left join WorkCenter on (Warehouse.W_WorkCenterID = WorkCenter.WC_Department_ID)");
            MyDBController.GetConnection();
            dt = MyDBController.GetDataSet(SQl, ds, "Warehouse").Tables["Warehouse"];
            MyDBController.CloseConnection();
            foreach (DataRow row in dt.Rows)
            {
                WarehouseLists wh = new WarehouseLists();
                wh.ID = (Int64)row["ID"];
                wh.W_ID = (Int64)row["W_ID"];
                wh.W_Code = row["W_Code"].ToString();
                wh.W_Name = row["W_Name"].ToString();
                wh.WC_Department_Name = row["WC_Department_Name"].ToString();
                wh.W_WorkCenterID = row["W_WorkCenterID"] is DBNull ? 0 : (Int64)row["W_WorkCenterID"];
                wh.W_SourceType = (Int32)row["W_SourceType"];
                wh.W_IsValidated = Convert.ToBoolean(row["W_IsValidated"]);
                wh.W_IsDefault = row["W_IsDefault"] is DBNull ? false : Convert.ToBoolean(row["W_IsDefault"]);
                whList.Add(wh);
            }

            return whList;
        }

    }
}
