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
    public partial class OfficerControl : UserControl
    {
        public DataStorage stf_Data;
        public bool is_Captain;
        public Int32 maxRanks;
        public List<Int32> ranksList;
        public string Job1;
        public string Job2;
        public string Job3;
        public Int32 Rank1;
        public Int32 Rank2;
        public Int32 Rank3;
        public string whichOfficer;
        public DataTable SelectedJobs;
        public Int32 availableTalents;

        public OfficerControl()
        {
            InitializeComponent();
            is_Captain = false;
            stf_Data = DataStorage.Instance;
            stf_Data.InstatiateTables();
            SetDataForm1();

        }
        public void ResetOfficerControl()
        {
            SelectedJobs = setNewOfficerTable(whichOfficer);
            SetConfig();
            jobBox1.SelectedIndex = -1;
            jobBox2.SelectedIndex = -1;
            jobBox3.SelectedIndex = -1;
            rankJob1.SelectedIndex = -1;
            rankJob2.SelectedIndex = -1;
            rankJob3.SelectedIndex = -1;
            SelectedJobs.Rows.Clear();
            updateSkillShow();
        }
        public void setWhichOfficer(string theOne)
        {
            whichOfficer = theOne;
            selectedCrew.Text = theOne;
            SelectedJobs = setNewOfficerTable(theOne);
            if (theOne.Equals("Captain"))
            {
                is_Captain = true;
                maxRanks = 45;
                setLists();
                populateBoxes();
            }
        }
        private void SetDataForm1()
        {
            SetConfig();
            populateBoxes();
        }
        private void SetConfig()
        {
            maxRanks = 40;
            if (whichOfficer == null)
            {

            }else
            {
                if(whichOfficer.Equals("Captain"))
                    maxRanks = 45;
            }
            setLists();
            Job1 = "";
            Job2 = "";
            Job3 = "";
            Rank1 = 0;
            Rank2 = 0;
            Rank3 = 0;
            availableTalents = 0;
        }
        private void setLists()
        {
            ranksList = new List<Int32>();
            for (int count = 1; count < maxRanks; count++)
            {
                ranksList.Add(count);
            }
        }
        private void populateBoxes()
        {
            populateFirstJobBox();
            populateSecondJobBox();
            populateThirdJobBox();
            populateFirstRankBox();
            populateSecondRankBox();
            populateThirdRankBox();
            updateInfo();
        }
        private DataTable setNewOfficerTable(string name)
        {
            var dt = new DataTable();
            dt.TableName = name;
            DataColumn dc = new DataColumn();
            dc.ColumnName = "Job";
            dc.DataType = typeof(String);
            dc.DefaultValue = "null";

            DataColumn dg = new DataColumn();
            dg.ColumnName = "Ranks";
            dg.DataType = typeof(Int32);
            dg.DefaultValue = 0;

            dt.Columns.Add(dc);
            dt.Columns.Add(dg);

            return dt;
        }
        public DataTable getSelectedJobsTable()
        {
            DataTable newTable = new DataTable();
            newTable = SelectedJobs.Copy();
            return newTable;
        }
        private void updateInfo()
        {
            if (whichOfficer == null)
            {
                return;
            }
            updateDataHolder(SelectedJobs);
        }
        private void updateDataHolder(DataTable dt)
        {
            dt.Rows.Clear();
            if (Job1.Length > 1 & Rank1 > 0)
            {
                DataRow dr;
                dr = dt.NewRow();
                dr[0] = Job1;
                dr[1] = Rank1;
                dt.Rows.Add(dr);
            }
            if (Job2.Length > 1 & Rank2 > 0)
            {
                DataRow dr;
                dr = dt.NewRow();
                dr[0] = Job2;
                dr[1] = Rank2;
                dt.Rows.Add(dr);
            }
            if (Job3.Length > 1 & Rank3 > 0)
            {
                DataRow dr;
                dr = dt.NewRow();
                dr[0] = Job3;
                dr[1] = Rank3;
                dt.Rows.Add(dr);
            }
            updateSkillShow();
        }
        private void updateSkillShow()
        {
            var tempTalents = 0;
            availableTalents = 0;
            tempTalents = Rank1 + Rank2 + Rank3;

            if (tempTalents > 0)
            {
                if (is_Captain == true)
                {
                    foreach (DataRow dr in stf_Data.capt_talent_points_table.Rows)
                    {
                        Int32 rankToTest = Int32.Parse(dr[0].ToString());
                        if (rankToTest < tempTalents || rankToTest == tempTalents)
                        {
                            availableTalents++;
                        }
                    }
                }
                else
                {
                    foreach (DataRow dr in stf_Data.talent_points_table.Rows)
                    {
                        Int32 rankToTest = Int32.Parse(dr[0].ToString());
                        if (rankToTest < tempTalents || rankToTest == tempTalents)
                        {
                            availableTalents++;
                        }
                    }
                }
            }

            BridgeMember myParent = (this.Parent as BridgeMember);
            myParent.updateSkillShow(SelectedJobs, availableTalents);
            MainForm theForm = (myParent.Parent as MainForm);
            theForm.shipCrewAndDiceControl1.UpdateCrewSkills();
        }
        private void populateFirstJobBox()
        {
            if (jobBox1.SelectedItem != null)
            {
                return;
            }
            DataTable dt = new DataTable();
            if (is_Captain == true)
            {
                dt = stf_Data.capt_job_list;
            }
            else
            {
                dt = stf_Data.job_list;
            }

            int count = dt.Rows.Count;
            if (count > 0)
            {
                jobBox1.Items.Clear();
                for (int i = 0; i < count; i++)
                {
                    string jobToAdd = dt.Rows[i][0].ToString();
                    if (jobToAdd.Equals(Job2) || jobToAdd.Equals(Job3))
                    {

                    }
                    else
                    {
                        jobBox1.Items.Add(jobToAdd);
                    }
                }
            }
        }
        private void populateSecondJobBox()
        {
            if (jobBox2.SelectedItem != null)
            {
                return;
            }
            DataTable dt = new DataTable();
            dt = stf_Data.job_list;

            int count = dt.Rows.Count;
            if (count > 0)
            {
                jobBox2.Items.Clear();
                for (int i = 0; i < count; i++)
                {
                    string jobToAdd = dt.Rows[i][0].ToString();
                    if (jobToAdd.Equals(Job1) || jobToAdd.Equals(Job3))
                    {

                    }
                    else
                    {
                        jobBox2.Items.Add(jobToAdd);
                    }
                }
            }
        }
        private void populateThirdJobBox()
        {
            if (jobBox3.SelectedItem != null)
            {
                return;
            }
            DataTable dt = new DataTable();
            dt = stf_Data.job_list;

            int count = dt.Rows.Count;
            if (count > 0)
            {
                jobBox3.Items.Clear();
                for (int i = 0; i < count; i++)
                {
                    string jobToAdd = dt.Rows[i][0].ToString();
                    if (jobToAdd.Equals(Job1) || jobToAdd.Equals(Job2))
                    {

                    }
                    else
                    {
                        jobBox3.Items.Add(jobToAdd);
                    }
                }
                
            }
        }
        private void populateFirstRankBox()
        {
            if (rankJob1.SelectedItem != null & Rank1 > 0)
            {
                return;
            }
            rankJob1.Items.Clear();
            var ourNum = ranksList.Count - (Rank2 + Rank3);
            if (ourNum > 32)
            {
                ourNum = 32;
            }
            for (int counter = 0; counter < ourNum; counter++)
            {
                rankJob1.Items.Add(ranksList[counter]);
            }
        }
        private void populateSecondRankBox()
        {
            if (rankJob2.SelectedItem != null & Rank2 > 0)
            {
                return;
            }
            rankJob2.Items.Clear();
            var ourNum = ranksList.Count - (Rank1 + Rank3);
            if (ourNum > 32)
            {
                ourNum = 32;
            }
            for (int counter = 0; counter < ourNum; counter++)
            {
                rankJob2.Items.Add(ranksList[counter]);
            }
        }
        private void populateThirdRankBox()
        {
            if (rankJob3.SelectedItem != null & Rank3 > 0)
            {
                return;
            }
            rankJob3.Items.Clear();
            var ourNum = ranksList.Count - (Rank2 + Rank1);
            if (ourNum > 32)
            {
                ourNum = 32;
            }
            for (int counter = 0; counter < ourNum; counter++)
            {
                rankJob3.Items.Add(ranksList[counter]);
            }
        }

        private void jobBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = jobBox1.SelectedItem;

            if (item == null)
                return;
            if (item.ToString().Equals("None"))
            {
                jobBox1.SelectedIndex = -1;
                rankJob1.SelectedIndex = -1;
                Job1 = "";
                Rank1 = 0;
                populateBoxes();
                return;
            }
            Job1 = item.ToString();
            populateBoxes();
        }
        

        private void jobBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            var item = jobBox2.SelectedItem;

            if (item == null)
                return;
            if (item.ToString().Equals("None"))
            {
                jobBox2.SelectedIndex = -1;
                rankJob2.SelectedIndex = -1;
                Job2 = "";
                Rank2 = 0;
                populateBoxes();
                return;
            }
            Job2 = item.ToString();
            populateBoxes();
        }

        private void jobBox3_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            var item = jobBox3.SelectedItem;

            if (item == null)
                return;
            if (item.ToString().Equals("None"))
            {
                jobBox3.SelectedIndex = -1;
                rankJob3.SelectedIndex = -1;
                Job3 = "";
                Rank3 = 0;
                populateBoxes();
                return;
            }
            Job3 = item.ToString();
            populateBoxes();
        }
        private void jobBox1_clickedTarget(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            if (is_Captain == true)
            {
                dt = stf_Data.capt_job_list;
            }
            else
            {
                dt = stf_Data.job_list;
            }

            int count = dt.Rows.Count;
            if (count > 0)
            {
                jobBox1.Items.Clear();
                for (int i = 0; i < count; i++)
                {
                    string jobToAdd = dt.Rows[i][0].ToString();
                    if (jobToAdd.Equals(Job2) || jobToAdd.Equals(Job3))
                    {

                    }
                    else
                    {
                        jobBox1.Items.Add(jobToAdd);
                    }
                }
            }
        }
        private void jobBox2_clickedTarget(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt = stf_Data.job_list;

            int count = dt.Rows.Count;
            if (count > 0)
            {
                jobBox2.Items.Clear();
                for (int i = 0; i < count; i++)
                {
                    string jobToAdd = dt.Rows[i][0].ToString();
                    if (jobToAdd.Equals(Job1) || jobToAdd.Equals(Job3))
                    {

                    }
                    else
                    {
                        jobBox2.Items.Add(jobToAdd);
                    }
                }
            }
        }
        private void jobBox3_clickedTarget(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt = stf_Data.job_list;

            int count = dt.Rows.Count;
            if (count > 0)
            {
                jobBox3.Items.Clear();
                for (int i = 0; i < count; i++)
                {
                    string jobToAdd = dt.Rows[i][0].ToString();
                    if (jobToAdd.Equals(Job1) || jobToAdd.Equals(Job2))
                    {

                    }
                    else
                    {
                        jobBox3.Items.Add(jobToAdd);
                    }
                }
            }
        }
        private void rankJob1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            var item = rankJob1.SelectedItem;

            if (item == null)
                return;
            Rank1 = (int)item;
            populateBoxes();
        }

        private void rankJob2_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            var item = rankJob2.SelectedItem;

            if (item == null)
                return;
            Rank2 = (int)item;
            populateBoxes();
        }

        private void rankJob3_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            var item = rankJob3.SelectedItem;

            if (item == null)
                return;
            Rank3 = (int)item;
            populateBoxes();
        }
        private void rankJob1_clickedTarget(object sender, EventArgs e)
        {
            rankJob1.Items.Clear();
            var ourNum = ranksList.Count - (Rank2 + Rank3);
            if (ourNum > 32)
            {
                ourNum = 32;
            }
            for (int counter = 0; counter < ourNum; counter++)
            {
                rankJob1.Items.Add(ranksList[counter]);
            }
        }
        private void rankJob2_clickedTarget(object sender, EventArgs e)
        {
            rankJob2.Items.Clear();
            var ourNum = ranksList.Count - (Rank1 + Rank3);
            if (ourNum > 32)
            {
                ourNum = 32;
            }
            for (int counter = 0; counter < ourNum; counter++)
            {
                rankJob2.Items.Add(ranksList[counter]);
            }
        }
        private void rankJob3_clickedTarget(object sender, EventArgs e)
        {
            rankJob3.Items.Clear();
            var ourNum = ranksList.Count - (Rank2 + Rank1);
            if (ourNum > 32)
            {
                ourNum = 32;
            }
            for (int counter = 0; counter < ourNum; counter++)
            {
                rankJob3.Items.Add(ranksList[counter]);
            }
        }
        public void LoadFromTemplate(DataTable SelectJobs)
        {
            int x = 0;
            foreach (DataRow dr in SelectJobs.Rows)
            {
                ReplaceJobRow(dr, x);
                x++;
            }
        }
        private void ReplaceJobRow(DataRow theRow, int rowNumber)
        {
            string jobName = theRow[0].ToString();
            Int32 jobRank = Int32.Parse(theRow[1].ToString());

            if (rowNumber == 0)
            {
                JobSelectRow(jobName, jobBox1);
                RankSelectRow(jobRank, rankJob1);
            }
            if (rowNumber == 1)
            {
                JobSelectRow(jobName, jobBox2);
                RankSelectRow(jobRank, rankJob2);
            }
            if (rowNumber == 2)
            {
                JobSelectRow(jobName, jobBox3);
                RankSelectRow(jobRank, rankJob3);
            }
        }
        private void JobSelectRow (string newJobName, ComboBox theBox)
        {
            foreach (var theItem in theBox.Items)
            {
                if (theItem.ToString().Equals(newJobName))
                {
                    theBox.SelectedItem = theItem;
                }
            }
        }
        private void RankSelectRow(Int32 newRankNum, ComboBox theBox)
        {
            var newCheck = newRankNum.ToString();
            foreach (var theItem in theBox.Items)
            {
                if (theItem.ToString().Equals(newCheck))
                {
                    theBox.SelectedItem = theItem;
                }
            }
        }
    }
}
