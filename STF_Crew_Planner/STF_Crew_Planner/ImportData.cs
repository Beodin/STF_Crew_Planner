using System;
using System.Data;
using System.IO;
using System.Text;
using CoreTechs.Common.Text;
using System.Linq;
using System.Collections.Generic;

namespace STF_CharacterPlanner
{
    public sealed class DataStorage
    {
        private static readonly Lazy<DataStorage> lazy =
            new Lazy<DataStorage>(() => new DataStorage());

        public static DataStorage Instance { get { return lazy.Value; } }
        public DataTable job_table;
        public DataTable skill_table;
        public DataTable skill_per_job_table;
        public DataTable stf_talent_job_table;
        public DataTable job_list;
        public DataTable capt_job_list;
        public DataTable talent_points_table;
        public DataTable capt_talent_points_table;
        public DataTable CaptainInfo;
        public DataTable firstOfficerInfo;
        public DataTable secondOfficerInfo;
        public DataTable thirdOfficerInfo;
        public DataTable fourthOfficerInfo;
        public DataTable STF_Ship_Data;
        public DataTable STF_Ship_Defaults;
        public DataTable STF_Ship_Components;
        public DataTable STF_Ship_Weapons;
        public DataTable STF_Ship_Engine;
        public DataTable small_craft_table;
        public BridgeMember member1;
        public BridgeMember member2;
        public BridgeMember member3;
        public BridgeMember member4;
        public BridgeMember member5;

        private DataStorage()
        {
        }
        public struct CrewDataStruct
        {
            public string Job;
            public int Num;
            public int Rank;
        }
        public struct SelectedComponents
        {
            public DataTable Small;
            public DataTable Medium;
            public DataTable Large;
        }
        public struct SelectedShip
        {
            public SelectedComponents Components;
            public DataTable Ship;
            public string Name;
            public bool isSet;
        }

