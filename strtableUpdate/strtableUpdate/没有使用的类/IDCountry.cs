using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace strtableUpdate
{
    //后期有许多对索引的操作，用类的话不好处理暂不用
    class IDCountry
    {
        string iD = "";
        /// <summary>
        /// 如果有相同的出现，只取第一次跳过第二次
        /// </summary>
        public string ID
        {
            get { return iD; }
            set { iD = value; }
        }
        CountryValue cv = new CountryValue();

        public CountryValue Cv
        {
            get { return cv; }
            set { cv = value; }
        }

    }
}
