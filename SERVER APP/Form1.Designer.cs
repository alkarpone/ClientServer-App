namespace SERVER_V2
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.wrongPortLabel = new System.Windows.Forms.Label();
            this.wrongIpLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.PortTextBox = new System.Windows.Forms.TextBox();
            this.IpAddressTextBox = new System.Windows.Forms.TextBox();
            this.StopButton = new System.Windows.Forms.Button();
            this.StartButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.serverinfo = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.sendlog = new System.Windows.Forms.TextBox();
            this.receivelog = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.serverPanel = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.wrongPortLabel);
            this.panel1.Controls.Add(this.wrongIpLabel);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.PortTextBox);
            this.panel1.Controls.Add(this.IpAddressTextBox);
            this.panel1.Controls.Add(this.StopButton);
            this.panel1.Controls.Add(this.StartButton);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(716, 86);
            this.panel1.TabIndex = 0;
            // 
            // wrongPortLabel
            // 
            this.wrongPortLabel.AutoSize = true;
            this.wrongPortLabel.ForeColor = System.Drawing.Color.Firebrick;
            this.wrongPortLabel.Location = new System.Drawing.Point(196, 55);
            this.wrongPortLabel.Name = "wrongPortLabel";
            this.wrongPortLabel.Size = new System.Drawing.Size(159, 15);
            this.wrongPortLabel.TabIndex = 7;
            this.wrongPortLabel.Text = "Port must have format \'0000\'";
            this.wrongPortLabel.Visible = false;
            // 
            // wrongIpLabel
            // 
            this.wrongIpLabel.AutoSize = true;
            this.wrongIpLabel.ForeColor = System.Drawing.Color.Firebrick;
            this.wrongIpLabel.Location = new System.Drawing.Point(16, 55);
            this.wrongIpLabel.Name = "wrongIpLabel";
            this.wrongIpLabel.Size = new System.Drawing.Size(144, 15);
            this.wrongIpLabel.TabIndex = 6;
            this.wrongIpLabel.Text = "Wrong form of IP Address";
            this.wrongIpLabel.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(196, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "Port";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "IP Address";
            // 
            // PortTextBox
            // 
            this.PortTextBox.Location = new System.Drawing.Point(196, 29);
            this.PortTextBox.Name = "PortTextBox";
            this.PortTextBox.Size = new System.Drawing.Size(100, 23);
            this.PortTextBox.TabIndex = 3;
            this.PortTextBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PortTextBox_MouseClick);
            // 
            // IpAddressTextBox
            // 
            this.IpAddressTextBox.Location = new System.Drawing.Point(16, 29);
            this.IpAddressTextBox.Name = "IpAddressTextBox";
            this.IpAddressTextBox.Size = new System.Drawing.Size(100, 23);
            this.IpAddressTextBox.TabIndex = 2;
            this.IpAddressTextBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.IpAddressTextBox_MouseClick);
            // 
            // StopButton
            // 
            this.StopButton.Enabled = false;
            this.StopButton.Location = new System.Drawing.Point(497, 29);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(100, 41);
            this.StopButton.TabIndex = 1;
            this.StopButton.Text = "Stop Server";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(380, 28);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(100, 41);
            this.StartButton.TabIndex = 0;
            this.StartButton.Text = "Start Server";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.serverinfo);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.sendlog);
            this.panel2.Controls.Add(this.receivelog);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Location = new System.Drawing.Point(12, 104);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(841, 558);
            this.panel2.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(628, 54);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 15);
            this.label6.TabIndex = 7;
            this.label6.Text = "Server info";
            // 
            // serverinfo
            // 
            this.serverinfo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.serverinfo.Location = new System.Drawing.Point(628, 75);
            this.serverinfo.MaximumSize = new System.Drawing.Size(200, 470);
            this.serverinfo.MinimumSize = new System.Drawing.Size(200, 200);
            this.serverinfo.Multiline = true;
            this.serverinfo.Name = "serverinfo";
            this.serverinfo.ReadOnly = true;
            this.serverinfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.serverinfo.Size = new System.Drawing.Size(200, 470);
            this.serverinfo.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(323, 54);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 15);
            this.label5.TabIndex = 5;
            this.label5.Text = "Send data";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Received data";
            // 
            // sendlog
            // 
            this.sendlog.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.sendlog.Location = new System.Drawing.Point(322, 75);
            this.sendlog.MaximumSize = new System.Drawing.Size(300, 470);
            this.sendlog.MinimumSize = new System.Drawing.Size(300, 200);
            this.sendlog.Multiline = true;
            this.sendlog.Name = "sendlog";
            this.sendlog.ReadOnly = true;
            this.sendlog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.sendlog.Size = new System.Drawing.Size(300, 470);
            this.sendlog.TabIndex = 3;
            // 
            // receivelog
            // 
            this.receivelog.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.receivelog.Location = new System.Drawing.Point(16, 75);
            this.receivelog.MaximumSize = new System.Drawing.Size(300, 0);
            this.receivelog.MinimumSize = new System.Drawing.Size(300, 200);
            this.receivelog.Multiline = true;
            this.receivelog.Name = "receivelog";
            this.receivelog.ReadOnly = true;
            this.receivelog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.receivelog.Size = new System.Drawing.Size(300, 470);
            this.receivelog.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(16, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(140, 28);
            this.label4.TabIndex = 1;
            this.label4.Text = "SERVER LOGS";
            // 
            // serverPanel
            // 
            this.serverPanel.BackColor = System.Drawing.Color.Red;
            this.serverPanel.Location = new System.Drawing.Point(734, 12);
            this.serverPanel.Name = "serverPanel";
            this.serverPanel.Size = new System.Drawing.Size(119, 86);
            this.serverPanel.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(865, 674);
            this.Controls.Add(this.serverPanel);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.MaximumSize = new System.Drawing.Size(881, 713);
            this.MinimumSize = new System.Drawing.Size(881, 713);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        private Label label2;
        private Label label1;
        private TextBox PortTextBox;
        private TextBox IpAddressTextBox;
        private Button StopButton;
        private Button StartButton;
        private Panel panel2;
        private Label wrongIpLabel;
        private Label wrongPortLabel;
        private Label label4;
        private Panel serverPanel;
        private TextBox receivelog;
        private Label label3;
        private TextBox sendlog;
        private TextBox serverinfo;
        private Label label5;
        private Label label6;
    }
}