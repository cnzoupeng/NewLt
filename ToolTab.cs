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
using DevExpress.Data;

namespace NewLt
{
    public partial class MainForm : XtraForm
    {
        public long _calcRedmap;
        public CountNum _countAc;
        public CountNum _countSum;
        public CountNum _countSd;

        public CountNum _countHisAc;
        public CountNum _countHisSum;
        public CountNum _countHisSd;

        public BindingList<CountExist> listFindExist;
        public List<TinyLott> listFilAll;
        public List<LtItem> listLtTaobao;
        public List<LtItem> listLtTaobaoFilter;
        public Dictionary<long, LottoryItem> mapHis5;
        public Dictionary<long, LottoryItem> mapHis6;
        public Dictionary<long, LottoryItem> mapTaob5;
        public Dictionary<long, LottoryItem> mapTaob6;

        public int his_filter_count;
        public int tao_filter_count;

        public void initToolTab()
        {
            _calcRedmap = 0;
            his_filter_count = 0;
            tao_filter_count = 0;
            _countAc = new CountNum();
            _countSum = new CountNum();
            _countSd = new CountNum();

            _countHisAc = new CountNum();
            _countHisSd = new CountNum();
            _countHisSum = new CountNum();

            listLtTaobao = new List<LtItem>();
            listLtTaobaoFilter = new List<LtItem>();
            listFindExist = new BindingList<CountExist>();
            mapHis5 = new Dictionary<long, LottoryItem>();
            mapHis6 = new Dictionary<long, LottoryItem>();
            mapTaob5 = new Dictionary<long, LottoryItem>();
            mapTaob6 = new Dictionary<long, LottoryItem>();
            comBoxAllAjax.SelectedIndex = 0;
            comBoxExistData.SelectedIndex = 0;
            
            labelFilterOut.Text = "";
            init_filter();
        }

        public void init_filter()
        {
            listFilter = new BindingList<TypeFilt>();
            listFilter.Add(new TypeFilt("红球", "1", "33", "", ""));
            listFilter.Add(new TypeFilt("和值", "21", "183", "", ""));
            listFilter.Add(new TypeFilt("AC", "0", "10", "", ""));
            listFilter.Add(new TypeFilt("散度", "3", "27", "", ""));
            listFilter.Add(new TypeFilt("偶数", "0", "6", "", ""));
            listFilter.Add(new TypeFilt("缺行", "0", "5", "", ""));

            listRule = new ArrayList();
            listFilAll = new List<TinyLott>();
            gridFilter.DataSource = listFilter;
            random = new Random(DateTime.Now.DayOfYear + DateTime.Now.Millisecond);

            build_history_map();
        }

        public void build_history_map()
        {
            LtCounter couter = new LtCounter();
            List<long> red5s = new List<long>();

            foreach (LottoryItem item in gridList)
            {
                red5s.Clear();
                long mapTmp = item.map & 0x1FFFFFFFF;
                if (!mapHis6.ContainsKey(mapTmp))
                {
                    mapHis6.Add(mapTmp, item);
                }

                couter.get_6_to_5(mapTmp, red5s);
                for (int i = 0; i < 6; ++i )
                {
                    if (!mapHis5.ContainsKey(red5s[i]))
                    {
                        mapHis5.Add(red5s[i], item);
                    }
                }
            }
        }

