using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.Text = "Forthxu\'s Browser";

            webBrowser1.ScriptErrorsSuppressed = true; //禁用错误脚本提示
            webBrowser1.IsWebBrowserContextMenuEnabled = false; //禁用右键菜单
            webBrowser1.WebBrowserShortcutsEnabled = false; //禁用快捷键
            //WindowState = FormWindowState.Maximized;//最大化
            // MessageBox.Show("sad");

            this.gotourl();
        }

        private int gotourl(string url="")
        {
            if(url=="")
            {
                url = textBox1.Text.Trim();
            }
            
            //正则判断
            string reg = @"^((http|ftp|https)://)?(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,4})*(/[a-zA-Z0-9\&%_\./-~-]*)?";
            Regex r = new Regex(reg);
            //给网址去所有空格
            Match m = r.Match(url);

            //判断是否带http://
            if (!m.Success)
                url="http://baidu.com";
            //给不带http://开头的加上 
            url = url.Replace("http://", "");
            url = url.Insert(0, "http://");

            webBrowser1.Url = new Uri(url);
            WaitWebPageLoad();
            this.textBox1.Text = this.webBrowser1.Url.ToString();
            this.Text = this.webBrowser1.Document.Title + "-Forthxu\'s Browser";

            return 1;
        }

        //窗口resize之前的大小
        private Size beforeResizeSize = Size.Empty;
        //改变窗口大小之前自发的事件
        protected override void OnResizeBegin(EventArgs e)
        {
            base.OnResizeBegin(e);
            beforeResizeSize = this.Size;
        }
        //改变窗口大小之后自发的事件
        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);
            //窗口resize之后的大小
            Size endResizeSize = this.Size;
            //获得变化比例
            float percentWidth = (float)endResizeSize.Width / beforeResizeSize.Width;
            float percentHeight = (float)endResizeSize.Height / beforeResizeSize.Height;
            //按比例改变控件大小
            this.webBrowser1.Width = (int)(webBrowser1.Width * percentWidth);
            this.webBrowser1.Height = (int)(webBrowser1.Height * percentHeight);
            //为了不使控件之间覆盖 位置也要按比例变化
            this.webBrowser1.Left = (int)(webBrowser1.Left * percentWidth);
            this.webBrowser1.Top = (int)(webBrowser1.Top * percentHeight);
        }

        //回车
        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.gotourl();
            }
        }

        //go
        private void button1_Click(object sender, EventArgs e)
        {
            this.gotourl();
        }

        //home
        private void button2_Click(object sender, EventArgs e)
        {
            this.gotourl("http://baidu.com");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.gotourl("http://skynetdoc.com");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.gotourl("http://forthxu.com");
        }

        //back
        private void button3_Click(object sender, EventArgs e)
        {
            this.webBrowser1.GoBack();
        }
        //prev
        private void button4_Click(object sender, EventArgs e)
        {
            this.webBrowser1.GoForward();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.gotourl("http://4399.com");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.gotourl("http://dreamlist.cc");
        }

        
        private void Delay(int Millisecond) //延迟系统时间，但系统又能同时能执行其它任务；
        {
            DateTime current = DateTime.Now;
            while (current.AddMilliseconds(Millisecond) > DateTime.Now)
            {
                Application.DoEvents();//转让控制权            
            }
            return;
        }
        private bool WaitWebPageLoad()
        {
            int i = 0;
            string sUrl;
            while (true)
            {
                Delay(50);  //系统延迟50毫秒，够少了吧！             
                if (webBrowser1.ReadyState == WebBrowserReadyState.Complete) //先判断是否发生完成事件。
                {
                    if (!webBrowser1.IsBusy) //再判断是浏览器是否繁忙                  
                    {
                        i = i + 1;
                        if (i == 2)
                        {
                            sUrl = webBrowser1.Url.ToString();
                            if (sUrl.Contains("res")) //这是判断没有网络的情况下                           
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                        continue;
                    }
                    i = 0;
                }
            }
        }

        //title change
        private void webBrowser1_DocumentTitleChanged(object sender, EventArgs e)
        {
            this.Text = webBrowser1.DocumentTitle + "-Forthxu\'s Browser";
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //将所有的链接的目标，指向本窗体
            foreach (HtmlElement archor in this.webBrowser1.Document.Links)
            {
                archor.SetAttribute("target", "_self");
            }

            //将所有的FORM的提交目标，指向本窗体
            foreach (HtmlElement form in this.webBrowser1.Document.Forms)
            {
                form.SetAttribute("target", "_self");
            }
        }

        private void webBrowser1_NewWindow(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.webBrowser1.Refresh();
        }


    }
}
