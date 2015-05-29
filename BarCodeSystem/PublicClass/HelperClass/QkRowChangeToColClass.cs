using System;
using System.Windows.Forms;
using System.Data;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel; 

namespace BarCodeSystem
{
    /// <summary>
    /// user_class 的摘要说明。
    /// </summary>
    public class QkRowChangeToColClass
    {

        /// <summary>
        /// 求天数差
        /// </summary>
        /// <param name="d1"> 开始日期</param>
        /// <param name="d2"> 结束日期</param>
        /// <returns></returns>
        private int get_day_count(DateTime d1, DateTime d2)
        {
            System.TimeSpan add_day = new TimeSpan(1, 0, 0, 0);
            int count = 0;
            DateTime d3 = d1;

            while (d2 >= d3)  //当结束日期大于等于交换日期
            {
                count++;
                d3 = d3 + add_day;
            }
            return count;
        }


        // 输出目标表，列名

        #region Excel剪切板数据导入到临时表格中
        public void write_excel_date_to_temp_table(DataTable TempTable,string ColNameList)
        {
            
            string []ColNameString = ColNameList.Split(',');
            int ColCount = ColNameString.Length;

            string data_string = System.Windows.Forms.Clipboard.GetText();  //得到剪切板数据
            data_string=data_string.Replace("\r\n", "\r");
            data_string = data_string.Replace("\n", "");
            string []string_list=data_string.Split('\r');
            string []ColNameStringList=ColNameList.Split(',');  //得到列集合
            string str;
            string[] str1;
            int i,j;
            string DataType;
            DataColumn dc;
            DataRow r1;
            for (i = 0; i<string_list.Length; i++) 
            {
                str = string_list[i];
                str1=str.Split('\t');
                if (str1.Length >= ColCount)
                {
                    r1 = TempTable.NewRow();
                    for (j = 0; j < ColNameStringList.Length; j++)
                    {
                        if (ColNameStringList[j] != "")
                        {
                            dc = TempTable.Columns[ColNameStringList[j].Trim()] ;
                            DataType = dc.DataType.ToString().Trim();
                            switch (DataType)
                            {
                                case "System.DateTime":
                                    r1[ColNameStringList[j].Trim()] = DateTime.Parse(str1[j]);
                                    break;
                                case "System.Int16":
                                    if (str1[j].Length==0)
                                    {
                                        r1[ColNameStringList[j].Trim()] = 0;
                                    }
                                    else
                                    {
                                        r1[ColNameStringList[j].Trim()] = int.Parse(str1[j]);
                                    }
                                    break;
                                case "System.Int32":
                                    if (str1[j].Length==0)
                                    {
                                        r1[ColNameStringList[j].Trim()] = 0;
                                    }
                                    else
                                    {
                                        r1[ColNameStringList[j].Trim()] = Int32.Parse(str1[j]);
                                    }
                                    break;
                                case "System.Int64":
                                    if (str1[j].Length == 0)
                                    {
                                        r1[ColNameStringList[j].Trim()] = 0;
                                    }
                                    else
                                    {
                                        r1[ColNameStringList[j].Trim()] = Int64.Parse(str1[j]);
                                    }
                                    break;
                                case "System.String":
                                    r1[ColNameStringList[j].Trim()] = str1[j].Trim();
                                    break;
                                case "System.Decimal":
                                    str = str1[j].Replace(",", "");
                                    if (str.Trim().Length == 0)
                                    {
                                        r1[ColNameStringList[j].Trim()] = 0m;
                                    }
                                    else
                                    {
                                        r1[ColNameStringList[j].Trim()] = decimal.Parse(str1[j]);
                                    }
                                    break;
                            }
                        }
                    }

                    if (r1.Table.Columns.Contains("No"))
                    {
                        if ((r1["No"] is DBNull) || (r1["No"] == null))
                        {
                            r1["No"] = (i + 1).ToString().Trim();
                        }
                    }
                    TempTable.Rows.Add(r1);
                    TempTable.AcceptChanges();
                }
            }
        }
        #endregion