        private void btDoFileter_Click(object sender, EventArgs e)
        {
            int randIndex = 0;
            string err = "";
            btDoFileter.Enabled = false;

            if (!parseFiltOpt(ref err))
            {
                MessageBox.Show(err);
                btDoFileter.Enabled = true;
                return;
            }
            
            listFilAll.Clear();   

            Combin cmb = new Combin(33, 6);
            long curMap = cmb.next();
            int allIndex = 0;

            int showStep = 5000;
            int prgsStep = (int)cmb.COUNT / showStep;
            progressFilter.Properties.Minimum = 0;
            progressFilter.Properties.Maximum = prgsStep;
            progressFilter.Properties.Step = 1;
            progressFilter.Position = 0;
            progressFilter.Refresh();

            LtCounter count = new LtCounter();
            List<long> allRed = new List<long>();
            List<long> red5s = new List<long>();
            bool matchOther = false;

            int his_fil_count = 0;
            int tao_fil_count = 0;
            long mapExist = 0;
            int[] redAllCount = new int[36];
            for (int i = 0; i < 36; i++)
            {
                redAllCount[i] = 0;
            }

            while (curMap != 0)
            {
                matchOther = false;
                TinyLott lott = new TinyLott(curMap);
                if (!isMatch(lott))
                {
                    goto FindNext;
                }

                //过滤历史
                red5s.Clear();
                if (his_filter_count > 0)
                {
                    if (his_filter_count == 5)
                    {
                        count.get_6_to_5(curMap, red5s);
                        for (int i = 0; i < 6; ++i)
                        {
                            if (mapHis5.ContainsKey(red5s[i]))
                            {
                                his_fil_count++;
                                goto FindNext;
                            }
                        }
                    }
                    if (his_filter_count == 6 && mapHis6.ContainsKey(curMap))
                    {
                        his_fil_count++;
                        goto FindNext;
                    }
                }

                //过滤淘宝
                if (tao_filter_count > 0)
                {                    
                    if (tao_filter_count == 5)
                    {
                        count.get_6_to_5(curMap, red5s);
                        for (int i = 0; i < 6; ++i)
                        {
                            if (mapTaob5.ContainsKey(red5s[i]))
                            {
                                tao_fil_count++;
                                goto FindNext;
                            }
                        }
                    }
                    if (tao_filter_count == 6 && mapTaob6.ContainsKey(curMap))
                    {
                        tao_fil_count++;
                        goto FindNext;
                    }
                }

                mapExist |= lott.map;
                listFilAll.Add(lott);
                if (listFilAll.Count % 10 == 0)
                {
                   labelFilterOut.Text = "   " + lott.toString() + "   ";
                   labelFilterOut.Refresh();
                }

                for (int i = 0; i < 6; ++i)
                {
                    redAllCount[lott.red[i]]++;
                }

            FindNext:
                curMap = cmb.next();

                allIndex++;
                if (allIndex % showStep == 0)
                {
                    progressFilter.PerformStep();
                    progressFilter.Refresh();
                    //gridBand2.Caption = "  找到 " + listFilAll.Count.ToString() + " 个号码";
                    bt_showcount.Text = listFilAll.Count.ToString();
                    bt_showcount.Refresh();
                    gridFilter.Refresh();
                }
            }

            if (listFilAll.Count > 0)
            {
                randIndex = random.Next(0, listFilAll.Count);
                labelFilterOut.Text = "   " + listFilAll[randIndex].toString() + "   ";
            }

            bt_showcount.Text = listFilAll.Count.ToString();
            bt_showcount.Refresh();
            progressFilter.Properties.Step = prgsStep;
            progressFilter.Refresh();
            btDoFileter.Enabled = true;


            //-----------------------------------
            gridFindExist.DataSource = listFindExist;
            listFindExist.Clear();
            for (int i = 1; i < 34; ++i)
            {
                CountExist countex = new CountExist(i, " ", redAllCount[i]);
                listFindExist.Add(countex);
            }
            gridViewFindExist.SortInfo.Add(gridViewFindExist.Columns[2], ColumnSortOrder.Descending);
            gridViewFindExist.Focus();
            SendKeys.SendWait("^{HOME}");
        }

        private void btRandChange_Click(object sender, EventArgs e)
        {
//             LtCounter count = new LtCounter();
//             List<long> red5s = new List<long>();
//             if (listFilAll.Count > 0)
//             {
//                 foreach (TinyLott item in listFilAll)
//                 {
//                     count.get_6_to_5(item.map, red5s);
//                     for (int i = 0; i < 6; ++i)
//                     {
//                         if (mapTaob5.ContainsKey(red5s[i]))
//                         {
//                             MessageBox.Show("xvcv");
//                             return;
//                         }
//                     }
//                 }
//             }

            if (listFilAll.Count > 0)
            {
                btRandChange.Enabled = false;
                int randIndex = random.Next(0, listFilAll.Count);
                labelFilterOut.Text = "   " + listFilAll[randIndex].toString() + "   ";
                btRandChange.Enabled = true;
            }
        }

