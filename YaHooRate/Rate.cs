using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;
using System.IO;
namespace YaHooRate
{
    #region 使用雅虎汇率接口转换汇率(扩展方法)
    /// <summary>
    /// 使用雅虎汇率接口转换汇率
    /// Create By Asen
    /// 2016-08-12
    /// </summary>
    #endregion
    public static class Rate
    {
        /// <summary>
        /// 雅虎汇率接口API地址
        /// </summary>
        internal static string ApiUrl = "https://query.yahooapis.com/v1/public/yql";
        /// <summary>
        /// 雅虎YQL语句
        /// </summary>
        internal static string ExRateYQL = "q=select * from yahoo.finance.xchange where pair in (\"#Origin#Target\")";

        #region 把当前金额(string)转换为指定汇率金额
        /// <summary>
        /// 把当前金额(string)转换为指定汇率金额
        /// </summary>
        /// <param name="RateMoney">金额字符串</param>
        /// <param name="OriginRate">原币种码</param>
        /// <param name="TargetRate">目标币种码</param>
        /// <returns>string类型金额</returns>
        public static string ConverterRate(this string RateMoney,string OriginRate, string TargetRate)
        {
           return RateFunc<string>(RateMoney, OriginRate, TargetRate).ToString();
        }
        #endregion

        #region 把当前金额(decimal)转换为指定汇率金额
        /// <summary>
        /// 把当前金额(decimal)转换为指定汇率金额
        /// </summary>
        /// <param name="RateMoney">金额字符串</param>
        /// <param name="OriginRate">原币种码</param>
        /// <param name="TargetRate">目标币种码</param>
        /// <returns>decimal类型金额</returns>
        public static decimal ConverterRate(this decimal RateMoney, string OriginRate, string TargetRate)
        {
            return RateFunc<decimal>(RateMoney, OriginRate, TargetRate);
        }
        #endregion

        #region 把当前金额(int)转换为指定汇率金额
        /// <summary>
        /// 把当前金额(int)转换为指定汇率金额
        /// </summary>
        /// <param name="RateMoney">金额字符串</param>
        /// <param name="OriginRate">原币种码</param>
        /// <param name="TargetRate">目标币种码</param>
        /// <returns>int类型金额</returns>
        public static int ConverterRate(this int RateMoney, string OriginRate, string TargetRate)
        {
            return Convert.ToInt32(RateFunc<int>(RateMoney, OriginRate, TargetRate));
        }
        #endregion

        #region 转换汇率金额主体方法
        /// <summary>
        /// 转换汇率金额主体方法
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="Money">金额</param>
        /// <param name="OriginRate">原币种码</param>
        /// <param name="TargetRate">目标币种码</param>
        /// <returns></returns>
        internal static decimal RateFunc<T>(T Money, string OriginRate, string TargetRate)
        {
            XmlDocument RateXml=ReadRateXml(OriginRate, TargetRate);
            decimal RateMoeny = Convert.ToDecimal(Money);
            decimal Rate = Convert.ToDecimal(RateXml.DocumentElement.SelectSingleNode("results").SelectSingleNode("rate").SelectSingleNode("Rate").InnerText);
            return (RateMoeny * Rate);
        }
        #endregion

        #region 查询原币种与目标币种之间的汇率(Xml字符串)
        /// <summary>
        /// 查询原币种与目标币种之间的汇率
        /// </summary>
        /// <param name="OriginRate">原币种码</param>
        /// <param name="TargetRate">目标币种码</param>
        /// <returns>汇率XML字符串</returns>
        public static string QueryRateToXml(string OriginRate, string TargetRate)
        {
           XmlDocument RateXml=ReadRateXml(OriginRate, TargetRate);
           return RateXml.InnerXml;
        }
        #endregion

