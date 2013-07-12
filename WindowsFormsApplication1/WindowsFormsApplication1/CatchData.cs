using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Skybound.Gecko;
using System.Threading;
using System.Text.RegularExpressions;

namespace WindowsFormsApplication1
{
    public partial class CatchData : Form
    {
        //     string LogPath = Application.StartupPath + "\\log";
        static private string xulrunnerPath =Application.StartupPath + "\\xulrunner";
        public CatchData()
        {
      
            InitializeComponent();
            InitAllCityData();
        }

        private void InitAllCityData()
        {
            Skybound.Gecko.Xpcom.Initialize(xulrunnerPath); 
            Browser.DocumentCompleted += new EventHandler(Browser_DocumentCompleted_Init); 
            GeckoPreferences.User["gfx.font_rendering.graphite.enabled"] = true; 
        }
        void Browser_DocumentCompleted_Init(object sender, EventArgs e)
        {
            GeckoWebBrowser br = sender as GeckoWebBrowser;
            if (br.Url.ToString() == "about:blank") { return; }
            ShowClickMsg("开始加载全国的城市数据" + br.Url.ToString());
            Log.logger("开始加载全国的城市数据" + br.Url.ToString());


            foreach (Skybound.Gecko.GeckoElement a in br.Document.Links)
            {
                if (a.Parent != null &&
                    a.Parent.Parent != null &&
                    a.Parent.Parent.ClassName != null &&
                    a.Parent.Parent.ClassName == "cBox" &&
                    a.Parent.Parent.ClassName != null &&
                    a.Parent.Parent.Parent.ClassName != null &&
                    a.Parent.Parent.Parent.ClassName == "s_Box_1 hidden"
               )
                {
             
                    if (!ListCItyUrl.ContainsKey(a.TextContent.Trim()))
                    {
                        ShowClickMsg(a.TextContent.Trim() + a.GetAttribute("href"));
                        ListCItyUrl.Add(a.TextContent.Trim(), a.GetAttribute("href"));
                    }
                }
            }
            //绑定到下拉框中啊
            IEUtil.CleanCookie();
            comboBox1.DataSource = ListCItyUrl.Keys.ToList();
            button1.Enabled = true;
        }
        void Browser_DocumentCompleted_xiangxi(object sender, EventArgs e)
        {
            GeckoWebBrowser br = sender as GeckoWebBrowser;
            if (br.Url.ToString() == "about:blank") { GetOneUrlandnagative(); return; } 
            System.GC.Collect();
            try
            {
                ShowClickMsg("开始加载每个楼盘的数据 " + br.Url.ToString());

                foreach (Skybound.Gecko.GeckoElement  a in br.Document.Links)
                {

                    if (
                        a.Parent != null &&
                        a.Parent.TagName != null &&
                        a.Parent.TagName.ToLower() == "li" &&
                            a.Parent.Parent != null &&
                        a.Parent.Parent.TagName != null &&
                        a.Parent.Parent.TagName.ToLower() == "ul" &&
                        a.Parent.Parent.Parent != null &&
                        a.Parent.Parent.Parent.ClassName != null &&
                        a.Parent.Parent.Parent.ClassName == "blockLA" &&
                         a.Parent.Parent.Parent.Parent != null &&
                        a.Parent.Parent.Parent.Parent.ClassName != null &&
                        a.Parent.Parent.Parent.Parent.ClassName == "l"
                        &&
                         a.Parent.Parent.Parent.Parent.Parent != null &&
                        a.Parent.Parent.Parent.Parent.Parent.ClassName != null &&
                        a.Parent.Parent.Parent.Parent.Parent.ClassName == "area"
                        &&
                        a.Parent.Parent.Parent.Parent.Parent.Id == "contentB"
                   )
                    {
                        House house = CItyhouseList.Where(item => item.urll == br.Url.ToString()) .ToList()[0];
                        if (house != null)
                        {
                            house.IsSearch = true;
                        }
                        else
                        {
                            GetOneUrlandnagative(); return;
                        }
                        if (house != null)
                        {
                            Skybound.Gecko.GeckoElement divinfoo = (Skybound.Gecko.GeckoElement)a.Parent.Parent.Parent.Parent.Parent;

                            Skybound.Gecko.GeckoNode divclass111 = (Skybound.Gecko.GeckoNode)divinfoo.ChildNodes[1];
                            Skybound.Gecko.GeckoNode divblockaaa = (Skybound.Gecko.GeckoNode)divclass111.ChildNodes[1];
                            Skybound.Gecko.GeckoNode ulll1 = (Skybound.Gecko.GeckoNode)divblockaaa.ChildNodes[1];
                            house.updateTime = ((Skybound.Gecko.GeckoNode)ulll1.ChildNodes[1]).ChildNodes[1].TextContent;
                            house.AlongArea = ((Skybound.Gecko.GeckoNode)ulll1.ChildNodes[5]).TextContent.Replace("所属片区：", "");

                            Skybound.Gecko.GeckoNode divblockBBBB = (Skybound.Gecko.GeckoNode)divclass111.ChildNodes[3];
                            Skybound.Gecko.GeckoNode ulllBBBB = (Skybound.Gecko.GeckoNode)divblockBBBB.ChildNodes[1];
                            house.Jiaotong = ((Skybound.Gecko.GeckoNode)ulllBBBB.ChildNodes[1]).TextContent.Replace("周围交通：", "");
                            house.opentime = ((Skybound.Gecko.GeckoNode)ulllBBBB.ChildNodes[3]).TextContent.Replace("开盘时间：", "");
                            house.LiveIntime = ((Skybound.Gecko.GeckoNode)ulllBBBB.ChildNodes[5]).TextContent.Replace("入住时间：", "");
                        }

                      

                        string name = ListCItyUrl.SingleOrDefault(item => item.Value == GetUrl()).Key;
                        Log.Datalogger(house.ID + "," +GetRightString( house.Name.Replace(",", "、") )+ "," +GetRightString(house.HouseType.Replace(",", "、")) + "," + house.Price + "," + GetRightString(house.Danwei )+ "," + GetRightString(house.phones.Replace(",", "、") )+ "," +GetRightString( house.AlongArea.Replace(",", "、"))+ "," +
                            house.address.Replace(",", "、").Replace("查看地图", "") + "," + GetRightString(house.Kaifangshang.Replace(",", "、"))+ "," + GetRightString(house.Jiaotong.Replace(",", "、")) + "," +
                            GetRightString(house.opentime.Replace(",", "、")) + "," +GetRightString( house.LiveIntime.Replace(",", "、") )+ "," + GetRightString(house.updateTime.Replace(",", "、")) + "," +GetRightString( house.wuyeleixing.Replace(",", "、")), name);
                        break;
                    }
                }
                GetOneUrlandnagative();
            }
            catch (Exception ex)
            {
                Log.logger(ex.Message + ex.StackTrace);
                GetOneUrlandnagative();
            }
        }