        public bool isMatch(TinyLott lott)
        {
            if (!((TypeFilt)listRule[0]).match_red(ref lott.red))
            {
                return false;
            }
            if (!((TypeFilt)listRule[1]).match(lott.sum))
            {
                return false;
            }
            if (!((TypeFilt)listRule[2]).match(lott.ac))
            {
                return false;
            }
            if (!((TypeFilt)listRule[3]).match(lott.sd))
            {
                return false;
            }
            if (!((TypeFilt)listRule[4]).match(lott.odd))
            {
                return false;
            }
            if (!((TypeFilt)listRule[5]).match(lott.miss))
            {
                return false;
            }
            return true;

        }

        public bool parseFiltOpt(ref string err)
        {
            listRule.Clear();
            for (int i = 0; i < listFilter.Count; i++)
            {
                TypeFilt ruleInGrid = (TypeFilt)listFilter[i];
                TypeFilt rule = new TypeFilt(ruleInGrid.type, ruleInGrid.min, ruleInGrid.max, ruleInGrid.inc, ruleInGrid.dec);

                if (!rule.parseRule(ref err))
                {
                    listRule.Clear();
                    return false;
                }
                listRule.Add(rule);
            }
            return true;
        }

        private void btCalc_Click(object sender, EventArgs e)
        {
            SimpleButton bt = (SimpleButton)sender;

            long ONE = 1;
            int redCount = 0;
            int num = Int32.Parse(bt.Text.Trim());

            if (bt.Appearance.ForeColor == System.Drawing.Color.Silver)
            {
                bt.Appearance.ForeColor = System.Drawing.Color.Tomato;
                _calcRedmap |= ONE << (num - 1);
            }
            else
            {
                bt.Appearance.ForeColor = System.Drawing.Color.Silver;
                _calcRedmap &= ~(ONE << (num - 1));
            }

            btCalcShow.Text = "";
            for (int i = 0; i < 33; ++i)
            {
                if ((_calcRedmap & (ONE << i)) != 0)
                {
                    redCount++;
                    num = i + 1;
                    btCalcShow.Text += string.Format("{0:00}", num);
                    btCalcShow.Text += " ";
                }
            }
            btCalcShow.Refresh();

            if (redCount == 6)
            {
                LottMin lt = new LottMin(_calcRedmap);
                string info = "sum=" + lt.sum.ToString();
                info += "  ac=" + lt.ac.ToString();
                info += "  sd=" + lt.sd.ToString();

                btCalcAttr.Text = info;
                btCalcAttr.Refresh();
            }
            else
            {
                btCalcAttr.Text = "";
            }
        }

