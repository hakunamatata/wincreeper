using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinApp.Core;
using WinApp.Core.ShareModule;

namespace WinApp.Components
{
    public partial class ShareConfigForm : Form
    {
        public PageResolve Page { get; private set; }
        public Share Share { get; set; }
        public ShareConfigForm()
        {
            InitializeComponent();
            Share = new Share();
        }

        public ShareConfigForm(PageResolve page) : this()
        {
            Page = page;
            Share.Id = Page.Id;
        }

        private void textBoxTitle_TextChanged(object sender, EventArgs e)
        {
            var box = sender as TextBox;
            Share.Title = box.Text.Trim();
        }

        private void textBoxDescription_TextChanged(object sender, EventArgs e)
        {
            var box = sender as TextBox;
            Share.Description = box.Text.Replace("\r", " ").Replace("\n", " ");
        }

        private void ShareConfigForm_Load(object sender, EventArgs e)
        {
            textBoxTitle.Text = Share.Title;
            textBoxDescription.Text = Share.Description;
            pictureBox1.Image = Share.Image;
        }

        private void lbBrowse_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (var dialog = new OpenFileDialog()) {
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                dialog.Filter = "(Image File)|*.jpg;*.png;*.jpeg";
                if (dialog.ShowDialog() == DialogResult.OK) {
                    var img = Image.FromFile(dialog.FileName);
                    Share.Image = img;
                    pictureBox1.Image = img;
                }
            }
        }

        private void lbGallary_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var avaliablePictureTypes = new List<string>() { ".jpg", ".png", ".jpeg" };
            if (Page?.Resources?.Count > 0 && Page.Resources.Any(p => avaliablePictureTypes.Contains(p.Extention))) {
                using (var gallary = new ImageGallary(Page.Resources.Where(p => avaliablePictureTypes.Contains(p.Extention)).Select(p => new FileInfo($"{Page.DownloadPath}/{p.ResolvedUrl}").FullName).ToArray())) {
                    if (gallary.ShowDialog() == DialogResult.OK) {
                        Share.Image = gallary.SelectedImage;
                        pictureBox1.Image = gallary.SelectedImage;
                    }
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            // 保存缩略图
            var thumbPath = $"{Page.DownloadPath}\\thumb.jpg";
            pictureBox1.Image.Thumbnail(thumbPath);
            Share.IconPath = $"{AppConfiguration.Current.AppSettings.SaveConfig.RemotePath}/{Page.Id}/thumb.jpg";
            Share.Url = $"{AppConfiguration.Current.AppSettings.SaveConfig.RemotePath}/{Page.Id}";
            DialogResult = DialogResult.OK;
        }
    }
}
