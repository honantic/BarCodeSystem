using BarCodeSystem.PublicClass.HelperClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows;

namespace BarCodeSystem.PublicClass
{
    public class ProduceOrderLists
    {
        /// <summary>
        /// 条码系统里面的ID
        /// </summary>
        public Int64 ID
        {
            get;
            set;
        }
        /// <summary>
        /// U9订单Id
        /// </summary>
        public Int64 PO_ID
        {
            get;
            set;
        }
        /// <summary>
        /// U9订单编号
        /// </summary>
        public string PO_Code
        {
            get;
            set;
        }
        /// <summary>
        /// U9料品ID，条码料品表里面没有这个字段
        /// </summary>
        public string PO_ItemID
        {
            get;
            set;
        }
        /// <summary>
        /// U9料品规格
        /// </summary>
        public string PO_ItemSpec
        {
            get;
            set;
        }
        /// <summary>
        /// U9料品编码
        /// </summary>
        public string PO_ItemCode
        {
            get;
            set;
        }
        /// <summary>
        /// U9料品名称
        /// </summary>
        public string PO_ItemName
        {
            get;
            set;
        }
        /// <summary>
        /// U9料品型号
        /// </summary>
        public string PO_ItemVersion
        {
            get;
            set;
        }
        /// <summary>
        /// U9项目编号，跟组织关联
        /// </summary>
        public string PO_ProjectCode
        {
            get;
            set;
        }

        /// <summary>
        /// U9项目mingcheng，跟组织关联
        /// </summary>
        public string PO_ProjectName
        {
            get;
            set;
        }
        ///// <summary>
        ///// 条码系统里面的工作中心ID
        ///// </summary>
        //public Int64 PO_WorkCenter
        //{
        //    get;
        //    set;
        //}
        /// <summary>
        /// 料品计量单位
        /// </summary>
        public string PO_Itemunit
        {
            get;
            set;
        }
        /// <summary>
        /// 单据创建日期
        /// </summary>
        public DateTime PO_CreateTime
        {
            get;
            set;
        }
        /// <summary>
        /// 创建人员姓名
        /// </summary>
        public string PO_CreateBy
        {
            get;
            set;
        }
        ///// <summary>
        ///// 需求日期
        ///// </summary>
        //public DateTime PO_DemandDate
        //{
        //    get;
        //    set;
        //}
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime PO_ModifyTime
        {
            get;
            set;
        }
        /// <summary>
        ///修改人员姓名
        /// </summary>
        public string PO_ModifyBy
        {
            get;
            set;
        }
        /// <summary>
        /// 开工日期
        /// </summary>
        public DateTime PO_StartDate
        {
            get;
            set;
        }
        /// <summary>
        /// 订单数量
        /// </summary>
        public Int32 PO_OrderAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 开工数量
        /// </summary>
        public Int32 PO_StartAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 已完工数量
        /// </summary>
        public Int32 PO_FinishedAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 单据来源 0:来自U9 1:条码系统手工录入 
        /// </summary>
        public Int32 PO_OrderSource
        {
            get;
            set;
        }
        /// <summary>
        /// 生产部门编号 
        /// </summary>
        public string PO_ProduceDepartCode
        {
            get;
            set;
        }

        /// <summary>
        /// 条码系统导入时间
        /// </summary>
        public DateTime PO_BCSCreateTime { get; set; }

        /// <summary>
        /// 生产部门名称
        /// </summary>
        public string PO_ProduceDepartName
        {
            get;
            set;
        }
        /// <summary>
        /// 是否返工订单
        /// </summary>
        public bool PO_IsReturn
        {
            get;
            set;
        }



