using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STF_CharacterPlanner
{
    public partial class MainForm : Form
    {
        public DataStorage stf_Data;
        public List<DataStorage.CrewDataStruct> theCrew;
        public DataStorage.SelectedShip theShip;

        public MainForm()
        {
            STF_DBExporter.Exporter();
            InitializeComponent();
            stf_Data = DataStorage.Instance;
            stf_Data.InstatiateTables();
            theCrew = new List<DataStorage.CrewDataStruct>();
            theShip = new DataStorage.SelectedShip();
            theShip.isSet = false;
            bridgeMember1.setWhichOfficer("Captain");
            bridgeMember2.setWhichOfficer("First Officer");
            bridgeMember3.setWhichOfficer("Second Officer");
            bridgeMember4.setWhichOfficer("Third Officer");
            bridgeMember5.setWhichOfficer("Fourth Officer");
            bridgeMember6.setWhichOfficer("Fifth Officer");
            bridgeMember7.setWhichOfficer("Sixth Officer");
            stf_Data.setBridgeMember1(bridgeMember1);
            stf_Data.setBridgeMember2(bridgeMember2);
            stf_Data.setBridgeMember3(bridgeMember3);
            stf_Data.setBridgeMember4(bridgeMember4);
            stf_Data.setBridgeMember5(bridgeMember5);
            stf_Data.setBridgeMember4(bridgeMember6);
            stf_Data.setBridgeMember5(bridgeMember7);
        }
        public DataStorage ReturnMainData()
        {
            return stf_Data;
        }
        public void resetCrewForms()
        {
            bridgeMember1.resetTheBridge();
            bridgeMember2.resetTheBridge();
            bridgeMember3.resetTheBridge();
            bridgeMember4.resetTheBridge();
            bridgeMember5.resetTheBridge();
            bridgeMember6.resetTheBridge();
            bridgeMember7.resetTheBridge();
            menu_Control1.SetGroupName("");
            theShip = new DataStorage.SelectedShip();
            theShip.isSet = false;
            theCrew = new List<DataStorage.CrewDataStruct>();
            shipCrewAndDiceControl1.UpdateCrewSkills();
        }
        public void createNewTextFile()
        {
            OutputToFile NewSave = new OutputToFile();
            NewSave.createNewTextFile(this);
        }
        public void createNewTableFile()
        {
            OutputToTable NewSave = new OutputToTable();
            NewSave.createNewTextFile(this);
        }
        public void SaveCrewTemplate()
        {
            var NewTemplateSave = new SaveLoadHandler();
            NewTemplateSave.SaveCrewTemplate(this);
        }
        public void LoadCrewTemplate()
        {
            var NewTemplateSave = new SaveLoadHandler();
            resetCrewForms();
            NewTemplateSave.LoadCrewTemplate(this);
        }
        public void RefreshDataFromWiki()
        { 
        }
        public void RefreshDataFromWiki_ShipData()
        {

        }
    }
}
