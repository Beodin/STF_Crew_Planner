using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SqlNado;

namespace STF_CharacterPlanner
{
    public partial class SmallCraftForm : Form
    {
        public DataStorage stf_Data;
        public DataTable SmallCraftDataTable;
        public List<String> SmallCraftList;
        BindingSource SBind;

        public SmallCraftForm()
        {
            InitializeComponent();
            stf_Data = DataStorage.Instance;
            stf_Data.InstatiateTables();
            SmallCraftDataTable = new DataTable();
            SmallCraftDataTable = stf_Data.small_craft_table.Copy();
            SmallCraftList = new List<String>();
            SBind = new BindingSource();
            dataGridView1.DataSource = SmallCraftDataTable;
            dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }
    }
}
