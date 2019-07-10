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
        

        public SmallCraftForm()
        {
            InitializeComponent();
            stf_Data = DataStorage.Instance;
            stf_Data.InstatiateTables();
            SmallCraftDataTable = new DataTable();
            SmallCraftDataTable = stf_Data.small_craft_table.Copy();            
            dataGridView1.DataSource = SmallCraftDataTable;           
        }
    }
}
