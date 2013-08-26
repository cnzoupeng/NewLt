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
using System.Collections;
using DevExpress.XtraCharts;



namespace NewLt
{
    public partial class MainForm : XtraForm
    {
        public LtData ltData;
        public BindingList<LottoryItem> gridList;
        public bool runchart_is_drawing;

        public MainForm()
        {
            InitializeComponent();

            ltData = new LtData();
            gridList = new BindingList<LottoryItem>();          

            if (ltData.load_history() < 0)
            {
                MessageBox.Show("Load history data failed\n");
            }

            ltData.clone_history(gridList);            
            grid_history.DataSource = gridList;            

            init_ctrl();

            this.WindowState = FormWindowState.Maximized;
        }

        public void init_ctrl()
        {
            init_runchart();

            comb_name_select_r.SelectedIndex = 7;
            comb_id_select_range_r.SelectedIndex = 1;
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            int xx = 6;
        }

        public void init_runchart()
        {
            comb_name_select_r.Properties.NullText = "Type";
            comb_id_select_begin_r.Properties.NullText = "ID begin";
            comb_id_select_end_r.Properties.NullText = "ID end";

            comb_name_select_r.Properties.Items.Clear();
            for (int i = 0; i < LottoryItem.names.Length; i++)
            {
                if (LottoryItem.names[i] == "ID"
                    || LottoryItem.names[i] == "EO")
                {
                    continue;
                }

                comb_name_select_r.Properties.Items.Add(LottoryItem.names[i]);
            }

            comb_id_select_begin_r.Properties.Items.Clear();
            comb_id_select_end_r.Properties.Items.Clear();

            foreach (LottoryItem item in gridList)
            {
                comb_id_select_begin_r.Properties.Items.Add(item.id);
                comb_id_select_end_r.Properties.Items.Add(item.id);
            }

            comb_name_select_r.SelectedIndex = -1;
            comb_id_select_begin_r.SelectedIndex = -1;
            comb_id_select_end_r.SelectedIndex = -1;
        }

        private void comb_id_select_range_r_SelectedIndexChanged(object sender, EventArgs e)
        {
            //rang_select = true;
            int range = 0;
            switch (comb_id_select_range_r.SelectedIndex)
            {
                case 0:
                    range = 10;
                    break;
                case 1:
                    range = 30;
                    break;
                case 2:
                    range = 50;
                    break;
                case 3:
                    range = 100;
                    break;
                case 4:
                    range = 300;
                    break;
                default:
                    range = 0;
                    break;
            }

            if (range <= 0)
            {
                return;
            }

            int all_count = comb_id_select_end_r.Properties.Items.Count;
            int begin_index = all_count - 1 - range;
            if (begin_index < 0)
            {
                begin_index = 0;
            }

            comb_id_select_end_r.SelectedIndex = comb_id_select_end_r.Properties.Items.Count - 1;
            comb_id_select_begin_r.SelectedIndex = begin_index;

            comb_id_select_begin_r.Refresh();
            comb_id_select_end_r.Refresh();

            refresh_runchart_data();
        }

