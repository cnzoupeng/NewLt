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
        public int index_begin;
        public int index_end;

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

            //this.TopMost = true;
            //this.FormBorderStyle = FormBorderStyle.None;
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

            index_begin = 0;
            index_end = 0;
            if (get_id_range(ref this.index_begin, ref this.index_end) < 0)
            {
                MessageBox.Show("获取数据范围失败");
                return;
            }

            this.Cursor = Cursors.WaitCursor;

            refresh_runchart_data();

            refresh_dig_data();

            this.Cursor = Cursors.Default;
        }

        public int get_id_range(ref int start, ref int stop)
        {
            if (comb_id_select_begin_r.SelectedIndex < 0
                || comb_id_select_end_r.SelectedIndex < 0
                || comb_name_select_r.SelectedIndex < 0)
            {
                runchart_is_drawing = false;
                return -1;
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
                return -1;
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
                return -1;
            }
            if (index_begin > index_end)
            {
                index_begin ^= index_end;
                index_end ^= index_begin;
                index_begin ^= index_end;
            }

            start = index_begin;
            stop = index_end;
            return 0;
        }

        public void get_item_census(int start, int stop, int typeid, BindingList<NumCensus> list)
        {
            int[] index = new int[256];
            for (int i = 0; i < 256; i++)
            {
                index[i] = 0;
            }

            for (int i = index_begin; i <= index_end; i++)
            {
                LottoryItem item = (LottoryItem)ltData.ltSet[i];
                index[item.allNums[typeid]]++;
            }

            for (int i = 0; i < 256; i++)
            {
                if (index[i] > 0)
                {
                    NumCensus census = new NumCensus(i, index[i]);
                    list.Add(census);
                }
            }
        }

        public void refresh_dig_data()
        {
            if (runchart_is_drawing)
            {
                return;
            }

            runchart_is_drawing = true;
            string nameid = comb_name_select_r.EditValue.ToString();
            
            //-------------------------------------------
            BindingList<NumCensus> list_census_1 = new BindingList<NumCensus>();
            BindingList<NumCensus> list_census_2 = new BindingList<NumCensus>();
            BindingList<NumCensus> list_census_3 = new BindingList<NumCensus>();
            BindingList<NumCensus> list_census_4 = new BindingList<NumCensus>();
            get_item_census(index_begin, index_end, LottoryItem.ItemId("BL"), list_census_1);
            get_item_census(index_begin, index_end, LottoryItem.ItemId("AC"), list_census_2);
            get_item_census(index_begin, index_end, LottoryItem.ItemId("SD"), list_census_3);
            get_item_census(index_begin, index_end, LottoryItem.ItemId("ODD_NUM"), list_census_4);


            //----------------------------------------------
            //chart_dig_one.DataSource = list_census;

            chart_dig_one.SeriesSerializable[0].DataSource = list_census_1;
            chart_dig_one.SeriesSerializable[1].DataSource = list_census_2;
            chart_dig_one.SeriesSerializable[2].DataSource = list_census_3;
            chart_dig_one.SeriesSerializable[3].DataSource = list_census_4;

            chart_dig_one.SeriesSerializable[0].Name = "BLUE";
            chart_dig_one.SeriesSerializable[0].ArgumentDataMember = "NUM";
            chart_dig_one.SeriesSerializable[0].ValueDataMembers[0] = "COUNT";

            chart_dig_one.SeriesSerializable[1].Name = "AC";
            chart_dig_one.SeriesSerializable[1].ArgumentDataMember = "NUM";
            chart_dig_one.SeriesSerializable[1].ValueDataMembers[0] = "COUNT";

            chart_dig_one.SeriesSerializable[2].Name = "SD";
            chart_dig_one.SeriesSerializable[2].ArgumentDataMember = "NUM";
            chart_dig_one.SeriesSerializable[2].ValueDataMembers[0] = "COUNT";

            chart_dig_one.SeriesSerializable[3].Name = "ODD";
            chart_dig_one.SeriesSerializable[3].ArgumentDataMember = "NUM";
            chart_dig_one.SeriesSerializable[3].ValueDataMembers[0] = "COUNT";

            
            runchart_is_drawing = false;
        }

        public void refresh_runchart_data()
        {
            if (runchart_is_drawing)
            {
                return;
            }
           
            runchart_is_drawing = true;        
            string nameid = comb_name_select_r.EditValue.ToString();
            //-------------------------------------------

            ArrayList list_data = ltData.ltSet.GetRange(index_begin, index_end - index_begin + 1);
            BindingList<LottoryItem> list_show = new BindingList<LottoryItem>();
            foreach (LottoryItem item in list_data)
            {
                list_show.Add(item);
            }

            //----------------------------------------------
            chart_line_one.DataSource = list_show;

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

            runchart_is_drawing = false;
        }

        public void refresh_all_data()
        {
            if (ltData.ltSet.Count == 0)
            {
                return;
            }

            index_begin = 0;
            index_end = 0;
            if (get_id_range(ref this.index_begin, ref this.index_end) < 0)
            {
                //MessageBox.Show("获取数据范围失败");
                return;
            }

            this.Cursor = Cursors.WaitCursor;

            refresh_runchart_data();

            refresh_dig_data();

            this.Cursor = Cursors.Default;
        }

        private void comb_id_select_begin_r_SelectedIndexChanged(object sender, EventArgs e)
        {
            refresh_all_data();
        }

        private void comb_id_select_end_r_SelectedIndexChanged(object sender, EventArgs e)
        {
            refresh_all_data();
        }

   }
}
