using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace BarCodeSystem
{
    public class User_Info
    {
        private static Int64 user_id;
        public static Int64 U9User_ID
        {
            get { return user_id; }
            set { user_id = value; }
        }

        private static string user_name = "";
        public static string User_Name
        {
            get { return user_name; }
            set { user_name = value; }
        }

        private static string user_code = "";
        public static string User_Code
        {
            get { return user_code; }
            set { user_code = value; }
        }

        /// <summary>
        /// 工作中心名称
        /// </summary>
        public static string User_WorkcenterName = "";

        /// <summary>
        /// 条码系统账号
        /// </summary>
        public static Int64 Account_ID { get; set; }

        /// <summary>
        /// 工作中心简称，派工的时候用来生成流转卡表头的
        /// </summary>
        public static string User_Workcenter_ShortName = "";

        /// <summary>
        /// 车间编码
        /// </summary>
        public static string User_Workcenter_Code = "";

        /// <summary>
        /// 车间ID
        /// </summary>
        public static Int64 User_Workcenter_ID = -1;

        /// <summary>
        /// 家用四通阀事业部：201；
        /// </summary>
        public static string[] User_Org_Code = { "201" };

        /// <summary>
        /// U9测试库；条码测试库；
        /// </summary>
        //public static string[] server = { "172.16.100.64", "172.16.100.53", "172.16.100.40" };
        public static string[] server = { "172.16.100.24", "172.16.100.40" };
        //public static string[] server = { "172.16.100.24", "172.16.52.150" };

        /// <summary>
        /// U9测试数据库；条码测试数据库
        /// </summary>
        //public static string[] database = { "SANHUADATA", "fajianBarCodeSystem", "BarCodeSystem" };
        public static string[] database = { "SANHUADATA20150917", "fajianBarCodeSystem" };


        /// <summary>
        /// U9测试库账号；条码测试库账号
        /// </summary>
        //public static string[] uid = { "sa", "sa", "sa" };
        public static string[] uid = { "sa", "sa" };
        //public static string[] uid = { "sa", "sa" };

        /// <summary>
        /// U9测试库密码；条码测试库密码
        /// </summary>
        //public static string[] pwd = { "Aa123456", "Ab123456", "Bb123" };
        public static string[] pwd = { "Sql2008", "Bb123" };
        //public static string[] pwd = { "Sql2008", "Aa1" };

        /// <summary>
        /// U9应用服务器
        /// </summary>
        public static string U9Server = "172.16.100.52/PortalV21";
        //public static string U9Server = "172.16.100.21/U9";

        /// <summary>
        /// U9企业号
        /// </summary>
        public static string EnterpriseCode = "200";
        //public static string EnterpriseCode = "0801";


        /// <summary>
        /// 编码文本框校验表达式；
        /// </summary>
        public static string[] pattern = { @"^\d+\z", @"^\d+\.?\d*\z", @"^[-]?[1-9]{1}\d*$|^[0]{1}$", @"^([0-9.]+)$" };

        /// <summary>
        /// 当前组织的组织信息：组织名称
        /// </summary>
        public static string Org_Info = "";

        /// <summary>
        /// U9数据库对应的企业号
        /// </summary>

        /// <summary>
        /// U9收发单据类型
        /// </summary>
        public static string POType = "";

        /// <summary>
        /// 登录人员岗位
        /// </summary>
        public static string P_Position = "";

        /// <summary>
        /// 组织ID
        /// </summary>
        public static Int64 Org_Id = 0;

        /// <summary>
        /// 记住的账号
        /// </summary>
        public static string rememberedAccountFileName = AppDomain.CurrentDomain.BaseDirectory + "SysPara/RememberedAccount.xml";

        /// <summary>
        /// 本地xml文档储存地点
        /// </summary>
        public static string sysPara = AppDomain.CurrentDomain.BaseDirectory + "SysPara";
        /// <summary>
        /// 当前屏幕分辨率参数：高
        /// </summary>
        public static int ScreenHeight
        {
            get;
            set;
        }

        /// <summary>
        /// 当前屏幕分辨率参数：宽
        /// </summary>
        public static int ScreenWidth
        {
            get;
            set;
        }

        /// <summary>
        /// 将指定的编码转换成一维码图片，非流转卡编码使用
        /// </summary>
        /// <param name="_code">指定的编码</param>
        /// <returns>一维码图片的url</returns>
        public static string FetchBarCodeImage(string _code)
        {
            string fileName = AppDomain.CurrentDomain.BaseDirectory + "/image_barcode/" + _code + ".jpg";
            if (File.Exists(fileName))
            {
            }
            else
                BarCodeByDcj.BarCodeForCSByDcj.GetBarCode(_code);

            Bitmap bitMap = new Bitmap(fileName);
            //int width = bitMap.Width, height = bitMap.Height;
            //RectangleF sourRec = new Rectangle(width * 2 / 10, 0, width * 6 / 10, height * 4 / 5);
            //剪裁一维码图片
            //Bitmap newMap = bitMap.Clone(sourRec, bitMap.PixelFormat);

            //string fileName = AppDomain.CurrentDomain.BaseDirectory + "/image_barcode/" + _code + "_副本.jpg";
            //if (File.Exists(fileName))
            //{
            //}
            //else
            //    //保存剪裁过的一维码图片
            //    newMap.Save(fileName, ImageFormat.Jpeg);

            BitmapImage bitImage = new BitmapImage(new Uri(fileName));
            return fileName;
        }
    }
}