        /// <summary>
        /// 从U9系统中获取生产订单消息
        /// </summary>
        public static void FetchPOFromU9()
        {
            string SQl = "";
            StringBuilder sb = new StringBuilder();
            DataSet ds = new DataSet();
            SqlConnection sqlcon = new SqlConnection() { ConnectionString = string.Format(@"server={0};database={1};uid={2};pwd={3}", User_Info.server[0], User_Info.database[0], User_Info.uid[0], User_Info.pwd[0]) };


            SQl = string.Format(@"Select [ID],[POF_LastImportDate],[POF_IsImportingFlag]  from [ProduceOrderFlag]");
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "ProduceOrderFlag");

            while (ds.Tables["ProduceOrderFlag"].Rows.Count == 0)
            {
                //初始化获取U9订单时间戳，设置为当前时间减去2个月
                string SQl1 = string.Format(@"insert into [ProduceOrderFlag](POF_LastImportDate,POF_IsImportingFlag) values(dateadd(month,-2,getdate()),'{0}')", false);
                MyDBController.ExecuteNonQuery(SQl1);
                MyDBController.GetDataSet(SQl, ds, "ProduceOrderFlag");
            }

            DataRow row = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1];
            //flagTime = Convert.ToDateTime(row["POF_LastImportDate"]);//这个会受到电脑系统设置影响  会出现星期几 和上下午的中文
            string lastOperateTime = Convert.ToDateTime(row["POF_LastImportDate"]).ToString("yyyy/MM/dd HH:mm:ss");

