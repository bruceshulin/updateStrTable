using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace strtableUpdate
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            strData1.ReadDataBaseTodicString();
            strData2.ReadDataBaseTodicString();
        }
        StringDataStruct strData1 = new StringDataStruct();
        StringDataStruct strData2 = new StringDataStruct();
        //未添加的ID
        StringDataStruct strOptionDataAddNewID = new StringDataStruct();
        List<Dictionary<string, string>> listStrOptionDataInsertValue = new List<Dictionary<string, string>>();
        string strprompor = "表2中有%0个ID在表1中未发现，表1与表2有%1个相同ID相同国家不同值。";
        int countNoTable1ID = 0;
        
        private void btnOpenStrTab_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "*.*|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = ofd.FileName;
                ExcelFile1 = ofd.FileName;
            }
        }
        string ExcelFile1 = "";
        string ExcelFile2 = "";

        
        private void importExcel(object str)
        { 
            strData1 = new StringDataStruct();
            ReadStrTable( ExcelFile1, ref strData1);

            strData2 = new StringDataStruct();
            ReadStrTable(ExcelFile2, ref strData2);

            comparerStrTable();
            Lv.Log.Init();
            
            //初始化对比结果
            ViewInit();
            MessageBox.Show("对比结束");
            this.Invoke(new EventHandler(delegate { 
                this.btnStartCompare.Enabled = true;
                this.btnImportIDtoTxt.Enabled = true;
                this.btnValuetoTxt.Enabled = true;
            }));
        }

        private void ViewInit()
        {

            //未添加ID初始化
            ViewIDInit();
            //不同值初始化
            ViewValueInit();            
            this.Invoke(new EventHandler(delegate {
                label1.Text = strprompor.Replace("%0", countNoTable1ID.ToString()).Replace("%1", listStrOptionDataInsertValue.Count.ToString());
            }));

        }

        private void ViewValueInit()
        {
            bool isValueEmpty = false;
            bool isTable1ExiseValue = false;
            this.Invoke(new EventHandler(delegate { 
                isValueEmpty = cbTable2ValueEmpty.Checked;
                isTable1ExiseValue = cbTable1ExiseValue.Checked;
            }));
            int i = 0;
            foreach (var itemvalue in listStrOptionDataInsertValue)
            {
                if (isValueEmpty == true && itemvalue["value2"] == "")
                {
                    continue;
                }
                if (isTable1ExiseValue == true && itemvalue["value1"] != "")
                {
                    continue;
                }
                this.Invoke(new EventHandler(delegate
                    {
                        UpdateComperValueView updateValue = new UpdateComperValueView(itemvalue["sheetname"], itemvalue["id"], itemvalue["contry"], itemvalue["value1"], itemvalue["value2"]);
                        updateValue.Location = new Point(0, (76 * i));
                        panel2.Controls.Add(updateValue);
                    }));
                    i++;

            }
        }

        private void ViewIDInit()
        {
            //记算有多少个未添加ID
           
           
            int i = 0;
            foreach ( string  itemSheet in strOptionDataAddNewID.DicStringTable.Keys)
            {
                //对表进行循环
                foreach ( string itemid in strOptionDataAddNewID.DicStringTable[itemSheet].DicIDCountry.Keys)
	            {
                    
                    this.Invoke(new EventHandler(delegate {
                        UpdateIDView upidview = new UpdateIDView(itemSheet, itemid);
                        upidview.Location = new Point(0, (30 * i));
                        panel1.Controls.Add(upidview);
                    }));
                    i++;
	            }
               
            }
            countNoTable1ID = i;


        }

        /// <summary>
        /// 对比两个表
        /// </summary>
        private void comparerStrTable()
        {
            //对问题数据保存变量初始化
            strOptionDataAddNewID = new StringDataStruct();
            listStrOptionDataInsertValue = new List<Dictionary<string, string>>();
            //清空界面
            this.Invoke(new EventHandler(delegate { panel1.Controls.Clear(); panel2.Controls.Clear(); }));
            foreach (var strDataitem in strData1.DicStringTable)
            {
                //表
                if (strData2.DicStringTable.ContainsKey(strDataitem.Key))
                {
                    //表2里面也有表1的sheet表那么进行对比
                    comparerIdValue(strDataitem.Value.DicIDCountry, strData2.DicStringTable[strDataitem.Key].DicIDCountry, strDataitem.Key);
                }
                else
                {
                    //表2中没有表1的当前表
                    string pormpot = "表2中没有  表" + strDataitem.Key;
                    Console.WriteLine(pormpot);
                    Lv.Log.Write(pormpot, Lv.Log.MessageType.Info);
                } 
            }
        }

        private void comparerIdValue(Dictionary<string, Dictionary<string, string>> dictionary1, Dictionary<string, Dictionary<string, string>> dictionary2,string sheetname)
        {
            //id <国家，值>
            string pormpot = "";
            foreach (var diciditem in dictionary1)
            {
                if (dictionary2.ContainsKey(diciditem.Key) == true)
                {
                    //Console.WriteLine("ID: " + diciditem.Key);
                    //表2里也有表1相同的key 进一步对比
                    comparerValueContry(diciditem.Value, dictionary2[diciditem.Key], sheetname, diciditem.Key);
                }
                else
                {
                    //输出表2没有表1引项id
                    if (strOptionDataAddNewID.DicStringTable.ContainsKey(sheetname) == true)
                    {
                        strOptionDataAddNewID.DicStringTable[sheetname].DicIDCountry.Add(diciditem.Key, diciditem.Value);
                    }
                    else
                    {
                        stringIdCountry strCountry = new stringIdCountry();
                        strCountry.DicIDCountry.Add(diciditem.Key, diciditem.Value);
                        strOptionDataAddNewID.DicStringTable.Add(sheetname, strCountry);
                    }

                    pormpot = "表2中表名：" + sheetname + " 没有此项id: " + diciditem.Key;
                    Console.WriteLine(pormpot);
                    Lv.Log.Write(pormpot, Lv.Log.MessageType.Info);
                }
            }
        }

        private void comparerValueContry(Dictionary<string, string> dictionary1, Dictionary<string, string> dictionary2,string sheetname,string id)
        {
            //国家，值
            string pormpot = "";
            foreach (var diccontryitem in dictionary1)
            {
                Dictionary<string, string> tmpOptiondic = new Dictionary<string, string>();
                if (dictionary2.ContainsKey(diccontryitem.Key) == true)
                {
                    //表2里也有表1相同的key 进一步对比
                    if (dictionary2[diccontryitem.Key] == diccontryitem.Value)
                    {
                        //两个字符串一致
                    }
                    else
                    {
                        //两个字符串不一致
                        tmpOptiondic.Add("sheetname", sheetname);
                        tmpOptiondic.Add("id", id);
                        tmpOptiondic.Add("contry", diccontryitem.Key);
                        tmpOptiondic.Add("value1", diccontryitem.Value);
                        tmpOptiondic.Add("value2", dictionary2[diccontryitem.Key]);
                        listStrOptionDataInsertValue.Add(tmpOptiondic);
                        pormpot = "表2中 表:" + sheetname + "  ID: " + id + "  国家:" + diccontryitem.Key + "值不一致: 表1＝" + diccontryitem.Value + "    表2＝" + dictionary2[diccontryitem.Key];
                        Console.WriteLine(pormpot);
                        Lv.Log.Write(pormpot, Lv.Log.MessageType.Info);
                    }
                }
                else
                {                    
                    tmpOptiondic.Add("sheetname", sheetname);
                    tmpOptiondic.Add("id", id);
                    tmpOptiondic.Add("contry", diccontryitem.Key);
                    tmpOptiondic.Add("value1", diccontryitem.Value);
                    tmpOptiondic.Add("value2", "");
                    listStrOptionDataInsertValue.Add(tmpOptiondic);
                    //输出表2没有表1引项id
                    pormpot = "表2中  表:" + sheetname + "  ID:" + id + "没有此国家: " + diccontryitem.Key;
                    Console.WriteLine(pormpot);
                    Lv.Log.Write(pormpot, Lv.Log.MessageType.Info);
                }
                
            }
        }


        private void ReadStrTable(string ExcelFile, ref StringDataStruct strData)
        {
           
            //把文件内容导入到工作薄当中，然后关闭文件
            FileStream fs = new FileStream(ExcelFile, FileMode.Open, FileAccess.Read, FileShare.Read);

            //获取Excel2007工作簿
            HSSFWorkbook workbook = new HSSFWorkbook(fs); //excel2007以下才可用
            //IWorkbook workbook = new XSSFWorkbook(fs); EXCEL2007可用
            fs.Close();

            //编辑工作薄当中内容
            //取表
            for (int sheeti = 0; sheeti < workbook.NumberOfSheets; sheeti++)
            {
                ISheet sheet = workbook.GetSheetAt(sheeti);                    
                if (sheet.GetRow(0) == null)
                {
                    continue;
                }
                //取行
                stringIdCountry stridvalue = new stringIdCountry();
                List<string> listTitle = new List<string>();
                for (int row = 0; row <= 1; row++)
                {
                    int col = 0;

                    if ( sheet.GetRow(row).Cells.Count <= 0)
                    {
                        continue;
                    }
                    foreach (ICell cell in sheet.GetRow(row).Cells) //cells  这一行的单元格
                    {
                        cell.SetCellType(CellType.String);
                        //取标题
                        if (row == 0 && col == 0)
                        {
                            //只是ID标题不需要记录
                        }
                        else if (row == 0)  //表示是ID列 表示是第一行标题行
                        {
                            listTitle.Add(cell.StringCellValue);
                            Console.WriteLine(" 标题：" + cell.StringCellValue);
                        }  
                        col++;
                    }
                }
                for (int row = 1; row <= sheet.LastRowNum; row++)
                {
                    int col = 0;
                    string id = "";
                    Dictionary<string, string> dicCountryValue = new Dictionary<string, string>();
                    //取数据
                    for (int cellindex = 0; cellindex < listTitle.Count(); cellindex++)
                    {
                        /*
                        * Excel数据Cell有不同的类型，当我们试图从一个数字类型的Cell读取出一个字符串并写入数据库时，就会出现Cannot get a text value from a numeric cell的异常错误。
                        * 解决办法：先设置Cell的类型，然后就可以把纯数字作为String类型读进来了
                        */
                        ICell cell = sheet.GetRow(row).GetCell(cellindex) ;//.SetCellType(CellType.String);
                        if (cell == null)
                        {
                            if (col == 0)  //表示是ID列 
                            {
                                break;//没有ID直接跳过这一行
                            }
                            else
                            {
                                dicCountryValue.Add(listTitle[col - 1], "");    //没有内空写上空
                            }
                            col++;
                            if (col > listTitle.Count)
                            {
                                break;
                            }
                            continue;
                        }
                        cell.SetCellType(CellType.String);
                        //cell.SetCellValue((Int32.Parse(cell.StringCellValue) * 2).ToString());
                        if (col == 0)  //表示是ID列 
                        {
                            //Console.WriteLine(cell.StringCellValue);
                            id = cell.StringCellValue;
                            if (id == "TXT_KEYLOCK_KEYPAD_UNLOCK_TIP_TECNO")
                            {
                                id = cell.StringCellValue;
                            }
                            if (id == "" || string.IsNullOrEmpty(id) == true || id == "0")
                            {
                                break;  //如果这一行没有ID，那么不保存这一行
                            }
                        }
                        else
                        {
                            //表示是内容值数据
                            //  Console.WriteLine(cell.StringCellValue);
                            dicCountryValue.Add(listTitle[col - 1], cell.StringCellValue);
                            if (listTitle[col - 1] == "Hebrew")
                            {
                                Console.WriteLine("haha");
                            }
                        }
                        col++;
                        if (col > listTitle.Count)
                        {
                            break;
                        }
                    }
                   // foreach (ICell cell in sheet.GetRow(row).Cells) //cells  这一行的单元格
                    //{

                       
                   // }
                    if (id == "" || string.IsNullOrEmpty(id) == true || id == "0")
                    {
                        //如果这一行没有ID，那么不保存这一行
                        continue;
                    }
                    else
                    {
                        if (stridvalue.DicIDCountry.ContainsKey(id) == true)
                        {
                            id = id + "bruce2";
                        }
                        stridvalue.DicIDCountry.Add(id, dicCountryValue);
                    }
                    
                }
                strData.DicStringTable.Add(workbook.GetSheetName(sheeti), stridvalue);

                //保存到数据库  后期字符串和网络参数都从数据库里拉
                //strData.SavedicStringtoDataBase();

            }

            //把编辑过后的工作薄重新保存为excel文件
            //FileStream fs2 = File.Create(@"F:\zhxl\NPOI\zhxl2.xlsx");
            //workbook.Write(fs2);
            //fs2.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string str1 = "ካብ መስመር ወፃኢ ገጽት ኣቐምጥ ";
            string str2 = "ካብ መስመር ወፃኢ ገጽት ኣቐምጥ ";
            if (str1 == str2)
            {
                MessageBox.Show("一样");
            }
            else
            {
                MessageBox.Show("不一样");
            }
        }

        private void btnStartCompare_Click(object sender, EventArgs e)
        {
            this.btnStartCompare.Enabled = false;
            this.btnImportIDtoTxt.Enabled = false;
            this.btnValuetoTxt.Enabled = false;
            Thread th = new Thread(new ParameterizedThreadStart(importExcel));
            th.Start((object)ExcelFile1);
        }

        private void btnOpenStr2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "*.*|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBox3.Text = ofd.FileName;
                ExcelFile2 = ofd.FileName;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            string txtpath = "tableid.txt";
            if (System.IO.File.Exists( txtpath) == true)
            {
                MessageBox.Show("tableid.txt 存在，无法导出，请在当前目录删除此文件！");
                return;
            }
            StringBuilder sb = new StringBuilder();
            foreach (string itemSheet in strOptionDataAddNewID.DicStringTable.Keys)
            {
                //对表进行循环
                foreach (string itemid in strOptionDataAddNewID.DicStringTable[itemSheet].DicIDCountry.Keys)
                {
                    sb.AppendLine("sheetName:" + itemSheet + "\tID:" + itemid + "\t");
                }

            }
            System.IO.File.WriteAllText(txtpath, sb.ToString());
            MessageBox.Show("文件已导出到本程序根目录 tableid.txt中");
        }

        private void btnValuetoTxt_Click(object sender, EventArgs e)
        {
            string txtpath = "tablevalue.txt";
            if (System.IO.File.Exists(txtpath) == true)
            {
                MessageBox.Show("tablevalue.txt 存在，无法导出，请在当前目录删除此文件！");
                return;
            }
            StringBuilder sb = new StringBuilder();
            foreach (var itemvalue in listStrOptionDataInsertValue)
            {
                if (cbTable2ValueEmpty.Checked == true && itemvalue["value2"] == "")
                {
                    continue;
                }
                if (cbTable1ExiseValue.Checked == true && itemvalue["value1"] != "")
                {
                    continue;
                }
                sb.AppendLine("sheetName:" + itemvalue["sheetname"] + "\tID:" + itemvalue["id"] + "\t国家:" + itemvalue["contry"]);
                sb.AppendLine("表1_value:" + itemvalue["value1"]);
                sb.AppendLine("表2_value:" + itemvalue["value2"]);
                sb.AppendLine("------");
            }

            System.IO.File.WriteAllText(txtpath, sb.ToString());
            MessageBox.Show("文件已导出到本程序根目录 tablevalue.txt中");
        }


    }
}
