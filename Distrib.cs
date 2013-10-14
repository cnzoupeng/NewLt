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
        public BindingList<DistData> listDistrib;

        public void initDistribTab()
        {
            listDistrib = new BindingList<DistData>();
            foreach (LottoryItem item in gridList)
            {
                DistData newIt = new DistData(item);
                listDistrib.Add(newIt);
            }

            gridDistrib.DataSource = listDistrib;
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            Dictionary<int, int> tmpMap = new Dictionary<int, int>();
            foreach (DistData item in listDistrib)
            {
                if (tmpMap.ContainsKey(item.distr))
                {
                    tmpMap[item.distr]++;
                }
                else
                {
                    tmpMap.Add(item.distr, 1);
                }
            }

            int allCount = listDistrib.Count;
            int perCount = 20;
            int count = 0;
            for (int i = 0; i < allCount - perCount; ++i)
            {
                for (int j = i + 1; j < (i + perCount); ++j )
                {
                    if (listDistrib[i].distr == listDistrib[j].distr)
                    {
                        count++;
                    }
                }
            }

            int xx = 0;
        }

    }

    public class DistData
    {
        public int id;
        public int miss;
        public int distr;
        public string num;

        public DistData(LottoryItem item)
        {
            id = item.id;
            miss = item.miss_row;
            num = string.Format("{0:00} {1:00} {2:00} {3:00} {4:00} {5:00} {6:00}"
                    , item.red[0], item.red[1], item.red[2], item.red[3], item.red[4], item.red[5], item.blue);


            long Fix = 0x3F;
            for (int i = 0; i < 5; ++i )
            {
                long mapTmp = item.map & (Fix << (i * 6));
                distr *= 10;
                distr += Combin.bit_count(mapTmp);
            }

            distr *= 10;
            distr += Combin.bit_count(item.map & 0x1C0000000);
        }

        public int ID
        {
            get { return id; }
        }

        public int MISS
        {
            get { return miss; }
        }

        public string NUM
        {
            get { return num; }
        }

        public string DIST
        {
            get { return string.Format("{0:000000}", distr); }
        }
    }
}