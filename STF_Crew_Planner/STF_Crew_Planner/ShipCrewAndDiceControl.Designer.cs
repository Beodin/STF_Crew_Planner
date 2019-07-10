namespace STF_CharacterPlanner
{
    partial class ShipCrewAndDiceControl
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
            this.configureCrewBut = new System.Windows.Forms.Button();
            this.crewListBox = new System.Windows.Forms.ListBox();
            this.shipDiceBox = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.shipReadout = new System.Windows.Forms.ListBox();
            this.configureShipBut = new System.Windows.Forms.Button();
            this.calcShipCombatBut = new System.Windows.Forms.Button();
            this.smallCraft = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // configureCrewBut
            // 
            this.configureCrewBut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(230)))), ((int)(((byte)(247)))));
            this.configureCrewBut.Location = new System.Drawing.Point(12, 14);
            this.configureCrewBut.Name = "configureCrewBut";
            this.configureCrewBut.Size = new System.Drawing.Size(104, 23);
            this.configureCrewBut.TabIndex = 0;
            this.configureCrewBut.Text = "Configure Crew";
            this.configureCrewBut.UseVisualStyleBackColor = false;
            this.configureCrewBut.Click += new System.EventHandler(this.configureCrewBut_Click);
            // 
            // crewListBox
            // 
            this.crewListBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(239)))), ((int)(((byte)(250)))));
            this.crewListBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.crewListBox.FormattingEnabled = true;
            this.crewListBox.ItemHeight = 14;
            this.crewListBox.Location = new System.Drawing.Point(860, 20);
            this.crewListBox.Name = "crewListBox";
            this.crewListBox.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.crewListBox.Size = new System.Drawing.Size(397, 130);
            this.crewListBox.TabIndex = 1;
            // 
            // shipDiceBox
            // 
            this.shipDiceBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(239)))), ((int)(((byte)(250)))));
            this.shipDiceBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.shipDiceBox.FormattingEnabled = true;
            this.shipDiceBox.ItemHeight = 14;
            this.shipDiceBox.Location = new System.Drawing.Point(505, 20);
            this.shipDiceBox.Name = "shipDiceBox";
            this.shipDiceBox.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.shipDiceBox.Size = new System.Drawing.Size(349, 130);
            this.shipDiceBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(213)))), ((int)(((byte)(241)))));
            this.label1.Location = new System.Drawing.Point(505, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Ship Dice Pools";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(213)))), ((int)(((byte)(241)))));
            this.label2.Location = new System.Drawing.Point(860, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Crew Members";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(213)))), ((int)(((byte)(241)))));
            this.label3.Location = new System.Drawing.Point(150, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Ship Readout";
            // 
            // shipReadout
            // 
            this.shipReadout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(239)))), ((int)(((byte)(250)))));
            this.shipReadout.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.shipReadout.FormattingEnabled = true;
            this.shipReadout.ItemHeight = 14;
            this.shipReadout.Location = new System.Drawing.Point(150, 20);
            this.shipReadout.Name = "shipReadout";
            this.shipReadout.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.shipReadout.Size = new System.Drawing.Size(349, 130);
            this.shipReadout.TabIndex = 5;
            // 
            // configureShipBut
            // 
            this.configureShipBut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(230)))), ((int)(((byte)(247)))));
            this.configureShipBut.Location = new System.Drawing.Point(12, 43);
            this.configureShipBut.Name = "configureShipBut";
            this.configureShipBut.Size = new System.Drawing.Size(104, 23);
            this.configureShipBut.TabIndex = 7;
            this.configureShipBut.Text = "Configure Ship";
            this.configureShipBut.UseVisualStyleBackColor = false;
            this.configureShipBut.Click += new System.EventHandler(this.configureShipBut_Click);
            // 
            // calcShipCombatBut
            // 
            this.calcShipCombatBut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(230)))), ((int)(((byte)(247)))));
            this.calcShipCombatBut.Location = new System.Drawing.Point(3, 85);
            this.calcShipCombatBut.Name = "calcShipCombatBut";
            this.calcShipCombatBut.Size = new System.Drawing.Size(132, 23);
            this.calcShipCombatBut.TabIndex = 8;
            this.calcShipCombatBut.Text = "Calculate Ship Combat";
            this.calcShipCombatBut.UseVisualStyleBackColor = false;
            this.calcShipCombatBut.Click += new System.EventHandler(this.calcShipCombatBut_Click);
            // 
            // smallCraft
            // 
            this.smallCraft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(230)))), ((int)(((byte)(247)))));
            this.smallCraft.Location = new System.Drawing.Point(3, 121);
            this.smallCraft.Name = "smallCraft";
            this.smallCraft.Size = new System.Drawing.Size(131, 21);
            this.smallCraft.TabIndex = 9;
            this.smallCraft.Text = "Show Small Crafts";
            this.smallCraft.UseVisualStyleBackColor = false;
            this.smallCraft.Click += new System.EventHandler(this.smallCraft_Click);
            // 
            // ShipCrewAndDiceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.smallCraft);
            this.Controls.Add(this.calcShipCombatBut);
            this.Controls.Add(this.configureShipBut);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.shipReadout);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.shipDiceBox);
            this.Controls.Add(this.crewListBox);
            this.Controls.Add(this.configureCrewBut);
            this.Name = "ShipCrewAndDiceControl";
            this.Size = new System.Drawing.Size(1260, 158);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button configureCrewBut;
        private System.Windows.Forms.ListBox crewListBox;
        private System.Windows.Forms.ListBox shipDiceBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox shipReadout;
        private System.Windows.Forms.Button configureShipBut;
        private System.Windows.Forms.Button calcShipCombatBut;
        private System.Windows.Forms.Button smallCraft;
    }
}