        private string GetRightString(string text)
        {
            if (text == null) { return ""; }
            string trim = text.Replace(" ", "");
            trim = trim.Replace("\r", "");
            trim = trim.Replace("\n", "");
            trim = trim.Replace("\t", "");
            return trim;
        }
        void Browser_DocumentCompleted(object sender, EventArgs e)
        {
            GeckoWebBrowser br = sender as GeckoWebBrowser; 
            //加载成功开始分析。 
            if (br.Url.ToString() == "about:blank") { return; }
            ShowClickMsg("加载成功开始分析" + br.Url.ToString());
            Log.logger("加载成功开始分析" + br.Url.ToString());
 
            int CountLoupan = 0;
            foreach (Skybound.Gecko.GeckoElement a in br.Document.Links)
            {
                if (a.Parent != null &&
               a.Parent.Parent != null &&
                   a.Parent.Parent.ClassName != null &&
               a.Parent.Parent.ClassName == "t"
               &&
               a.Parent.Parent.Parent != null &&
                     a.Parent.Parent.Parent.ClassName != null &&
                           a.Parent.Parent.Parent.ClassName == "h3 clear" &&
                  a.Parent.Parent.Parent.Parent != null &&
                    a.Parent.Parent.Parent.Parent.ClassName != null &&
               a.Parent.Parent.Parent.Parent.ClassName == "rinfo"
               )
                {
                    CountLoupan++;
                    if (a.TextContent == null) { continue; }

                    House hous = new House();
                    string price = a.Parent.Parent.Parent.ChildNodes[3].TextContent.Replace("\n\n\t\t\t    \n\t\t\t    ","");
                    //		price.Replace("元/平米", "").Replace("均价:", "").Replace(",", "").Trim()	"起价:75万元/套"	string
                    if (price == "        ")
                    {
                        hous.Price = 0;
                    }
                    else
                    {
                        //最高价:8,016万元/套
                        int priceint = 0;
                        string trimprice = price.Replace(",", "");
                        if (price.IndexOf("元/平米") >= 0)
                        {
                            trimprice = price.Replace("元/平米", "").Replace("均价:", "").Replace(",", "").Replace("起价:", "").Replace("二手房参考价:", "");
                            int.TryParse(trimprice, out priceint);
                            hous.Price = priceint;
                            hous.Danwei = "元/平米";
                        }
                        if (price.IndexOf("万元/套") >= 0)
                        {
                            trimprice = price.Replace("万元/套", "").Replace("最高价:", "").Replace(",", "").Replace("起价:", "");
                            int.TryParse(trimprice, out priceint);
                            hous.Price = priceint;
                            hous.Danwei = "万元/套";
                        }
                    }

                    hous.Name = a.Parent.Parent.Parent.ChildNodes[1].ChildNodes[0].TextContent;
                   
                    Skybound.Gecko.GeckoElement isnew = (Skybound.Gecko.GeckoElement)a.Parent.Parent.ChildNodes[1];

                    hous.HouseType = isnew.TextContent;
           

                    string[] sArray = Regex.Split(GetUrl(), "housemarket", RegexOptions.IgnoreCase);

                    // 均价:12,800元/平米
                    hous.urll =sArray[0]+ a.GetAttribute("href").TrimStart('/');
                     
                    Skybound.Gecko.GeckoElement divadd = (Skybound.Gecko.GeckoElement)a.Parent.Parent.Parent.Parent;
                    Skybound.Gecko.GeckoElement wuye = (Skybound.Gecko.GeckoElement)divadd.ChildNodes[3];
                    string wuyename = wuye.ChildNodes[1].TextContent;//物业类型：普通住宅 别墅 花园洋房 

                    string phone = wuye.ChildNodes[3].TextContent.Replace("\n\t\t\t\t", "");//电话：400-888-2200 转 13923

                    Skybound.Gecko.GeckoNode kaifangshang = (Skybound.Gecko.GeckoNode)divadd.ChildNodes[5];
                    string address = kaifangshang.TextContent;
                    if (wuyename != null)
                    {
                        hous.wuyeleixing = wuyename.Replace("物业类型：", "");
                    }
                    else
                    {
                        hous.wuyeleixing = "无";
                    }
                    if (phone != null)
                    {
                        hous.phones = phone.Replace("电话：", "");
                    }
                    else
                    {
                        hous.phones = "无";
                    }
                    string[] kaifang_address = address.Split(new string[] { "查看地图" }, StringSplitOptions.RemoveEmptyEntries);
                    if (kaifang_address.Length >= 2)
                    {
                        hous.Kaifangshang = kaifang_address[1].Replace("开 发 商：", "");
                        hous.address = kaifang_address[0].Replace("物业地址：", "");
                    }
                    else
                    {
                        hous.Kaifangshang = address;//物业地址：大兴京开高速庞各庄出口南1000米查看地图\r\n开 发 商：北京富源盛达房地产开发有限公司
                        hous.address = "无法识别地址";
                    }
                    if (hous.Danwei == null)
                    {
                        hous.Danwei = "";
                    }
                    if (hous.wuyeleixing == null)
                    {
                        hous.wuyeleixing = "";
                    }
                    if (hous.phones == null)
                    {
                        hous.phones = "";
                    }
                    if (hous.Kaifangshang == null)
                    {
                        hous.Kaifangshang = "";
                    }
               
                    ShowClickMsg(i + "_" + "楼盘:" + hous.Name.Trim() + ";" + hous.Price + " " + hous.Danwei.Trim() + ";" + hous.wuyeleixing.Trim() + ";" + hous.phones.Trim() + ";" + hous.Kaifangshang.Trim());
                    House house = CItyhouseList.SingleOrDefault(item => item.urll == hous.urll);
                    if (house == null)
                    {
                        hous.ID = AllCItyCount;
                        ShowLoadddingMsg(AllCItyCount.ToString());
                        CItyhouseList.Add(hous);
                        AllCItyCount++;
                    } 
                }
            }
            i++;
            count++;
            string urll = GetUrl() + "index.php?page=" + i;// "http://sh.focus.cn/housemarket/house_search/index.php?page=" + i; 
            ShowClickMsg(urll);
            string page = GetPage();
            if (page == "")
            {
                page = "300";
            }
        
            System.GC.Collect();
            if (i < int.Parse(page) && CountLoupan > 0)
            {
                this.Browser.Navigate(urll);
            }
            else // if (CountLoupan == 0) //没有查找到新页面了。就可以查找二级页面了。
            {
                this.Browser.DocumentCompleted -= new  EventHandler(Browser_DocumentCompleted);
                this.Browser.DocumentCompleted += new EventHandler(Browser_DocumentCompleted_xiangxi);
                GetOneUrlandnagative();

            }


        }
        int count = 1;
        private void GetOneUrlandnagative()
        {
            try
            {
                //保存文件
                string name = ListCItyUrl.SingleOrDefault(a => a.Value == GetUrl()).Key; 
                List<House> housList = CItyhouseList.Where(a => a.IsSearch == false).OrderBy(a => a.ID).ToList();
                ShowLoadddingMsg(housList.Count + "/" + CItyhouseList.Count);
                if (housList.Count > 0)
                {
                    House hous = housList[0];
                    if (hous.urll.Length > 0)
                    {
                        this.Browser.Navigate(hous.urll);
                        hous.IsSearch = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.logger(ex.Message + ex.StackTrace);
            }
        }
        public delegate void LogClickMsgDelegate(string text);
        public void ShowClickMsg(string text)
        {
            if (this.clickText.InvokeRequired)
            {
                object[] parameters = new object[] { text };
                this.clickText.Invoke(new LogClickMsgDelegate(ShowClickMsg), parameters);
            }
            else
            {

                this.clickText.Items.Add(text);

            }
        }

        /// <summary>
        /// 加载当前正在运行的业务
        /// </summary>
        /// <param name="SchemeID"></param>
        public void ShowLoadddingMsg(string msg)
        {

            if (this.InvokeRequired)
            {
                Action<string> action = this.ShowLoadddingMsg;
                this.BeginInvoke(action, msg);
            }
            else
            {
                this.txtloadding.Text = msg;
            }
        }
        public delegate string GetPageDelegate();
        public string GetPage()
        {
            if (this.clickText.InvokeRequired)
            {
                object[] parameters = new object[] { };
                this.textBox1.Invoke(new GetPageDelegate(GetPage), parameters);
                return this.txtpage.Text;
            }
            else
            {
                return this.txtpage.Text;
            }
        }

        public delegate string GetUrlDelegate();
        public string GetUrl()
        {
            if (this.clickText.InvokeRequired)
            {
                object[] parameters = new object[] { };
                this.textBox1.Invoke(new GetUrlDelegate(GetUrl), parameters);
                return this.textBox1.Text;
            }
            else
            {
                return this.textBox1.Text;
            }
        }
        int AllCItyCount = 1;
        List<House> CItyhouseList = new List<House>();
        Dictionary<string, string> ListCItyUrl = new Dictionary<string, string>();
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string name = ListCItyUrl.SingleOrDefault(a => a.Value == GetUrl()).Key;
                Log.FileDelete(name);
                Log.Datalogger("" + "," + "楼盘" + "," + "开盘" + "," + "价格" + "," + "单位" + "," + "电话" + "," + "片区" + "," +
                 "地址" + "," + "开发商" + "," + "交通情况" + "," +
                 "开盘时间" + "," + "入住时间" + "," + "更新时间" + "," + "物业情况", name);
                CItyhouseList.Clear();
                beginSearch();
            }
            catch (Exception ex)
            {
                Log.logger(ex.ToString());
                MessageBox.Show("button1_Click" + ex.ToString());
            }
        }
        Thread workingThread;
        private void beginSearch()
        {
            try
            { 
                if (this.workingThread != null)
                {
                    this.workingThread.Abort();
                }
                this.Browser.DocumentCompleted -= new EventHandler(Browser_DocumentCompleted_Init);
                this.Browser.DocumentCompleted +=new EventHandler(Browser_DocumentCompleted);
                //this.workingThread = new Thread(new ThreadStart(working_thread_method));
                //this.workingThread.IsBackground = true;
             
                //this.workingThread.Start();
                try
                {
                    ShowClickMsg("查询");
                    i = 1;
                    string urll = GetUrl() + "index.php?page=" + i;// "http://sh.focus.cn/housemarket/house_search/index.php?page=" + i; 
                    //  System.Threading.Thread.Sleep(20000);
                    this.Browser.Navigate(urll);
                }
                catch (Exception ex)
                {
                    Log.logger("working_thread_method:" + ex.Message.ToString() + ex.StackTrace);
                    ShowClickMsg("working_thread_method:" + ex.Message.ToString() + ex.StackTrace);
                }

            }
            catch (Exception ex)
            {
                Log.logger("beginSearch" + ex.ToString());
            }
        }
        int i = 1;
        void working_thread_method()
        {
            try
            { 
                ShowClickMsg("查询");
                i = 1; 
                string urll = GetUrl() + "index.php?page=" + i;// "http://sh.focus.cn/housemarket/house_search/index.php?page=" + i; 
                //  System.Threading.Thread.Sleep(20000);
                this.Browser.Navigate(urll);
            }
            catch (Exception ex)
            {
                Log.logger("working_thread_method:" + ex.Message.ToString() + ex.StackTrace);
                ShowClickMsg("working_thread_method:" + ex.Message.ToString() + ex.StackTrace);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Browser.Navigate("http://house.focus.cn/housemarket/house_search/");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ListCItyUrl.ContainsKey(comboBox1.SelectedItem.ToString().Trim()))
            {
                this.textBox1.Text = ListCItyUrl[comboBox1.SelectedItem.ToString().Trim()];
            }
        }
    }
}