        private void bt_clear_Click(object sender, EventArgs e)
        {
            this.btCalc1.Appearance.ForeColor = System.Drawing.Color.Silver;
            this.btCalc2.Appearance.ForeColor = System.Drawing.Color.Silver;
            this.btCalc3.Appearance.ForeColor = System.Drawing.Color.Silver;
            this.btCalc4.Appearance.ForeColor = System.Drawing.Color.Silver;
            this.btCalc5.Appearance.ForeColor = System.Drawing.Color.Silver;
            this.btCalc6.Appearance.ForeColor = System.Drawing.Color.Silver;
            this.btCalc7.Appearance.ForeColor = System.Drawing.Color.Silver;
            this.btCalc8.Appearance.ForeColor = System.Drawing.Color.Silver;
            this.btCalc9.Appearance.ForeColor = System.Drawing.Color.Silver;
            this.btCalc10.Appearance.ForeColor = System.Drawing.Color.Silver;
            this.btCalc11.Appearance.ForeColor = System.Drawing.Color.Silver;
            this.btCalc12.Appearance.ForeColor = System.Drawing.Color.Silver;
            this.btCalc13.Appearance.ForeColor = System.Drawing.Color.Silver;
            this.btCalc14.Appearance.ForeColor = System.Drawing.Color.Silver;
            this.btCalc15.Appearance.ForeColor = System.Drawing.Color.Silver;
            this.btCalc16.Appearance.ForeColor = System.Drawing.Color.Silver;
            this.btCalc17.Appearance.ForeColor = System.Drawing.Color.Silver;
            this.btCalc18.Appearance.ForeColor = System.Drawing.Color.Silver;
            this.btCalc19.Appearance.ForeColor = System.Drawing.Color.Silver;
            this.btCalc20.Appearance.ForeColor = System.Drawing.Color.Silver;
            this.btCalc21.Appearance.ForeColor = System.Drawing.Color.Silver;
            this.btCalc22.Appearance.ForeColor = System.Drawing.Color.Silver;
            this.btCalc23.Appearance.ForeColor = System.Drawing.Color.Silver;
            this.btCalc24.Appearance.ForeColor = System.Drawing.Color.Silver;
            this.btCalc25.Appearance.ForeColor = System.Drawing.Color.Silver;
            this.btCalc26.Appearance.ForeColor = System.Drawing.Color.Silver;
            this.btCalc27.Appearance.ForeColor = System.Drawing.Color.Silver;
            this.btCalc28.Appearance.ForeColor = System.Drawing.Color.Silver;
            this.btCalc29.Appearance.ForeColor = System.Drawing.Color.Silver;
            this.btCalc30.Appearance.ForeColor = System.Drawing.Color.Silver;
            this.btCalc31.Appearance.ForeColor = System.Drawing.Color.Silver;
            this.btCalc32.Appearance.ForeColor = System.Drawing.Color.Silver;
            this.btCalc33.Appearance.ForeColor = System.Drawing.Color.Silver;
            _calcRedmap = 0;
            btCalcShow.Text = " ";
            btCalcAttr.Text = "";
        }
        
        public void build_all_census()
        {
            LottMin ltt;
            Combin cbm = new Combin(33, 6);
            long itemMap = cbm.next();

            while (itemMap != 0)
            {
                ltt = new LottMin(itemMap);
                _countSum.add(ltt.sum);
                _countAc.add(ltt.ac);
                _countSd.add(ltt.sd);
                itemMap = cbm.next();
            }

            foreach (LottoryItem item in gridList)
            {
                _countHisSum.add(item.sum_red);
                _countHisSd.add(item.sd);
                _countHisAc.add(item.ac);
            }

            _countSum.buildCensus();
            _countAc.buildCensus();
            _countSd.buildCensus();

            _countHisSum.min = _countSum.min;
            _countHisSum.add(_countSum.max);
            _countHisSum.list[_countSum.max]--;

            _countHisAc.min = _countAc.min;
            _countHisAc.add(10);
            _countHisAc.list[10]--;

            _countHisSd.min = _countSd.min;
            _countHisSd.add(27);
            _countHisSd.list[27]--;

            _countHisSum.buildCensus();
            _countHisAc.buildCensus();
            _countHisSd.buildCensus();
        }

        private void comboBoxAllAjax_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (appInit)
            {
                return;
            }

            if (comBoxAllAjax.SelectedText != "Nothing")
            {
                if (_countSum.list.Count == 0)
                {
                    build_all_census();
                }
            }
            else
            {
                chartCensus.SeriesSerializable[0].DataSource = null;
                chartCensusHis.SeriesSerializable[0].DataSource = null;
            }

            if (comBoxAllAjax.SelectedText == "ALL AC")
            {
                chartCensus.SeriesSerializable[0].DataSource = _countAc.listCen;
                chartCensusHis.SeriesSerializable[0].DataSource = _countHisAc.listCen;
            }
            else if (comBoxAllAjax.SelectedText == "ALL SUM")
            {
                chartCensus.SeriesSerializable[0].DataSource = _countSum.listCen;
                chartCensusHis.SeriesSerializable[0].DataSource = _countHisSum.listCen;
            }
            else if (comBoxAllAjax.SelectedText == "ALL SD")
            {
                chartCensus.SeriesSerializable[0].DataSource = _countSd.listCen;
                chartCensusHis.SeriesSerializable[0].DataSource = _countHisSd.listCen;
            }

