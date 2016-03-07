using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace strtableUpdate
{
    //后期有许多对索引的操作，用类的话不好处理暂不用
    /// <summary>
    /// 国家值，
    /// </summary>
    public class CountryValue
    {
        List<CountryValue> listCv = new List<CountryValue>();
        /// <summary>
        /// 
        /// </summary>
        public List<CountryValue> ListCv
        {
            get { return listCv; }
            set { listCv = value; }
        }
        string countrye = "";

        public string Countrye
        {
            get { return countrye; }
            set { countrye = value; }
        }
        string value = "";

        /// <summary>
        /// 值
        /// </summary>
        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
        bool isReplace = false;

        /// <summary>
        /// 是否替换
        /// </summary>
        public bool IsReplace
        {
            get { return isReplace; }
            set { isReplace = value; }
        }
    }
}
