using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Collections;
using System.Threading;

namespace NewLt
{
    public partial class FormFileter : DevExpress.XtraEditors.XtraForm
    {
        public BindingList<TypeFiltOut> listFiltOut;
        BindingList<TypeFilt> listRuleInGrid;
        public ArrayList listFilAll;
        public ArrayList listRule;
        public const int SHOW_MATCH_COUT = 6;
        Random random;

        public FormFileter(BindingList<TypeFilt> list)
        {
            InitializeComponent();
            listRule = new ArrayList();
            listFilAll = new ArrayList();
            listFiltOut = new BindingList<TypeFiltOut>();
            listRuleInGrid = list;
            gridFilter.DataSource = list;
            gridFiltOut.DataSource = listFiltOut;
            random = new Random(DateTime.Now.DayOfYear + DateTime.Now.Millisecond);
            this.TopMost = true;
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
            for (int i = 0; i < listRuleInGrid.Count; i++)
            {
                TypeFilt ruleInGrid = (TypeFilt)listRuleInGrid[i];
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

        public void calc_test()
        {
            int ac_min = 111;
            int ac_max = 0;
            int sd_min = 111;
            int sd_max = 0;
            Combin cmb = new Combin(33, 6);
            long curMap = cmb.next();
            while (curMap != 0)
            {
                TinyLott lott = new TinyLott(curMap);
                curMap = cmb.next();

                if (lott.ac < ac_min)
                {
                    ac_min = lott.ac;
                }
                if (lott.ac > ac_max)
                {
                    ac_max = lott.ac;
                }
                if (lott.sd < sd_min)
                {
                    sd_min = lott.sd;
                }
                if (lott.sd > sd_max)
                {
                    sd_max = lott.sd;
                }
            }
            curMap = 0;
        }

        private void btQuickFilter_Click(object sender, EventArgs e)
        {
            string err = "";
            //simpleButton3.Enabled = false;
            listFiltOut.Clear();
            gridFiltOut.Refresh();
            listFilAll.Clear();

            if (!parseFiltOpt(ref err))
            {
                MessageBox.Show(err);
                //simpleButton3.Enabled = true;
                return;
            }

            progressFilter.Properties.Minimum = 0;
            progressFilter.Properties.Maximum = 5;
            progressFilter.Properties.Step = 1;
            progressFilter.Position = 0;
            progressFilter.Refresh();
            Thread.Sleep(500);

            Combin cmb = new Combin(33, 6);
            long curMap = cmb.next();
            while (curMap != 0)
            {
                TinyLott lott = new TinyLott(curMap);
                if (isMatch(lott))
                {
                    listFilAll.Add(lott);
                    progressFilter.PerformStep();
                    progressFilter.Refresh();
                }
                if (listFilAll.Count >= SHOW_MATCH_COUT)
                {
                    break;
                }
                curMap = cmb.next();
            }

            listFiltOut.Clear();
            for (int i = 0; i < listFilAll.Count && i < SHOW_MATCH_COUT; i++)
            {
                listFiltOut.Add(new TypeFiltOut((TinyLott)listFilAll[i]));
            }
           
            gridBand2.Caption = "  快速命中 " + listFiltOut.Count.ToString() + " 个号码";
            progressFilter.Position = 5;
            progressFilter.Refresh();
            gridFiltOut.Refresh();
            //simpleButton3.Enabled = true;
        }

        private void btAllFilter_Click(object sender, EventArgs e)
        {
            int randIndex = 0;
            string err = "";
            btDoFileter.Enabled = false;
            listFiltOut.Clear();
            listFilAll.Clear();
            gridFiltOut.Refresh();
            

            if (!parseFiltOpt(ref err))
            {
                MessageBox.Show(err);
                btDoFileter.Enabled = true;
                return;
            }

            Combin cmb = new Combin(33, 6);
            long curMap = cmb.next();
            int allIndex = 0;

            int showStep = 1000;
            int prgsStep = (int)cmb.COUNT / showStep;
            progressFilter.Properties.Minimum = 0;
            progressFilter.Properties.Maximum = prgsStep;
            progressFilter.Properties.Step = 1;
            progressFilter.Position = 0;
            progressFilter.Refresh();
            
            while (curMap != 0)
            {
                TinyLott lott = new TinyLott(curMap);
                if (isMatch(lott))
                {                    
                    listFilAll.Add(lott);                   
                }
                curMap = cmb.next();

                allIndex++;
                if (allIndex % showStep == 0)
                {
                    progressFilter.PerformStep();
                    progressFilter.Refresh();
                    gridBand2.Caption = "  找到 " + listFilAll.Count.ToString() + " 个号码";
                    gridFiltOut.Refresh();
                }
            }
            
            for (int i = 0; i < listFilAll.Count && i < SHOW_MATCH_COUT; i++)
            {
                randIndex = random.Next(0, listFilAll.Count);
                listFiltOut.Add(new TypeFiltOut((TinyLott)listFilAll[randIndex]));
            }
            
            gridBand2.Caption = "  找到 " + listFilAll.Count.ToString() + " 个号码";
            progressFilter.Properties.Step = prgsStep;
            progressFilter.Refresh();
            gridFiltOut.Refresh();
            btDoFileter.Enabled = true;
        }

        private void btNextFilterOut_Click(object sender, EventArgs e)
        {
            btRandChange.Enabled = false;
            listFiltOut.Clear();
            gridFiltOut.Refresh();
            
            int randIndex = 0;
            for (int i = 0; i < listFilAll.Count && i < SHOW_MATCH_COUT; i++)
            {
                randIndex = random.Next(0, listFilAll.Count);
                listFiltOut.Add(new TypeFiltOut((TinyLott)listFilAll[randIndex]));
            }
            gridFiltOut.Refresh();
            btRandChange.Enabled = true;
        }

        private void btAddFilterOutToHis_Click(object sender, EventArgs e)
        {

        }
    }

    public class Combin
    {
        private int _base;
        private int _sub;
        private uint _count;
        private long _curOne;
        private long _lastOne;

        public Combin(int cbase, int csub)
        {
            _base = cbase > csub ? cbase : csub;
            _sub = cbase < csub ? cbase : csub;
            _curOne = ((long)0x1 << _sub) - 1;
            _lastOne = _curOne << (_base - _sub);
            _count = _calcCount();
        }

        public int bit_count(long u)
        {
            int ret = 0;
            while (u != 0)
            {
                u = (u & (u - 1));
                ret++;
            }
            return ret;
        }

        public long next()
        {
            const long ONE = 1;
            long tmpmap = 0;
            long retMap = _curOne;            
            if (_curOne > _lastOne)
            {
                return 0;
            }

            //calc next map
            for (int i = 0; i < _base; i++)
            {
                if (((_curOne & (ONE << i)) != 0) && ((_curOne & (ONE << (i + 1))) == 0))
                {
                    _curOne &= ~(ONE << i);
                    _curOne |= ONE << (i + 1);

                    if (i > 1)
                    {
                        tmpmap = _curOne & ((ONE << i) - 1);
                        int bit1 = bit_count(tmpmap);
                        _curOne &= ~((ONE << i) - 1);
                        _curOne |= ((ONE << bit1) - 1);
                    }
                    break;
                }
            }

            return retMap;
        }

        private uint _calcCount()
        {
            ulong bigNum = 1;
            ulong smallNum = 1;
            uint count = 0;
            uint longItem = (uint)(_base - _sub);
            uint shortItem = (uint)_sub;
            if (longItem < _sub)
            {
                longItem = (uint)_sub;
                shortItem = (uint)(_base - _sub);
            }

            for (uint i = (uint)_base; i > longItem; --i)
            {
                bigNum *= i;
            }
            for (uint i = 1; i <= shortItem; ++i)
            {
                smallNum *= i;
            }
            
            count = (uint)(bigNum / smallNum);
            return count;

//             int bigNum = _base;
//             long ret = bigNum;
//             long smRet = 1;
//             const long ZERO = 0;
//             const long ONE = 1;            
//             int bit = (_base - _sub) > _sub ? _sub : (_base - _sub);
//             long smap = (ONE << bit) - 1;
// 
//             int bitBig = bit;
//             int bitSmall = bit;
//             while (bitBig > 1)
//             {
//                 //找到所有能够整除的数 然后整除 
//                 for (int i = bitSmall; (i > 0 && smap > 0); i--)
//                 {
//                     if (((smap & (ONE << (i - 1))) > 0) && ret % i == 0)
//                     {
//                         ret /= i;
//                         smap &= ~(ONE << (i - 1));
//                     }
//                 }
// 
//                 bitBig--;
//                 bigNum--;
// 
//                 //越界
//                 if ((~ZERO / ret) < bigNum)
//                 {
//                     MessageBox.Show("Big out aero\n");
//                     return 0;
//                 }
// 
//                 ret *= bigNum;
// 
//                 //找到所有能够整除的数 然后整除 
//                 for (int i = bitSmall; (i > 0 && smap > 0); i--)
//                 {
//                     if (((smap & (ONE << (i - 1))) > 0) && ret % i == 0)
//                     {
//                         ret /= i;
//                         smap &= ~(ONE << (i - 1));
//                     }
//                 }
//             }
// 
//             //计算所有未整除的数字 的剩积
//             for (int j = bitSmall; (j > 0 && smap > 0); j--)
//             {
//                 if (((smap & (ONE << (j - 1))) > 0))
//                 {
//                     if ((~ZERO / smRet) < j)
//                     {
//                         MessageBox.Show("Small out aero\n");
//                         return 0;
//                     }
//                     smRet *= j;
//                     smap &= ~(ONE << (j - 1));
//                 }
//             }
// 
//             ret /= smRet;
// 
//             return (int)ret;
        }

        public uint COUNT
        {
            get { return _count; }
        }

        public long MIN
        {
            get { return (((long)0x1 << _sub) - 1); }
        }

        public long MAX
        {
            get { return _lastOne; }
        }
    }

    public class TinyLott
    {
        public long map;
        public int[] red = { 0, 0, 0, 0, 0, 0 };
        public int blue;
        public int sum;
        public int ac;
        public int sd;
        public int odd;
        public int miss;

        public TinyLott(long map)
        {
            sum = 0;
            this.map = map;            
            const long ONE = 1;
            long tmpMap = map;
            for(int i = 0, j = 0; i < 33 && j < 6; i++)
            {
                if ((map & (ONE << i)) != 0)
                {
                    red[j++] = i + 1;
                    sum += i + 1;
                }
            }
            calc_ac();
            calc_sandu();
            calc_odd_miss();
        }

        public int bit_count(long u)
        {
            int ret = 0;
            while (u != 0)
            {
                u = (u & (u - 1));
                ret++;
            }
            return ret;
        }

        public int calc_ac()
        {
            long acmap = 0;
            long one = 1;
            int ac = 0;
            for (var i = 0; i < 5; i++)
            {
                for (var j = i + 1; j < 6; j++)
                {
                    ac = Math.Abs((int)red[j] - (int)red[i]);
                    acmap |= one << (ac - 1);
                }
            }
            this.ac = bit_count(acmap) - 5;
            return this.ac;
        }

        public int calc_sandu()
        {
            long max = 0;
            for (var i = 1; i < 34; i++)
            {
                long min = 0xFF;
                long cal = 0;
                for (var j = 0; j < 6; j++)
                {
                    cal = Math.Abs(i - (int)red[j]);
                    if (cal < min)
                    {
                        min = cal;
                    }
                }

                if (min > max)
                {
                    max = min;
                }
            }
            this.sd = (int)max;
            return this.sd;
        }

        public void calc_odd_miss()
        {
            this.odd = 0;
            this.miss = 0;
            long missBase = 0x3F;
            for (int i = 0; i < 6; i++ )
            {
                //odd
                if (red[i] % 2 == 0)
                {
                    this.odd ++;
                }
                //miss
                if ((map & missBase) == 0)
                {
                    this.miss++;
                }
                missBase <<= 6;
            }
        }
    }
}