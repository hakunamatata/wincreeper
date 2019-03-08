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
        private AppConfiguration configuration => AppConfiguration.Current;
        public PageResolve Page { get; set; }
        public ChromiumWebBrowser Browser { get; set; }
        public RequestEventHandler BrowserEventHandler { get; private set; }
        public List<string> AvaliableResources { get; private set; } = new List<string>();
        public MainForm()
        {
            InitializeComponent();
            //CheckForIllegalCrossThreadCalls = false;
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

            AppMenuBootstrap.Bootstrap(menuStrip);
            AppMenuBootstrap.PageSave.Click += PageSave_Click;
            AppMenuBootstrap.PageShare.Click += PageShare_Click;
            AppMenuBootstrap.SettingConfig.Click += SettingConfig_Click;
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

        #region ===== 菜单点击事件 =====
        /// <summary>
        /// 分享菜单事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PageShare_Click(object sender, EventArgs e)
        {
            var qrForm = new QrcodeDisplayForm(Utlis.GenerateQRCode(textBoxUrl.Text.Trim()));
            qrForm.ShowDialog();
        }

        /// <summary>
        /// 保存菜单点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PageSave_Click(object sender, EventArgs e)
        {
            var actionForm = new ActionBase();
            var downloadHandler = new PageDownloadHandler(actionForm.ProgressBar);
            actionForm.Process(downloadHandler, async () => { await downloadHandler.DownloadPageAsync(Page); });
            var rootPath = $"{configuration.AppSettings.SaveConfig.RootLocalDirectory}\\{Page.Id}";
            Process.Start(rootPath);
        }


        /// <summary>
        /// 配置菜单点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingConfig_Click(object sender, EventArgs e)
        {
            new ConfigForm().ShowDialog();
        }
        #endregion

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
                //资源加载完成
                if (AvaliableResources.Contains(e.Request.Url) && e.Response.StatusCode < 300) {
                    Page.Resources.Add(new Resource(e.Request.Url, null));
                    Invoke(new Action(() => {
                        if (progressBar.Value + 1 < 99)
                            progressBarUpdate(progressBar.Value + 1);
                    }));
                }
            };

        }

    }
}
