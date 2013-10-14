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
using DevExpress.XtraGrid.Views.Base;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Xml;



namespace NewLt
{
    public partial class MainForm : XtraForm
    {
        public LtData ltData;
        public BindingList<LottoryItem> gridList;
        public BindingList<NumberAll> gridListAll;
        public BindingList<TypeFilt> listFilter;
        public bool runchart_is_drawing;
        public int index_begin;
        public int index_end;
        public int chart_range;
        public bool appInit;
        
        public ArrayList listRule;
        public const int SHOW_MATCH_COUT = 6;
        Random random;
        UpCheck updateCheck;
        public string version;

        public MainForm()
        {
            chart_range = 10;
            InitializeComponent();
            viewSet();
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            appInit = true;                 
            load_history();
            init_number_all();
            init_ctrl();
            refresh_all_data();            
            initToolTab();
            initDistribTab();
            set_version();
            AppUpdateCheck();
            appInit = false;
        }

        public void AppUpdateCheck()
        {
            string curDir = Directory.GetCurrentDirectory();
            string dstFile = curDir + "\\" + LtNet.UPDATA_EXE;
            string updateFile = curDir + "\\" + LtNet.UPDATA_DIR + "\\" + LtNet.UPDATA_EXE;
            try
            {
                if (File.Exists(updateFile))
                {
                    File.Delete(dstFile);
                    File.Move(updateFile, dstFile);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("移动更新文件失败: " + ex.Message);
            }

            updateCheck = new UpCheck(this);
            updateCheck.doCheck();
        }

        public void set_version()
        {
            string curDir = Directory.GetCurrentDirectory();
            string lclXmlFile = curDir + "\\version.xml";
            XmlDocument xmlLocal = new XmlDocument();
            string ver = "";
            int lcl_ver_main = 0;
            int lcl_ver_sub = 0;

            try
            {
                xmlLocal.Load(lclXmlFile);
                XmlNodeList topM = xmlLocal.DocumentElement.ChildNodes;
                foreach (XmlElement elm in topM)
                {
                    if (elm.Name.ToLower() == "version")
                    {
                        lcl_ver_main = Int32.Parse(elm.GetAttribute("major"));
                        lcl_ver_sub = Int32.Parse(elm.GetAttribute("sub"));
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("读取版本信息失败：" + ex.Message);
            }
            ver = lcl_ver_main.ToString() + "." + lcl_ver_sub.ToString();
            this.Text = this.Text + " " + ver;
        }
        
        public void viewSet()
        {
            //Form size
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            System.Drawing.Point pt = new System.Drawing.Point(0, 0);
            this.StartPosition = FormStartPosition.Manual;
            this.PointToScreen(pt);
            this.Width = screenWidth;
            this.Height = screenHeight - 45;
        }
        
        public void load_history()
        {
            ltData = new LtData(this);
            gridList = new BindingList<LottoryItem>();

            if (ltData.load_history() < 0)
            {
                MessageBox.Show("Load history data failed\n");
            }
            ltData.clone_history(gridList);
        }

        public void init_number_all()
        {
            gridListAll = new BindingList<NumberAll>();
            NumberAllBuild.init_num(gridListAll, gridList);
            gridHis.DataSource = gridListAll;
        }

        public void init_ctrl()
        {
            init_runchart();

            comb_name_select_r.SelectedIndex = 0;
            comb_id_select_range_r.SelectedIndex = 1;
        }

        public void init_runchart()
        {
            comb_name_select_r.Properties.NullText = "Type";
            comb_id_select_begin_r.Properties.NullText = "ID begin";
            comb_id_select_end_r.Properties.NullText = "ID end";

            comb_name_select_r.Properties.Items.Clear();
            for (int i = 0; i < LottoryItem.namesCn.Length; i++)
            {
                if (LottoryItem.namesCn[i] == "ID")
                {
                    continue;
                }

                if (LottoryItem.namesCn[i] == "EO")
                {
                    break;
                }

                comb_name_select_r.Properties.Items.Add(LottoryItem.namesCn[i]);                
            }
            comb_name_select_r.Properties.Items.Add("热球");
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

        private void comb_id_select_begin_r_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!appInit)
            {
                refresh_all_data();
            }    
        }

        private void comb_id_select_end_r_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!appInit)
            {
                refresh_all_data();
            }    
        }

