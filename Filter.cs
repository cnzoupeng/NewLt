using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel;

namespace NewLt
{
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
            for (int i = 0, j = 0; i < 33 && j < 6; i++)
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
            for (int i = 0; i < 6; i++)
            {
                //odd
                if (red[i] % 2 == 0)
                {
                    this.odd++;
                }
                //miss
                if ((map & missBase) == 0)
                {
                    this.miss++;
                }
                missBase <<= 6;
            }
        }

        public string toString()
        {
            return string.Format("{0:00}  {1:00}  {2:00}  {3:00}  {4:00}  {5:00}"
                , red[0], red[1], red[2], red[3], red[4], red[5]);
        }
    }

    public class LtItem
    {
        public string user;
        public string red;
        public string blue;
        public string url;
        public int count;

        public LtItem()
        {
            user = "";
            red = "";
            blue = "";
            url = "";
            count = 0;
        }

        public LtItem(string u, string r, string b, string l, int c)
        {
            user = u;
            red = r;
            blue = b;
            url = l;
            count = c;
        }

        public int fromString(string str)
        {
            string[] strs = str.Split(':');
            if (strs.Length != 6)
            {
                return -1;
            }
            user = strs[0];
            red = strs[1];
            blue = strs[2];
            count = Int32.Parse(strs[3]);
            url = strs[4] + ":" + strs[5];

            blue.Trim();
            user.Trim();
            red.Trim();
            return 0;
        }

        public string USER
        {
            get
            {
                return user;
            }
        }

        public string RED
        {
            get
            {
                return red;
            }
        }

        public string BLUE
        {
            get
            {
                return blue;
            }
        }

        public string URL
        {
            get
            {
                return url;
            }
        }

        public int COUNT
        {
            get
            {
                return count;
            }
        }
    }

    public class TypeFilt
    {
        public const int SUM_MIN = 21;
        public const int SUM_MAX = 183;
        public const int AC_MIN = 0;
        public const int AC_MAX = 10;
        public const int SD_MIN = 3;
        public const int SD_MAX = 27;
        public const int ODD_MIN = 0;
        public const int ODD_MAX = 6;
        public const int MISS_MIN = 0;
        public const int MISS_MAX = 5;
        public const int BL_MIN = 1;
        public const int BL_MAX = 16;
        public const int RED_MIN = 1;
        public const int RED_MAX = 33;

        public string type;
        public string min;
        public string max;
        public string inc;
        public string dec;

        public int num_min;
        public int num_max;
        public ArrayList num_inc;
        public ArrayList num_dec;

        public int match_success;
        public int match_failed;        

        public TypeFilt(string type, string min, string max, string inc, string dec)
        {
            this.type = type.Trim();
            this.min = min.Trim();
            this.max = max.Trim();
            this.inc = inc.Trim();
            this.dec = dec.Trim();
            match_success = 0;
            match_failed = 0;
            num_inc = new ArrayList();
            num_dec = new ArrayList();
            setDefaultValue();
        }

        public TypeFilt(string type, int min, int max, string inc, string dec)
        {
            this.type = type;
            this.min = min.ToString();
            this.max = max.ToString();
            this.inc = inc;
            this.dec = dec;
            match_success = 0;
            match_failed = 0;
            num_inc = new ArrayList();
            num_dec = new ArrayList();
            setDefaultValue();
        }

        public void setDefaultValue()
        {
            if (type == "和值")
            {
                if (min.Length == 0)
                {
                    min = SUM_MIN.ToString();
                }
                if (max.Length == 0)
                {
                    max = SUM_MAX.ToString();
                }
            }
            else if (type == "AC")
            {
                if (min.Length == 0)
                {
                    min = AC_MIN.ToString();
                }
                if (max.Length == 0)
                {
                    max = AC_MAX.ToString();
                }
            }
            else if (type == "散度")
            {
                if (min.Length == 0)
                {
                    min = SD_MIN.ToString();
                }
                if (max.Length == 0)
                {
                    max = SD_MAX.ToString();
                }
            }
            else if (type == "偶数")
            {
                if (min.Length == 0)
                {
                    min = ODD_MIN.ToString();
                }
                if (max.Length == 0)
                {
                    max = ODD_MAX.ToString();
                }
            }
            else if (type == "缺行")
            {
                if (min.Length == 0)
                {
                    min = MISS_MIN.ToString();
                }
                if (max.Length == 0)
                {
                    max = MISS_MAX.ToString();
                }
            }
            else if (type == "篮球")
            {
                if (min.Length == 0)
                {
                    min = BL_MIN.ToString();
                }
                if (max.Length == 0)
                {
                    max = BL_MAX.ToString();
                }
            }
            else if (type == "红球")
            {
                if (min.Length == 0)
                {
                    min = RED_MIN.ToString();
                }
                if (max.Length == 0)
                {
                    max = RED_MAX.ToString();
                }
            }
            else if (type == "同往")
            {
                if (min.Length == 0)
                {
                    min = "0";
                }
                if (max.Length == 0)
                {
                    max = "6";
                }
            }
        }

        public bool match_red(ref int[] red)
        {
            for (int i = 0; i < 6; i++)
            {
                if (_isOutOf(red[i], num_min, num_max))
                {
                    match_failed++;
                    return false;
                }
            }

            //排除不要的号码
            for (int i = 0; i < num_dec.Count; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (red[j] == (int)num_dec[i])
                    {
                        match_failed++;
                        return false;
                    }
                }
            }

            int existCount = 0;
            for (int i = 0; i < num_inc.Count; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (red[j] == (int)num_inc[i])
                    {
                        existCount++;
                        break;
                    }
                }
            }
            if (existCount != num_inc.Count)
            {
                match_failed++;
                return false;
            }

            match_success++;
            return true;
        }

        public bool match(int val)
        {
            for (int i = 0; i < num_inc.Count; i++)
            {
                if (val == (int)num_inc[i])
                {
                    match_success++;
                    return true;
                }
            }

            for (int i = 0; i < num_dec.Count; i++)
            {
                if (val == (int)num_dec[i])
                {
                    match_failed++;
                    return false;
                }
            }

            if (_isOutOf(val, num_min, num_max))
            {
                match_failed++;
                return false;
            }

            match_success++;
            return true;
        }

        public ArrayList removeDup(ArrayList list)
        {
            ArrayList newList = new ArrayList();
            for (int i = 0; i < list.Count; ++i)
            {
                if (newList.IndexOf(list[i]) < 0)
                {
                    newList.Add(list[i]);
                }
            }
            return newList;
        }

        public bool parseRule(ref string err)
        {
            min = min.Trim();
            max = max.Trim();
            inc = inc.Trim();
            dec = dec.Trim();
            err = "【" + type + "】";
            char[] separator = { ',', ' ', ':' };

            try
            {
                num_min = int.Parse(min);
            }
            catch
            {
                err += "【最小值】 不是数字";
                return false;
            }

            try
            {
                num_max = int.Parse(max);
            }
            catch
            {
                err += "【最大】 不是数字";
                return false;
            }

            try
            {
                num_inc.Clear();
                string[] inc_s = inc.Split(separator);
                for (int i = 0; i < inc_s.Length; i++)
                {
                    if (inc_s[i].Length > 0)
                    {
                        num_inc.Add(int.Parse(inc_s[i]));
                    }
                }
                num_inc = removeDup(num_inc);
                num_inc.Sort();
            }
            catch
            {
                err += "【包含值】 不是数字";
                return false;
            }

            try
            {
                num_dec.Clear();
                string[] dec_s = dec.Split(separator);
                for (int i = 0; i < dec_s.Length; i++)
                {
                    if (dec_s[i].Length > 0)
                    {
                        num_dec.Add(int.Parse(dec_s[i]));
                    }
                }
                num_dec = removeDup(num_dec);
                num_dec.Sort();
            }
            catch
            {
                err += "【排除值】 不是数字";
                return false;
            }

            //范围检查
            if (type == "和值")
            {
                if (!_rangeCheck(SUM_MIN, SUM_MAX, ref err))
                {
                    return false;
                }
            }
            else if (type == "AC")
            {
                if (!_rangeCheck(AC_MIN, AC_MAX, ref err))
                {
                    return false;
                }
            }
            else if (type == "散度")
            {
                if (!_rangeCheck(SD_MIN, SD_MAX, ref err))
                {
                    return false;
                }
            }
            else if (type == "偶数")
            {
                if (!_rangeCheck(ODD_MIN, ODD_MAX, ref err))
                {
                    return false;
                }
            }
            else if (type == "缺行")
            {
                if (!_rangeCheck(MISS_MIN, MISS_MAX, ref err))
                {
                    return false;
                }
            }
            else if (type == "篮球")
            {
                if (!_rangeCheck(BL_MIN, BL_MAX, ref err))
                {
                    return false;
                }
            }
            else if (type == "红球")
            {
                if (!_rangeCheck(RED_MIN, RED_MAX, ref err))
                {
                    return false;
                }
            }

            err = null;
            return true;
        }

        private bool _isCover(int val, int min, int max)
        {
            return (val >= min && val <= max);
        }

        private bool _isOutOf(int val, int min, int max)
        {
            return !(val >= min && val <= max);
        }

        private bool _rangeCheck(int min, int max, ref string err)
        {
            if (_isOutOf(num_min, min, max))
            {
                err += "  【最小值】超出范围【" + min.ToString() + "--" + max.ToString() + "】";
                return false;
            }
            if (_isOutOf(num_max, min, max))
            {
                err += "  【最大值】超出范围【" + min.ToString() + "--" + max.ToString() + "】";
                return false;
            }

            for (int i = 0; i < num_inc.Count; i++)
            {
                if (_isOutOf((int)num_inc[i], min, max))
                {
                    err += "  【包含值】超出范围【" + min.ToString() + "--" + max.ToString() + "】";
                    return false;
                }
            }

            for (int i = 0; i < num_dec.Count; i++)
            {
                if (_isOutOf((int)num_dec[i], min, max))
                {
                    err += "  【排除值】超出范围【" + min.ToString() + "--" + max.ToString() + "】";
                    return false;
                }
            }
            return true;
        }

        public string TYPE
        {
            get { return type; }
            set { type = value; }
        }

        public string MIN
        {
            get { return min; }
            set { min = value; }
        }

        public string MAX
        {
            get { return max; }
            set { max = value; }
        }

        public string INC
        {
            get { return inc; }
            set { inc = value; }
        }

        public string DEC
        {
            get { return dec; }
            set { dec = value; }
        }
    }

    public class TypeFiltOut
    {
        public string num;
        public int sum;
        public int ac;
        public int sd;
        public int ou;

        public TypeFiltOut(string num, int sum, int ac, int sd, int ou)
        {
            this.num = num;
            this.sum = sum;
            this.ac = ac;
            this.sd = sd;
            this.ou = ou;
        }

        public TypeFiltOut(TinyLott ltt)
        {
            this.num = "";
            for (int i = 0; i < 6; i++)
            {
                this.num += string.Format("{0:00}  ", ltt.red[i]);
            }
            this.sum = ltt.sum;
            this.ac = ltt.ac;
            this.sd = ltt.sd;
            this.ou = ltt.odd;
        }

        public string NUM
        {
            get { return num; }
        }

        public int SUM
        {
            get { return sum; }
        }

        public int AC
        {
            get { return ac; }
        }

        public int SD
        {
            get { return sd; }
        }

        public int ODD
        {
            get { return ou; }
        }
    }

    public class CountNum
    {
        public int min;
        public int max;
        public List<int> list;
        public BindingList<NumCensus> listCen;

        public CountNum(int amin = 99999, int amax = 0)
        {
            min = amin;
            max = amax;
            list = new List<int>();
            listCen = new BindingList<NumCensus>();
        }

        public void add(int key, int count = 1)
        {
            if (key >= list.Count)
            {
                max = key + 1;
                for (int i = list.Count; i < max; ++i)
                {
                    list.Add(0);
                }
            }
            list[key] += count;
            if (min > key)
            {
                min = key;
            }
        }

        public void buildCensus()
        {
            listCen.Clear();
            for (int i = min; i < max; ++i)
            {
                NumCensus cen = new NumCensus(i, list[i]);
                listCen.Add(cen);
            }
        }
    }

    public class CountExist
    {
        public string name;
        public string num;
        public int count;

        public CountExist(string sname, string snum, int scount)
        {
            name = sname;
            num = snum;
            count = scount;
        }

        public CountExist(int sname, string snum, int scount)
        {
            name = sname.ToString();
            num = snum;
            count = scount;
        }

        public string NAME
        {
            get { return name; }
        }

        public string NUM
        {
            get { return num; }
        }

        public int COUNT
        {
            get { return count; }
        }
    }

    public class LtCounter
    {
        public int getItem(string reds, List<long> allItem)
        {
            int pos = reds.IndexOf("(");
            if (pos > 0)
            {
                return -1;
            }
            if (pos == 0)
            {
                return get_comb(reds, allItem);
            }

            string[] redAll = reds.Split(' ');
            List<int> redList = new List<int>();
            foreach (string item in redAll)
            {
                int red = Int32.Parse(item);
                if (red > 33 || red < 1)
                {
                    return -1;
                }
                redList.Add(red);
            }

            if (redList.Count < 6)
            {
                return -1;
            }
            Combin cb = new Combin(redList.Count, 6);
            long map = cb.next();
            while (map != 0)
            {
                allItem.Add(map_to_33map(map, redList.Count, redList));
                map = cb.next();
            }

            return 0;
        }

        public void maps_to_str(List<long> maps, List<string> listOut)
        {
            long ONE = 1;
            string strMap = "";
            foreach (long map in maps)
            {
                strMap = "";
                for (int i = 0; i < 33; ++i)
                {
                    if ((map & (ONE << i)) != 0)
                    {
                        int num = i + 1;
                        strMap += num.ToString();
                        strMap += " ";
                    }
                }
                listOut.Add(strMap);
            }
        }

        public long map_to_33map(long map, int count, List<int> listNum)
        {
            long ONE = 1;
            long mapRet = 0;
            for (int i = 0; i < count; ++i)
            {
                if ((map & (ONE << i)) != 0)
                {
                    mapRet |= ONE << (listNum[i] - 1);
                }
            }
            return mapRet;
        }

        public int get_comb(string reds, List<long> allItem)
        {
            long mapFix = 0;
            long map = 0;
            long ONE = 1;

            int fixCount = 0;
            int unfixCount = 0;

            int posStop = reds.IndexOf(")");
            if (posStop < 0)
            {
                return -1;
            }

            //get map fix
            string fixRed = reds.Substring(1, posStop - 1);
            fixRed = fixRed.Trim();
            string[] fixReds = fixRed.Split(' ');
            if (fixReds.Length > 5)
            {
                return -1;
            }

            foreach (string item in fixReds)
            {
                int red = Int32.Parse(item);
                if (red > 33 || red < 1)
                {
                    return -1;
                }
                mapFix |= ONE << (red - 1);
                fixCount++;
            }

            //get map unfix
            string unfix = reds.Substring(posStop + 1);
            unfix = unfix.Trim();
            string[] unfixs = unfix.Split(' ');
            if (unfixs.Length < 1)
            {
                return -1;
            }

            List<int> unfixReds = new List<int>();
            foreach (string unitem in unfixs)
            {
                int red = Int32.Parse(unitem);
                if (red > 33 || red < 1)
                {
                    return -1;
                }
                unfixReds.Add(red);
                unfixCount++;
            }

            int combCount = 6 - fixCount;
            Combin cb = new Combin(unfixCount, combCount);
            long mapTmp = cb.next();
            while (mapTmp != 0)
            {
                map = mapFix | map_to_33map(mapTmp, unfixCount, unfixReds);
                allItem.Add(map);
                mapTmp = cb.next();
            }

            return 0;
        }

        public int get_6_to_5(long map, List<long> allItem)
        {
            Combin cb = new Combin(6, 5);            
            long ONE = 1;
            List<int> listRed = new List<int>();

            for (int i = 0; i < 33; ++i )
            {
                if ((map & (ONE << i)) != 0)
                {
                    listRed.Add(i + 1);
                }
            }

            long mapTmp = cb.next();
            while (mapTmp != 0)
            {
                map = map_to_33map(mapTmp, 6, listRed);
                allItem.Add(map);
                mapTmp = cb.next();
            }

            return 0;
        }
    }
}