        public void InstatiateTables()
        {
            createOfficerTables();
            //Load Game Data
            job_table = loadDataFromCSV(File.ReadAllText(Directory.GetCurrentDirectory() + @"\STFPlannerFiles\job_list.csv"));
            //job_table = loadDataFromCSV(Properties.Resources.job_list);
            skill_table = loadDataFromCSV(File.ReadAllText(Directory.GetCurrentDirectory() + @"\STFPlannerFiles\skill_list.csv"));
            //skill_table = loadDataFromCSV(Properties.Resources.skill_list);
            skill_per_job_table = loadDataFromCSV(File.ReadAllText(Directory.GetCurrentDirectory() + @"\STFPlannerFiles\skill_per_job_list.csv"));
            stf_talent_job_table = loadDataFromCSV(File.ReadAllText(Directory.GetCurrentDirectory() + @"\STFPlannerFiles\stf_talent_job.csv"));
            talent_points_table = loadDataFromCSV(File.ReadAllText(Directory.GetCurrentDirectory() + @"\STFPlannerFiles\talent_points.csv"));
            STF_Ship_Data = loadDataFromCSV(File.ReadAllText(Directory.GetCurrentDirectory() + @"\STFPlannerFiles\STF_Ship_Data.csv"));
            STF_Ship_Defaults = loadDataFromCSV(File.ReadAllText(Directory.GetCurrentDirectory() + @"\STFPlannerFiles\STF_Ship_Default_Comp.csv"));
            STF_Ship_Components = loadDataFromCSV(File.ReadAllText(Directory.GetCurrentDirectory() + @"\STFPlannerFiles\ship_components.csv"));
            STF_Ship_Weapons = loadDataFromCSV(File.ReadAllText(Directory.GetCurrentDirectory() + @"\STFPlannerFiles\stf_weapon_data.csv"));
            STF_Ship_Engine = loadDataFromCSV(File.ReadAllText(Directory.GetCurrentDirectory() + @"\STFPlannerFiles\stf_engine_data.csv"));
            capt_talent_points_table = loadDataFromCSV(File.ReadAllText(Directory.GetCurrentDirectory() + @"\STFPlannerFiles\cap_talent_points.csv"));
            small_craft_table = loadDataFromCSV(File.ReadAllText(Directory.GetCurrentDirectory() + @"\STFPlannerFiles\smallCraft.csv"));

            //Create Job Lists
            job_list = CreateJobList(job_table);
            capt_job_list = CreateCaptJobList(job_table);


            //Reconfigure integer rows
            SetShipDataTablesToInt();
            SetCompDataTablesToInt();
            SetShipWeaponTablesToInt();
            SetShipEngineTablesToInt();

            ChangeColumnType(stf_talent_job_table, "Rank", typeof(int));
            ChangeColumnType(skill_per_job_table, "Rank", typeof(int));
            ChangeColumnType(skill_per_job_table, "1-Num", typeof(int));
            ChangeColumnType(skill_per_job_table, "2-Num", typeof(int));
            ChangeColumnType(skill_per_job_table, "3-Num", typeof(int));
        }
        public void SetShipDataTablesToInt()
        {
            var STF_Ship_List = new List<string>();
            foreach (DataColumn theColumn in STF_Ship_Data.Columns)
            {
                string columnName = theColumn.ColumnName.ToString();
                if (columnName.Equals("Ship") || columnName.Equals("Price") || columnName.Equals("Engine"))
                {

                }else
                {
                    STF_Ship_List.Add(columnName);
                }
            }
            foreach (var theString in STF_Ship_List)
            {
                ChangeColumnType(STF_Ship_Data, theString, typeof(int));
            }
            STF_Ship_Data = STF_Ship_Data.AsEnumerable().OrderBy(row => row.Field<int>("Max Mass")).ThenBy(row => row.Field<string>("Price")).CopyToDataTable();
        }
        public void SetShipWeaponTablesToInt()
        {
            var STF_Data_List = new List<string>();
            foreach (DataColumn theColumn in STF_Ship_Weapons.Columns)
            {
                string columnName = theColumn.ColumnName.ToString();
                if (columnName.Equals("Name") || columnName.Equals("Type") || (columnName.Equals("Range")))
                {

                }
                else
                {
                    STF_Data_List.Add(columnName);
                }
            }
            foreach (var theString in STF_Data_List)
            {
                ChangeColumnType(STF_Ship_Weapons, theString, typeof(int));
            }
            foreach (DataRow dr in STF_Ship_Weapons.Rows)
            {
                int MainRange = Int32.Parse(dr["Range"].ToString());
                int LowRange = MainRange - 1;
                int MaxRange = MainRange + 1;
                if (LowRange < 1)
                {
                    LowRange = 1;
                }
                if (MaxRange > 5)
                {
                    MaxRange = 5;
                }
                var myString = LowRange.ToString() + "-" + MaxRange.ToString();
                dr["Range"] = myString;
                dr.AcceptChanges();
            }
            STF_Ship_Weapons = STF_Ship_Weapons.AsEnumerable().OrderBy(row => row.Field<string>("Type")).ThenBy(row => row.Field<string>("Name")).CopyToDataTable();
        }
        public void SetShipEngineTablesToInt()
        {
            var STF_Data_List = new List<string>();
            foreach (DataColumn theColumn in STF_Ship_Engine.Columns)
            {
                string columnName = theColumn.ColumnName.ToString();
                if (columnName.Equals("Name"))
                {

                }
                else
                {
                    STF_Data_List.Add(columnName);
                }
            }
            foreach (var theString in STF_Data_List)
            {
                ChangeColumnType(STF_Ship_Engine, theString, typeof(int));
            }
        }
        public void SetCompDataTablesToInt()
        {
            var STF_Comp_List = new List<string>();
            foreach (DataColumn theColumn in STF_Ship_Components.Columns)
            {
                string columnName = theColumn.ColumnName.ToString();
                if (columnName.Equals("Name") || columnName.Equals("Size"))
                {

                }
                else
                {
                    STF_Comp_List.Add(columnName);
                }
            }
            foreach (var theString in STF_Comp_List)
            {
                ChangeColumnType(STF_Ship_Components, theString, typeof(int));
            }
            STF_Ship_Data = STF_Ship_Data.AsEnumerable().OrderBy(row => row.Field<int>("Max Mass")).ThenBy(row => row.Field<string>("Price")).CopyToDataTable();
        }
        public void setBridgeMember1(BridgeMember memberAye)
        {
            member1 = memberAye;
        }
        public void setBridgeMember2(BridgeMember memberAye)
        {
            member2 = memberAye;
        }
        public void setBridgeMember3(BridgeMember memberAye)
        {
            member3 = memberAye;
        }
        public void setBridgeMember4(BridgeMember memberAye)
        {
            member4 = memberAye;
        }
        public void setBridgeMember5(BridgeMember memberAye)
        {
            member5 = memberAye;
        }
        public void createOfficerTables()
        {
            CaptainInfo = setNewOfficerTable("Captain");
            firstOfficerInfo = setNewOfficerTable("First Officer");
            secondOfficerInfo = setNewOfficerTable("Second Officer");
            thirdOfficerInfo = setNewOfficerTable("Third Officer");
            fourthOfficerInfo = setNewOfficerTable("Fourth Officer");
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

        private DataTable loadDataFromCSV(string theString)
        {
            var table = new DataTable();
            TextReader reader = new StringReader(theString);
            using (var it = reader.ReadCsvWithHeader(null, ':', '"').GetEnumerator())
            {

                if (!it.MoveNext()) return table;

                foreach (var k in it.Current.Keys)
                    table.Columns.Add(k);

                do
                {
                    var row = table.NewRow();
                    foreach (var k in it.Current.Keys)
                        row[k] = it.Current[k];

                    table.Rows.Add(row);
                } while (it.MoveNext());
            }
            return table;
        }

        private static void SortDataTable(DataTable dt, string col_name, string term)
        {
            DataTable tblFiltered = dt.AsEnumerable().Where(row => row.Field<String>(col_name) == term).OrderBy(row => row.Field<String>("Job")).CopyToDataTable();
            var myObject = new DataObject();
            var theString = myObject.TableToString(tblFiltered);
            Console.WriteLine(theString);
        }
        private DataTable CreateFilteredTable(DataTable dt, string col_name, string ordered, string term)
        {
            DataTable tblFiltered = dt.AsEnumerable().Where(row => row.Field<String>(col_name) == term).OrderBy(row => row.Field<Int32>(ordered)).CopyToDataTable();
            var myObject = new DataObject();
            var theString = myObject.TableToString(tblFiltered);
            Console.WriteLine(theString);
            return tblFiltered;
        }
        private void ChangeColumnType(DataTable dt, string p, Type type)
        {
            dt.Columns.Add(p + "_new", type);
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                dr[p + "_new"] = Int32.Parse(dr[p].ToString());
            }
            dt.Columns.Remove(p);
            dt.Columns[p + "_new"].ColumnName = p;
        }

