using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace strtableUpdate
{
   public class DataBaseOption
    {
       private static DataBaseOption instance = null;

       internal static DataBaseOption Instance
        {
            get { return getInstance(); }
        }
       private static class SingletonFactory
       {
           public static DataBaseOption instance = new DataBaseOption();
       }
       public static DataBaseOption getInstance()
       {
           return SingletonFactory.instance;
       }  


       public DataBaseOption()
       {
           init();
       }
        
        Lv.Database.SQLite sqlite = null;
        public void init()
        {
            string SqliteFile = "";
            try
            {
                SqliteFile = "StrTabDataBase.db3";
                if (System.IO.File.Exists(SqliteFile) == true)
                {
                    sqlite = new Lv.Database.SQLite(Lv.Database.Static.SqliteConn(SqliteFile));
                    Console.WriteLine("sqlite 数据连接成功");
                    Lv.Log.Write("sqlite 数据连接成功 :" + SqliteFile, Lv.Log.MessageType.Info);
                    //sqlite.PassWord = "bruce";

                }
                else
                {
                    //创建数据库
                    CreateDataBase(SqliteFile);

                }


            }
            catch (Exception err)
            {
                Console.WriteLine("sqlite init() 初始化错误");
                Lv.Log.Write("sqlite init() 初始化错误" + SqliteFile, Lv.Log.MessageType.Error);
            }

        }

        private void CreateDataBase(string SqliteFile)
        {
            SQLiteConnection.CreateFile(SqliteFile);
            if (System.IO.File.Exists(SqliteFile) == true)
            {
                sqlite = new Lv.Database.SQLite(Lv.Database.Static.SqliteConn(SqliteFile));
                //sqlite.PassWord = "bruce";
                Console.WriteLine("sqlite 数据连接成功" + SqliteFile);
                Lv.Log.Write("sqlite 数据连接成功 " + SqliteFile, Lv.Log.MessageType.Info);
                Create();//创建工作需要的表
            }
            else
            {
                Console.WriteLine("sqlite 连接数据库失败,请关闭程序，重新打开。");
                Lv.Log.Write("sqlite 连接数据库失败,请关闭程序，重新打开。" + SqliteFile, Lv.Log.MessageType.Error);
                throw new Exception("无法创建sqlite数据库请检查");
            }
        }

        /// <summary>
        /// 初始化创建表
        /// </summary>
        /// <returns></returns>
        public bool Create()
        {

            try
            {
                sqlite.ExeQuery("CREATE TABLE  if not exists  default ([Id] integer PRIMARY KEY AUTOINCREMENT, [keymd5] Text, [日期] Text, [星期] Text, [广告来源] Text, [设备类型] Text, [广告系列] Text, [广告分组] Text, [广告关键词] Text, [展现量] Text, [点击量] Text, [消费] Text, [点击率] Text, [平均点击价格] Text, [登陆] Text, [注册] Text, [APP下载] Text, [注册成功] Text, [首参] Text);");
                return true;
            }
            catch (Exception err)
            {
                Console.WriteLine("sqlite init() 初始化错误 " + err.Message);
                Lv.Log.Write("sqlite init() 初始化错误 " + err.Message, Lv.Log.MessageType.Error);
                return false;
            }
        }



       public bool SaveDataToSqlite(StringDataStruct strData)
       {
           return true;
       }


    }
}
