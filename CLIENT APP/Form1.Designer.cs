namespace EPRIN_Klient_V3

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
            this.components = new System.ComponentModel.Container();
            this.PersonBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.NameTextBox = new System.Windows.Forms.TextBox();
            this.SurnameTextBox = new System.Windows.Forms.TextBox();
            this.NameLabel = new System.Windows.Forms.Label();
            this.SurnameLabel = new System.Windows.Forms.Label();
            this.AddButton = new System.Windows.Forms.Button();
            this.DataGridView1 = new System.Windows.Forms.DataGridView();
            this.iDDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SurnameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AgeDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AgeTextBox = new System.Windows.Forms.TextBox();
            this.AgeLabel = new System.Windows.Forms.Label();
            this.AgeErrorLabel = new System.Windows.Forms.Label();
            this.SurnameErrorLabel = new System.Windows.Forms.Label();
            this.NameErrorLabel = new System.Windows.Forms.Label();
            this.EditButton = new System.Windows.Forms.Button();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.DeleteLabel = new System.Windows.Forms.Label();
            this.SignInPanel = new System.Windows.Forms.Panel();
            this.PortLabel = new System.Windows.Forms.Label();
            this.PortErrorLabel = new System.Windows.Forms.Label();
            this.PortTextBox = new System.Windows.Forms.TextBox();
            this.IpErrorLabel = new System.Windows.Forms.Label();
            this.IPAddressLabel = new System.Windows.Forms.Label();
            this.SignInLabel = new System.Windows.Forms.Label();
            this.SignInButton = new System.Windows.Forms.Button();
            this.IPAddressTextBox = new System.Windows.Forms.TextBox();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.LogoutButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.PersonBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView1)).BeginInit();
            this.SignInPanel.SuspendLayout();
            this.MainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // PersonBindingSource
            // 
            this.PersonBindingSource.DataSource = typeof(EPRIN_Klient_V3.Person);
            // 
            // NameTextBox
            // 
            this.NameTextBox.Location = new System.Drawing.Point(60, 412);
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new System.Drawing.Size(139, 23);
            this.NameTextBox.TabIndex = 1;
            this.NameTextBox.TextChanged += new System.EventHandler(this.NameTextBox_TextChanged);
            // 
            // SurnameTextBox
            // 
            this.SurnameTextBox.Location = new System.Drawing.Point(60, 456);
            this.SurnameTextBox.Name = "SurnameTextBox";
            this.SurnameTextBox.Size = new System.Drawing.Size(139, 23);
            this.SurnameTextBox.TabIndex = 2;
            this.SurnameTextBox.TextChanged += new System.EventHandler(this.SurnameTextBox_TextChanged);
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Location = new System.Drawing.Point(60, 394);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(39, 15);
            this.NameLabel.TabIndex = 0;
            this.NameLabel.Text = "Name";
            // 
            // SurnameLabel
            // 
            this.SurnameLabel.AutoSize = true;
            this.SurnameLabel.Location = new System.Drawing.Point(60, 438);
            this.SurnameLabel.Name = "SurnameLabel";
            this.SurnameLabel.Size = new System.Drawing.Size(54, 15);
            this.SurnameLabel.TabIndex = 0;
            this.SurnameLabel.Text = "Surname";
            // 
            // AddButton
            // 
            this.AddButton.Location = new System.Drawing.Point(157, 525);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(75, 23);
            this.AddButton.TabIndex = 4;
            this.AddButton.Text = "Add";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // DataGridView1
            // 
            this.DataGridView1.AllowUserToAddRows = false;
            this.DataGridView1.AllowUserToDeleteRows = false;
            this.DataGridView1.AllowUserToResizeColumns = false;
            this.DataGridView1.AllowUserToResizeRows = false;
            this.DataGridView1.AutoGenerateColumns = false;
            this.DataGridView1.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.iDDataGridViewTextBoxColumn1,
            this.NameDataGridViewTextBoxColumn,
            this.SurnameDataGridViewTextBoxColumn,
            this.AgeDataGridViewTextBoxColumn1});
            this.DataGridView1.DataSource = this.PersonBindingSource;
            this.DataGridView1.GridColor = System.Drawing.SystemColors.Control;
            this.DataGridView1.Location = new System.Drawing.Point(60, 18);
            this.DataGridView1.MultiSelect = false;
            this.DataGridView1.Name = "DataGridView1";
            this.DataGridView1.ReadOnly = true;
            this.DataGridView1.RowHeadersVisible = false;
            this.DataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.DataGridView1.RowTemplate.Height = 25;
            this.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DataGridView1.Size = new System.Drawing.Size(423, 351);
            this.DataGridView1.TabIndex = 5;
            this.DataGridView1.TabStop = false;
            this.DataGridView1.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DataGridView1_CellMouseClick);
            // 
            // iDDataGridViewTextBoxColumn1
            // 
            this.iDDataGridViewTextBoxColumn1.DataPropertyName = "ID";
            this.iDDataGridViewTextBoxColumn1.HeaderText = "ID";
            this.iDDataGridViewTextBoxColumn1.Name = "iDDataGridViewTextBoxColumn1";
            this.iDDataGridViewTextBoxColumn1.ReadOnly = true;
            this.iDDataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // NameDataGridViewTextBoxColumn
            // 
            this.NameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.NameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.NameDataGridViewTextBoxColumn.Name = "NameDataGridViewTextBoxColumn";
            this.NameDataGridViewTextBoxColumn.ReadOnly = true;
            this.NameDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // SurnameDataGridViewTextBoxColumn
            // 
            this.SurnameDataGridViewTextBoxColumn.DataPropertyName = "Surname";
            this.SurnameDataGridViewTextBoxColumn.HeaderText = "Surname";
            this.SurnameDataGridViewTextBoxColumn.Name = "SurnameDataGridViewTextBoxColumn";
            this.SurnameDataGridViewTextBoxColumn.ReadOnly = true;
            this.SurnameDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // AgeDataGridViewTextBoxColumn1
            // 
            this.AgeDataGridViewTextBoxColumn1.DataPropertyName = "Age";
            this.AgeDataGridViewTextBoxColumn1.HeaderText = "Age";
            this.AgeDataGridViewTextBoxColumn1.Name = "AgeDataGridViewTextBoxColumn1";
            this.AgeDataGridViewTextBoxColumn1.ReadOnly = true;
            this.AgeDataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // AgeTextBox
            // 
            this.AgeTextBox.Location = new System.Drawing.Point(60, 500);
            this.AgeTextBox.Name = "AgeTextBox";
            this.AgeTextBox.Size = new System.Drawing.Size(39, 23);
            this.AgeTextBox.TabIndex = 3;
            this.AgeTextBox.TextChanged += new System.EventHandler(this.AgeTextBox_TextChanged);
            // 
            // AgeLabel
            // 
            this.AgeLabel.AutoSize = true;
            this.AgeLabel.Location = new System.Drawing.Point(60, 482);
            this.AgeLabel.Name = "AgeLabel";
            this.AgeLabel.Size = new System.Drawing.Size(28, 15);
            this.AgeLabel.TabIndex = 0;
            this.AgeLabel.Text = "Age";
            // 
            // AgeErrorLabel
            // 
            this.AgeErrorLabel.AutoSize = true;
            this.AgeErrorLabel.ForeColor = System.Drawing.Color.Firebrick;
            this.AgeErrorLabel.Location = new System.Drawing.Point(99, 482);
            this.AgeErrorLabel.Name = "AgeErrorLabel";
            this.AgeErrorLabel.Size = new System.Drawing.Size(0, 15);
            this.AgeErrorLabel.TabIndex = 8;
            // 
            // SurnameErrorLabel
            // 
            this.SurnameErrorLabel.AutoSize = true;
            this.SurnameErrorLabel.BackColor = System.Drawing.Color.CornflowerBlue;
            this.SurnameErrorLabel.ForeColor = System.Drawing.Color.Firebrick;
            this.SurnameErrorLabel.Location = new System.Drawing.Point(120, 438);
            this.SurnameErrorLabel.Name = "SurnameErrorLabel";
            this.SurnameErrorLabel.Size = new System.Drawing.Size(0, 15);
            this.SurnameErrorLabel.TabIndex = 9;
            // 
            // NameErrorLabel
            // 
            this.NameErrorLabel.AutoSize = true;
            this.NameErrorLabel.ForeColor = System.Drawing.Color.Firebrick;
            this.NameErrorLabel.Location = new System.Drawing.Point(105, 394);
            this.NameErrorLabel.Name = "NameErrorLabel";
            this.NameErrorLabel.Size = new System.Drawing.Size(0, 15);
            this.NameErrorLabel.TabIndex = 10;
            // 
            // EditButton
            // 
            this.EditButton.Location = new System.Drawing.Point(238, 525);
            this.EditButton.Name = "EditButton";
            this.EditButton.Size = new System.Drawing.Size(75, 23);
            this.EditButton.TabIndex = 5;
            this.EditButton.Text = "Edit";
            this.EditButton.UseVisualStyleBackColor = true;
            this.EditButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // DeleteButton
            // 
            this.DeleteButton.Location = new System.Drawing.Point(319, 525);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(75, 23);
            this.DeleteButton.TabIndex = 6;
            this.DeleteButton.Text = "Delete";
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // DeleteLabel
            // 
            this.DeleteLabel.AutoSize = true;
            this.DeleteLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.DeleteLabel.ForeColor = System.Drawing.Color.Firebrick;
            this.DeleteLabel.Location = new System.Drawing.Point(279, 530);
            this.DeleteLabel.Name = "DeleteLabel";
            this.DeleteLabel.Size = new System.Drawing.Size(0, 21);
            this.DeleteLabel.TabIndex = 13;
            this.DeleteLabel.Visible = false;
            // 
            // SignInPanel
            // 
            this.SignInPanel.AutoSize = true;
            this.SignInPanel.BackColor = System.Drawing.Color.White;
            this.SignInPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.SignInPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SignInPanel.Controls.Add(this.PortLabel);
            this.SignInPanel.Controls.Add(this.PortErrorLabel);
            this.SignInPanel.Controls.Add(this.PortTextBox);
            this.SignInPanel.Controls.Add(this.IpErrorLabel);
            this.SignInPanel.Controls.Add(this.IPAddressLabel);
            this.SignInPanel.Controls.Add(this.SignInLabel);
            this.SignInPanel.Controls.Add(this.SignInButton);
            this.SignInPanel.Controls.Add(this.IPAddressTextBox);
            this.SignInPanel.Location = new System.Drawing.Point(121, 31);
            this.SignInPanel.Name = "SignInPanel";
            this.SignInPanel.Size = new System.Drawing.Size(342, 340);
            this.SignInPanel.TabIndex = 0;
            // 
            // PortLabel
            // 
            this.PortLabel.AutoSize = true;
            this.PortLabel.Location = new System.Drawing.Point(128, 194);
            this.PortLabel.Name = "PortLabel";
            this.PortLabel.Size = new System.Drawing.Size(32, 15);
            this.PortLabel.TabIndex = 13;
            this.PortLabel.Text = "Port:";
            // 
            // PortErrorLabel
            // 
            this.PortErrorLabel.AutoSize = true;
            this.PortErrorLabel.ForeColor = System.Drawing.Color.Firebrick;
            this.PortErrorLabel.Location = new System.Drawing.Point(163, 217);
            this.PortErrorLabel.Name = "PortErrorLabel";
            this.PortErrorLabel.Size = new System.Drawing.Size(0, 15);
            this.PortErrorLabel.TabIndex = 12;
            this.PortErrorLabel.Visible = false;
            // 
            // PortTextBox
            // 
            this.PortTextBox.Location = new System.Drawing.Point(163, 191);
            this.PortTextBox.Name = "PortTextBox";
            this.PortTextBox.Size = new System.Drawing.Size(54, 23);
            this.PortTextBox.TabIndex = 11;
            this.PortTextBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PortTextBox_MouseClick);
            // 
            // IpErrorLabel
            // 
            this.IpErrorLabel.AutoSize = true;
            this.IpErrorLabel.ForeColor = System.Drawing.Color.Firebrick;
            this.IpErrorLabel.Location = new System.Drawing.Point(163, 166);
            this.IpErrorLabel.Name = "IpErrorLabel";
            this.IpErrorLabel.Size = new System.Drawing.Size(0, 15);
            this.IpErrorLabel.TabIndex = 10;
            this.IpErrorLabel.Visible = false;
            // 
            // IPAddressLabel
            // 
            this.IPAddressLabel.AutoSize = true;
            this.IPAddressLabel.Location = new System.Drawing.Point(95, 143);
            this.IPAddressLabel.Name = "IPAddressLabel";
            this.IPAddressLabel.Size = new System.Drawing.Size(65, 15);
            this.IPAddressLabel.TabIndex = 5;
            this.IPAddressLabel.Text = "IP Address:";
            // 
            // SignInLabel
            // 
            this.SignInLabel.AutoSize = true;
            this.SignInLabel.Font = new System.Drawing.Font("Bahnschrift Condensed", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.SignInLabel.Location = new System.Drawing.Point(115, 55);
            this.SignInLabel.Name = "SignInLabel";
            this.SignInLabel.Size = new System.Drawing.Size(111, 48);
            this.SignInLabel.TabIndex = 0;
            this.SignInLabel.Text = "Sign In";
            // 
            // SignInButton
            // 
            this.SignInButton.Location = new System.Drawing.Point(70, 274);
            this.SignInButton.Name = "SignInButton";
            this.SignInButton.Size = new System.Drawing.Size(196, 40);
            this.SignInButton.TabIndex = 4;
            this.SignInButton.Text = "Connect";
            this.SignInButton.UseVisualStyleBackColor = true;
            this.SignInButton.Click += new System.EventHandler(this.SignInButton_Click);
            // 
            // IPAddressTextBox
            // 
            this.IPAddressTextBox.AllowDrop = true;
            this.IPAddressTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.IPAddressTextBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.IPAddressTextBox.Location = new System.Drawing.Point(163, 140);
            this.IPAddressTextBox.Name = "IPAddressTextBox";
            this.IPAddressTextBox.Size = new System.Drawing.Size(94, 23);
            this.IPAddressTextBox.TabIndex = 1;
            this.IPAddressTextBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.IPAddressTextBox_MouseClick);
            // 
            // MainPanel
            // 
            this.MainPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.MainPanel.Controls.Add(this.DeleteLabel);
            this.MainPanel.Controls.Add(this.DeleteButton);
            this.MainPanel.Controls.Add(this.EditButton);
            this.MainPanel.Controls.Add(this.NameErrorLabel);
            this.MainPanel.Controls.Add(this.SurnameErrorLabel);
            this.MainPanel.Controls.Add(this.AgeErrorLabel);
            this.MainPanel.Controls.Add(this.AgeLabel);
            this.MainPanel.Controls.Add(this.AgeTextBox);
            this.MainPanel.Controls.Add(this.AddButton);
            this.MainPanel.Controls.Add(this.SurnameLabel);
            this.MainPanel.Controls.Add(this.NameLabel);
            this.MainPanel.Controls.Add(this.SurnameTextBox);
            this.MainPanel.Controls.Add(this.NameTextBox);
            this.MainPanel.Controls.Add(this.DataGridView1);
            this.MainPanel.Location = new System.Drawing.Point(12, 51);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(565, 551);
            this.MainPanel.TabIndex = 1;
            this.MainPanel.Visible = false;
            // 
            // LogoutButton
            // 
            this.LogoutButton.Location = new System.Drawing.Point(496, 14);
            this.LogoutButton.Name = "LogoutButton";
            this.LogoutButton.Size = new System.Drawing.Size(75, 23);
            this.LogoutButton.TabIndex = 2;
            this.LogoutButton.Text = "Log Out";
            this.LogoutButton.UseVisualStyleBackColor = true;
            this.LogoutButton.Visible = false;
            this.LogoutButton.Click += new System.EventHandler(this.LogoutButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.CornflowerBlue;
            this.ClientSize = new System.Drawing.Size(586, 614);
            this.Controls.Add(this.SignInPanel);
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.LogoutButton);
            this.MaximumSize = new System.Drawing.Size(602, 653);
            this.MinimumSize = new System.Drawing.Size(602, 653);
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PersonBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView1)).EndInit();
            this.SignInPanel.ResumeLayout(false);
            this.SignInPanel.PerformLayout();
            this.MainPanel.ResumeLayout(false);
            this.MainPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private BindingSource PersonBindingSource;
        private TextBox NameTextBox;
        private TextBox SurnameTextBox;
        private Label NameLabel;
        private Label SurnameLabel;
        private Button AddButton;
        private DataGridView DataGridView1;
        private TextBox AgeTextBox;
        private Label AgeLabel;
        private Label AgeErrorLabel;
        private Label SurnameErrorLabel;
        private Label NameErrorLabel;
        private Button EditButton;
        private Button DeleteButton;
        private Label DeleteLabel;
        private Panel SignInPanel;
        private Label IPAddressLabel;
        private Label SignInLabel;
        private Button SignInButton;
        private TextBox IPAddressTextBox;
        private Panel MainPanel;
        private Label IpErrorLabel;
        private TextBox PortTextBox;
        private Label PortLabel;
        private Label PortErrorLabel;
        private Button LogoutButton;
        private DataGridViewTextBoxColumn iDDataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn NameDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn SurnameDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn AgeDataGridViewTextBoxColumn1;
    }
}