            chartCensus.SeriesSerializable[0].ArgumentDataMember = "NUM";
            chartCensus.SeriesSerializable[0].ValueDataMembers[0] = "COUNT";
            chartCensus.Refresh();

            chartCensusHis.SeriesSerializable[0].ArgumentDataMember = "NUM";
            chartCensusHis.SeriesSerializable[0].ValueDataMembers[0] = "COUNT";
            chartCensusHis.Refresh();
        }

        private void btCalcCombin_Click(object sender, EventArgs e)
        {
            if (Combin.bit_count(_calcRedmap) != 2)
            {
                return;
            }

            long ONE = 1;
            int m = 0;
            int n = 0;
            for (int i = 0; i < 33; ++i)
            {
                if ((_calcRedmap & (ONE << i)) != 0)
                {
                    if (m == 0)
                    {
                        m = i + 1;
                    }
                    else
                    {
                        n = i + 1;
                        break;
                    }
                }
            }

            int tmp = 0;
            if (m < n)
            {
                tmp = n;
                n = m;
                m = tmp;
            }
            Combin cb = new Combin(m, n);
            btCalcAttr.Text = cb.COUNT.ToString();
            btCalcAttr.Refresh();
        }

        private void btFindExist_Click(object sender, EventArgs e)
        {
            string numFind = numToFindExist.Text.Trim();
            if (numFind.Length < 2)
            {
                return;
            }

            long ONE = 1;
            long mapFind = 0;
            int redCount = 0;
            int[] red = { 0, 0, 0, 0, 0, 0 };
            string[] nums = numFind.Split(' ');
            for (int i = 0; i < 6 && i < nums.Length; ++i )
            {
                red[i] = Int32.Parse(nums[i]);
                if (red[i] < 1 || red[i] > 33)
                {
                    return;
                }
                mapFind |= ONE << (red[i] - 1);
                redCount++;
            }

            //----------------------------------------------
            if (comBoxExistData.SelectedIndex == 0)
            {
                if (gridList.Count < 1)
                {
                    return;
                }

                int[] countIndex = { 0, 0, 0, 0, 0, 0, 0 };
                gridFindExist.DataSource = listFindExist;
                listFindExist.Clear();

                foreach (LottoryItem item in gridList)
                {
                    long tmpMap = mapFind & item.map;
                    tmpMap &= 0x1FFFFFFFF;
                    int same = Combin.bit_count(tmpMap);
                    countIndex[same]++;
                    string numStr = string.Format("{0:00} {1:00} {2:00} {3:00} {4:00} {5:00} {6:00}"
                    , item.red[0], item.red[1], item.red[2], item.red[3], item.red[4], item.red[5], item.blue);
                    CountExist countex = new CountExist(item.id, numStr, same);
                    listFindExist.Add(countex);
                }

                for (int i = 0; i < 7; ++i)
                {
                    CountExist countex = new CountExist(i, countIndex[i].ToString(), 10 + i);
                    listFindExist.Add(countex);
                }

                gridViewFindExist.SortInfo.Add(gridViewFindExist.Columns[2], ColumnSortOrder.Descending);
                gridViewFindExist.Focus();
                SendKeys.SendWait("^{HOME}");
            }
            else if (comBoxExistData.SelectedIndex == 1)
            {
                if (listLtTaobao.Count < 1)
                {
                    return;
                }

                int matchCount = 0;
                LtCounter count = new LtCounter();
                List<long> allRed = new List<long>();
                List<long> red5s = new List<long>();
                int[] countIndex = { 0, 0, 0, 0, 0, 0, 0 };
                gridFindExist.DataSource = listFindExist;
                listFindExist.Clear();

                foreach (LtItem item in listLtTaobao)
                {
                    matchCount = 0;
                    if (count.getItem(item.red, allRed) == 0)
                    {
                        foreach (long map in allRed)
                        {
                            int mcount = Combin.bit_count(map & mapFind);
                            if (mcount > matchCount)
                            {
                                matchCount = mcount;
                            }

                            red5s.Clear();
                            long mapTmp = map & 0x1FFFFFFFF;
                            if (!mapTaob6.ContainsKey(mapTmp))
                            {
                                mapTaob6.Add(mapTmp, null);
                            }

                            count.get_6_to_5(mapTmp, red5s);
                            for (int i = 0; i < 6; ++i)
                            {
                                if (!mapTaob5.ContainsKey(red5s[i]))
                                {
                                    mapTaob5.Add(red5s[i], null);
                                }
                            }
                        }
                        countIndex[matchCount]++;
                        CountExist countex = new CountExist(item.user, item.red, matchCount);
                        listFindExist.Add(countex);
                    }
                    allRed.Clear();
                }

                for (int i = 0; i < 7; ++i)
                {
                    CountExist countex = new CountExist(i, countIndex[i].ToString(), 10 + i);
                    listFindExist.Add(countex);
                }

                gridViewFindExist.SortInfo.Add(gridViewFindExist.Columns[2], ColumnSortOrder.Descending);
                gridViewFindExist.Focus();
                SendKeys.SendWait("^{HOME}");
            }

        }

