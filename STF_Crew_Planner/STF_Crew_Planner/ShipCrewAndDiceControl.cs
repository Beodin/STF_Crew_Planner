using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SqlNado;

namespace STF_CharacterPlanner
{
    public partial class ShipCrewAndDiceControl : UserControl
    {
        public List<String> SkillsList;
        public List<String> NumList;
        public DataStorage stf_Data;
        public List<String> SkillDisplayStrings;
        public List<String> CrewDisplayStrings;
        public List<String> ShipDisplayStrings;
        public DataTable CrewSkillDataTable;
        public DataTable OfficerSkillDataTable;
        public DataTable TotalSkillDataTable;
        public List<DataStorage.CrewDataStruct> theCrew;

        public ShipCrewAndDiceControl()
        {
            InitializeComponent();
            stf_Data = DataStorage.Instance;
            stf_Data.InstatiateTables();
            CrewSkillDataTable = new DataTable();
            OfficerSkillDataTable = new DataTable();
            TotalSkillDataTable = new DataTable();
            SkillDisplayStrings = new List<string>();
            CrewDisplayStrings = new List<string>();
            ShipDisplayStrings = new List<string>();
            theCrew = new List<DataStorage.CrewDataStruct>();
            CrewSkillDataTable = CreateSkillTable();
            OfficerSkillDataTable = CreateSkillTable();
            TotalSkillDataTable = CreateSkillTable();
        }

        private void configureCrewBut_Click(object sender, EventArgs e)
        {
            ConfigureCrewForm formMe = new ConfigureCrewForm();
            MainForm theForm = (this.Parent as MainForm);
            formMe.Show();
            formMe.SetPrevForm(theForm);
            formMe.shipCrewMenu1.SetPrevForm(theForm);
            if (theForm.theCrew.Count > 0)
            {
                formMe.CrewUpdateToOld(theForm.theCrew);
            }
        }

