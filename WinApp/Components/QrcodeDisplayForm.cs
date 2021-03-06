﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinApp.Components
{
    public partial class QrcodeDisplayForm : Form
    {
        public QrcodeDisplayForm()
        {
            InitializeComponent();
        }

        public QrcodeDisplayForm(Image image) : this()
        {
            InitImage(image);
        }

        public QrcodeDisplayForm(byte[] imageBytes) : this()
        {
            using (var ms = new MemoryStream(imageBytes)) {
                InitImage(Image.FromStream(ms));
            }
        }

        public QrcodeDisplayForm(Bitmap bitmap) : this()
        {
            using (var ms = new MemoryStream()) {
                bitmap.Save(ms, ImageFormat.Jpeg);
                InitImage(Image.FromStream(ms));
            }

        }

        private void InitImage(Image image)
        {
            pictureBox1.Image = image;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void downloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog()) {
                dialog.Title = "保存二维码";
                dialog.Filter = "jpeg文件(*.jpg)";
                dialog.RestoreDirectory = true;
                dialog.FileName = "网页二维码" + DateTime.Now.ToString("yyyyMMddHHmmss");
                if (dialog.ShowDialog() == DialogResult.OK) {
                    pictureBox1.Image.Save(dialog.FileName);
                }
            }
        }
    }
}
