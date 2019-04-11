using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinApp.Components
{
    public partial class ImageGallary : Form
    {
        public string[] Images { get; private set; }
        public Image SelectedImage { get; private set; }
        public ImageGallary()
        {
            InitializeComponent();
        }
        public ImageGallary(string[] images) : this()
        {
            Images = images;
        }

        private void ImageGallary_Load(object sender, EventArgs e)
        {
            foreach (var p in Images)
                flowLayoutPanel1.Controls.Add(addPictures(p));
        }

        private PictureBox addPictures(string path)
        {
            Image i = Image.FromFile(path);
            var pictureBox = new PictureBox() {
                Width = 100,
                Height = 100,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = i
            };
            pictureBox.Click += PictureBox_Click;
            return pictureBox;
        }

        private void PictureBox_Click(object sender, EventArgs e)
        {
            var picture = sender as PictureBox;
            SelectedImage = picture.Image;
            DialogResult = DialogResult.OK;
        }
    }
}