            //条码系统中现有的生产订单的编号列表
            List<string> PO_CodeList = new List<string>();
            FetchProduceOrderInfo(true).ForEach(
                    p =>
                    {
                        PO_CodeList.Add(p.PO_Code);
                    });
            try
            {
                if (!Convert.ToBoolean(row["POF_IsImportingFlag"]))
                {
                    MyDBController.GetConnection();
                    //将条码系统中的生产订单标记改为true，锁定导入操作。
                    SQl = string.Format(@"update [ProduceOrderFlag] set [POF_IsImportingFlag] ='true' where [ID]={0}", Convert.ToInt64(row["ID"]));
                    MyDBController.ExecuteNonQuery(SQl);

                    SQl = string.Format(@"Select [ID],[II_Code] from [ItemInfo]");
                    MyDBController.GetDataSet(SQl, ds, "ItemInfo");
                    Dictionary<Int64, string> itemCodeList = new Dictionary<Int64, string>();
                    foreach (DataRow codeRow in ds.Tables["ItemInfo"].Rows)
                    {
                        itemCodeList.Add(Convert.ToInt64(codeRow["ID"]), codeRow["II_Code"].ToString());
                    }

                    SQl = string.Format(@"select A.[ID] as U9订单Id,A.[CreatedOn] as 创建日期, A.[CreatedBy] as 创建人姓名,A.[ModifiedOn] as 修改日期, A.[ModifiedBy] as 修改人姓名, A.[DocNo] as 生产订单_单号, A.[ProductQty] as 订单数量,A.[StartDate] as 开工日期, A.[TotalStartQty] as 开工数量, A.[TotalCompleteQty] as 已完工数量, A.[TotalEligibleQty] as 累计合格数量, A.[TotalScrapQty] as 累计报废数量, A.[TotalReworkingQty] as 累计返工数量, A1.[ID] as 料品Id,A1.[Code] as 料号, A1.[Name] as 料品名称, A1.[SPECS] as 料品规格,A1.[DescFlexField_PubDescSeg1] as 料品型号,A6.[ID] as 计量单位ID,A7.[name] as 料品计量单位,A6.[Code] as 计量单位编码 ,A3.[Name] as 部门名称, A2.[Code] as 部门编号, A4.[Code] as 项目编号, A5.[Name] as 项目名称 ,A.[DescFlexField_PubDescSeg8] as 是否导入条码系统标记 from  MO_MO as A  left join [CBO_ItemMaster] as A1 on (A.[ItemMaster] = A1.[ID]) left join [Base_UOM] as A6 on (A1.[InventoryUOM]=A6.[ID])left join [Base_UOM_Trl] as A7 on (A6.[ID]=A7.[ID] )left join [CBO_Department] as A2 on (A.[Department] = A2.[ID])  left join [CBO_Department_Trl] as A3 on (A3.SysMlFlag = 'zh-CN') and (A2.[ID] = A3.[ID])  left join [CBO_Project] as A4 on (A.[Project] = A4.[ID])  left join [CBO_Project_Trl] as A5 on (A5.SysMlFlag = 'zh-CN') and (A4.[ID] = A5.[ID]) where A.[ModifiedOn] > '{0}' and A.[DocState]=2 and A.[Org]={1} ", lastOperateTime, User_Info.Org_Id);
                    MyDBController.GetDataSet(new SqlConnection() { ConnectionString = sqlcon.ConnectionString }, SQl, ds, "Mo_Mo");


                    if (ds.Tables["Mo_Mo"].Rows.Count > 0)
                    {
                        SQl = string.Format(@"Select top 0 * from [ProduceOrder]");
                        MyDBController.GetDataSet(SQl, ds, "ProduceOrder");
                        List<string> colList = new List<string>();
                        foreach (DataColumn col in ds.Tables["ProduceOrder"].Columns)
                        {
                            colList.Add(col.ColumnName);
                        }
                        ds.Tables["ProduceOrder"].Columns.Add(new DataColumn("IDNew", typeof(Int64)));
                        foreach (DataRow originrow in ds.Tables["Mo_Mo"].Rows)
                        {
                            if (!PO_CodeList.Contains(originrow["生产订单_单号"].ToString()))
                            {
                                Int64 itemid = -1;
                                if (itemCodeList.Values.Contains(originrow["料号"].ToString()))
                                {
                                    itemid = itemCodeList.First(p => p.Value.Equals(originrow["料号"].ToString())).Key;
                                }
                                else
                                {
                                    SQl = string.Format(@"insert into [ItemInfo](II_Code,II_Spec,II_Version,II_Name,II_UnitID,II_UnitCode,II_UnitName) values('{0}','{1}','{2}','{3}',{4},'{5}','{6}')", originrow["料号"].ToString(), originrow["料品规格"].ToString(), originrow["料品型号"].ToString(), originrow["料品名称"].ToString(), Convert.ToInt64(originrow["计量单位ID"]), originrow["计量单位编码"].ToString(), originrow["料品计量单位"].ToString());
                                    MyDBController.ExecuteNonQuery(SQl);
                                    SQl = string.Format(@"select [ID] from [ItemInfo] where [II_Code]='{0}'", originrow["料号"].ToString());
                                    itemid = Convert.ToInt64(MyDBController.ExecuteScalar(SQl));
                                    itemCodeList.Add(itemid, originrow["料号"].ToString());
                                }

                                DataRow newrow = ds.Tables["ProduceOrder"].NewRow();
                                newrow["PO_ID"] = originrow["U9订单Id"];
                                newrow["PO_Code"] = originrow["生产订单_单号"];
                                newrow["PO_ItemID"] = itemid;
                                newrow["PO_ItemCode"] = originrow["料号"];
                                newrow["PO_ItemSpec"] = originrow["料品规格"];
                                newrow["PO_ItemVersion"] = originrow["料品型号"];
                                newrow["PO_ProjectCode"] = originrow["项目编号"] == null ? "" : originrow["项目编号"];
                                newrow["PO_ProjectName"] = originrow["项目名称"] == null ? "" : originrow["项目名称"];
                                newrow["PO_ItemName"] = originrow["料品名称"];
                                newrow["PO_Itemunit"] = originrow["料品计量单位"];
                                newrow["PO_CreateTime"] = originrow["创建日期"];
                                newrow["PO_CreateBy"] = originrow["创建人姓名"];
                                newrow["PO_ModifyTime"] = originrow["修改日期"];
                                newrow["PO_ModifyBy"] = originrow["修改人姓名"];
                                newrow["PO_StartDate"] = originrow["开工日期"];
                                newrow["PO_OrderAmount"] = originrow["开工数量"];//u9订单开工数量，就是条码系统里面的可派工数量
                                newrow["PO_StartAmount"] = originrow["已完工数量"];//已派工数量，第一次导入条码系统里面的都等于已完工数量，正常情况下都是零
                                newrow["PO_FinishedAmount"] = originrow["已完工数量"];
                                newrow["PO_OrderSource"] = 0;
                                newrow["PO_ProduceDepartName"] = originrow["部门名称"];
                                newrow["PO_ProduceDepartCode"] = originrow["部门编号"];
                                newrow["PO_IsReturn"] = false;
                                newrow["PO_BCSCreateTime"] = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                                ds.Tables["ProduceOrder"].Rows.Add(newrow);

                                sb.Append(sb.Length == 0 ? originrow["U9订单Id"].ToString() : "," + originrow["U9订单Id"].ToString());
                                originrow["是否导入条码系统标记"] = "已导入条码系统";
                            }
                        }
                        int updateNum, insertNum;
                        MyDBController.InsertSqlBulk(ds.Tables["ProduceOrder"], colList, out updateNum, out insertNum);

                        if (ds.Tables["ProduceOrder"].Rows.Count > 0)
                        {
                            //修改最新操作时间
                            SQl = string.Format(@"select max([PO_ModifyTime]) from  produceorder as A  where A.[PO_ModifyTime] > '{0}' ", lastOperateTime);
                            lastOperateTime = Convert.ToDateTime(MyDBController.ExecuteScalar(SQl)).ToString("yyyy/MM/dd HH:mm:ss");

                            //修改条码系统中订单时间戳，这次从条码系统中获取生产订单的最大修改时间
                            SQl = string.Format(@"update [ProduceOrderFlag] set [POF_LastImportDate]='{0}' ,[POF_IsImportingFlag] ='false' where [ID]={1}", lastOperateTime, Convert.ToInt64(row["ID"]));
                            MyDBController.ExecuteNonQuery(SQl);
                            MyDBController.CloseConnection();
                            if (sb.Length > 0)
                            {
                                SQl = string.Format(@"update [MO_MO] set [DescFlexField_PubDescSeg8]='{0}' where [ID] in ({1})", "已导入条码系统", sb);
                                MyDBController.ExecuteNonQuery(new SqlConnection() { ConnectionString = sqlcon.ConnectionString }, SQl);
                            }
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message + "\n\r" + lastOperateTime);
            }
            finally
            {
                //修改条码系统中订单时间戳，这次从条码系统中获取生产订单的最大修改时间
                SQl = string.Format(@"update [ProduceOrderFlag] set [POF_IsImportingFlag] ='false' where [ID]={0}", Convert.ToInt64(row["ID"]));
                MyDBController.GetConnection();
                MyDBController.ExecuteNonQuery(SQl);
            }
            MyDBController.CloseConnection();
        }

        /// <summary>
        /// 报工结束的时候，更新U9中的生产订单的数量信息，并生成完工报告
        /// </summary>
        public static bool UpdateU9POInfo(DataSet _ds, out string _str)
        {
            bool flag = false;
            _str = "";
            WebService.Service ws = new WebService.Service();
            string message = ws.CreateCompRpt(_ds);
            if (message.Contains("成功"))
            {
                _str = message.Split(':')[1].Trim();
                flag = true;
            }
            else
            {
                _str = message;
            }
            return flag;
        }

        /// <summary>
        /// 审核U9系统中的完工报告
        /// </summary>
        /// <param name="_ds"></param>
        public static bool CheckU9POInfo(DataSet _ds, out string _str)
        {
            bool flag = false;
            WebService.Service ws = new WebService.Service();
            _str = ws.ApproveCompRpt(_ds);
            if (_str.Contains("成功"))
            {
                flag = true;
            }
            else
            {
            }
            return flag;
        }

        /// <summary>
        /// 获取条码系统中，可以用来派工的生产订单信息
        /// </summary>
        /// <returns></returns>
        public static List<ProduceOrderLists> FetchProduceOrderInfo()
        {
            DataSet ds = new DataSet();
            List<ProduceOrderLists> pols = new List<ProduceOrderLists>();
            string SQl = "";
            if (User_Info.User_Code.Equals("admin"))
            {
                SQl = string.Format(@"Select * from [ProduceOrder] where [PO_OrderAmount]>[PO_StartAmount]");//订单数量>开工数量
            }
            else
            {
                SQl = string.Format(@"Select * from [ProduceOrder] where [PO_OrderAmount]>[PO_StartAmount] and [PO_ProduceDepartCode]='{0}'", User_Info.User_Workcenter_Code);//订单数量>开工数量
            }

            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "ProduceOrder");
            MyDBController.CloseConnection();
            DataRowCollection drc = ds.Tables["ProduceOrder"].Rows;
            try
            {
                foreach (DataRow row in drc)
                {
                    ProduceOrderLists pol = new ProduceOrderLists();

                    pol.ID = Convert.ToInt64(row["ID"]);
                    pol.PO_ID = Convert.ToInt64(row["PO_ID"]);
                    pol.PO_Code = row["PO_Code"].ToString();
                    pol.PO_ItemID = row["PO_ItemID"].ToString();
                    pol.PO_ItemCode = row["PO_ItemCode"].ToString();
                    pol.PO_ItemSpec = row["PO_ItemSpec"].ToString();
                    pol.PO_ItemVersion = row["PO_ItemVersion"].ToString();
                    pol.PO_ProjectCode = row["PO_ProjectCode"].ToString();
                    pol.PO_ProjectName = row["PO_ProjectName"].ToString();
                    //pol.PO_WorkCenter = Convert.ToInt64(row["PO_WorkCenter"]);
                    pol.PO_ProduceDepartCode = row["PO_ProduceDepartCode"].ToString();
                    pol.PO_ProduceDepartName = row["PO_ProduceDepartName"].ToString();
                    pol.PO_ItemName = row["PO_ItemName"].ToString();
                    pol.PO_Itemunit = row["PO_Itemunit"].ToString();
                    pol.PO_CreateTime = Convert.ToDateTime(row["PO_CreateTime"]);
                    pol.PO_CreateBy = row["PO_CreateBy"].ToString();
                    pol.PO_ModifyTime = Convert.ToDateTime(row["PO_ModifyTime"]);
                    pol.PO_ModifyBy = row["PO_ModifyBy"].ToString();
                    pol.PO_StartDate = Convert.ToDateTime(row["PO_StartDate"]);
                    pol.PO_OrderAmount = Convert.ToInt32(row["PO_OrderAmount"]);
                    pol.PO_StartAmount = Convert.ToInt32(row["PO_StartAmount"]);
                    pol.PO_FinishedAmount = Convert.ToInt32(row["PO_FinishedAmount"]);
                    pol.PO_OrderSource = Convert.ToInt32(row["PO_OrderSource"]);
                    pol.PO_IsReturn = Convert.ToBoolean(row["PO_IsReturn"]);
                    pol.PO_BCSCreateTime = row["PO_BCSCreateTime"] is DBNull ? new DateTime() : Convert.ToDateTime(row["PO_BCSCreateTime"]);
                    pols.Add(pol);

                }
            }
            catch (Exception ee)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ee.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            pols = pols.Distinct(new ListComparer<ProduceOrderLists>((x, y) => { return x.PO_Code.Equals(y.PO_Code); })).ToList();
            return pols;
        }

