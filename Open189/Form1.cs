using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace Open189
{
    public partial class Form1 : Form
    {
        public Access_Token token = new Access_Token();
        public MyApp myappinfo = new MyApp();
        public List<CityModel> list = new List<CityModel>();

        public Form1()
        {
            InitializeComponent();
            initCityList();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
          
            myappinfo.api_id = "374691450000031423";
            myappinfo.app_secret = "38229781970eab5c17cfd119d52d8a16";

            myappinfo.url1 = "http://mytg.wzcbd.com/runpage/189sms.aspx";
            myappinfo.url2 = "http://mytg.wzcbd.com/runpage/callback.aspx";

            string requesturl = txtRequestUrl.Text;
            //string para = "app_id={0}&app_secret={1}&grant_type=client_credentials&redirect_uri={2}"
            string para = String.Format(txtPostPara.Text, myappinfo.api_id, myappinfo.app_secret, myappinfo.url1);
            CookieContainer cookie = new CookieContainer();

            string returnjson = SendDataByPost(requesturl, para, ref cookie);

            JObject access_token = JsonConvert.DeserializeObject(returnjson) as JObject;
            token.res_code = access_token["res_code"].ToString();
            token.res_message = access_token["res_message"].ToString();
            token.access_token = access_token["access_token"].ToString();
            token.expires_in = access_token["expires_in"].ToString();




            //{"res_code":"0","res_message":"Success","access_token":"e2bf450638feb46f2b7010a28577922d1371786068514","expires_in":2592000}
            txtReturnJson.Text = returnjson;


        }
        #region 同步通过POST方式发送数据
        /// <summary>
        /// 通过POST方式发送数据
        /// </summary>
        /// <param name="Url">url</param>
        /// <param name="postDataStr">Post数据</param>
        /// <param name="cookie">Cookie容器</param>
        /// <returns></returns>
        public string SendDataByPost(string Url, string postDataStr, ref CookieContainer cookie)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            if (cookie.Count == 0)
            {
                request.CookieContainer = new CookieContainer();
                cookie = request.CookieContainer;
            }
            else
            {
                request.CookieContainer = cookie;
            }
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postDataStr.Length;
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }
        #endregion
        #region 同步通过GET方式发送数据
        /// <summary>
        /// 通过GET方式发送数据
        /// </summary>
        /// <param name="Url">url</param>
        /// <param name="postDataStr">GET数据</param>
        /// <param name="cookie">GET容器</param>
        /// <returns></returns>
        public string SendDataByGET(string Url, string postDataStr, ref CookieContainer cookie)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            if (cookie.Count == 0)
            {
                request.CookieContainer = new CookieContainer();
                cookie = request.CookieContainer;
            }
            else
            {
                request.CookieContainer = cookie;
            }
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }
        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            string url = String.Format("http://api.189.cn/huafeng/api/getforecast24");
            string cityid = "0101010102";
            if (listBox1.SelectedIndex > -1)
            {
                cityid = GetCityModel(listBox1.SelectedItem.ToString()).id;
            }
            string para = string.Format(txtForecastpara.Text, token.access_token, myappinfo.api_id, cityid);
            CookieContainer cookie = new CookieContainer();
            string returnxml = SendDataByGET(url, para, ref cookie);
            txtDemo.Text = returnxml;
           
        }

       
   
       


        private void initCityList()
        {
            #region 城市列表
            StringBuilder sb = new StringBuilder();
            sb.Append("安徽0101021001淮南,");
            sb.Append("安徽0101021602芜湖,");
            sb.Append("安徽0101021101黄山,");
            sb.Append("安徽0101020201蚌埠,");
            sb.Append("安徽0101020801合肥,");
            sb.Append("安徽0101020901淮北,");
            sb.Append("安徽0101021203六安,");
            sb.Append("安徽0101021502铜陵,");
            sb.Append("安徽0101020304亳州,");
            sb.Append("安徽0101020705阜阳,");
            sb.Append("安徽0101020403巢湖,");
            sb.Append("安徽0101021405宿州,");
            sb.Append("安徽0101020604滁州,");
            sb.Append("安徽0101021707宣城,");
            sb.Append("安徽0101020502池州,");
            sb.Append("安徽0101020101安庆,");
            sb.Append("安徽0101021302马鞍山,");
            sb.Append("澳门01010301澳门,");
            sb.Append("北京0101010102北京,");
            sb.Append("福建0101050205龙岩,");
            sb.Append("福建0101050609南安,");
            sb.Append("福建0101050704三明,");
            sb.Append("福建0101050611厦门,");
            sb.Append("福建0101050101福州,");
            sb.Append("福建0101050905漳州,");
            sb.Append("福建0101050309南平,");
            sb.Append("福建0101050501莆田,");
            sb.Append("福建0101050404宁德,");
            sb.Append("甘肃0101060201定西,");
            sb.Append("甘肃0101060606酒泉,");
            sb.Append("甘肃0101060901成县,");
            sb.Append("甘肃0101061005平凉,");
            sb.Append("甘肃0101060502永昌,");
            sb.Append("甘肃0101060305玛曲,");
            sb.Append("甘肃0101061302武威,");
            sb.Append("甘肃0101061204天水,");
            sb.Append("甘肃0101061105庆阳,");
            sb.Append("甘肃0101060804临夏,");
            sb.Append("甘肃0101061401张掖,");
            sb.Append("甘肃0101060105白银,");
            sb.Append("甘肃0101060701兰州,");
            sb.Append("广东0101072102珠海,");
            sb.Append("广东0101070103潮州,");
            sb.Append("广东0101070305顺德,");
            sb.Append("广东0101071105清远,");
            sb.Append("广东0101071801湛江,");
            sb.Append("广东0101070802揭阳,");
            sb.Append("广东0101071501深圳,");
            sb.Append("广东01010713汕尾,");
            sb.Append("广东0101071201汕头,");
            sb.Append("广东01010702东莞,");
            sb.Append("广东0101071408韶关,");
            sb.Append("广东0101070401广州,");
            sb.Append("广东0101070501河源,");
            sb.Append("广东01010720中山,");
            sb.Append("广东0101070904茂名,");
            sb.Append("广东0101070702鹤山,");
            sb.Append("广东0101071004梅县,");
            sb.Append("广东0101071704云浮,");
            sb.Append("广东0101071601阳江,");
            sb.Append("广东0101071902高要,");
            sb.Append("广东0101070604惠州,");
            sb.Append("广西0101080804钟山,");
            sb.Append("广西0101081203钦州,");
            sb.Append("广西0101080112百色,");
            sb.Append("广西0101080403防城港,");
            sb.Append("广西0101080904象州,");
            sb.Append("广西0101080305凭祥,");
            sb.Append("广西0101081303梧州,");
            sb.Append("广西0101081103南宁,");
            sb.Append("广西0101081001柳州,");
            sb.Append("广西0101080601桂林,");
            sb.Append("广西0101080501贵港,");
            sb.Append("广西0101081403陆川,");
            sb.Append("广西0101080707河池,");
            sb.Append("广西0101080201北海,");
            sb.Append("贵州0101090603都匀,");
            sb.Append("贵州01010902毕节,");
            sb.Append("贵州0101090806铜仁,");
            sb.Append("贵州0101090707兴义,");
            sb.Append("贵州0101090304贵阳,");
            sb.Append("贵州0101090904遵义,");
            sb.Append("贵州0101090402盘县,");
            sb.Append("贵州0101090104安顺,");
            sb.Append("贵州0101090507凯里,");
            sb.Append("海南0101101201琼海,");
            sb.Append("海南0101101801通什,");
            sb.Append("海南0101101601万宁,");
            sb.Append("海南0101101101陵水,");
            sb.Append("海南0101100501儋州,");
            sb.Append("海南0101100601定安,");
            sb.Append("海南0101100101白沙,");
            sb.Append("海南0101100201保亭,");
            sb.Append("海南0101100801海口,");
            sb.Append("海南0101100401澄迈,");
            sb.Append("海南0101101301琼中,");
            sb.Append("海南0101101501屯昌,");
            sb.Append("海南0101101401三亚,");
            sb.Append("海南0101100701东方,");
            sb.Append("海南0101101001临高,");
            sb.Append("海南0101100301昌江,");
            sb.Append("海南0101100901乐东,");
            sb.Append("海南0101101701文昌,");
            sb.Append("河北0101110903唐山,");
            sb.Append("河北0101110701秦皇岛,");
            sb.Append("河北0101110402邯郸,");
            sb.Append("河北0101110801石家庄,");
            sb.Append("河北0101111107张家口,");
            sb.Append("河北0101110103保定,");
            sb.Append("河北0101110307承德,");
            sb.Append("河北0101110202沧州,");
            sb.Append("河北0101111012邢台,");
            sb.Append("河北0101110508衡水,");
            sb.Append("河北0101110601廊坊,");
            sb.Append("河南0101121001三门峡,");
            sb.Append("河南0101120201鹤壁,");
            sb.Append("河南0101120101安阳,");
            sb.Append("河南0101120708南阳,");
            sb.Append("河南0101120503漯河,");
            sb.Append("河南0101121809驻马店,");
            sb.Append("河南0101121502许昌,");
            sb.Append("河南0101120902濮阳,");
            sb.Append("河南0101121201济源,");
            sb.Append("河南0101120804平顶山,");
            sb.Append("河南0101120601洛阳,");
            sb.Append("河南0101121406信阳,");
            sb.Append("河南0101120401开封,");
            sb.Append("河南0101120302焦作,");
            sb.Append("河南0101121101商丘,");
            sb.Append("河南0101121602郑州,");
            sb.Append("河南0101121303新乡,");
            sb.Append("河南0101121701周口,");
            sb.Append("黑龙江0101130901齐齐哈尔,");
            sb.Append("黑龙江0101131102双鸭山,");
            sb.Append("黑龙江0101130401鹤岗,");
            sb.Append("黑龙江01011302大兴安岭,");
            sb.Append("黑龙江0101131301伊春,");
            sb.Append("黑龙江0101131001勃利,");
            sb.Append("黑龙江0101130701佳木斯,");
            sb.Append("黑龙江0101131203海伦,");
            sb.Append("黑龙江0101130801牡丹江,");
            sb.Append("黑龙江0101130501黑河,");
            sb.Append("黑龙江0101130601鸡西,");
            sb.Append("黑龙江0101130304哈尔滨,");
            sb.Append("黑龙江0101130109大庆,");
            sb.Append("湖北0101140101鄂州,");
            sb.Append("湖北0101140303黄冈,");
            sb.Append("湖北0101140202恩施,");
            sb.Append("湖北0101141305孝感,");
            sb.Append("湖北0101140501荆门,");
            sb.Append("湖北0101140606荆州,");
            sb.Append("湖北0101140902随州,");
            sb.Append("湖北0101141002武汉,");
            sb.Append("湖北0101140803十堰,");
            sb.Append("湖北0101141403宜昌,");
            sb.Append("湖北0101140402黄石,");
            sb.Append("湖北0101141106咸宁,");
            sb.Append("湖北0101141202襄樊,");
            sb.Append("湖南0101151202岳阳,");
            sb.Append("湖南0101151402株洲,");
            sb.Append("湖南0101150502怀化,");
            sb.Append("湖南0101151004桃江,");
            sb.Append("湖南0101150902凤凰,");
            sb.Append("湖南01011504衡阳,");
            sb.Append("湖南0101150701邵阳,");
            sb.Append("湖南0101150802湘潭,");
            sb.Append("湖南0101151107永州,");
            sb.Append("湖南0101150202长沙,");
            sb.Append("湖南0101150102常德,");
            sb.Append("湖南0101150603娄底,");
            sb.Append("湖南0101151303张家界,");
            sb.Append("湖南0101150302郴州,");
            sb.Append("吉林0101160502辽源,");
            sb.Append("吉林01011608通化,");
            sb.Append("吉林0101160908延吉,");
            sb.Append("吉林0101160604四平,");
            sb.Append("吉林0101160703松原,");
            sb.Append("吉林0101160201白山,");
            sb.Append("吉林0101160301长春,");
            sb.Append("吉林0101160401吉林市,");
            sb.Append("吉林0101160102白城,");
            sb.Append("江苏0101170103常州,");
            sb.Append("江苏0101170601苏州,");
            sb.Append("江苏0101171107盐城,");
            sb.Append("江苏0101170801泰州,");
            sb.Append("江苏0101170901无锡,");
            sb.Append("江苏0101170401南京,");
            sb.Append("江苏0101171301镇江,");
            sb.Append("江苏0101170704宿迁,");
            sb.Append("江苏0101170501南通,");
            sb.Append("江苏0101170305连云港,");
            sb.Append("江苏0101171203扬州,");
            sb.Append("江苏0101170201楚州,");
            sb.Append("江苏0101171002徐州,");
            sb.Append("江西0101181103鹰潭,");
            sb.Append("江西0101180701萍乡,");
            sb.Append("江西0101180304吉安,");
            sb.Append("江西0101180602南昌,");
            sb.Append("江西0101180511九江,");
            sb.Append("江西0101180902新余,");
            sb.Append("江西0101180401景德镇,");
            sb.Append("江西0101180205赣州,");
            sb.Append("江西01011801抚州,");
            sb.Append("江西0101180809上饶,");
            sb.Append("江西0101181009宜春,");
            sb.Append("辽宁0101190504丹东,");
            sb.Append("辽宁0101190606新宾,");
            sb.Append("辽宁0101190302朝阳,");
            sb.Append("辽宁0101191102盘锦,");
            sb.Append("辽宁0101190402大连,");
            sb.Append("辽宁0101190902锦州,");
            sb.Append("辽宁0101190203本溪,");
            sb.Append("辽宁0101191401营口,");
            sb.Append("辽宁0101190102鞍山,");
            sb.Append("辽宁0101190702阜新县,");
            sb.Append("辽宁0101191304铁岭,");
            sb.Append("辽宁0101191001辽阳,");
            sb.Append("辽宁0101191201沈阳,");
            sb.Append("辽宁0101190802葫芦岛,");
            sb.Append("内蒙古0101200701乌海,");
            sb.Append("内蒙古0101201101临河,");
            sb.Append("内蒙古0101200103包头,");
            sb.Append("内蒙古0101200802阿拉善右旗,");
            sb.Append("内蒙古0101200205赤峰,");
            sb.Append("内蒙古0101200301东胜,");
            sb.Append("内蒙古0101200902二连浩特,");
            sb.Append("内蒙古0101200404呼和浩特,");
            sb.Append("内蒙古0101201001乌兰浩特,");
            sb.Append("内蒙古0101200603通辽,");
            sb.Append("内蒙古0101200508满洲里,");
            sb.Append("内蒙古0101201201集宁,");
            sb.Append("宁夏0101210402银川,");
            sb.Append("宁夏0101210301吴忠,");
            sb.Append("宁夏0101210201石嘴山,");
            sb.Append("宁夏0101210105固原,");
            sb.Append("青海0101220102达日,");
            sb.Append("青海0101220301德令哈,");
            sb.Append("青海0101220401刚察,");
            sb.Append("青海0101220501共和,");
            sb.Append("青海0101220601河南县,");
            sb.Append("青海01012207西宁,");
            sb.Append("青海01012208玉树,");
            sb.Append("青海0101220205海东,");
            sb.Append("山东0101230407菏泽,");
            sb.Append("山东0101231001青岛,");
            sb.Append("山东0101231504烟台,");
            sb.Append("山东0101230802聊城,");
            sb.Append("山东0101231404潍坊,");
            sb.Append("山东0101230502济南,");
            sb.Append("山东0101231301威海,");
            sb.Append("山东01012301滨州,");
            sb.Append("山东0101231601枣庄,");
            sb.Append("山东0101230903临沂,");
            sb.Append("山东0101230701莱芜,");
            sb.Append("山东0101231201泰安,");
            sb.Append("山东0101231101日照,");
            sb.Append("山东01012302德州,");
            sb.Append("山东0101231706淄博,");
            sb.Append("山东01012303东营,");
            sb.Append("山东0101230605济宁,");
            sb.Append("山西0101240103长治,");
            sb.Append("山西0101240301晋城,");
            sb.Append("山西0101240610文水,");
            sb.Append("山西0101241111运城,");
            sb.Append("山西01012410阳泉,");
            sb.Append("山西0101240704朔州,");
            sb.Append("山西0101240802太原,");
            sb.Append("山西0101240515临汾,");
            sb.Append("山西0101240913忻州,");
            sb.Append("山西0101240404平遥,");
            sb.Append("山西0101240201大同,");
            sb.Append("陕西0101250405商州,");
            sb.Append("陕西0101250501铜川,");
            sb.Append("陕西0101250810咸阳,");
            sb.Append("陕西0101251011榆林,");
            sb.Append("陕西0101250701西安,");
            sb.Append("陕西0101250902延安,");
            sb.Append("陕西0101250205宝鸡,");
            sb.Append("陕西0101250303汉中,");
            sb.Append("陕西0101250606渭南,");
            sb.Append("陕西0101250102安康,");
            sb.Append("上海0101260102上海,");
            sb.Append("四川0101270905乐山,");
            sb.Append("四川0101271505资中,");
            sb.Append("四川0101271701遂宁,");
            sb.Append("四川0101271902宜宾,");
            sb.Append("四川01012707广安,");
            sb.Append("四川0101271103泸州,");
            sb.Append("四川0101271013西昌,");
            sb.Append("四川0101272101自贡,");
            sb.Append("四川01012716攀枝花,");
            sb.Append("四川0101270304成都,");
            sb.Append("四川0101270105九寨沟,");
            sb.Append("四川0101271204彭山,");
            sb.Append("四川0101272004资阳,");
            sb.Append("四川0101270502德阳,");
            sb.Append("四川01012706甘孜,");
            sb.Append("四川0101270201巴中,");
            sb.Append("四川0101271303绵阳,");
            sb.Append("四川0101271401南充,");
            sb.Append("四川0101270401达州,");
            sb.Append("四川0101271808雅安,");
            sb.Append("四川0101270802广元,");
            sb.Append("台湾01012802高雄,");
            sb.Append("台湾01012801台北,");
            sb.Append("天津0101290106天津,");
            sb.Append("西藏0101300602日喀则,");
            sb.Append("西藏0101300403林芝,");
            sb.Append("西藏0101300105普兰,");
            sb.Append("西藏0101300203昌都,");
            sb.Append("西藏0101300506那曲,");
            sb.Append("西藏0101300301拉萨,");
            sb.Append("西藏0101300701隆子,");
            sb.Append("香港01013101香港,");
            sb.Append("新疆0101320901克拉玛依,");
            sb.Append("新疆0101320101阿克苏,");
            sb.Append("新疆0101320401博乐,");
            sb.Append("新疆0101321003阿图什,");
            sb.Append("新疆01013211塔城,");
            sb.Append("新疆01013205昌吉,");
            sb.Append("新疆01013212吐鲁番,");
            sb.Append("新疆0101320311巴音布鲁克,");
            sb.Append("新疆0101321408伊宁,");
            sb.Append("新疆0101321501石河子,");
            sb.Append("新疆0101321107和布克赛尔,");
            sb.Append("新疆0101320803喀什,");
            sb.Append("新疆0101321303乌鲁木齐,");
            sb.Append("新疆0101320602哈密,");
            sb.Append("新疆0101320201阿勒泰,");
            sb.Append("新疆0101320304库尔勒,");
            sb.Append("新疆01013207和田,");
            sb.Append("新疆0101320603伊吾,");
            sb.Append("云南01013306红河,");
            sb.Append("云南0101330801丽江,");
            sb.Append("云南0101330403潞西,");
            sb.Append("云南01013302楚雄,");
            sb.Append("云南0101330503中甸,");
            sb.Append("云南0101331401景洪,");
            sb.Append("云南01013312曲靖,");
            sb.Append("云南0101330103保山,");
            sb.Append("云南0101331503玉溪,");
            sb.Append("云南0101330705昆明,");
            sb.Append("云南01013303大理,");
            sb.Append("云南0101331610昭通,");
            sb.Append("云南0101331108普洱,");
            sb.Append("云南0101331306文山,");
            sb.Append("云南0101330904临沧,");
            sb.Append("云南0101331002贡山,");
            sb.Append("浙江0101340304嘉兴,");
            sb.Append("浙江0101340704衢州,");
            sb.Append("浙江0101340805绍兴,");
            sb.Append("浙江0101340901临海,");
            sb.Append("浙江0101341102定海,");
            sb.Append("浙江0101340604鄞县,");
            sb.Append("浙江0101340101杭州,");
            sb.Append("浙江0101340402金华,");
            sb.Append("浙江0101341004温州,");
            sb.Append("浙江0101340503丽水,");
            sb.Append("浙江0101340204湖州,");
#endregion
            sb.Append("重庆0101040106重庆");
            String[] citys = sb.ToString().Split(',');
            foreach (var q in citys)
            {
                list.Add(GetCityModel(q));
            }
            listBox1.Items.AddRange(citys);

        }
        private CityModel GetCityModel(string txtcode)
        {
            string patten = @"([\S\s]*?)(\d{8,10})([\S\s]*?)";
            Regex r = new Regex(patten);
            Match m = r.Match(txtcode);
            //return m.Result("$1");
            CityModel model = new CityModel();
            model.province = m.Result("$1");
            model.id = m.Result("$2");
            model.city = m.Result("$3");
            return model;
        }

    }

    //{"res_code":"0","res_message":"Success","access_token":"e2bf450638feb46f2b7010a28577922d1371786068514","expires_in":2592000}

//province	id	name

    public class CityModel
    {
        public string id { get; set; }
        public string city { get; set; }
        public string province { get; set; }
    }

    public class Access_Token
    {
        public string res_code { get; set; }
        public string res_message { get; set; }
        public string access_token { get; set; }
        public string expires_in { get; set; }
    }

    public class MyApp
    {
        public string api_id { get; set; }
        public string app_secret { get; set; }
        public string url1 { get; set; }
        public string url2 { get; set; }
    }
}