        private void comb_name_select_r_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!appInit)
            {
                refresh_all_data();
            }    
        }

        private void comb_id_select_range_r_SelectedIndexChanged(object sender, EventArgs e)
        {
            chart_range = 10;
            switch (comb_id_select_range_r.SelectedIndex)
            {
                case 0:
                    chart_range = 5;
                    break;
                case 1:
                    chart_range = 10;
                    break;
                case 2:
                    chart_range = 15;
                    break;
                case 3:
                    chart_range = 20;
                    break;
                case 4:
                    chart_range = 30;
                    break;
                case 5:
                    chart_range = 50;
                    break;
                case 6:
                    chart_range = 100;
                    break;
                case 7:
                    chart_range = 300;
                    break;
                default:
                    chart_range = 10;
                    break;
            }

            if (chart_range <= 0)
            {
                return;
            }

            int all_count = comb_id_select_end_r.Properties.Items.Count;
            int begin_index = all_count - 1 - chart_range;
            if (begin_index < 0)
            {
                begin_index = 0;
            }

            comb_id_select_end_r.SelectedIndex = comb_id_select_end_r.Properties.Items.Count - 1;
            comb_id_select_begin_r.SelectedIndex = begin_index;

            if (!appInit)
            {
                refresh_all_data();
            }
        }

        private void comb_id_select_range_r_EditValueChanged(object sender, EventArgs e)
        {
            chart_range = 10;
            try
            {
                chart_range = Int32.Parse(comb_id_select_range_r.Text);
            }
            catch (System.Exception ex)
            {
                return;
            }

            if (chart_range <= 0)
            {
                return;
            }

            int all_count = comb_id_select_end_r.Properties.Items.Count;
            int begin_index = all_count - 1 - chart_range;
            if (begin_index < 0)
            {
                begin_index = 0;
            }

            comb_id_select_end_r.SelectedIndex = comb_id_select_end_r.Properties.Items.Count - 1;
            comb_id_select_begin_r.SelectedIndex = begin_index;

            if (!appInit)
            {
                refresh_all_data();
            }
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

            int valIndex = 0;
            int id_begin = Int32.Parse(comb_id_select_begin_r.EditValue.ToString());
            int id_end = Int32.Parse(comb_id_select_end_r.EditValue.ToString());
            string nameid = comb_name_select_r.EditValue.ToString();

            if (nameid == "热球")
            {
                valIndex = 1;
            }
            else
            {
                for (; valIndex < LottoryItem.namesCn.Length; valIndex++)
                {
                    if (LottoryItem.namesCn[valIndex] == nameid)
                    {
                        break;
                    }
                }

                if (valIndex >= LottoryItem.namesCn.Length)
                {
                    runchart_is_drawing = false;
                    return -1;
                }
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

        public void get_hot_red_census(int start, int stop, BindingList<NumCensus> list)
        {
            int[] index = new int[36];
            for (int i = 0; i < 36; i++)
            {
                index[i] = 0;
            }

            for (int i = index_begin; i <= index_end; i++)
            {
                LottoryItem item = (LottoryItem)ltData.ltSet[i];
                for (int k = 0; k < 6; ++k )
                {
                    index[item.red[k]]++;
                }
            }

            for (int i = 0; i < 36; i++)
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
            if (nameid == "热球")
            {
                get_hot_red_census(index_begin, index_end, list_census_1);
            }
            else
            {
                get_item_census(index_begin, index_end, LottoryItem.ItemCnId(comb_name_select_r.Text), list_census_1);
            }
            

            //平均值
            float sum = 0;
            float count = 0;
            float aver = 0;
            foreach (NumCensus item in list_census_1)
            {
                count += item.COUNT;
                sum += item.NUM * item.COUNT;
            }
            aver = sum / count;
            label_average.Text = "平均值: " + aver.ToString("F2");

            //----------------------------------------------
            chart_dig_one.SeriesSerializable[0].DataSource = list_census_1;
            chart_dig_one.SeriesSerializable[0].Name = comb_name_select_r.Text;
            chart_dig_one.SeriesSerializable[0].ArgumentDataMember = "NUM";
            chart_dig_one.SeriesSerializable[0].ValueDataMembers[0] = "COUNT";

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

            if (nameid == "热球")
            {
                runchart_is_drawing = false;
                return;
            }
            //-------------------------------------------

            ArrayList list_data = ltData.ltSet.GetRange(index_begin, index_end - index_begin + 1);
            BindingList<LottoryItem> list_show = new BindingList<LottoryItem>();
            foreach (LottoryItem item in list_data)
            {
                list_show.Add(item);
            }

            //----------------------------------------------
            chart_line_one.DataSource = list_show;

            chart_line_one.SeriesSerializable[0].Name = comb_name_select_r.Text;
            chart_line_one.SeriesSerializable[0].ArgumentDataMember = "ID";
            chart_line_one.SeriesSerializable[0].ValueDataMembers[0] = LottoryItem.names[LottoryItem.ItemCnId(comb_name_select_r.Text)];

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

        public void refresh_all_data_update()
        {
            gridHis.DataSource = null;
            ltData.clone_history(gridList);
            init_number_all();
            init_ctrl();
            refresh_all_data();
            comb_id_select_range_r.SelectedIndex = -1;
            comb_id_select_range_r.SelectedIndex = 1;
            gridHis.Focus();
            SendKeys.SendWait("^{END}");
        }

        public delegate void UpdateDatas();
        public delegate void UpdateNotify();

        public void otherUpdateDatas()
        {
            UpdateDatas updt = new UpdateDatas(refresh_all_data_update);
            this.BeginInvoke(updt);
        }

        public void doCheckNotify()
        {
            if (UpCheck.needUp)
            {
                DialogResult dr = MessageBox.Show("检查到新版本 现在升级？", "更新提示", 
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    try
                    {
                        Process.Start(LtNet.UPDATA_EXE);
                        System.Environment.Exit(0);
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }                    
                }
            }

            if (UpCheck.err != null)
            {
                MessageBox.Show("升级失败 " + UpCheck.err);
            }
        }

        public void upCheckNotify()
        {            
            UpdateNotify notify = new UpdateNotify(doCheckNotify);
            this.Invoke(notify);
            //this.BeginInvoke(notify);
        }

        private void bandedGridViewDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.ColumnHandle > 0 && gridListAll.Count > 0)
            {
                NumberAll number = (NumberAll)gridListAll[e.RowHandle];

                if (number.map[e.Column.ColumnHandle - 1] > 0)
                {
                    if (e.Column.AbsoluteIndex > 33)
                    {
                        e.Appearance.BackColor = Color.LightBlue;
                        //e.Appearance.BackColor = Color.DeepSkyBlue;
                    }
                    else
                    {
                        //e.Appearance.BackColor = Color.Moccasin;
                        e.Appearance.BackColor = Color.PeachPuff;
                    }
                    e.Appearance.ForeColor = Color.DarkBlue;
                }

            }  
        }

        private void girdHisView_RowLoaded(object sender, DevExpress.XtraGrid.Views.Base.RowEventArgs e)
        {
            ColumnView view = sender as ColumnView;
            girdHisView.MoveLast();
        }
        
        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (this.FormBorderStyle == FormBorderStyle.None)
                {
                    this.TopMost = false;
                    this.FormBorderStyle = FormBorderStyle.Sizable;
                    this.WindowState = FormWindowState.Normal;
                }
                else
                {
                    this.TopMost = true;
                    this.FormBorderStyle = FormBorderStyle.None;
                    this.WindowState = FormWindowState.Maximized;
                }

                this.Refresh();
            }

            if (e.KeyChar == (char)27)
            {
                this.TopMost = false;
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            gridHis.Focus();
            SendKeys.SendWait("^{END}");
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //System.Environment.Exit(0);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            int all_count = comb_id_select_end_r.Properties.Items.Count;
            int end_index = comb_id_select_end_r.SelectedIndex - chart_range;
            int begin_index = end_index - chart_range;
            if (begin_index < 0)
            {
                begin_index = 0;
            }
            if (end_index < 0)
            {
                end_index = 0;
            }

            comb_id_select_end_r.SelectedIndex = end_index;
            comb_id_select_begin_r.SelectedIndex = begin_index;

            refresh_all_data();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            int all_count = comb_id_select_end_r.Properties.Items.Count;
            int end_index = comb_id_select_end_r.SelectedIndex + chart_range;
            int begin_index = end_index - chart_range;
            if (begin_index >= all_count)
            {
                begin_index = all_count - 1;
            }
            if (end_index >= all_count)
            {
                end_index = all_count - 1;
            }

            comb_id_select_end_r.SelectedIndex = end_index;
            comb_id_select_begin_r.SelectedIndex = begin_index;

            refresh_all_data();
        }

        private void btSortCount_Click(object sender, EventArgs e)
        {
            BindingList<NumCensus> list_census = (BindingList<NumCensus>)chart_dig_one.SeriesSerializable[0].DataSource;
            if (list_census == null || list_census.Count == 0)
            {
                return;
            }
            List<NumCensus> list = new List<NumCensus>();
            foreach (NumCensus item in list_census)
            {
                list.Add(item);
            }
            list.Sort();

            list_census.Clear();
            foreach (NumCensus item in list)
            {
                list_census.Add(item);
            }
        }
    }
}