        /// <summary>
        /// 获取条码系统中生产订单信息
        /// </summary>
        /// <param name="_key">关键词</param>
        /// <param name="type">关键词种类 0：生产订单编号，1：车间编号，2：料号，3：流转卡号</param>
        /// <returns></returns>
        public static List<ProduceOrderLists> FetchProduceOrderInfo(string _key, int type)
        {
            DataSet ds = new DataSet();
            List<ProduceOrderLists> pols = new List<ProduceOrderLists>();
            string SQl = "";
            switch (type)
            {
                case 0:
                    SQl = string.Format(@"Select * from [ProduceOrder] where [PO_Code] = '{0}'", _key);
                    break;
                case 1:
                    SQl = string.Format(@"Select * from [ProduceOrder] where [PO_ProduceDepartCode] like '%{0}%'", _key);
                    break;
                case 2:
                    SQl = string.Format(@"Select * from [ProduceOrder] where [PO_ItemCode] = '{0}'", _key);
                    break;
                case 3:
                    SQl = string.Format(@"select * from [ProduceOrder] where [PO_ID]=(select FC_SourceOrderID from [FlowCard] where [FC_Code]='{0}')", _key);
                    break;
                default:
                    break;
            }
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "ProduceOrder");
            MyDBController.CloseConnection();
            DataRowCollection drc = ds.Tables["ProduceOrder"].Rows;
            try
            {
                foreach (DataRow row in drc)
                {
                    ProduceOrderLists pol = new ProduceOrderLists();
                    pol.ID = Convert.ToInt64(row["ID"]);
                    pol.PO_ID = Convert.ToInt64(row["PO_ID"]);
                    pol.PO_Code = row["PO_Code"].ToString();
                    pol.PO_ItemID = row["PO_ItemID"].ToString();
                    pol.PO_ItemCode = row["PO_ItemCode"].ToString();
                    pol.PO_ItemSpec = row["PO_ItemSpec"].ToString();
                    pol.PO_ItemVersion = row["PO_ItemVersion"].ToString();
                    pol.PO_ProjectCode = row["PO_ProjectCode"].ToString();
                    pol.PO_ProjectName = row["PO_ProjectName"].ToString();
                    //pol.PO_WorkCenter = Convert.ToInt64(row["PO_WorkCenter"]);
                    pol.PO_ProduceDepartCode = row["PO_ProduceDepartCode"].ToString();
                    pol.PO_ProduceDepartName = row["PO_ProduceDepartName"].ToString();
                    pol.PO_ItemName = row["PO_ItemName"].ToString();
                    pol.PO_Itemunit = row["PO_Itemunit"].ToString();
                    pol.PO_CreateTime = Convert.ToDateTime(row["PO_CreateTime"]);
                    pol.PO_CreateBy = row["PO_CreateBy"].ToString();
                    pol.PO_ModifyTime = Convert.ToDateTime(row["PO_ModifyTime"]);
                    pol.PO_ModifyBy = row["PO_ModifyBy"].ToString();
                    pol.PO_StartDate = Convert.ToDateTime(row["PO_StartDate"]);
                    pol.PO_OrderAmount = Convert.ToInt32(row["PO_OrderAmount"]);
                    pol.PO_StartAmount = Convert.ToInt32(row["PO_StartAmount"]);
                    pol.PO_FinishedAmount = Convert.ToInt32(row["PO_FinishedAmount"]);
                    pol.PO_OrderSource = Convert.ToInt32(row["PO_OrderSource"]);
                    pol.PO_IsReturn = Convert.ToBoolean(row["PO_IsReturn"]);
                    pol.PO_BCSCreateTime = row["PO_BCSCreateTime"] is DBNull ? new DateTime() : Convert.ToDateTime(row["PO_BCSCreateTime"]);
                    pols.Add(pol);
                }
            }
            catch (Exception ee)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ee.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            pols = pols.Distinct(new ListComparer<ProduceOrderLists>((x, y) => { return x.PO_Code.Equals(y.PO_Code); })).ToList();
            return pols;
        }
        /// <summary>
        /// 获取条码系统中所有的订单信息,用来做U9生产订单信息排除的
        /// </summary>
        /// <param name="isSelectAll">可选参数，取值不重要，但是必须要一个参数</param>
        /// <returns></returns>
        public static List<ProduceOrderLists> FetchProduceOrderInfo(bool isSelectAll = true)
        {
            DataSet ds = new DataSet();
            List<ProduceOrderLists> pols = new List<ProduceOrderLists>();
            string SQl = string.Format(@"Select * from [ProduceOrder] ");//订单数量>开工数量
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "ProduceOrder");
            MyDBController.CloseConnection();
            DataRowCollection drc = ds.Tables["ProduceOrder"].Rows;
            try
            {
                foreach (DataRow row in drc)
                {
                    ProduceOrderLists pol = new ProduceOrderLists();

                    pol.ID = Convert.ToInt64(row["ID"]);
                    pol.PO_ID = Convert.ToInt64(row["PO_ID"]);
                    pol.PO_Code = row["PO_Code"].ToString();
                    pol.PO_ItemID = row["PO_ItemID"].ToString();
                    pol.PO_ItemCode = row["PO_ItemCode"].ToString();
                    pol.PO_ItemSpec = row["PO_ItemSpec"].ToString();
                    pol.PO_ItemVersion = row["PO_ItemVersion"].ToString();
                    pol.PO_ProjectCode = row["PO_ProjectCode"].ToString();
                    pol.PO_ProjectName = row["PO_ProjectName"].ToString();
                    //pol.PO_WorkCenter = Convert.ToInt64(row["PO_WorkCenter"]);
                    pol.PO_ProduceDepartCode = row["PO_ProduceDepartCode"].ToString();
                    pol.PO_ProduceDepartName = row["PO_ProduceDepartName"].ToString();
                    pol.PO_ItemName = row["PO_ItemName"].ToString();
                    pol.PO_Itemunit = row["PO_Itemunit"].ToString();
                    pol.PO_CreateTime = Convert.ToDateTime(row["PO_CreateTime"]);
                    pol.PO_CreateBy = row["PO_CreateBy"].ToString();
                    pol.PO_ModifyTime = Convert.ToDateTime(row["PO_ModifyTime"]);
                    pol.PO_ModifyBy = row["PO_ModifyBy"].ToString();
                    pol.PO_StartDate = Convert.ToDateTime(row["PO_StartDate"]);
                    pol.PO_OrderAmount = Convert.ToInt32(row["PO_OrderAmount"]);
                    pol.PO_StartAmount = Convert.ToInt32(row["PO_StartAmount"]);
                    pol.PO_FinishedAmount = Convert.ToInt32(row["PO_FinishedAmount"]);
                    pol.PO_OrderSource = Convert.ToInt32(row["PO_OrderSource"]);
                    pol.PO_IsReturn = Convert.ToBoolean(row["PO_IsReturn"]);
                    pols.Add(pol);

                }
            }
            catch (Exception ee)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ee.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            pols = pols.Distinct(new ListComparer<ProduceOrderLists>((x, y) => { return x.PO_Code.Equals(y.PO_Code); })).ToList();
            return pols;
        }

        /// <summary>
        /// 检查输入的生产订单编号是否存在
        /// </summary>
        /// <param name="_orderCode"></param>
        /// <returns></returns>
        public static bool CheckForCode(string _orderCode)
        {
            bool flag = false;
            string SQl = string.Format(@"select count(*) from [ProduceOrder] where [PO_Code]='{0}'", _orderCode);
            MyDBController.GetConnection();
            int count;
            try
            {
                count = Convert.ToInt32(MyDBController.ExecuteScalar(SQl));
                if (count > 0)
                {
                    flag = true;
                }
                else
                {
                }
            }
            catch (Exception)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("该生产订单编号不存在！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            MyDBController.CloseConnection();
            return flag;
        }
    }
}