        private void configureShipBut_Click(object sender, EventArgs e)
        {
            ConfigureShipForm formMe = new ConfigureShipForm();
            MainForm theForm = (this.Parent as MainForm);
            formMe.Show();
            formMe.SetPrevForm(theForm);

            if (theForm.theShip.isSet)
            {
                formMe.ShipUpdate(theForm.theShip);
            }
        }
        public void UpdateCrewSkills()
        {
            theCrew.Clear();
            MainForm theForm = (this.Parent as MainForm);
            List<DataStorage.CrewDataStruct> aCrew = theForm.theCrew;
            if (aCrew == null)
            {
                return;
            }
            foreach (DataStorage.CrewDataStruct member in aCrew)
            {
                theCrew.Add(member);
            }
            SetCrewSkillPool();
            PollOfficerPool();
            CombinePools();
            SetSkillsDisplayString();
            SetCrewDisplayString();
            SetShipDisplayString();
        }
        private void SetShipDisplayString()
        {
            MainForm theForm = (this.Parent as MainForm);
            shipReadout.Items.Clear();
            if (theForm.theShip.isSet)
            {
                var dt = theForm.theShip.Ship.Copy();
                shipReadout.Items.Clear();
                ShipDisplayStrings.Clear();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        for (int x = 0; x < dt.Columns.Count; x++)
                        {
                            var colName = dt.Columns[x].ColumnName;
                            var aString = dr[x].ToString();
                            var shipString = ShipRowString(colName, aString);
                            if (colName.Equals("Tier"))
                            {

                            }else
                            {
                                ShipDisplayStrings.Add(shipString);
                            }
                        }
                    }
                }
                foreach (string myString in ShipDisplayStrings)
                {
                    shipReadout.Items.Add(myString);
                }
                shipReadout.Refresh();
            }
            else
            {
                shipReadout.Refresh();
            }

        }
        
        private string ShipRowString(string NameString, string ValueString)
        {
            var snglTab = "\t";
            var dblTab = "\t\t";
            var trpTab = "\t\t\t";
            var myString = "";
            if (NameString.Length > 15)
            {
                myString = NameString + ":" + snglTab + ValueString;
            }
            else if (NameString.Length > 6)
            {
                myString = NameString + ":" + snglTab + ValueString;
            }
            else
            {
                myString = NameString + ":" + dblTab + ValueString;
            }

            return myString;
        }
        private void SetCrewDisplayString()
        {
            crewListBox.Items.Clear();
            CrewDisplayStrings.Clear();

            var HeaderString = "Job\t\t\tNumber\t\tRank";
            CrewDisplayStrings.Add(HeaderString);
            foreach (DataStorage.CrewDataStruct aCrew in theCrew)
            {
                if (aCrew.Num > 0)
                {
                    var myString = CrewRowString(aCrew);
                    CrewDisplayStrings.Add(myString);
                }
            }
            foreach (string myString in CrewDisplayStrings)
            {
                crewListBox.Items.Add(myString);
            }
        }
        private string CrewRowString(DataStorage.CrewDataStruct aCrew)
        {
            var snglTab = "\t";
            var dblTab = "\t\t";
            var trpTab = "\t\t\t";
            var myString = "";
            if (aCrew.Job.Length > 15)
            {
                myString = aCrew.Job + snglTab + aCrew.Num + dblTab + aCrew.Rank;
            }
            else if (aCrew.Job.Length > 7)
            {
                myString = aCrew.Job + dblTab + aCrew.Num + dblTab + aCrew.Rank;
            }
            else
            {
                myString = aCrew.Job + trpTab + aCrew.Num + dblTab + aCrew.Rank;
            }

            return myString;
        }
        private void SetSkillsDisplayString()
        {
            SkillDisplayStrings.Clear();
            MainForm theForm = (this.Parent as MainForm);
            if (TotalSkillDataTable.Rows.Count > 0 && theForm.theShip.isSet)
            {
                var TitleString = "Skill" + "\t\t" + "Crew\t" + "Ship\t" + "Pct";
                SkillDisplayStrings.Add(TitleString);
                foreach (DataRow dr in TotalSkillDataTable.Rows)
                {
                    var checkName = dr[0].ToString();
                    if (checkName.Equals("Pilot") || checkName.Equals("Navigation") || checkName.Equals("Electronics") || checkName.Equals("Ship Ops") || checkName.Equals("Gunnery"))
                    {
                        var shipString = theForm.theShip.Ship.Rows[0][checkName].ToString();
                        var myString = DisplayRowPlusShipString(dr, shipString);
                        SkillDisplayStrings.Add(myString);
                    }
                }
                foreach (DataRow dr in TotalSkillDataTable.Rows)
                {
                    var myString = DisplayRowString(dr);
                    var checkName = dr[0].ToString();
                    if (checkName.Equals("Blades") || checkName.Equals("Pistols") || checkName.Equals("Evasion") || checkName.Equals("Rifles") || checkName.Equals("Pilot") || checkName.Equals("Rifles") || checkName.Equals("Navigation") || checkName.Equals("Electronics") || checkName.Equals("Ship Ops") || checkName.Equals("Gunnery"))
                    {

                    }
                    else
                    {
                        SkillDisplayStrings.Add(myString);
                    }
                }
            }else if (TotalSkillDataTable.Rows.Count > 0)
            {
                var TitleString = "Skill" + "\t\t" + "Crew";
                SkillDisplayStrings.Add(TitleString);
                foreach (DataRow dr in TotalSkillDataTable.Rows)
                {
                    var myString = DisplayRowString(dr);
                    var checkName = dr[0].ToString();
                    if (checkName.Equals("Blades") || checkName.Equals("Pistols") || checkName.Equals("Evasion") || checkName.Equals("Rifles"))
                    {

                    }else
                    {
                        SkillDisplayStrings.Add(myString);
                    }
                }
            }else {
                var myString = "No Crew Data to Display";
                SkillDisplayStrings.Add(myString);
            }
            shipDiceBox.Items.Clear();
            foreach (string aString in SkillDisplayStrings)
            {
                shipDiceBox.Items.Add(aString);
            }
            shipDiceBox.Refresh();
        }
        private string DisplayRowString(DataRow dr)
        {
            var snglTab = "\t";
            var dblTab = "\t\t";
            var trpTab = "\t\t\t";
            var myString = "";
            var SkillName = dr[0].ToString();
            var ValueName = dr[1].ToString();

            if (SkillName.Length > 15)
            {
                myString = SkillName + snglTab + ValueName;
            }
            else if (SkillName.Length > 7)
            {
                myString = SkillName + snglTab + ValueName;
            }
            else
            {
                myString = SkillName + dblTab + ValueName;
            }
            return myString;
        }
        private string DisplayRowPlusShipString(DataRow dr, string ShipValue)
        {
            var snglTab = "\t";
            var dblTab = "\t\t";
            var trpTab = "\t\t\t";
            var myString = "";
            var SkillName = dr[0].ToString();
            var ValueName = dr[1].ToString();
            var ShipPool = "(" + ShipValue + ")";
            var ShipName = ShipValue;
            double CrewDice = Int32.Parse(dr[1].ToString());
            double ShipDice = Int32.Parse(ShipValue);
            double PctValue = (CrewDice / ShipDice) * 100;
            PctValue = Math.Round(PctValue, 2);

            if (SkillName.Length > 15)
            {
                myString = SkillName + snglTab + ValueName + snglTab + ShipName + snglTab + PctValue.ToString();
            }
            else if (SkillName.Length > 7)
            {
                myString = SkillName + snglTab + ValueName + snglTab + ShipName + snglTab + PctValue.ToString();
            }
            else
            {
                myString = SkillName + dblTab + ValueName + snglTab + ShipName + snglTab + PctValue.ToString();
            }
            return myString;
        }

        private void CombinePools()
        {
            TotalSkillDataTable.Rows.Clear();
            TotalSkillDataTable = CreateSkillTable();
            if (OfficerSkillDataTable.Rows.Count > 0 && CrewSkillDataTable.Rows.Count > 0)
            {
                for (int x = 0; x < CrewSkillDataTable.Rows.Count; x++)
                {
                    var newInt = CrewSkillDataTable.Rows[x][1];
                    var oldInt = OfficerSkillDataTable.Rows[x][1];
                    int sumInt = Int32.Parse(newInt.ToString()) + Int32.Parse(oldInt.ToString());
                    TotalSkillDataTable.Rows[x][1] = sumInt;
                }
            }
        }
        private void PollOfficerPool()
        {
            OfficerSkillDataTable.Rows.Clear();
            OfficerSkillDataTable = CreateSkillTable();
            MainForm theForm = (this.Parent as MainForm);
            var Officer1 = new DataTable();
            var Officer2 = new DataTable();
            var Officer3 = new DataTable();
            var Officer4 = new DataTable();
            var Officer5 = new DataTable();
            var Officer6 = new DataTable();
            var Officer7 = new DataTable();
            Officer1 = theForm.bridgeMember1.officerSkillDisplay1.SkillDataTable.Copy();
            Officer2 = theForm.bridgeMember2.officerSkillDisplay1.SkillDataTable.Copy();
            Officer3 = theForm.bridgeMember3.officerSkillDisplay1.SkillDataTable.Copy();
            Officer4 = theForm.bridgeMember4.officerSkillDisplay1.SkillDataTable.Copy();
            Officer5 = theForm.bridgeMember5.officerSkillDisplay1.SkillDataTable.Copy();
            Officer6 = theForm.bridgeMember6.officerSkillDisplay1.SkillDataTable.Copy();
            Officer7 = theForm.bridgeMember7.officerSkillDisplay1.SkillDataTable.Copy();

            if (Officer1.Rows.Count > 0)
            {
                TabulateOfficerTable(Officer1);
            }
            if (Officer2.Rows.Count > 0)
            {
                TabulateOfficerTable(Officer2);
            }
            if (Officer3.Rows.Count > 0)
            {
                TabulateOfficerTable(Officer3);
            }
            if (Officer4.Rows.Count > 0)
            {
                TabulateOfficerTable(Officer4);
            }
            if (Officer5.Rows.Count > 0)
            {
                TabulateOfficerTable(Officer5);
            }
            if (Officer6.Rows.Count > 0)
            {
                TabulateOfficerTable(Officer6);
            }
            if (Officer7.Rows.Count > 0)
            {
                TabulateOfficerTable(Officer7);
            }
        }
        private void TabulateOfficerTable(DataTable dt)
        {
            for (int x = 0; x < dt.Rows.Count; x++)
            {
                var newInt = dt.Rows[x][1];
                var oldInt = OfficerSkillDataTable.Rows[x][1];
                int sumInt = Int32.Parse(newInt.ToString()) + Int32.Parse(oldInt.ToString());
                OfficerSkillDataTable.Rows[x][1] = sumInt;
            }
        }
        private void SetCrewSkillPool()
        {
            CrewSkillDataTable.Rows.Clear();
            CrewSkillDataTable = CreateSkillTable();
            foreach (DataStorage.CrewDataStruct aCrew in theCrew)
            {
                var newDT = GetSkillData(aCrew.Job, aCrew.Rank);
                PollSkillData(newDT, aCrew.Num);
            }
        }
        private DataTable GetSkillData(string term, Int32 theRank)
        {
            DataTable tblFiltered = stf_Data.skill_per_job_table.AsEnumerable().Where(row => row.Field<String>("Job") == term && row.Field<Int32>("Rank") == theRank).CopyToDataTable();
            var myObject = new DataObject();
            var theString = myObject.TableToString(tblFiltered);
            return tblFiltered;
        }
        private void PollSkillData(DataTable dt, int crewNumber)
        {
            if (dt == null)
            {
                return;
            }
            foreach (DataRow dr in dt.Rows)
            {
                string polledString1 = dr.Field<string>("1-Name");
                Int32 skillToAdd1 = dr.Field<Int32>("1-Num") * crewNumber;
                string polledString2 = dr.Field<string>("2-Name");
                Int32 skillToAdd2 = dr.Field<Int32>("2-Num") * crewNumber;
                string polledString3 = dr.Field<string>("3-Name");
                Int32 skillToAdd3 = dr.Field<Int32>("3-Num") * crewNumber;
                foreach (DataRow ds in CrewSkillDataTable.Rows)
                {
                    string testString = ds[0].ToString();
                    if (polledString1.Equals(testString))
                    {
                        Int32 skillAlready = Int32.Parse(ds[1].ToString());
                        Int32 newSkillNum = skillAlready + skillToAdd1;
                        ds[1] = newSkillNum;
                    }
                    if (polledString2.Equals(testString))
                    {
                        Int32 skillAlready = Int32.Parse(ds[1].ToString());
                        Int32 newSkillNum = skillAlready + skillToAdd2;
                        ds[1] = newSkillNum;
                    }
                    if (polledString3.Equals(testString))
                    {
                        Int32 skillAlready = Int32.Parse(ds[1].ToString());
                        Int32 newSkillNum = skillAlready + skillToAdd3;
                        ds[1] = newSkillNum;
                    }
                }
            }
        }
        private DataTable CreateSkillTable()
        {
            var dt = new DataTable();
            if (stf_Data == null)
            {
                return dt;
            }
            
            dt.TableName = "Skill List";
            DataColumn dc = new DataColumn();
            dc.ColumnName = "Skill";
            dc.DataType = typeof(String);
            dc.DefaultValue = "null";

            DataColumn dg = new DataColumn();
            dg.ColumnName = "Ranks";
            dg.DataType = typeof(Int32);
            dg.DefaultValue = 0;

            dt.Columns.Add(dc);
            dt.Columns.Add(dg);
            DataTable tblFiltered = stf_Data.skill_table.AsEnumerable().OrderBy(row => row.Field<String>("Skill")).CopyToDataTable();
            foreach (DataRow dr in tblFiltered.Rows)
            {
                DataRow newRow = dt.NewRow();
                newRow[0] = dr[0];
                newRow[1] = 0;
                dt.Rows.Add(newRow);
            }
            return dt;
        }

        private void calcShipCombatBut_Click(object sender, EventArgs e)
        {
            UpdateShipCombat();
        }
        private void UpdateShipCombat()
        {
            MainForm theForm = (this.Parent as MainForm);
            bool hasCombatPool = false;

            foreach (var myString in SkillDisplayStrings)
            {
                if (myString.Contains("Ship Combat"))
                {
                    hasCombatPool = true;
                }
            }
            if (!hasCombatPool)
            {
                CombatPools myCombat = new CombatPools();
                List<string> combatStrings = myCombat.CalculateCombatPools(theCrew, theForm);
                List<string> OldDisplayStrings = new List<string>();

                foreach (var myString in SkillDisplayStrings)
                {
                    OldDisplayStrings.Add(myString);
                }
                SkillDisplayStrings.Clear();

                foreach (var myString in combatStrings)
                {
                    Console.WriteLine(myString);
                    SkillDisplayStrings.Add(myString);
                }
                SkillDisplayStrings.Add("-----------------------------\n");
                foreach (var myString in OldDisplayStrings)
                {
                    SkillDisplayStrings.Add(myString);
                }

                shipDiceBox.Items.Clear();
                foreach (var myString in SkillDisplayStrings)
                {
                    shipDiceBox.Items.Add(myString);
                }
                shipDiceBox.Refresh();
            }
            
        }

        private void smallCraft_Click(object sender, EventArgs e)
        {
            SmallCraftForm frm = new SmallCraftForm();
            frm.Show();
        }
    }
}
