using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using mshtml;
using System.Collections;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            button1.Enabled = false;
            InitAllCityData();
        }

        bool islaodded = false;
        private void InitAllCityData()
        {
            webBrowser.ScriptErrorsSuppressed = true;
           
            this.webBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser_DocumentCompleted2);

            this.webBrowser.Navigate("http://house.focus.cn/housemarket/house_search/");

        }
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
        //对错误进行处理
        void Window_Error(object sender, HtmlElementErrorEventArgs e)
        {
            // 自己的处理代码
            Log.logger(e.Description);
            System.GC.Collect();
   
            e.Handled = true;
        }
        Thread workingThread;
        private void beginSearch()
        {
            try
            {
                //捕获控件的错误
                if (this.webBrowser.Document != null
                    && this.webBrowser.Document.Window != null &&
                    this.webBrowser.Document.Window != null
                    )
                {

                    this.webBrowser.Document.Window.Error += new HtmlElementErrorEventHandler(Window_Error);
                }
                this.webBrowser.DocumentCompleted -= new WebBrowserDocumentCompletedEventHandler(webBrowser_DocumentCompleted2);

                this.webBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser_DocumentCompleted);

                if (this.workingThread != null)
                {
                    this.workingThread.Abort();
                }
                this.workingThread = new Thread(new ThreadStart(working_thread_method));
                this.workingThread.IsBackground = true;
                stopWorkingThread = false;
                this.workingThread.Start();
            }
            catch (Exception ex)
            {
                Log.logger("beginSearch" + ex.ToString());
            }
        }

        int AllCItyCount = 1;
        List<House> CItyhouseList = new List<House>();
        protected virtual void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //加载成功开始分析。 
            if (e.Url.ToString() == "about:blank") { return; }
            ShowClickMsg("加载成功开始分析" + e.Url.ToString());
            Log.logger("加载成功开始分析" + e.Url.ToString());
      
            IHTMLDocument2 doc = (IHTMLDocument2)webBrowser.Document.DomDocument;
            int CountLoupan = 0;
                foreach (mshtml.IHTMLElement a in doc.links)
                {
                    if (a.parentElement != null && 
                   a.parentElement.parentElement != null &&
                       a.parentElement.parentElement.className != null &&
                   a.parentElement.parentElement.className== "t"
                   &&
                   a.parentElement.parentElement.parentElement != null &&
                         a.parentElement.parentElement.parentElement.className != null &&
                               a.parentElement.parentElement.parentElement.className == "h3 clear" &&
                      a.parentElement.parentElement.parentElement.parentElement != null &&
                        a.parentElement.parentElement.parentElement.parentElement.className != null &&
                   a.parentElement.parentElement.parentElement.parentElement.className == "rinfo"  
                   )
                    {
                        CountLoupan++;
                        if (a.innerText == null) { continue; }
                   
                        House hous = new House();
                        string price = a.parentElement.parentElement.parentElement.children[1].innerText;
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
                            if (price.IndexOf("元/平米")>=0){
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
                        hous.Name=a.innerText;

                        HTMLDivElement isnew = (HTMLDivElement) a.parentElement.parentElement.children[1];

                        hous.HouseType = isnew.innerText;
                        // 均价:12,800元/平米
                        hous.urll = a.getAttribute("href");
                     
                        HTMLDivElement divadd=(HTMLDivElement)a.parentElement.parentElement.parentElement.parentElement;
                        HTMLDivElement wuye = (HTMLDivElement)divadd.children[1];
                        string wuyename = wuye.children[0].innerText;//物业类型：普通住宅 别墅 花园洋房 

                        string phone = wuye.children[1].innerText;//电话：400-888-2200 转 13923

                        HTMLDivElement kaifangshang = (HTMLDivElement)divadd.children[2];
                        string address = kaifangshang.innerText;
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
                        string[] kaifang_address = address.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
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
                        ShowClickMsg(i+"_"+"楼盘:" + hous.Name +";"+ hous.Price+" "+hous.Danwei +";"+hous.wuyeleixing+";"+hous.phones+";"+hous.Kaifangshang);
                        House house = CItyhouseList.SingleOrDefault(item => item.urll == hous.urll);
                        if (house == null)
                        {
                            hous.ID = AllCItyCount;
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
                IEUtil.CleanCookie();
                IEUtil.CleanTempFiles();
                System.GC.Collect();
                if (i < int.Parse(page) && CountLoupan > 0)
                {
                    this.webBrowser.Navigate(urll);
                }
                else   if (CountLoupan == 0) //没有查找到新页面了。就可以查找二级页面了。
                {
                    this.webBrowser.DocumentCompleted -= new WebBrowserDocumentCompletedEventHandler(webBrowser_DocumentCompleted);
                    this.webBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser_DocumentCompleted3);
                    GetOneUrlandnagative();
                    
                }
          
        }

        private void GetOneUrlandnagative()
        {
            try
            {
                //保存文件
                string name = ListCItyUrl.SingleOrDefault(a => a.Value == GetUrl()).Key;
                IEUtil.CleanTempFiles();
                List<House> housList = CItyhouseList.Where(a => a.IsSearch == false).OrderBy(a => a.ID).ToList();
                ShowLoadddingMsg(housList.Count + "/" + CItyhouseList.Count);
                if (housList.Count > 0)
                {
                    House hous = housList[0];
                    if (hous.urll.Length > 0)
                    {
                        this.webBrowser.Navigate(hous.urll);
                        hous.IsSearch = true;
                    }
                } 
            }
            catch (Exception ex)
            {
                Log.logger(ex.Message + ex.StackTrace);
            }
        }
        protected virtual void webBrowser_DocumentCompleted3(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //加载成功开始分析。
              if (e.Url.AbsolutePath != (sender as WebBrowser).Url.AbsolutePath)
    return; 
            if ( webBrowser.ReadyState != WebBrowserReadyState.Complete)
    return;
            if (e.Url.ToString() == "about:blank") {  return;  }
            IEUtil.CleanCookie();
            IEUtil.CleanTempFiles();
            System.GC.Collect();
            try
            {
                ShowClickMsg("开始加载每个楼盘的数据 " + (sender as WebBrowser).Url.AbsolutePath); 
                IHTMLDocument2 doc = (IHTMLDocument2)webBrowser.Document.DomDocument; 
                foreach (mshtml.IHTMLElement a in doc.links)
                {

                    if (
                        a.parentElement != null &&
                        a.parentElement.tagName != null &&
                        a.parentElement.tagName.ToLower() == "li" &&
                            a.parentElement.parentElement != null &&
                        a.parentElement.parentElement.tagName != null &&
                        a.parentElement.parentElement.tagName.ToLower() == "ul" &&
                        a.parentElement.parentElement.parentElement != null &&
                        a.parentElement.parentElement.parentElement.className != null &&
                        a.parentElement.parentElement.parentElement.className == "blockLA" &&
                         a.parentElement.parentElement.parentElement.parentElement != null &&
                        a.parentElement.parentElement.parentElement.parentElement.className != null &&
                        a.parentElement.parentElement.parentElement.parentElement.className == "l"
                        &&
                         a.parentElement.parentElement.parentElement.parentElement.parentElement != null &&
                        a.parentElement.parentElement.parentElement.parentElement.parentElement.className != null &&
                        a.parentElement.parentElement.parentElement.parentElement.parentElement.className == "area"
                        &&
                        a.parentElement.parentElement.parentElement.parentElement.parentElement.id == "contentB"
                   )
                    {
                        House house = CItyhouseList.Where(item => item.urll == (sender as WebBrowser).Url.AbsoluteUri.ToString()).ToList()[0];
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
                            HTMLDivElement divinfoo = (HTMLDivElement)a.parentElement.parentElement.parentElement.parentElement.parentElement;

                            HTMLDivElement divclass111 = (HTMLDivElement)divinfoo.children[0];
                            HTMLDivElement divblockaaa = (HTMLDivElement)divclass111.children[0];
                            HTMLUListElement ulll1 = (HTMLUListElement)divblockaaa.children[0];
                            house.updateTime = ((HTMLLIElement)ulll1.children[0]).children[1].innerText;
                            house.AlongArea = ((HTMLLIElement)ulll1.children[2]).innerText.Replace("所属片区：", "");

                            HTMLDivElement divblockBBBB = (HTMLDivElement)divclass111.children[1];
                            HTMLUListElement ulllBBBB = (HTMLUListElement)divblockBBBB.children[0];
                            house.Jiaotong = ((HTMLLIElement)ulllBBBB.children[0]).innerText.Replace("周围交通：", "");
                            house.opentime = ((HTMLLIElement)ulllBBBB.children[1]).innerText.Replace("开盘时间：", "");
                            house.LiveIntime = ((HTMLLIElement)ulllBBBB.children[2]).innerText.Replace("入住时间：", "");
                        }
                        string name = ListCItyUrl.SingleOrDefault(item => item.Value == GetUrl()).Key;
                        Log.Datalogger(house.ID + "," + house.Name.Replace(",", "、") + "," + house.HouseType.Replace(",", "、") + "," + house.Price + "," + house.Danwei + "," + house.phones.Replace(",", "、") + "," + house.AlongArea.Replace(",", "、") + "," +
                            house.address.Replace(",", "、").Replace("查看地图", "") + "," + house.Kaifangshang.Replace(",", "、") + "," + house.Jiaotong.Replace(",", "、") + "," +
                            house.opentime.Replace(",", "、") + "," + house.LiveIntime.Replace(",", "、") + "," + house.updateTime.Replace(",", "、") + "," + house.wuyeleixing.Replace(",", "、"), name);
                        break;
                    }
                }
                GetOneUrlandnagative();
            }
            catch (Exception ex)
            {
                Log.logger(ex.Message+ex.StackTrace);
                GetOneUrlandnagative();
            }
        } 

        /// <summary>
        /// 全国城市列表
        /// </summary>
        Dictionary<string, string> ListCItyUrl = new Dictionary<string, string>();
        protected virtual void webBrowser_DocumentCompleted2(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //加载成功开始分析。

            if (e.Url.ToString() == "about:blank") { return; }
            ShowClickMsg("开始加载全国的城市数据" + e.Url.ToString());
            Log.logger("开始加载全国的城市数据" + e.Url.ToString());
            IHTMLDocument2 doc = (IHTMLDocument2)webBrowser.Document.DomDocument;
            ArrayList advertiseArray = new ArrayList();
            foreach (mshtml.IHTMLElement a in doc.links)
            {
                if (a.parentElement != null &&
                    a.parentElement.parentElement != null &&
                    a.parentElement.parentElement.className != null &&
                    a.parentElement.parentElement.className == "cBox" &&
                    a.parentElement.parentElement.parentElement != null &&
                    a.parentElement.parentElement.parentElement.className != null &&
                    a.parentElement.parentElement.parentElement.className == "s_Box_1 hidden" 
               )
                {
                    if (!ListCItyUrl.ContainsKey(a.innerText.Trim()))
                    {
                        ListCItyUrl.Add(a.innerText.Trim(), a.getAttribute("href"));
                    }
                }
            }
            //绑定到下拉框中啊
            IEUtil.CleanCookie();
            comboBox1.DataSource = ListCItyUrl.Keys.ToList();
            button1.Enabled = true;
        } 
        int count = 1;
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
   public delegate string  GetPageDelegate();
   public string GetPage()
        {
            if (this.clickText.InvokeRequired)
            {
                object[] parameters = new object[] {   };
                this.textBox1.Invoke(new GetPageDelegate(GetPage), parameters);
                return this.txtpage.Text;
            }
            else
            {
                return this.txtpage.Text;
            }
        }

        public delegate string  GetUrlDelegate();
        public string GetUrl()
        {
            if (this.clickText.InvokeRequired)
            {
                object[] parameters = new object[] {   };
                this.textBox1.Invoke(new GetUrlDelegate(GetUrl), parameters);
                return this.textBox1.Text;
            }
            else
            {
                return this.textBox1.Text;
            }
        }

        int i = 1;
        bool stopWorkingThread = true;
        void working_thread_method()
        {
            try
            {
                if (stopWorkingThread) return; 
                //Clear sort index number 
                ShowClickMsg("查询");
                i = 1;
                //http://www.soufun.com/house/%B1%B1%BE%A9_________________1_.htm
                //http://sh.focus.cn/housemarket/house_search/index.php?page=2
                string urll = GetUrl() + "index.php?page=" + i;// "http://sh.focus.cn/housemarket/house_search/index.php?page=" + i; 
                  //  System.Threading.Thread.Sleep(20000);
                this.webBrowser.Navigate(urll); 
            }
            catch (Exception ex)
            {
                Log.logger("working_thread_method:" + ex.Message.ToString() + ex.StackTrace);
                ShowClickMsg("working_thread_method:" + ex.Message.ToString() + ex.StackTrace);
            }
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