        private DataTable CreateJobList(DataTable dt)
        {
            var newDataTable = new DataTable();
            newDataTable.Columns.Add("Job");

            foreach (DataRow item in dt.Rows)
                newDataTable.Rows.Add(item[0]);

            return newDataTable;
        }
        private DataTable CreateCaptJobList(DataTable dt)
        {
            var newDataTable = new DataTable();
            newDataTable.Columns.Add("Job");

            DataTable tblFiltered = dt.AsEnumerable().Where(row => row.Field<String>("Starter") == "Yes").OrderBy(row => row.Field<String>("Job")).CopyToDataTable();

            foreach (DataRow item in tblFiltered.Rows)
                newDataTable.Rows.Add(item[0]);

            return newDataTable;
        }
        public void TestTable(DataTable dt)
        {
            var myObject = new DataObject();

            var myString = myObject.TableToString(dt);
            Console.WriteLine(myString);
        }
    }
    class DataObject
    {
        public string TableToString(DataTable dt)
        {
            StringBuilder TableData = new StringBuilder();
            foreach (DataColumn dc in dt.Columns)
                TableData.AppendFormat("\t{0}", dc.ColumnName);
            TableData.AppendFormat("\n");

            int j = -1;
            foreach (DataRow dr in dt.Rows)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    TableData.AppendFormat("\t{0}", dr[dc]);
                }
                TableData.AppendFormat("\n");
            }
            return TableData.ToString();
        }
        public string EvenColumns(int desiredWidth, IEnumerable<IEnumerable<string>> lists)
        {
            return string.Join(Environment.NewLine, EvenColumns(desiredWidth, true, lists));
        }

        public IEnumerable<string> EvenColumns(int desiredWidth, bool rightOrLeft, IEnumerable<IEnumerable<string>> lists)
        {
            return lists.Select(o => EvenColumns(desiredWidth, rightOrLeft, o.ToArray()));
        }
        public bool DataTableValid(DataTable dt)
        {
            bool myBool = false;

            if (dt.Rows.Count > 0)
            {
                myBool = true;
            }

            return myBool;
        }
        public string EvenColumns(int desiredWidth, bool rightOrLeftAlignment, string[] list, bool fitToItems = false)
        {
            int columnWidth = (rightOrLeftAlignment ? -1 : 1) *
                                (fitToItems
                                    ? Math.Max(desiredWidth, list.Select(o => o.Length).Max())
                                    : desiredWidth
                                );

            string format = string.Concat(Enumerable.Range(rightOrLeftAlignment ? 0 : 1, list.Length - 1).Select(i => string.Format("{{{0},{1}}}", i, columnWidth)));

            if (rightOrLeftAlignment)
            {
                format += "{" + (list.Length - 1) + "}";
            }
            else
            {
                format = "{0}" + format;
            }

            return string.Format(format, list);
        }
        
    }
    class CombatPools
    {
        public DataTable CrewSkillPools;
        public DataTable OfficerSkillPools;
        public DataTable TotalSkillDataTable;
        public DataStorage.SelectedShip MyShip;
        public int Tactics;
        public int Command;
        public int Electronics;
        public int Gunnery;
        public int Pilot;
        public int Navigation;
        public int Speed;
        public int Agility;

        public List<string> CalculateCombatPools(List<DataStorage.CrewDataStruct> theCrew, MainForm theForm)
        {

            List<string> myStrings = new List<string>();
            DataStorage stf_Data = theForm.ReturnMainData();

            TotalSkillDataTable = new DataTable();
            TotalSkillDataTable = CreateSkillTable(theForm);
            MyShip = new DataStorage.SelectedShip();
            MyShip = theForm.theShip;

            PollOfficerPool(theForm);
            SetCrewSkillPool(theCrew,theForm);
            CreateCrewDicePools();
            SetStats();

            int gunneryL = Speed + Navigation + Gunnery + Tactics;
            int gunneryS = Agility + Pilot + Gunnery + Tactics;

            int evadeL = Speed + Pilot + Electronics + Command;
            int evadeS = Agility + Pilot + Electronics + Command;

            int escape = Speed + Pilot + Tactics + Command;

            int rangeL = Speed + Navigation + Tactics + Command;
            int rangeS = Agility + Pilot + Tactics + Command;

            myStrings.Add("Ship Combat Pool\tDice");
            string LongRangeAcc = "Long Accuracy:\t\t" + gunneryL;
            myStrings.Add(LongRangeAcc);
            string ShortRangeAcc = "Short Accuracy:\t\t" + gunneryS;
            myStrings.Add(ShortRangeAcc);

            string EvadeLong = "Long Evasion:\t\t" + evadeL;
            myStrings.Add(EvadeLong);
            string EvadeShort = "Short Evasion:\t\t" + evadeS;
            myStrings.Add(EvadeShort);

            string RangeLong = "Long Change:\t\t" + rangeL;
            myStrings.Add(RangeLong);
            string RangeShort = "Short Change:\t\t" + rangeS;
            myStrings.Add(RangeShort);
            string EscapeMe = "Escape:\t\t\t" + escape;
            myStrings.Add(EscapeMe);

            //long range gunnery (speed, navigation, gunnery*, Tactics*)
            //short range gunnery (agility, pilot, gunnery*, Tactics*)

            //evade long range (speed, pilot, electronics*, command*)
            //evade short range (agility, pilot, electronics*, command*)

            //escape (speed, nav, tactics*, command*)

            //long range range change (speed, nav, tactics*, command*)
            //short range change (agility, pilot, tactics*, command*)
            return myStrings;
        }
        public List<string> CalculateOutputCombatPools(List<DataStorage.CrewDataStruct> theCrew, MainForm theForm)
        {

            List<string> myStrings = new List<string>();
            DataStorage stf_Data = theForm.ReturnMainData();

            TotalSkillDataTable = new DataTable();
            TotalSkillDataTable = CreateSkillTable(theForm);
            MyShip = new DataStorage.SelectedShip();
            MyShip = theForm.theShip;

            PollOfficerPool(theForm);
            SetCrewSkillPool(theCrew,theForm);
            CreateCrewDicePools();
            SetStats();

            int gunneryL = Speed + Navigation + Gunnery + Tactics;
            int gunneryS = Agility + Pilot + Gunnery + Tactics;

            int evadeL = Speed + Pilot + Electronics + Command;
            int evadeS = Agility + Pilot + Electronics + Command;

            int escape = Speed + Pilot + Tactics + Command;

            int rangeL = Speed + Navigation + Tactics + Command;
            int rangeS = Agility + Pilot + Tactics + Command;

            myStrings.Add("Ship Combat Pool\tDice");
            string LongRangeAcc = "Long Accuracy:\t\t" + gunneryL;
            myStrings.Add(LongRangeAcc);
            string ShortRangeAcc = "Short Accuracy:\t\t" + gunneryS;
            myStrings.Add(ShortRangeAcc);

            string EvadeLong = "Long Evasion:\t\t" + evadeL;
            myStrings.Add(EvadeLong);
            string EvadeShort = "Short Evasion:\t\t" + evadeS;
            myStrings.Add(EvadeShort);

            string RangeLong = "Long Change:\t\t" + rangeL;
            myStrings.Add(RangeLong);
            string RangeShort = "Short Change:\t\t" + rangeS;
            myStrings.Add(RangeShort);
            string EscapeMe = "Escape:\t\t\t\t" + escape;
            myStrings.Add(EscapeMe);

            //long range gunnery (speed, navigation, gunnery*, Tactics*)
            //short range gunnery (agility, pilot, gunnery*, Tactics*)

            //evade long range (speed, pilot, electronics*, command*)
            //evade short range (agility, pilot, electronics*, command*)

            //escape (speed, nav, tactics*, command*)

            //long range range change (speed, nav, tactics*, command*)
            //short range change (agility, pilot, tactics*, command*)
            return myStrings;
        }
        public DataTable CalculateTableCombatPools(List<DataStorage.CrewDataStruct> theCrew, MainForm theForm)
        {

            List<string> myStrings = new List<string>();
            DataStorage stf_Data = theForm.ReturnMainData();

            TotalSkillDataTable = new DataTable();
            TotalSkillDataTable = CreateSkillTable(theForm);
            MyShip = new DataStorage.SelectedShip();
            MyShip = theForm.theShip;

            PollOfficerPool(theForm);
            SetCrewSkillPool(theCrew,theForm);
            CreateCrewDicePools();
            SetStats();

            int gunneryL = Speed + Navigation + Gunnery + Tactics;
            int gunneryS = Agility + Pilot + Gunnery + Tactics;

            int evadeL = Speed + Pilot + Electronics + Command;
            int evadeS = Agility + Pilot + Electronics + Command;

            int escape = Speed + Pilot + Tactics + Command;

            int rangeL = Speed + Navigation + Tactics + Command;
            int rangeS = Agility + Pilot + Tactics + Command;

            var dt = new DataTable();
            DataColumn dc = new DataColumn();
            dc.ColumnName = "Type";
            dc.DataType = typeof(String);
            dc.DefaultValue = "null";

            DataColumn dg = new DataColumn();
            dg.ColumnName = "Dice";
            dg.DataType = typeof(Int32);
            dg.DefaultValue = 0;

            dt.Columns.Add(dc);
            dt.Columns.Add(dg);

            DataRow longAcc = dt.NewRow();
            DataRow shortAcc = dt.NewRow();
            DataRow longEvade = dt.NewRow();
            DataRow shortEvade = dt.NewRow();
            DataRow longRange = dt.NewRow();
            DataRow shortRange = dt.NewRow();
            DataRow escapeRow = dt.NewRow();

            longAcc[0] = "Long Accuracy";
            longAcc[1] = gunneryL;
            shortAcc[0] = "Short Accuracy";
            shortAcc[1] = gunneryS;

            longEvade[0] = "Long Evasion";
            longEvade[1] = evadeL;
            shortEvade[0] = "Short Evasion";
            shortEvade[1] = evadeS;

            longRange[0] = "Long Range Change";
            longRange[1] = rangeL;
            shortRange[0] = "Short Range Change";
            shortRange[1] = rangeS;
            escapeRow[0] = "Escape";
            escapeRow[1] = escape;

            dt.Rows.Add(longAcc);
            dt.Rows.Add(shortAcc);
            dt.Rows.Add(longEvade);
            dt.Rows.Add(shortEvade);
            dt.Rows.Add(longRange);
            dt.Rows.Add(shortRange);
            dt.Rows.Add(escapeRow);

            return dt;
        }
        private void PrintTest()
        {
            Console.WriteLine("Pilot: " + Pilot);
            Console.WriteLine("Navigation: " + Navigation);
            Console.WriteLine("Electronics: " + Electronics);
            Console.WriteLine("Gunnery: " + Gunnery);
            Console.WriteLine("Command: " + Command);
            Console.WriteLine("Tactics: " + Tactics);
            Console.WriteLine("Speed: " + Speed);
            Console.WriteLine("Agility: " + Agility);
        }
        private void SetStats()
        {
            Tactics = 0;
            Command = 0;
            Pilot = 0;
            Navigation = 0;
            Electronics = 0;
            Gunnery = 0;
            Speed = 0;
            Agility = 0;

            if (MyShip.isSet)
            {
                Speed = Int32.Parse(MyShip.Ship.Rows[0]["Speed"].ToString());
                Agility = Int32.Parse(MyShip.Ship.Rows[0]["Agility"].ToString());

                foreach (DataRow dr in TotalSkillDataTable.Rows)
                {
                    if (dr[0].ToString().Equals("Tactics"))
                    {
                        Tactics = Int32.Parse(dr[1].ToString());
                        Tactics = Tactics / 2;
                    }
                    if (dr[0].ToString().Equals("Command"))
                    {
                        Command = Int32.Parse(dr[1].ToString());
                        Command = Command / 2;
                    }
                    if (dr[0].ToString().Equals("Pilot"))
                    {
                        Pilot = Int32.Parse(dr[1].ToString());
                        int ShipPilot = Int32.Parse(MyShip.Ship.Rows[0]["Pilot"].ToString());
                        if (Pilot > ShipPilot)
                        {
                            Pilot = ShipPilot;
                        }
                    }
                    if (dr[0].ToString().Equals("Navigation"))
                    {
                        Navigation = Int32.Parse(dr[1].ToString());
                        int ShipNav = Int32.Parse(MyShip.Ship.Rows[0]["Navigation"].ToString());
                        if (Navigation > ShipNav)
                        {
                            Navigation = ShipNav;
                        }
                    }
                    if (dr[0].ToString().Equals("Electronics"))
                    {
                        Electronics = Int32.Parse(dr[1].ToString());
                        int ShipEle = Int32.Parse(MyShip.Ship.Rows[0]["Electronics"].ToString());
                        if (Electronics > ShipEle)
                        {
                            Electronics = ShipEle;
                        }
                        Electronics = Electronics / 2;
                    }
                    if (dr[0].ToString().Equals("Gunnery"))
                    {
                        Gunnery = Int32.Parse(dr[1].ToString());
                        int ShipGun = Int32.Parse(MyShip.Ship.Rows[0]["Gunnery"].ToString());
                        if (Gunnery > ShipGun)
                        {
                            Gunnery = ShipGun;
                        }
                        Gunnery = Gunnery / 2;
                    }
                }
            }
        }
        private void CreateCrewDicePools()
        {
            if (OfficerSkillPools.Rows.Count > 0 && CrewSkillPools.Rows.Count > 0)
            {
                for (int x = 0; x < CrewSkillPools.Rows.Count; x++)
                {
                    var newInt = CrewSkillPools.Rows[x][1];
                    var oldInt = OfficerSkillPools.Rows[x][1];
                    int sumInt = Int32.Parse(newInt.ToString()) + Int32.Parse(oldInt.ToString());
                    TotalSkillDataTable.Rows[x][1] = sumInt;
                }
            }
        }
        private DataTable CreateSkillTable(MainForm theForm)
        {
            var dt = new DataTable();
            DataStorage stf_Data = theForm.ReturnMainData();

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
        private void PollOfficerPool(MainForm theForm)
        {
            OfficerSkillPools = new DataTable();
            OfficerSkillPools = CreateSkillTable(theForm);
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
                var oldInt = OfficerSkillPools.Rows[x][1];
                int sumInt = Int32.Parse(newInt.ToString()) + Int32.Parse(oldInt.ToString());
                OfficerSkillPools.Rows[x][1] = sumInt;
            }
        }
        private void SetCrewSkillPool(List<DataStorage.CrewDataStruct> theCrew, MainForm theForm)
        {
            CrewSkillPools = new DataTable();
            CrewSkillPools = CreateSkillTable(theForm);
            foreach (DataStorage.CrewDataStruct aCrew in theCrew)
            {
                var newDT = GetSkillData(aCrew.Job, aCrew.Rank,theForm);
                PollSkillData(newDT, aCrew.Num);
            }
        }
        private DataTable GetSkillData(string term, Int32 theRank, MainForm theForm)
        {
            DataStorage stf_Data = theForm.ReturnMainData();

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
                foreach (DataRow ds in CrewSkillPools.Rows)
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
    }
    class SkillPools
    {
        public DataTable CrewSkillPools;
        public DataTable OfficerSkillPools;
        public DataTable TotalSkillDataTable;
        public DataStorage.SelectedShip MyShip;

        public List<string> CalculateSkillPools(List<DataStorage.CrewDataStruct> theCrew, MainForm theForm)
        {

            List<string> myStrings = new List<string>();
            DataStorage stf_Data = theForm.ReturnMainData();

            TotalSkillDataTable = new DataTable();
            TotalSkillDataTable = CreateSkillTable(theForm);
            MyShip = new DataStorage.SelectedShip();
            MyShip = theForm.theShip;

            PollOfficerPool(theForm);
            SetCrewSkillPool(theCrew, theForm);
            CreateCrewDicePools();

            myStrings = SetSkillsDisplayString(theForm);
            var aString = CreateCardGameList();
            myStrings.Add(aString);
            return myStrings;
        }
        private string CreateCardGameList()
        {
            var CardGameString = "";
            int Patrol = 0;
            int Blockade = 0;
            int Spy = 0;
            int Exploration = 0;
            int BlackMarket = 0;

            foreach (DataRow dr in TotalSkillDataTable.Rows)
            {
                if (dr[0].ToString().Contains("Tactics") || dr[0].ToString().Contains("Navigation") || dr[0].ToString().Contains("Command"))
                {
                    Patrol += Int32.Parse(dr[1].ToString());
                }
                if (dr[0].ToString().Contains("Pilot") || dr[0].ToString().Contains("Intimidate"))
                {
                    Blockade += Int32.Parse(dr[1].ToString());
                }
                if (dr[0].ToString().Contains("Pilot") || dr[0].ToString().Contains("Electronics"))
                {
                    Spy += Int32.Parse(dr[1].ToString());
                }
                if (dr[0].ToString().Contains("Tactics") || dr[0].ToString().Contains("Explore"))
                {
                    Exploration += Int32.Parse(dr[1].ToString());
                }
                if (dr[0].ToString().Contains("Intimidate") || dr[0].ToString().Contains("Stealth"))
                {
                    BlackMarket += Int32.Parse(dr[1].ToString());
                }
            }
            CardGameString += "-------------------------------------------\n";
            CardGameString += "Card Game Rewards\n";
            CardGameString += "-------------------------------------------\n";
            CardGameString += "Patrol			(Tactics+Navigation+Command) + Cpt's Charisma\n";
            CardGameString += Patrol + "\n\n";
            CardGameString += "Blockade		(Pilot+Intimidate) + Cpt's Charisma & Resilience\n";
            CardGameString += Blockade + "\n\n";
            CardGameString += "Spy				(Pilot + Electronics) + Cpt's Wisdom & Resilience\n";
            CardGameString += Spy + "\n\n";
            CardGameString += "Explore			(Tactics + Explore) + Cpt's Wisdom\n";
            CardGameString += Exploration + "\n\n";
            CardGameString += "Black Market     (Intimidate + Stealth) + Cpt's Charisma\n";
            CardGameString += BlackMarket;
            return CardGameString;
        }
        public DataTable CalculateShipSkillTable(List<DataStorage.CrewDataStruct> theCrew, MainForm theForm)
        {

            List<string> myStrings = new List<string>();
            DataStorage stf_Data = theForm.ReturnMainData();

            TotalSkillDataTable = new DataTable();
            TotalSkillDataTable = CreateSkillTable(theForm);
            MyShip = new DataStorage.SelectedShip();
            MyShip = theForm.theShip;

            PollOfficerPool(theForm);
            SetCrewSkillPool(theCrew,theForm);
            CreateCrewDicePools();

            var dt = new DataTable();
            DataColumn dc = new DataColumn();
            dc.ColumnName = "Skill";
            dc.DataType = typeof(String);
            dc.DefaultValue = "null";

            DataColumn dg = new DataColumn();
            dg.ColumnName = "Crew";
            dg.DataType = typeof(Int32);
            dg.DefaultValue = 0;

            DataColumn dh = new DataColumn();
            dh.ColumnName = "Ship";
            dh.DataType = typeof(Int32);
            dh.DefaultValue = 0;

            DataColumn di = new DataColumn();
            di.ColumnName = "Percent";
            di.DataType = typeof(String);
            di.DefaultValue = 0;

            dt.Columns.Add(dc);
            dt.Columns.Add(dg);
            dt.Columns.Add(dh);
            dt.Columns.Add(di);

            if (TotalSkillDataTable.Rows.Count > 0 && theForm.theShip.isSet)
            {
                foreach (DataRow dr in TotalSkillDataTable.Rows)
                {
                    var checkName = dr[0].ToString();
                    if (checkName.Equals("Pilot") || checkName.Equals("Navigation") || checkName.Equals("Electronics") || checkName.Equals("Ship Ops") || checkName.Equals("Gunnery"))
                    {
                        var shipString = theForm.theShip.Ship.Rows[0][checkName].ToString();
                        DataRow myRow = ReturnShipSkillRow(dt, dr, shipString);
                        dt.Rows.Add(myRow);
                    }
                }
            }
            return dt;
        }
        public DataTable CalculateCrewSkillTable(List<DataStorage.CrewDataStruct> theCrew, MainForm theForm)
        {

            List<string> myStrings = new List<string>();
            DataStorage stf_Data = theForm.ReturnMainData();

            TotalSkillDataTable = new DataTable();
            TotalSkillDataTable = CreateSkillTable(theForm);
            MyShip = new DataStorage.SelectedShip();
            MyShip = theForm.theShip;

            PollOfficerPool(theForm);
            SetCrewSkillPool(theCrew,theForm);
            CreateCrewDicePools();

            var dt = new DataTable();
            DataColumn dc = new DataColumn();
            dc.ColumnName = "Skill";
            dc.DataType = typeof(String);
            dc.DefaultValue = "null";

            DataColumn dg = new DataColumn();
            dg.ColumnName = "Crew";
            dg.DataType = typeof(Int32);
            dg.DefaultValue = 0;

            dt.Columns.Add(dc);
            dt.Columns.Add(dg);


            if (TotalSkillDataTable.Rows.Count > 0 && theForm.theShip.isSet)
            {
                foreach (DataRow dr in TotalSkillDataTable.Rows)
                {
                    var checkName = dr[0].ToString();
                    if (checkName.Equals("Blades") || checkName.Equals("Pistols") || checkName.Equals("Evasion") || checkName.Equals("Rifles") || checkName.Equals("Pilot") || checkName.Equals("Rifles") || checkName.Equals("Navigation") || checkName.Equals("Electronics") || checkName.Equals("Ship Ops") || checkName.Equals("Gunnery"))
                    {

                    }
                    else
                    {
                        DataRow myRow = dt.NewRow();
                        myRow[0] = dr[0].ToString();
                        myRow[1] = Int32.Parse(dr[1].ToString());
                    }
                }
            }
            else if (TotalSkillDataTable.Rows.Count > 0)
            {
                foreach (DataRow dr in TotalSkillDataTable.Rows)
                {
                    var myString = DisplayRowString(dr);
                    var checkName = dr[0].ToString();
                    if (checkName.Equals("Blades") || checkName.Equals("Pistols") || checkName.Equals("Evasion") || checkName.Equals("Rifles"))
                    {

                    }
                    else
                    {
                        DataRow myRow = dt.NewRow();
                        myRow[0] = dr[0].ToString();
                        myRow[1] = Int32.Parse(dr[1].ToString());
                    }
                }
            }
            return dt;
        }
        private List<string> SetSkillsDisplayString(MainForm theForm)
        {
            List<string> SkillDisplayStrings = new List<string>();

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
            }
            else if (TotalSkillDataTable.Rows.Count > 0)
            {
                var TitleString = "Skill" + "\t\t" + "Crew";
                SkillDisplayStrings.Add(TitleString);
                foreach (DataRow dr in TotalSkillDataTable.Rows)
                {
                    var myString = DisplayRowString(dr);
                    var checkName = dr[0].ToString();
                    if (checkName.Equals("Blades") || checkName.Equals("Pistols") || checkName.Equals("Evasion") || checkName.Equals("Rifles"))
                    {

                    }
                    else
                    {
                        SkillDisplayStrings.Add(myString);
                    }
                }
            }
            else
            {
                var myString = "No Crew Data to Display";
                SkillDisplayStrings.Add(myString);
            }
            return SkillDisplayStrings;
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
        private DataRow ReturnShipSkillRow(DataTable dt, DataRow inputRow, string ShipValue)
        {
            var dr = dt.NewRow();

            double CrewDice = Int32.Parse(inputRow[1].ToString());
            double ShipDice = Int32.Parse(ShipValue);
            double PctValue = (CrewDice / ShipDice) * 100;
            PctValue = Math.Round(PctValue, 2);
            string myPct = PctValue.ToString();

            dr[0] = inputRow[0].ToString();
            dr[1] = inputRow[1];
            dr[2] = Int32.Parse(ShipValue);
            dr[3] = myPct;

            return dr;
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
                myString = SkillName + snglTab + ValueName + dblTab + ShipName + dblTab + PctValue.ToString();
            }
            else if (SkillName.Length > 7)
            {
                myString = SkillName + snglTab + ValueName + dblTab + ShipName + dblTab + PctValue.ToString();
            }
            else
            {
                myString = SkillName + dblTab + ValueName + dblTab + ShipName + dblTab + PctValue.ToString();
            }
            return myString;
        }
        private void CreateCrewDicePools()
        {
            if (OfficerSkillPools.Rows.Count > 0 && CrewSkillPools.Rows.Count > 0)
            {
                for (int x = 0; x < CrewSkillPools.Rows.Count; x++)
                {
                    var newInt = CrewSkillPools.Rows[x][1];
                    var oldInt = OfficerSkillPools.Rows[x][1];
                    int sumInt = Int32.Parse(newInt.ToString()) + Int32.Parse(oldInt.ToString());
                    TotalSkillDataTable.Rows[x][1] = sumInt;
                }
            }
        }
        private DataTable CreateSkillTable(MainForm theForm)
        {
            var dt = new DataTable();
            DataStorage stf_Data = theForm.ReturnMainData();

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
        private void PollOfficerPool(MainForm theForm)
        {
            OfficerSkillPools = new DataTable();
            OfficerSkillPools = CreateSkillTable(theForm);
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
                var oldInt = OfficerSkillPools.Rows[x][1];
                int sumInt = Int32.Parse(newInt.ToString()) + Int32.Parse(oldInt.ToString());
                OfficerSkillPools.Rows[x][1] = sumInt;
            }
        }
        private void SetCrewSkillPool(List<DataStorage.CrewDataStruct> theCrew, MainForm theForm)
        {
            CrewSkillPools = new DataTable();
            CrewSkillPools = CreateSkillTable(theForm);
            foreach (DataStorage.CrewDataStruct aCrew in theCrew)
            {
                var newDT = GetSkillData(aCrew.Job, aCrew.Rank,theForm);
                PollSkillData(newDT, aCrew.Num);
            }
        }
        private DataTable GetSkillData(string term, Int32 theRank, MainForm theForm)
        {
            DataStorage stf_Data = theForm.ReturnMainData();

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
                foreach (DataRow ds in CrewSkillPools.Rows)
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

    }
}