        public void refresh_runchart_data()
        {
            if (runchart_is_drawing)
            {
                return;
            }
            runchart_is_drawing = true;

            if (comb_id_select_begin_r.SelectedIndex < 0
                || comb_id_select_end_r.SelectedIndex < 0
                || comb_name_select_r.SelectedIndex < 0)
            {
                runchart_is_drawing = false;
                return;
            }

            int id_begin = Int32.Parse(comb_id_select_begin_r.EditValue.ToString());
            int id_end = Int32.Parse(comb_id_select_end_r.EditValue.ToString());
            string nameid = comb_name_select_r.EditValue.ToString();

            int valIndex = 0;
            for (; valIndex < LottoryItem.names.Length; valIndex++)
            {
                if (LottoryItem.names[valIndex] == nameid)
                {
                    break;
                }
            }

            if (valIndex >= LottoryItem.names.Length)
            {
                runchart_is_drawing = false;
                return;
            }

            int index_begin = -1;
            int index_end = -1;
            int id = 0;
            foreach (LottoryItem item in ltData.ltSet)
            {
                if (item.id == id_begin)
                {
                    index_begin = id;
                }

                if (item.id == id_end)
                {
                    index_end = id;
                }

                if (index_begin >= 0 && index_end >= 0)
                {
                    break;
                }

                id++;
            }

            if (index_begin < 0 && index_end < 0)
            {
                runchart_is_drawing = false;
                return;
            }
            if (index_begin > index_end)
            {
                index_begin ^= index_end;
                index_end ^= index_begin;
                index_begin ^= index_end;
            }

            this.Cursor = Cursors.WaitCursor;

            //-------------------------------------------

            ArrayList list_data = ltData.ltSet.GetRange(index_begin, index_end - index_begin);
            BindingList<LottoryItem> list_show = new BindingList<LottoryItem>();
            foreach (LottoryItem item in list_data)
            {
                list_show.Add(item);
            }

            //----------------------------------------------
            //chart_line_one.Series.Clear();
            chart_line_one.DataSource = list_show;

            //Series series_show = new Series(nameid, ViewType.Line);
            //series_show.ArgumentDataMember = "ID";
            //series_show.ValueDataMembers[0] = nameid;
            //chart_line_one.Series.Add(series_show);
            chart_line_one.SeriesSerializable[0].Name = "SUM";
            chart_line_one.SeriesSerializable[0].ArgumentDataMember = "ID";
            chart_line_one.SeriesSerializable[0].ValueDataMembers[0] = "SUM_RED";

            chart_line_one.SeriesSerializable[1].Name = "AC";
            chart_line_one.SeriesSerializable[1].ArgumentDataMember = "ID";
            chart_line_one.SeriesSerializable[1].ValueDataMembers[0] = "AC";

            chart_line_one.SeriesSerializable[2].Name = "SD";
            chart_line_one.SeriesSerializable[2].ArgumentDataMember = "ID";
            chart_line_one.SeriesSerializable[2].ValueDataMembers[0] = "SD";

            chart_line_one.SeriesSerializable[3].Name = "ODD";
            chart_line_one.SeriesSerializable[3].ArgumentDataMember = "ID";
            chart_line_one.SeriesSerializable[3].ValueDataMembers[0] = "ODD_NUM";


            //XYDiagram diagram = (XYDiagram)chart_line_one.Diagram;
            //diagram.AxisX.Label.Angle = 40;
            //diagram.AxisX.Label.Antialiasing = true;

            //int max_width = chart_line_one.Width / 20;
            //diagram.AxisX.Range.Auto = true;
            //diagram.AxisX.Range.SetInternalMinMaxValues(1, max_width);
            //diagram.AxisX.MinorCount = max_width;

            //if (list_show.Count < max_width)
            //{
            //    diagram.EnableAxisXScrolling = false;
            //}
            //else
            //{
            //    diagram.EnableAxisXScrolling = true;
            //    diagram.DefaultPane.EnableAxisXScrolling = DefaultBoolean.Default;

            //}
            //diagram.EnableAxisYScrolling = false;

            //// Adjust how the scrolling can be performed.
            //diagram.ScrollingOptions.UseKeyboard = false;
            //diagram.ScrollingOptions.UseMouse = true;
            //diagram.ScrollingOptions.UseScrollBars = true;


            //ScrollBarOptions scrollBarOptions = diagram.DefaultPane.ScrollBarOptions;
            //scrollBarOptions.BarThickness = 12;
            //scrollBarOptions.BackColor = Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(227)))), ((int)(((byte)(235)))));
            //scrollBarOptions.BarColor = Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(140)))), ((int)(((byte)(197)))));

            this.Cursor = Cursors.Default;
            runchart_is_drawing = false;
        }

   }
}
