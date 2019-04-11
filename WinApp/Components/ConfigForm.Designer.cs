namespace WinApp.Components
{
    partial class ConfigForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageSetting = new System.Windows.Forms.TabPage();
            this.textBoxConnection = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxShareApi = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tabPageDownload = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxRemoteDirectory = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBoxLocal = new System.Windows.Forms.GroupBox();
            this.linkLabelBrowse = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxLocalRootDirectory = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxIndividule = new System.Windows.Forms.CheckBox();
            this.tabPageSave = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonSaveConfig = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxUpload = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPageSetting.SuspendLayout();
            this.tabPageDownload.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBoxLocal.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageSetting);
            this.tabControl1.Controls.Add(this.tabPageDownload);
            this.tabControl1.Controls.Add(this.tabPageSave);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(604, 354);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageSetting
            // 
            this.tabPageSetting.Controls.Add(this.textBoxConnection);
            this.tabPageSetting.Controls.Add(this.label5);
            this.tabPageSetting.Controls.Add(this.textBoxShareApi);
            this.tabPageSetting.Controls.Add(this.label4);
            this.tabPageSetting.Location = new System.Drawing.Point(4, 22);
            this.tabPageSetting.Name = "tabPageSetting";
            this.tabPageSetting.Size = new System.Drawing.Size(596, 328);
            this.tabPageSetting.TabIndex = 2;
            this.tabPageSetting.Text = "Settings";
            this.tabPageSetting.UseVisualStyleBackColor = true;
            // 
            // textBoxConnection
            // 
            this.textBoxConnection.Location = new System.Drawing.Point(184, 53);
            this.textBoxConnection.Name = "textBoxConnection";
            this.textBoxConnection.Size = new System.Drawing.Size(390, 21);
            this.textBoxConnection.TabIndex = 3;
            this.textBoxConnection.TextChanged += new System.EventHandler(this.textBoxConnection_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(131, 12);
            this.label5.TabIndex = 2;
            this.label5.Text = "Database Connection: ";
            // 
            // textBoxShareApi
            // 
            this.textBoxShareApi.Location = new System.Drawing.Point(184, 16);
            this.textBoxShareApi.Name = "textBoxShareApi";
            this.textBoxShareApi.Size = new System.Drawing.Size(390, 21);
            this.textBoxShareApi.TabIndex = 1;
            this.textBoxShareApi.TextChanged += new System.EventHandler(this.textBoxShareApi_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(48, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "Host share API:";
            // 
            // tabPageDownload
            // 
            this.tabPageDownload.Controls.Add(this.groupBox1);
            this.tabPageDownload.Controls.Add(this.groupBoxLocal);
            this.tabPageDownload.Location = new System.Drawing.Point(4, 22);
            this.tabPageDownload.Name = "tabPageDownload";
            this.tabPageDownload.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDownload.Size = new System.Drawing.Size(596, 328);
            this.tabPageDownload.TabIndex = 0;
            this.tabPageDownload.Text = "Page Downlaod";
            this.tabPageDownload.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxUpload);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.textBoxRemoteDirectory);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(9, 169);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(584, 109);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Remote ";
            // 
            // textBoxRemoteDirectory
            // 
            this.textBoxRemoteDirectory.Location = new System.Drawing.Point(138, 21);
            this.textBoxRemoteDirectory.Name = "textBoxRemoteDirectory";
            this.textBoxRemoteDirectory.Size = new System.Drawing.Size(397, 21);
            this.textBoxRemoteDirectory.TabIndex = 1;
            this.textBoxRemoteDirectory.TextChanged += new System.EventHandler(this.textBoxRemoteDirectory_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "remote path: ";
            // 
            // groupBoxLocal
            // 
            this.groupBoxLocal.Controls.Add(this.linkLabelBrowse);
            this.groupBoxLocal.Controls.Add(this.label2);
            this.groupBoxLocal.Controls.Add(this.textBoxLocalRootDirectory);
            this.groupBoxLocal.Controls.Add(this.label1);
            this.groupBoxLocal.Controls.Add(this.checkBoxIndividule);
            this.groupBoxLocal.Location = new System.Drawing.Point(6, 20);
            this.groupBoxLocal.Name = "groupBoxLocal";
            this.groupBoxLocal.Size = new System.Drawing.Size(582, 142);
            this.groupBoxLocal.TabIndex = 0;
            this.groupBoxLocal.TabStop = false;
            this.groupBoxLocal.Text = "Local Directory";
            // 
            // linkLabelBrowse
            // 
            this.linkLabelBrowse.AutoSize = true;
            this.linkLabelBrowse.Location = new System.Drawing.Point(479, 32);
            this.linkLabelBrowse.Name = "linkLabelBrowse";
            this.linkLabelBrowse.Size = new System.Drawing.Size(59, 12);
            this.linkLabelBrowse.TabIndex = 4;
            this.linkLabelBrowse.TabStop = true;
            this.linkLabelBrowse.Text = "Browse...";
            this.linkLabelBrowse.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelBrowse_LinkClicked);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "root directory: ";
            // 
            // textBoxLocalRootDirectory
            // 
            this.textBoxLocalRootDirectory.Location = new System.Drawing.Point(141, 29);
            this.textBoxLocalRootDirectory.Name = "textBoxLocalRootDirectory";
            this.textBoxLocalRootDirectory.Size = new System.Drawing.Size(332, 21);
            this.textBoxLocalRootDirectory.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label1.Location = new System.Drawing.Point(52, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(530, 33);
            this.label1.TabIndex = 1;
            this.label1.Text = "if checked, each page will optimize images, scripts, styles and source html file " +
    "in an individule directory. otherwise all pages will be optimize together";
            // 
            // checkBoxIndividule
            // 
            this.checkBoxIndividule.AutoSize = true;
            this.checkBoxIndividule.Checked = true;
            this.checkBoxIndividule.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxIndividule.Enabled = false;
            this.checkBoxIndividule.Location = new System.Drawing.Point(36, 73);
            this.checkBoxIndividule.Name = "checkBoxIndividule";
            this.checkBoxIndividule.Size = new System.Drawing.Size(234, 16);
            this.checkBoxIndividule.TabIndex = 0;
            this.checkBoxIndividule.Text = "each page with individule directory";
            this.checkBoxIndividule.UseVisualStyleBackColor = true;
            // 
            // tabPageSave
            // 
            this.tabPageSave.Location = new System.Drawing.Point(4, 22);
            this.tabPageSave.Name = "tabPageSave";
            this.tabPageSave.Size = new System.Drawing.Size(596, 328);
            this.tabPageSave.TabIndex = 1;
            this.tabPageSave.Text = "Page Save";
            this.tabPageSave.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonSaveConfig);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 313);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(604, 41);
            this.panel1.TabIndex = 1;
            // 
            // buttonSaveConfig
            // 
            this.buttonSaveConfig.Location = new System.Drawing.Point(263, 8);
            this.buttonSaveConfig.Name = "buttonSaveConfig";
            this.buttonSaveConfig.Size = new System.Drawing.Size(75, 23);
            this.buttonSaveConfig.TabIndex = 0;
            this.buttonSaveConfig.Text = "Save";
            this.buttonSaveConfig.UseVisualStyleBackColor = true;
            this.buttonSaveConfig.Click += new System.EventHandler(this.buttonSaveConfig_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(40, 60);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 2;
            this.label6.Text = "upload API";
            // 
            // textBoxUpload
            // 
            this.textBoxUpload.Location = new System.Drawing.Point(138, 57);
            this.textBoxUpload.Name = "textBoxUpload";
            this.textBoxUpload.Size = new System.Drawing.Size(397, 21);
            this.textBoxUpload.TabIndex = 3;
            this.textBoxUpload.TextChanged += new System.EventHandler(this.textBoxUpload_TextChanged);
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 354);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tabControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ConfigForm";
            this.Load += new System.EventHandler(this.ConfigForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPageSetting.ResumeLayout(false);
            this.tabPageSetting.PerformLayout();
            this.tabPageDownload.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBoxLocal.ResumeLayout(false);
            this.groupBoxLocal.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageDownload;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonSaveConfig;
        private System.Windows.Forms.TabPage tabPageSave;
        private System.Windows.Forms.TabPage tabPageSetting;
        private System.Windows.Forms.GroupBox groupBoxLocal;
        private System.Windows.Forms.CheckBox checkBoxIndividule;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxLocalRootDirectory;
        private System.Windows.Forms.LinkLabel linkLabelBrowse;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxRemoteDirectory;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxShareApi;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxConnection;
        private System.Windows.Forms.TextBox textBoxUpload;
        private System.Windows.Forms.Label label6;
    }
}