        private void comBoxLoadTaobao_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = Directory.GetCurrentDirectory();
            if (dlg.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            comBoxLoadTaobao.Text = dlg.SafeFileName;

            //--------------------------------------------
            string sLine = "";
            StreamReader objReader = new StreamReader(dlg.FileName);
            listLtTaobao.Clear();
            while (sLine != null)
            {
                sLine = objReader.ReadLine();
                if (sLine != null && !sLine.Equals(string.Empty))
                {
                    LtItem item = new LtItem();
                    if (item.fromString(sLine) < 0)
                    {
                        MessageBox.Show("Wrong data: " + sLine);
                        return;
                    }
                    listLtTaobao.Add(item);
                }

            }
            objReader.Close();
            comBoxExistData.SelectedIndex = 1;
        }

        private void comBoxLoadTaobaoFileter_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = Directory.GetCurrentDirectory();
            if (dlg.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            comBoxLoadTaobaoFileter.Text = dlg.SafeFileName;

            //--------------------------------------------
            string sLine = "";
            StreamReader objReader = new StreamReader(dlg.FileName);
            listLtTaobaoFilter.Clear();
            while (sLine != null)
            {
                sLine = objReader.ReadLine();
                if (sLine != null && !sLine.Equals(string.Empty))
                {
                    LtItem item = new LtItem();
                    if (item.fromString(sLine) < 0)
                    {
                        MessageBox.Show("Wrong data: " + sLine);
                        return;
                    }
                    listLtTaobaoFilter.Add(item);
                }

            }
            objReader.Close();


            LtCounter count = new LtCounter();
            List<long> allRed = new List<long>();
            List<long> red5s = new List<long>();

            foreach (LtItem item in listLtTaobaoFilter)
            {
                if (count.getItem(item.red, allRed) == 0)
                {
                    foreach (long map in allRed)
                    {
                        red5s.Clear();
                        long mapTmp = map & 0x1FFFFFFFF;
                        if (!mapTaob6.ContainsKey(mapTmp))
                        {
                            mapTaob6.Add(mapTmp, null);
                        }

                        count.get_6_to_5(mapTmp, red5s);
                        for (int i = 0; i < 6; ++i)
                        {
                            if (!mapTaob5.ContainsKey(red5s[i]))
                            {
                                mapTaob5.Add(red5s[i], null);
                            }
                        }
                    }
                }
                allRed.Clear();
            }

        }

        private void filterOther_EditValueChanged(object sender, EventArgs e)
        {
            his_filter_count = 0;
            tao_filter_count = 0;
            string strOther = (string)filterOther.EditValue;
            if (strOther == "")
            {
                return;
            }
            string[] strOpt = strOther.Split(',');
            int [] num = {0,0,0,0,0};


            foreach(string str in strOpt)
            {
                int count = Int32.Parse(str);
                num[count] = 1;
            }

            if (num[1] != 0)
            {
                his_filter_count = 5;
            }
            else if (num[2] != 0)
            {
                his_filter_count = 6;
            }

            if (num[3] != 0)
            {
                tao_filter_count = 5;
            }
            else if (num[4] != 0)
            {
                tao_filter_count = 6;
            }
        }
    }
     


}
