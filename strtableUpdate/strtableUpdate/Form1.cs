using NPOI.HSSF.UserModel;
using NPOI.POIFS.FileSystem;
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
            ReadStrTable(ExcelFile1, ref strData1);

            strData2 = new StringDataStruct();
            ReadStrTable(ExcelFile2, ref strData2);

            comparerStrTable();
            Lv.Log.Init();

            //初始化对比结果
            ViewInit();
            MessageBox.Show("对比结束");
            this.Invoke(new EventHandler(delegate
            {
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
            this.Invoke(new EventHandler(delegate
            {
                label1.Text = strprompor.Replace("%0", countNoTable1ID.ToString()).Replace("%1", listStrOptionDataInsertValue.Count.ToString());
            }));
        }

        private void ViewValueInit()
        {
            bool isValueEmpty = false;
            bool isTable1ExiseValue = false;
            this.Invoke(new EventHandler(delegate
            {
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
            foreach (string itemSheet in strOptionDataAddNewID.DicStringTable.Keys)
            {
                //对表进行循环
                foreach (string itemid in strOptionDataAddNewID.DicStringTable[itemSheet].DicIDCountry.Keys)
                {
                    this.Invoke(new EventHandler(delegate
                    {
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

        private void comparerIdValue(Dictionary<string, Dictionary<string, string>> dictionary1, Dictionary<string, Dictionary<string, string>> dictionary2, string sheetname)
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

        private void comparerValueContry(Dictionary<string, string> dictionary1, Dictionary<string, string> dictionary2, string sheetname, string id)
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

                    if (sheet.GetRow(row).Cells.Count <= 0)
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
                        ICell cell = sheet.GetRow(row).GetCell(cellindex);//.SetCellType(CellType.String);
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
            if (System.IO.File.Exists(txtpath) == true)
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

        private void btnTab2ImpTab1_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == MessageBox.Show("您确定把表2里指定的ID导入到表1？", "提示", MessageBoxButtons.OKCancel))
            {
                //打开表2，把数据导入进来
                //打开表1等后续操作
                //循环表1数据，对表2数据循环提取，
                //      改变表1数据
                //保存
                Thread th = new Thread(new ParameterizedThreadStart(updateTable));
               // Thread th = new Thread(new ParameterizedThreadStart(importTab2toTab1));
                th.Start((object)ExcelFile1);
                CheckdicValue = new Dictionary<string, Dictionary<string, string>>();
            }
        }

        private void updateTable(object obj)
        {
            //这里要更新两个表，str_table,str_table1
            //把需要更新的表数据提取出来放到专门的一个类里面
            //<id, <country,value,是否更新〉〉
            UpTab updateTab = GetUpTable(ExcelFile2);

            List<string> strtable1_language = new List<string>();
            List<string> strtable2_language = new List<string>();
            string savePath1 = "str_table_bruce.xls";
            //更新到表1
            UpdateToStr_table(ref updateTab, ExcelFile1, savePath1,ref strtable1_language);
            //更新到表2
            string savePath2 = "str_table_bruce1.xls";
            string strtable2 = ExcelFile1.Replace(".xls", "1.xls");
            UpdateToStr_table(ref updateTab, strtable2, savePath2,ref strtable2_language);

            //对正常更新中未更新的ID进行增加操作
                    //1对每个要更新的字符分组，看他属于哪个表，如果这个国家都没有，那么不更新
                    //2打开原表，在最后一行添加需要添加的数据
            foreach (string itemcountry in strtable1_language)
            {
                if (strtable2_language.Contains(itemcountry) == true)
                {
                    strtable2_language.Remove(itemcountry);
                }
            }
            insertIDtoTable(ref updateTab, savePath1, strtable1_language);


            //统计还没有被替换的值
            outPutNoReplaceValue(updateTab);
            MessageBox.Show("替换表格完成");
        }

        /// <summary>
        /// 把表里没有的ID插入进去
        /// </summary>
        /// <param name="updateTab"></param>
        /// <param name="savePath1"></param>
        /// <param name="strtable1_language"></param>
        private void insertIDtoTable(ref UpTab updateTab, string savePath, List<string> strtable1_language)
        {
            // updateTab.DicidValue[id].DicCountryIsUpdate[country]  这个值是真说明已经更新过了，没有说明需要插入操作
            FileStream fs = new FileStream(savePath, FileMode.Open, FileAccess.Read, FileShare.Read);

            POIFSFileSystem ps = new POIFSFileSystem(fs);//需using NPOI.POIFS.FileSystem;
            IWorkbook workbook = new HSSFWorkbook(ps);
            ISheet sheet = workbook.GetSheetAt(0);//获取工作表

            IRow row = sheet.GetRow(0); //得到表头
            
            FileStream fout = new FileStream(savePath, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);//写入流

            //取行
            List<string> listtitle = new List<string>();
            stringIdCountry stridvalue = new stringIdCountry();
            for (int rowindex = 0; rowindex < 1; rowindex++)      //循环第一行
            {
                int col = 0;
                if (sheet.GetRow(rowindex).Cells.Count <= 0)
                {
                    continue;
                }
                foreach (ICell cell in sheet.GetRow(rowindex).Cells) //cells  这一行的单元格
                {
                    cell.SetCellType(CellType.String);
                    //取标题

                    listtitle.Add(cell.StringCellValue);
                    Console.WriteLine(" 标题：" + cell.StringCellValue);
                    col++;
                }
            }
            //结束取标题

            UpTab tmpTab = new UpTab();
            //写入数据区
            foreach (var item in updateTab.DicidValue)
            {
                foreach (var itemcountry in item.Value.DicCountryIsUpdate)
                {
                    if (itemcountry.Value == true)
                    {
                        continue;
                    }
                    else
                    {
                        //string info = string.Format("ID:{0} \tCountry:{1} \tValue:{2} 没有更新", item.Key, itemcountry.Key, item.Value.DicCountryValue[itemcountry.Key]);
                        //sb.AppendLine(info);
                        row = sheet.CreateRow((sheet.LastRowNum + 1));//在工作表中添加一行
                        ICell cell1 = row.CreateCell(0);
                        cell1.SetCellValue(item.Key);//赋值ID
                        UpTabCountry tmpcountry = new UpTabCountry();
                        for (int indextitle = 1; indextitle < listtitle.Count; indextitle++)
                        {
                            if (itemcountry.Key == listtitle[indextitle])   //要添加的国家一致
                            {
                                cell1 = row.CreateCell(indextitle);
                                cell1.SetCellValue(item.Value.DicCountryValue[itemcountry.Key]);//赋值国家值
                                tmpcountry.AddCountry(itemcountry.Key,item.Value.DicCountryValue[itemcountry.Key]);
                                if (tmpTab.DicidValue.ContainsKey(item.Key))
                                {
                                    tmpTab.DicidValue[item.Key].AddCountry(itemcountry.Key, item.Value.DicCountryValue[itemcountry.Key]);
                                    tmpTab.DicidValue[item.Key].DicCountryIsUpdate[itemcountry.Key] = true;
                                }
                                else
                                {
                                    tmpTab.DicidValue.Add(item.Key, tmpcountry);
                                }
                                
                            }
                        }

                    }
                }
            }

            //把插入的数据更新到updateTab里去
            foreach (var item in tmpTab.DicidValue)
            {
                foreach (var itemcountry in item.Value.DicCountryIsUpdate)
                {
                    if (updateTab.DicidValue.ContainsKey(item.Key))
                    {
                        if (updateTab.DicidValue.ContainsKey(itemcountry.Key))
                        {
                            updateTab.DicidValue[item.Key].DicCountryIsUpdate[itemcountry.Key] = true;
                        }
                    }
                }
            }

            //结束写入数据区




            fout.Flush();
            workbook.Write(fout);//写入文件
            workbook = null;
            fout.Close();

            /*
            //获取Excel2007工作簿
            HSSFWorkbook workbook = new HSSFWorkbook(fs); //excel2007以下才可用
            fs.Close();

            //编辑工作薄当中内容
            //取表

                ISheet sheet = workbook.GetSheetAt(1);
                if (sheet.GetRow(0) == null)
                {
                    return;
                }
                //取行
                List<string> listtitle = new List<string>();
                stringIdCountry stridvalue = new stringIdCountry();
                for (int rowindex = 0; rowindex <= 1; rowindex++)      //循环第一行
                {
                    int col = 0;
                    if (sheet.GetRow(rowindex).Cells.Count <= 0)
                    {
                        continue;
                    }
                    foreach (ICell cell in sheet.GetRow(rowindex).Cells) //cells  这一行的单元格
                    {
                        cell.SetCellType(CellType.String);
                        //取标题
                        if (rowindex == 0 && col == 0)
                        {
                            //只是ID不需要记录
                        }
                        else if (rowindex == 0)  //表示是ID列 表示是第一行标题行
                        {
                            listtitle.Add(cell.StringCellValue);
                            Console.WriteLine(" 标题：" + cell.StringCellValue);
                        }
                        col++;
                    }
                }
                //结束取标题


                IRow row = sheet.CreateRow(sheet.LastRowNum);//在工作表中添加一行
                for (int i = 0; i < listtitle.Count -1; i++)
                {
                     ICell cell = row.CreateCell(0);//创建单元格
                     cell.SetCellValue("领用单位");//赋值
                }
                

            if (System.IO.File.Exists(savePath) == true)
            {
                System.IO.File.Delete(savePath);
            }
            FileStream fs2 = File.Create(savePath);
            workbook.Write(fs2);
            fs2.Close();
             * 
             */
        }

        private void outPutNoReplaceValue(UpTab updateTab)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in updateTab.DicidValue)
            {
                foreach (var itemcountry in item.Value.DicCountryIsUpdate)
                {
                    if (itemcountry.Value == true)
                    {
                        continue;
                    }
                    else
                    {
                        string info = string.Format("ID:{0} \tCountry:{1} \tValue:{2} 没有更新", item.Key, itemcountry.Key, item.Value.DicCountryValue[itemcountry.Key]);
                        sb.AppendLine(info);
                    }
                }
            }
            string Nopath = "NoReplaceValue.txt";
            if (System.IO.File.Exists(Nopath) == true)
            {
                System.IO.File.Delete(Nopath);
            }
            System.IO.File.WriteAllText(Nopath, sb.ToString());
        }

        private void UpdateToStr_table(ref UpTab updateTab, string readExcelPath, string savePath, ref List<string> strtable_language)
        {
            int findid = 0;
            int replaceValue = 0;
            int repateValue = 0;
            if (System.IO.File.Exists( readExcelPath) == false)
            {
                MessageBox.Show("文件：" + readExcelPath + "未找到！");
                return;
            }
            //把文件内容导入到工作薄当中，然后关闭文件
            FileStream fs = new FileStream(readExcelPath, FileMode.Open, FileAccess.Read, FileShare.Read);

            //获取Excel2007工作簿
            HSSFWorkbook workbook = new HSSFWorkbook(fs); //excel2007以下才可用
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
                for (int row = 0; row <= 1; row++)      //循环第一行
                {
                    int col = 0;
                    if (sheet.GetRow(row).Cells.Count <= 0)
                    {
                        continue;
                    }
                    foreach (ICell cell in sheet.GetRow(row).Cells) //cells  这一行的单元格
                    {
                        cell.SetCellType(CellType.String);
                        //取标题
                        if (row == 0 && col == 0)
                        {
                            //只是ID不需要记录
                        }
                        else if (row == 0)  //表示是ID列 表示是第一行标题行
                        {
                            strtable_language.Add(cell.StringCellValue);
                            Console.WriteLine(" 标题：" + cell.StringCellValue);
                        }
                        col++;
                    }
                }
                //结束取标题

                for (int row = 1; row <= sheet.LastRowNum; row++)
                {
                    ICell cellid = sheet.GetRow(row).GetCell(0);//测试ID 是否存在
                    if (cellid == null)
                    {
                        continue;
                    }
                    cellid.SetCellType(CellType.String);
                    string id = cellid.StringCellValue;
                    if (updateTab.DicidValue.ContainsKey(id) == true)
                    {

                        findid++;
                        //修改内容 
                        for (int cellindex = 0; cellindex < strtable_language.Count(); cellindex++)
                        {
                            string country = strtable_language[cellindex];
                            //这个ID里有该国家并且  这个国家的值之前没有被替换过
                            if (updateTab.DicidValue[id].DicCountryValue.ContainsKey(country) == true && updateTab.DicidValue[id].DicCountryIsUpdate[country] == false)
                            {

                                updateTab.DicidValue[id].DicCountryIsUpdate[country] = true;//更新到updateTable里面去以后碰到了就跳过
                                string value = updateTab.DicidValue[id].DicCountryValue[country];
                                if (value == "")
                                {
                                    continue;
                                }

                                ICell cellValue = sheet.GetRow(row).GetCell(cellindex + 1);
                                if (cellValue == null)
                                {
                                    sheet.CreateRow(row).CreateCell(cellindex);
                                    cellValue = sheet.GetRow(row).GetCell(cellindex);
                                }
                                if (value == cellValue.StringCellValue)
                                {
                                    repateValue++;
                                    continue; //前后字符串一致
                                }
                                cellValue.SetCellValue(value);
                                replaceValue++;
                            }
                        }//endforcell
                    }
                }//endforrow
            }//endforsheet
            if (System.IO.File.Exists(savePath) == true)
            {
                System.IO.File.Delete(savePath);
            }
            FileStream fs2 = File.Create(savePath);
            workbook.Write(fs2);
            fs2.Close();
            //MessageBox.Show("表2导入到表1完成,更新后的数据已导出到 " + savePath + "表中");
            string prompt = "表2导入到表1完成,更新后的数据已导出到 " + savePath + "表中\r\n"+"已找到的ID有：" + findid.ToString() + "已替换的Value有：" + replaceValue.ToString() + "重复数Value有：" + repateValue.ToString();
            MessageBox.Show(prompt);
            Console.WriteLine("已找到的ID有：" + findid.ToString());
            Console.WriteLine("已替换的Value有：" + replaceValue.ToString());
            Console.WriteLine("重复数Value有：" + repateValue.ToString());


        }

        private UpTab GetUpTable(string excelPath)
        {
            UpTab updateTable = new UpTab();

            Dictionary<string, Dictionary<string, string>> dicValue = new Dictionary<string, Dictionary<string, string>>();
            //把文件内容导入到工作薄当中，然后关闭文件
            FileStream fs = new FileStream(excelPath, FileMode.Open, FileAccess.Read, FileShare.Read);

            //获取Excel2007工作簿
            HSSFWorkbook workbook = new HSSFWorkbook(fs); //excel2007以下才可用
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
                for (int row = 0; row <= 1; row++)      //循环第一行
                {
                    int col = 0;
                    if (sheet.GetRow(row).Cells.Count <= 0)
                    {
                        continue;
                    }
                    foreach (ICell cell in sheet.GetRow(row).Cells) //cells  这一行的单元格
                    {
                        cell.SetCellType(CellType.String);

                        if (row == 0)  //表示是ID列 表示是第一行标题行
                        {
                            listTitle.Add(cell.StringCellValue);
                            Console.WriteLine(" 标题：" + cell.StringCellValue);
                        }
                        col++;
                    }
                }
                //结束取标题

                for (int row = 1; row <= sheet.LastRowNum; row++)
                {
                    string id = "";
                    ICell cellid = sheet.GetRow(row).GetCell(0);
                    if (cellid == null || cellid.StringCellValue == "")
                    {
                        continue;
                    }
                    cellid.SetCellType(CellType.String);
                   id = cellid.StringCellValue;
                    //修改内容 
                    UpTabCountry tmpCountry = new UpTabCountry();
                    for (int cellindex = 1; cellindex < listTitle.Count(); cellindex++)
                    {
                        ICell cellvalue = sheet.GetRow(row).GetCell(cellindex);
                        if (cellvalue == null || cellvalue.StringCellValue == "")
                        {
                            continue;
                        }
                        tmpCountry.AddCountry(listTitle[cellindex], cellvalue.StringCellValue);
                    }//endforcell
                    updateTable.DicidValue.Add(id, tmpCountry);
                }//endforrow
            }//endforsheet
            return updateTable;
        }
        class UpTab
        {
            Dictionary<string, UpTabCountry> dicidValue = new Dictionary<string, UpTabCountry>();

            public Dictionary<string, UpTabCountry> DicidValue
            {
                get { return dicidValue; }
                set { dicidValue = value; }
            }
        }
        class UpTabCountry
        {


            Dictionary<string, string> dicCountryValue = new Dictionary<string, string>();

            public Dictionary<string, string> DicCountryValue
            {
                get { return dicCountryValue; }
                set { dicCountryValue = value; }
            }
            Dictionary<string, bool> dicCountryIsUpdate = new Dictionary<string, bool>();

            public Dictionary<string, bool> DicCountryIsUpdate
            {
                get { return dicCountryIsUpdate; }
                set { dicCountryIsUpdate = value; }
            }
            public bool AddCountry(string country, string value)
            {
                if (dicCountryValue.ContainsKey(country) == true)
                {
                    //如果添加的数据有重复的国家那么不添加进来
                    Lv.Log.Write("添加国家的数据有重复的国家那么不添加进来!", Lv.Log.MessageType.Warn);
                    return false;
                }
                dicCountryValue.Add(country, value);
                dicCountryIsUpdate.Add(country, false);
                return true;
            }
            public bool updateCountry(string country)
            {
                if (dicCountryIsUpdate.ContainsKey(country)==true)
                {
                    dicCountryIsUpdate[country] = true;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        private void importTab2toTab1(object obj)
        {
            int findid = 0;
            int replaceValue = 0;
            int repateValue = 0;
            string ExcelFile = (string)obj;
            //把文件内容导入到工作薄当中，然后关闭文件
            FileStream fs = new FileStream(ExcelFile, FileMode.Open, FileAccess.Read, FileShare.Read);

            //获取Excel2007工作簿
            HSSFWorkbook workbook = new HSSFWorkbook(fs); //excel2007以下才可用
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
                for (int row = 0; row <= 1; row++)      //循环第一行
                {
                    int col = 0;
                    if (sheet.GetRow(row).Cells.Count <= 0)
                    {
                        continue;
                    }
                    foreach (ICell cell in sheet.GetRow(row).Cells) //cells  这一行的单元格
                    {
                        cell.SetCellType(CellType.String);
                        //取标题
                        if (row == 0 && col == 0)
                        {
                            //只是ID不需要记录
                        }
                        else if (row == 0)  //表示是ID列 表示是第一行标题行
                        {
                            listTitle.Add(cell.StringCellValue);
                            Console.WriteLine(" 标题：" + cell.StringCellValue);
                        }
                        col++;
                    }
                }
                //结束取标题

                for (int row = 1; row <= sheet.LastRowNum; row++)
                {
                    ICell cellid = sheet.GetRow(row).GetCell(0);//测试ID 是否存在
                    if (cellid == null)
                    {
                        continue;
                    }
                    cellid.SetCellType(CellType.String);
                    if (CheckID(cellid.StringCellValue))    //用表2的数据来查找
                    {
                        findid++;
                        //修改内容 
                        for (int cellindex = 0; cellindex < listTitle.Count(); cellindex++)
                        {
                            if (CheckdicValue[cellid.StringCellValue].ContainsKey(listTitle[cellindex]))
                            {
                               
                                string value = CheckdicValue[cellid.StringCellValue][listTitle[cellindex]];
                                if (value == "")
                                {
                                    continue;
                                }

                                ICell cellValue = sheet.GetRow(row).GetCell(cellindex+1);
                                if (cellValue == null)
                                {
                                    sheet.CreateRow(row).CreateCell(cellindex);
                                    cellValue = sheet.GetRow(row).GetCell(cellindex);
                                }
                                if (value == cellValue.StringCellValue)
                                {
                                    repateValue++;
                                    continue; //前后字符串一致
                                }
                                cellValue.SetCellValue(value);
                                replaceValue++;
                            }
                        }//endforcell
                    }
                }//endforrow
            }//endforsheet
            string path = @"strtab_bruce.xls";
            if (System.IO.File.Exists(path) == true)
            {
                System.IO.File.Delete(path);
            }
            FileStream fs2 = File.Create(path);
            workbook.Write(fs2);
            fs2.Close();
            MessageBox.Show("表2导入到表1完成");
            Console.WriteLine("已找到的ID有：" + findid.ToString());
            Console.WriteLine("已替换的Value有：" + replaceValue.ToString());
            Console.WriteLine("重复数Value有：" + repateValue.ToString());
        }//end fun


        Dictionary<string,Dictionary<string,string>> CheckdicValue = new  Dictionary<string,Dictionary<string,string>>();
        private bool CheckID(string p)
        {
            if (CheckdicValue.Count<1)
	        {
                CheckdicValue = importTab2toTab1();
	        }
            if (CheckdicValue.ContainsKey(p) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
           
            //return true;
            //throw new NotImplementedException();
        }

        private Dictionary<string, Dictionary<string, string>> importTab2toTab1()
        {
            Dictionary<string, Dictionary<string, string>> dicValue = new Dictionary<string, Dictionary<string, string>>();
            //把文件内容导入到工作薄当中，然后关闭文件
            FileStream fs = new FileStream(ExcelFile2, FileMode.Open, FileAccess.Read, FileShare.Read);

            //获取Excel2007工作簿
            HSSFWorkbook workbook = new HSSFWorkbook(fs); //excel2007以下才可用
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
                for (int row = 0; row <= 1; row++)      //循环第一行
                {
                    int col = 0;
                    if (sheet.GetRow(row).Cells.Count <= 0)
                    {
                        continue;
                    }
                    foreach (ICell cell in sheet.GetRow(row).Cells) //cells  这一行的单元格
                    {
                        cell.SetCellType(CellType.String);

                        if (row == 0)  //表示是ID列 表示是第一行标题行
                        {
                            listTitle.Add(cell.StringCellValue);
                            Console.WriteLine(" 标题：" + cell.StringCellValue);
                        }
                        col++;
                    }
                }
                //结束取标题

                for (int row = 1; row <= sheet.LastRowNum; row++)
                {
                    ICell cellid = sheet.GetRow(row).GetCell(0);
                    if (cellid == null || cellid.StringCellValue == "" )
	                {
		                 continue;
	                }
                    cellid.SetCellType(CellType.String);
                    
                        //修改内容 
                    Dictionary<string,string> dic  = new Dictionary<string,string>();
                    
                    for (int cellindex =1; cellindex < listTitle.Count(); cellindex++)
                    {
                        ICell cellvalue = sheet.GetRow(row).GetCell(cellindex);
                        if (cellvalue == null || cellvalue.StringCellValue =="" )
	                    {
		                    continue;
	                    }
                        dic.Add(listTitle[cellindex], cellvalue.StringCellValue);
                    }//endforcell
                    dicValue.Add(cellid.StringCellValue,dic);
                }//endforrow
            }//endforsheet
            return dicValue;
        }
    }
}
