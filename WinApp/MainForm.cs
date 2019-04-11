using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinApp.Components;
using WinApp.Core;
using WinApp.Core.Handlers;
using WinApp.ReqeustHandler;

namespace WinApp
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// APP配置文件
        /// </summary>
        private AppConfiguration configuration => AppConfiguration.Current;
        /// <summary>
        /// 页面处理程序
        /// </summary>
        public PageResolve Page { get; set; }
        /// <summary>
        /// 浏览器
        /// </summary>
        public ChromiumWebBrowser Browser { get; set; }
        public RequestEventHandler BrowserEventHandler { get; private set; }
        public List<string> AvaliableResources { get; private set; } = new List<string>();
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            progressBarInit();
            BrowserEventHandler = new RequestEventHandler();
            registerBrowserEvents();
            Browser = new ChromiumWebBrowser("about:blank");
            Browser.RequestHandler = BrowserEventHandler;
            Browser.Dock = DockStyle.Fill;

            PanelBrowser.Controls.Add(Browser);
            textBoxUrl.AutoSize = false;
            textBoxUrl.KeyPress += TextBoxUrl_KeyPress;

        }


        void progressBarInit()
        {
            progressBar.Minimum = 0;
            progressBar.Value = 0;
        }

        void progressBarStart(int maxValue)
        {
            progressBar.Visible = true;
            progressBar.Maximum = maxValue;
        }
        void progressBarUpdate(int value)
        {
            if (value < progressBar.Maximum)
                progressBar.Value = value;
            else {
                progressBar.Value = progressBar.Maximum;
                Thread.Sleep(200);
                progressBar.Visible = false;
                progressBarInit();
            }
        }

        private void TextBoxUrl_KeyPress(object sender, KeyPressEventArgs e)
        {
            var textbox = sender as TextBox;
            if (e.KeyChar == (char)Keys.Enter) {
                Go(textbox.Text);
            }
        }

        /// <summary>
        /// 跳转到某一个网页,并且生成该网页的资源与实体
        /// </summary>
        /// <param name="url"></param>
        private void Go(string url)
        {
            saveToolStripMenuItem.Enabled = false;
            shareToolStripMenuItem.Enabled = false;
            Page = new PageResolve();
            Browser.FrameLoadStart += (s, e) => {
                if (IsHandleCreated)
                    Invoke(new Action(() => {
                        progressBarStart(100);
                    }));
            };
            // 浏览器加载结束
            Browser.FrameLoadEnd += async (s, e) => {
                if (e.Frame.IsMain) {
                    Page.RawHtml = (await e.Frame.GetSourceAsync()).Replace("&amp;", "&");
                    Invoke(new Action(() => {
                        saveToolStripMenuItem.Enabled = true;
                        shareToolStripMenuItem.Enabled = true;
                    }));
                }
                if (IsHandleCreated)
                    Invoke(new Action(() => {
                        progressBarUpdate(100);
                    }));
            };
            if (url.StartsWith("http://") || url.StartsWith("https://"))
                Browser.Load(url);
            else {
                textBoxUrl.Text = $"http://{url}";
                Browser.Load(textBoxUrl.Text);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Go(textBoxUrl.Text);
        }

        /* *
         * 
         *  菜单功能
         * 
         * */
        /// <summary>
        /// Menu: New
        /// 重置当前浏览器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxUrl.Text = "about:blank";
            Go(textBoxUrl.Text);
            textBoxUrl.Focus();
            textBoxUrl.SelectAll();
        }
        /// <summary>
        /// 注册浏览器加载过程中的事件
        /// </summary>
        void registerBrowserEvents()
        {
            BrowserEventHandler.OnBeforeResourceLoadEvent += (s, e) => {
                //资源加载前
                // Debug.Print("Begin load resource: " + e.Request.Url);
                //可以添加过滤项来过滤掉不需要的Resource
                AvaliableResources.Add(e.Request.Url);
            };

            BrowserEventHandler.OnResourceLoadCompleteEvent += (s, e) => {
                //Content-Type filter
                List<string> ignoreContentType = new List<string>() {
                    "text/html"
                };

                //资源加载完成
                if (AvaliableResources.Contains(e.Request.Url) && e.Response.StatusCode < 300) {
                    var res = new Resource(e.Request.Url, null);
                    res.ContentType = e.Response.Headers["Content-Type"];
                    if (!ignoreContentType.Contains(res.ContentType))
                        Page.Resources.Add(res);
                    if (IsHandleCreated)
                        Invoke(new Action(async () => {
                            if (progressBar.Value + 1 < 99) {
                                Page.RawHtml = (await e.Frame.GetSourceAsync()).Replace("&amp;", "&");
                                progressBarUpdate(progressBar.Value + 1);
                            }
                        }));
                }
            };

        }
        /// <summary>
        /// Menu: Quit
        /// 退出程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Menu: Upload
        /// 上传网页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uploadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //var actionForm = new ActionBase();
            //var uploadHandler = new PageUploadHandler(actionForm.ProgressBar);
            //actionForm.Process(uploadHandler, () => { uploadHandler.Upload(Page.DownloadPath); });
        }

        /// <summary>
        /// 分享并上传网页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void shareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var shareConfigForm = new ShareConfigForm(Page)) {
                if (shareConfigForm.ShowDialog() == DialogResult.OK) {
                    Page.Share = shareConfigForm.Share;
                }
            }
            var actionForm = new ActionBase();
            var uploadHandler = new PageUploadHandler(actionForm.ProgressBar);
            actionForm.Process(uploadHandler, () => { uploadHandler.Upload(Page.DownloadPath); });
            var qrCode = Page.Share.ShareInWechat();
            var qrForm = new QrcodeDisplayForm(qrCode);
            qrForm.ShowDialog();
        }

        private void preferenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ConfigForm().ShowDialog();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var actionForm = new ActionBase();
            var downloadHandler = new PageDownloadHandler(actionForm.ProgressBar);
            actionForm.Process(downloadHandler, async () => { await downloadHandler.DownloadPageAsync(Page); });
            var rootPath = $"{configuration.AppSettings.SaveConfig.RootLocalDirectory}\\{Page.Id}";
            Page.DownloadPath = rootPath;
            Process.Start(rootPath);
        }

        private void manuallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("1. 输入网页地址, 按回车跳转或点击Go按钮.\r\n2. 点击\"File->Save\"将网页保存至本地\r\n3. 点击\"File->Share\"将下载的网页压缩打包,上传至服务器,生成微信分享二维码;服务器会自行解压生成网页.", "提示");
        }
    }
}
