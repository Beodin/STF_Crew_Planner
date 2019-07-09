using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STF_CharacterPlanner
{
    public partial class Menu_Control : UserControl
    {
        MainForm myParent;

        public Menu_Control()
        {
            InitializeComponent();
            refreshWikiButton.Hide();
            myParent = (this.Parent as MainForm);
        }

        private void saveTextButton_Click(object sender, EventArgs e)
        {
            myParent = (this.Parent as MainForm);
            myParent.createNewTextFile();
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            myParent = (this.Parent as MainForm);
            myParent.resetCrewForms();
        }
        public string ReturnGroupName()
        {
            string myString = "";
            myString = groupNameBox.Text;
            return myString;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            myParent = (this.Parent as MainForm);
            myParent.SaveCrewTemplate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            myParent = (this.Parent as MainForm);
            myParent.LoadCrewTemplate();
        }
        public void SetGroupName(string GroupName)
        {
            groupNameBox.Text = GroupName;
        }

        private void searchTalentButton_Click(object sender, EventArgs e)
        {
            TalentSearchForm formMe = new TalentSearchForm();
            formMe.Show();
        }

        private void refreshWikiButton_Click(object sender, EventArgs e)
        {
            myParent = (this.Parent as MainForm);
            myParent.RefreshDataFromWiki();
        }

        private void saveTablesButton_Click(object sender, EventArgs e)
        {
            myParent = (this.Parent as MainForm);
            myParent.createNewTableFile();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //STF_DBExporter.Exporter();
        }
    }
}
