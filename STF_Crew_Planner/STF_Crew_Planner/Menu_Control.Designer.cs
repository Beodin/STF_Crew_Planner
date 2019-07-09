namespace STF_CharacterPlanner
{
    partial class Menu_Control
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
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.resetButton = new System.Windows.Forms.Button();
            this.saveTextButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupNameBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.searchTalentButton = new System.Windows.Forms.Button();
            this.refreshWikiButton = new System.Windows.Forms.Button();
            this.saveTablesButton = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // resetButton
            // 
            this.resetButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(230)))), ((int)(((byte)(247)))));
            this.resetButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.resetButton.Location = new System.Drawing.Point(3, 63);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(75, 23);
            this.resetButton.TabIndex = 1;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = false;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // saveTextButton
            // 
            this.saveTextButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(230)))), ((int)(((byte)(247)))));
            this.saveTextButton.ForeColor = System.Drawing.SystemColors.Desktop;
            this.saveTextButton.Location = new System.Drawing.Point(575, 63);
            this.saveTextButton.Name = "saveTextButton";
            this.saveTextButton.Size = new System.Drawing.Size(101, 23);
            this.saveTextButton.TabIndex = 2;
            this.saveTextButton.Text = "Output Text File";
            this.saveTextButton.UseVisualStyleBackColor = false;
            this.saveTextButton.Click += new System.EventHandler(this.saveTextButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(249)))), ((int)(((byte)(253)))));
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 22);
            this.label1.TabIndex = 3;
            this.label1.Text = "Menu";
            // 
            // groupNameBox
            // 
            this.groupNameBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(239)))), ((int)(((byte)(250)))));
            this.groupNameBox.Location = new System.Drawing.Point(75, 24);
            this.groupNameBox.MaxLength = 60;
            this.groupNameBox.Name = "groupNameBox";
            this.groupNameBox.Size = new System.Drawing.Size(237, 20);
            this.groupNameBox.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(213)))), ((int)(((byte)(241)))));
            this.label2.Location = new System.Drawing.Point(4, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Crew Name:";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(230)))), ((int)(((byte)(247)))));
            this.button1.ForeColor = System.Drawing.SystemColors.Desktop;
            this.button1.Location = new System.Drawing.Point(84, 63);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(84, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Save Crew";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(230)))), ((int)(((byte)(247)))));
            this.button2.ForeColor = System.Drawing.SystemColors.Desktop;
            this.button2.Location = new System.Drawing.Point(174, 63);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(84, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "Load Crew";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // searchTalentButton
            // 
            this.searchTalentButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(230)))), ((int)(((byte)(247)))));
            this.searchTalentButton.ForeColor = System.Drawing.SystemColors.Desktop;
            this.searchTalentButton.Location = new System.Drawing.Point(1100, 63);
            this.searchTalentButton.Name = "searchTalentButton";
            this.searchTalentButton.Size = new System.Drawing.Size(95, 23);
            this.searchTalentButton.TabIndex = 8;
            this.searchTalentButton.Text = "Search Talents";
            this.searchTalentButton.UseVisualStyleBackColor = false;
            this.searchTalentButton.Click += new System.EventHandler(this.searchTalentButton_Click);
            // 
            // refreshWikiButton
            // 
            this.refreshWikiButton.Location = new System.Drawing.Point(1066, 3);
            this.refreshWikiButton.Name = "refreshWikiButton";
            this.refreshWikiButton.Size = new System.Drawing.Size(129, 23);
            this.refreshWikiButton.TabIndex = 9;
            this.refreshWikiButton.Text = "Refresh Data from Wiki";
            this.refreshWikiButton.UseVisualStyleBackColor = true;
            this.refreshWikiButton.Click += new System.EventHandler(this.refreshWikiButton_Click);
            // 
            // saveTablesButton
            // 
            this.saveTablesButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(230)))), ((int)(((byte)(247)))));
            this.saveTablesButton.ForeColor = System.Drawing.SystemColors.Desktop;
            this.saveTablesButton.Location = new System.Drawing.Point(442, 63);
            this.saveTablesButton.Name = "saveTablesButton";
            this.saveTablesButton.Size = new System.Drawing.Size(127, 23);
            this.saveTablesButton.TabIndex = 10;
            this.saveTablesButton.Text = "Output Forum Format";
            this.saveTablesButton.UseVisualStyleBackColor = false;
            this.saveTablesButton.Click += new System.EventHandler(this.saveTablesButton_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(264, 63);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 11;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // Menu_Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.saveTablesButton);
            this.Controls.Add(this.refreshWikiButton);
            this.Controls.Add(this.searchTalentButton);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupNameBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.saveTextButton);
            this.Controls.Add(this.resetButton);
            this.Name = "Menu_Control";
            this.Size = new System.Drawing.Size(1198, 98);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.Button saveTextButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox groupNameBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button searchTalentButton;
        private System.Windows.Forms.Button refreshWikiButton;
        private System.Windows.Forms.Button saveTablesButton;
        private System.Windows.Forms.Button btnUpdate;
    }
}
