using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace strtableUpdate
{

    public class StringDataStruct
    {
        string StrExcelTable = "";

        public string StrExcelTable1
        {
            get { return StrExcelTable; }
            set { StrExcelTable = value; }
        }

        //<表名<id值,<国家，国家值>>>
        Dictionary<string, stringIdCountry> dicStringTable = new Dictionary<string, stringIdCountry>();
        public Dictionary<string, stringIdCountry> DicStringTable
        {
            get { return dicStringTable; }
            set { dicStringTable = value; }
        }
        /// <summary>
        /// 从数据库里读数据到字符串
        /// </summary>
        public void ReadDataBaseTodicString()
        { 
        
        }
        /// <summary>
        /// 保存到数据库里面去
        /// </summary>
        public void SavedicStringtoDataBase()
        { 
            
        }
    }
    public class stringIdCountry
    {
        //<id值,<国家，国家值>>
        Dictionary<string, Dictionary<string, string>> dicIDCountry = new Dictionary<string, Dictionary<string, string>>();

        public Dictionary<string, Dictionary<string, string>> DicIDCountry
        {
            get { return dicIDCountry; }
            set { dicIDCountry = value; }
        }

    }
}
