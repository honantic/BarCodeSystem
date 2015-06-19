using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BarCodeSystem.PublicClass.DatabaseEntity
{
    public class FlowCardQualityLists : INotifyPropertyChanged
    {
        /// <summary>
        /// ID
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        /// 流转卡子表id
        /// </summary>
        public Int64 FCQ_FlowCardSubID { get; set; }
        /// <summary>
        /// 质量id
        /// </summary>
        public Int64 FCQ_QulityIssueID { get; set; }
        /// <summary>
        /// 报废数量
        /// </summary>
        public int FCQ_ScrapAmount { get; set; }

        string qi_Code="";
        /// <summary>
        /// 质量问题编码，从质量问题信息表中关联得到
        /// </summary>
        public string QI_Code
        {
            get
            {
                if (!string.IsNullOrEmpty(qi_Code))
                {
                    return qi_Code;
                }
                else
                    return "";
            }
            set
            {
                if (!qi_Code.Equals(value))
                {
                    qi_Code = value;
                    OnPropertyChanged("QI_Code");
                }
            }
        }
        string qi_Name="";
        /// <summary>
        /// 质量问题名称，从质量问题信息表中关联得到
        /// </summary>
        public string QI_Name
        {
            get
            {
                if (!string.IsNullOrEmpty(qi_Name))
                {
                    return qi_Name;
                }
                else
                    return "";
            }
            set
            {
                if (!qi_Name.Equals(value))
                {
                    qi_Name = value;
                    OnPropertyChanged("QI_Name");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
