using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using DevExpress.UserSkins;
using DevExpress.XtraPivotGrid;
using DevExpress.XtraEditors;
using DevExpress.Utils;



namespace NewLt
{
    
    public partial class MainForm : XtraForm
    {
        public LtData ltData;

        public MainForm()
        {
            ltData = new LtData();
            InitializeComponent();

            if (ltData.load_history() < 0)
            {
                MessageBox.Show("Load history data failed\n");
            }
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            int xx = 6;
        }
    }
}