        #region 查询原币种与目标币种之间的汇率(汇率对象)
        /// <summary>
        /// 查询原币种与目标币种之间的汇率
        /// </summary>
        /// <param name="OriginRate">原币种码</param>
        /// <param name="TargetRate">目标币种码</param>
        /// <returns>汇率对象</returns>
        public static RateObj QueryRateToObj(string OriginRate, string TargetRate)
        {
            XmlDocument RateXml = ReadRateXml(OriginRate, TargetRate);
            XmlNode RateNode = RateXml.DocumentElement.SelectSingleNode("results").SelectSingleNode("rate");
            RateObj _RateObj = new RateObj();
            _RateObj.Name = RateNode.SelectSingleNode("Name").InnerText.ToString();
            _RateObj.Rate = RateNode.SelectSingleNode("Rate").InnerText.ToString();
            _RateObj.Time = RateNode.SelectSingleNode("Time").InnerText.ToString();
            _RateObj.Date = RateNode.SelectSingleNode("Date").InnerText.ToString();
            _RateObj.Bid = RateNode.SelectSingleNode("Bid").InnerText.ToString();
            _RateObj.Ask = RateNode.SelectSingleNode("Ask").InnerText.ToString();
            return _RateObj;
        }
        #endregion

        #region 读取雅虎汇率API接口
        /// <summary>
        /// 读取雅虎汇率API接口
        /// </summary>
        /// <param name="OriginRate">原币种码</param>
        /// <param name="TargetRate">目标币种码</param>
        /// <returns>汇率XmlDocument</returns>
        internal static XmlDocument ReadRateXml(string OriginRate, string TargetRate)
        {
            StringBuilder StrYQL = new StringBuilder(ExRateYQL);
            StrYQL.Replace("#Origin", OriginRate);
            StrYQL.Replace("#Target", TargetRate);
            StrYQL.Append("&format=xml");
            StrYQL.Append("&env=store://datatables.org/alltableswithkeys");
            string StrXml = PostRequest(ApiUrl, "Get", StrYQL.ToString(), "utf-8");

            XmlDocument RateXml = new XmlDocument();
            RateXml.LoadXml(StrXml);

            return RateXml;
        }
        #endregion

        #region 发送web请求/Post Or Get
        /// <summary>
        /// 发送web请求/Post Or Get
        /// </summary>
        /// <param name="Url">请求地址</param>
        /// <param name="Method">Post/Get请求</param>
        /// <param name="parame">参数</param>
        /// <param name="EncodeStr">编码方式</param>
        /// <returns>返回的字符串</returns>
        internal static string PostRequest(string Url, string Method, string parame, string EncodeStr)
        {
            Method = Method.ToUpper();
            EncodeStr = EncodeStr.ToUpper();
            //parame = System.Web.HttpUtility.UrlEncode(parame, Encoding.GetEncoding(EncodeStr)); 
            string Result = "";

            if (Method == "GET")
            {
                Url = Url + "?" + parame;
            }

            /*构建请求*/
            Stream StrInput = null;
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(Url);
            myRequest.Method = Method;

            if (Method == "POST")
            {
                byte[] data = Encoding.GetEncoding(EncodeStr).GetBytes(parame);
                myRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                myRequest.ContentLength = data.Length;
                Stream newStream = myRequest.GetRequestStream();
                /*发送请求*/
                newStream.Write(data, 0, data.Length);
                newStream.Close();
            }
            /*获取请求返回数据*/
            StrInput = myRequest.GetResponse().GetResponseStream();
            StreamReader StrRead = new StreamReader(StrInput);
            Result = StrRead.ReadToEnd();
            StrRead.Close();
            return Result;

        }
        #endregion
    }

    #region 汇率对象
    /// <summary>
    /// 汇率对象
    /// Create By Asen
    /// 2016-08-12
    /// </summary>

    public class RateObj
    {
        /// <summary>
        /// 转换汇率
        /// </summary>
        public string Name;
        /// <summary>
        /// 汇率
        /// </summary>
        public string Rate;
        /// <summary>
        /// 日期
        /// </summary>
        public string Date;
        /// <summary>
        /// 时间
        /// </summary>
        public string Time;
        /// <summary>
        /// 卖出外汇的汇率
        /// </summary>
        public string Ask;
        /// <summary>
        /// 买入外汇的汇率
        /// </summary>
        public string Bid;
    }
    #endregion



}
