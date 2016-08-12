using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YaHooRate;

namespace ExchangeRateDemo.YaHoo
{
    public partial class Demo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                return;
            }

            if(Request.Form.Count==0){
                return;
            }

            string OriginRate=Request.Form["OriginRate"].ToString();
            string TargetRate=Request.Form["TargetRate"].ToString();
            
            /*支持转换类型
             * string
             * int
             * deciaml
             */
            string Moeny = Request.Form["Money"].ToString().ConverterRate(OriginRate,TargetRate);


            /*查询汇率（返回XML字符串）*/
            string Xml = Rate.QueryRateToXml(OriginRate, TargetRate);
            
            /*查询汇率（返回Obj对象）*/
            RateObj _RateObj=Rate.QueryRateToObj(OriginRate, TargetRate);

            Response.Write("<div>兑换的金额是:" + Request.Form["Money"] + "</div>"); 
            Response.Write("<div>"+OriginRate+"兑换"+TargetRate+"的汇率是："+_RateObj.Rate+"</div>");
            Response.Write("<div>"+OriginRate+"汇率兑换"+TargetRate+"的金额是:"+Moeny+"</div>");
            Response.Write("<div></div><div></div>"); 
        }
    }
}