        /// <summary>
        /// 该方法通过流的形式往Excel中写入数据，优点是速度快，
        /// 缺点就是由于Excel里的智能识别功能，把数字首位的0去掉了，并且数字以科学记数法显式。
        /// </summary>
        /// <param name="dt"></param>
        private void OutToExcel(System.Data.DataTable dt)
        {
            #region   验证可操作性

            //定义表格内数据的行数和列数   
            int rowscount = dt.Rows.Count;
            int colscount = dt.Columns.Count;
            //行数必须大于0   
            if (rowscount <= 0)
            {
                MessageBox.Show("没有数据可供保存 ", "提示 ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //列数必须大于0   
            if (colscount <= 0)
            {
                MessageBox.Show("没有数据可供保存 ", "提示 ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //行数不可以大于65536   
            if (rowscount > 65536)
            {
                MessageBox.Show("数据记录数太多(最多不能超过65536条)，不能保存 ", "提示 ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //列数不可以大于255   
            if (colscount > 255)
            {
                MessageBox.Show("数据记录行数太多，不能保存 ", "提示 ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            #endregion
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Execl files (*.xls)|*.xls";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.CreatePrompt = true;
            saveFileDialog.Title = "保存为Excel文件";
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName.IndexOf(":") < 0) return; //被点了"取消"

            Stream myStream;
            myStream = saveFileDialog.OpenFile();
            StreamWriter sw = new StreamWriter(myStream, System.Text.Encoding.GetEncoding(-0));
            string columnTitle = "";
            try
            {
                //写入列标题
                for (int i = 0; i < colscount; i++)
                {
                    if (i > 0)
                    {
                        columnTitle += "\t";
                    }
                    columnTitle += dt.Columns[i].ColumnName;
                }
                sw.WriteLine(columnTitle);

                //写入列内容
                for (int j = 0; j < rowscount; j++)
                {
                    string columnValue = "";
                    for (int k = 0; k < colscount; k++)
                    {
                        if (k > 0)
                        {
                            columnValue += "\t";
                        }
                        if (dt.Rows[j][k] == null)
                            columnValue += "";
                        else
                        {
                            if (dt.Rows[j][k].GetType() == typeof(string) && dt.Rows[j][k].ToString().StartsWith("0"))
                            {
                                columnValue += "'" + dt.Rows[j][k].ToString();
                            }
                            else
                                columnValue += dt.Rows[j][k].ToString();
                        }
                    }
                    sw.WriteLine(columnValue);
                }
                sw.Close();
                myStream.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sw.Close();
                myStream.Close();
                MessageBox.Show("数据导出成功,共导出" + dt.Rows.Count.ToString() + "条记录");
            }
        }

        /// <summary>
        /// 该方法能避免Excel对数据的自动处理，
        /// 但是由于是往Excel的单元格逐一写入数据，数据量小的时候速度还可以，
        /// 如果数据量太大，速度就很慢了。
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool CreateExcelFileForDataTable(System.Data.DataTable dt)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Excel文件名称 (*.xls)|*.xls|Excel文件名称 (*.xlsx)|*.xlsx";
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK && saveFileDialog1.FileName != null)
            {
                //文件存在时先删除文件后再进行下一步操作
                int rowIndex = 1;      //开始写入数据的单元格行
                int colIndex = 0;      //开始写入数据的单元格列
                //int columnNum = size;
                System.Reflection.Missing miss = System.Reflection.Missing.Value;
                Excel.Application xlapp = new Excel.Application();
                Excel.ApplicationClass mExcel = new Excel.ApplicationClass();
                mExcel.Visible = false;
                Excel.Workbooks mBooks = (Excel.Workbooks)mExcel.Workbooks;
                Excel.Workbook mBook = (Excel.Workbook)(mBooks.Add(miss));
                Excel.Worksheet mSheet = (Excel.Worksheet)mBook.ActiveSheet;
                Excel.Range range = mSheet.get_Range((object)"A1", System.Reflection.Missing.Value);
                try
                {                // Headers.  
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        colIndex++;
                        mSheet.Cells[rowIndex, colIndex] = dt.Columns[i].ColumnName;
                    }
                    foreach (DataRow row in dt.Rows)    //同样方法处理数据
                    {
                        rowIndex++;
                        colIndex = 0;
                        foreach (DataColumn col in dt.Columns)
                        {
                            colIndex++;
                            if (row[col.ColumnName].GetType() == typeof(string))
                            {
                                mSheet.Cells[rowIndex, colIndex] = "'" + row[col.ColumnName].ToString();
                            }
                            else
                            {
                                mSheet.Cells[rowIndex, colIndex] = row[col.ColumnName].ToString();
                            }
                        }
                    }
                    //保存工作已写入数据的工作表，加亮处为解决整个问题的关键
                    mBook.SaveAs(saveFileDialog1.FileName, Excel.XlFileFormat.xlExcel7, miss, miss, miss, miss, Excel.XlSaveAsAccessMode.xlNoChange, miss, miss, miss, miss, miss);
                    return true;
                }
                catch (Exception ee)
                {
                    throw new Exception(ee.Message);
                }
               finally //finally中的代码主要用来释放内存和中止进程()
                {
                    mBook.Close(false, miss, miss);
                    mBooks.Close();
                    mExcel.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(range);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(mSheet);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(mBook);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(mBooks);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(mExcel);
                    GC.Collect();
                }
            }
            else
            {
                return false;
            }
        } 
    }
}