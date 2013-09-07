using System.IO;
using System.Collections;
using System.Windows.Forms;
using System.Net;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System;

namespace NewLt
{
    public class LtData
    {
        public ArrayList ltSet;
        public MainForm form;

        public LtData()
        {
            ltSet = new ArrayList();
        }

        public LtData(MainForm form)
        {
            this.form = form;
            ltSet = new ArrayList();
        }

        public void clone_history(BindingList<LottoryItem> list)
        {
            list.Clear();
            foreach (LottoryItem item in ltSet)
            {
                list.Add((LottoryItem)item);
            }
        }

        public static int string_to_item(string str, LottoryItem item)
        {
            long bit1 = 1;
            string[] strItem = str.Split(' ');
            if (strItem.Length > 7)
            {
                item.map = 0;
                item.id = int.Parse(strItem[0]);
                for (var i = 0; i < DEFINE.RED_ITEM_COUNT; i++)
                {
                    item.red[i] = int.Parse(strItem[i + 1]);
                    item.sum_all += item.red[i];
                    item.sum_red += item.red[i];
                    item.map |= bit1 << ((char)(item.red[i] - 1));
                }
                item.blue = int.Parse(strItem[7]);
                item.sum_all += item.blue;

                item.init();
                return 0;
            }

            MessageBox.Show("Wrong data");
            return -1;
        }

        public int load_from_file(string history_file)
        {
            var sLine = string.Empty;
            var objReader = new StreamReader(history_file);

            while (sLine != null)
            {
                sLine = objReader.ReadLine();
                if (sLine != null && !sLine.Equals(string.Empty))
                {
                    var item = new LottoryItem();
                    if (string_to_item(sLine, item) == 0)
                    {
                        ltSet.Add(item);
                    }
                }
            }
            objReader.Close();
            return ltSet.Count;
        }

        public int load_history()
        {
            ltSet.Clear();

            if (!File.Exists(DEFINE.HISTORY_FILE))
            {
                File.Create(DEFINE.HISTORY_FILE);
            }
            else
            {
                load_from_file(DEFINE.HISTORY_FILE);
            }

            var th2 = new Thread(LtData.update_history);
            th2.Start(this);

            return 0;
        }

        public static void update_history(object ltd)
        {
            var ltData = (LtData)ltd;

            var url = DEFINE.WEB_DATA_URL + "/ssqhis/";
            if (ltData.ltSet.Count == 0)
            {
                url += "2003001";
            }
            else
            {
                var item = (LottoryItem)ltData.ltSet[ltData.ltSet.Count - 1];
                url += (item.id + 1).ToString();
            }
            url += "/2040001";

            var strResult = string.Empty;
            int tryTimes = 3;
RETRY:
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 8000;
                request.Headers.Set("Pragma", "no-cache");
                var response = (HttpWebResponse)request.GetResponse();
                var streamReceive = response.GetResponseStream();
                var encoding = Encoding.GetEncoding("GB2312");
                var streamReader = new StreamReader(streamReceive, encoding);
                strResult = streamReader.ReadToEnd();
                streamReader.Close();
            }
            catch (System.Exception ex)
            {
                tryTimes--;
                if (tryTimes > 0)
                {
                    goto RETRY;
                }

                MessageBox.Show("更新数据失败：" + ex.Message);
                return;
            }


            var items = strResult.Split('\n');
            if (items.Length <= 0)
            {
                MessageBox.Show("服务器返回数据为空\n");
                return;
            }

            if (items[0] != "SUCCESS")
            {
                MessageBox.Show("更新数据失败：\n" + items[0]);
                return;
            }

            var objWriter = new StreamWriter(DEFINE.HISTORY_FILE, true);
            for (var i = 1; i < items.Length; i++)
            {
                if (items[i] == string.Empty)
                {
                    continue;
                }

                var item = new LottoryItem();
                if (string_to_item(items[i], item) == 0)
                {
                    ltData.ltSet.Add(item);
                    items[i] += "\n";
                    objWriter.Write(items[i]);
                }
            }

            objWriter.Close();

            if (items.Length > 2)
            {
                ltData.form.otherUpdateDatas();
            }
            
        }
    }

    public class NumCensus
    {
        public int num;
        public int count;

        public NumCensus(int num, int count)
        {
            this.num = num;
            this.count = count;
        }

        public int NUM
        {
            get { return num;}
        }

        public int COUNT
        {
            get{return count;}
        }
    }

    public class TypeFilt
    {
        public const int SUM_MIN = 21;
        public const int SUM_MAX = 183;
        public const int AC_MIN = 1;
        public const int AC_MAX = 11;
        public const int SD_MIN = 1;
        public const int SD_MAX = 18;
        public const int BL_MIN = 1;
        public const int BL_MAX = 16;
        public const int ODD_MIN = 0;
        public const int ODD_MAX = 6;
        public const int MISS_MIN = 0;
        public const int MISS_MAX = 5;

        public string type;
        public string min;
        public string max;
        public string inc;
        public string dec;

        public int num_min;
        public int num_max;
        public int[] num_inc;
        public int[] num_dec;

        public int match_success;
        public int match_failed;

        public TypeFilt(string type, string min, string max, string inc, string dec)
        {
            this.type = type;
            this.min = min;
            this.max = max;
            this.inc = inc;
            this.dec = dec;
            match_success = 0;
            match_failed = 0;
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
            for (int i = 0; i < num_dec.Length; i++)
            {
                for (int j = 0; j < 6; j++ )
                {
                    if (red[j] == num_dec[i])
                    {
                        match_failed++;
                        return false;
                    }
                }
            }

            int existCount = 0;
            for (int i = 0; i < num_inc.Length; i++)
            {                
                for (int j = 0; j < 6; j++)
                {
                    if (red[j] == num_inc[i])
                    {
                        existCount++;
                        break;
                    }
                }
            }
            if (existCount != num_inc.Length)
            {
                match_failed++;
                return false;
            }

            match_success++;
            return true;
        }

        public bool match(int val)
        {
            for (int i = 0; i < num_inc.Length; i++)
            {
                if (val == num_inc[i])
                {
                    match_success++;
                    return true;
                }
            }

            for (int i = 0; i < num_dec.Length; i++)
            {
                if (val == num_dec[i])
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
                string[] inc_s = inc.Split(separator);
                if (inc_s.Length > 0)
                {
                    num_inc = new int[inc_s.Length];
                }
                for (int i = 0; i < inc_s.Length; i++)
                {
                    num_inc[i] = int.Parse(inc_s[i]);
                
                }
                Array.Sort(num_inc);
            }
            catch
            {
                err += "【包含值】 不是数字";
                return false;
            }

            try
            {
                string[] dec_s = dec.Split(separator);
                if (dec_s.Length > 0)
                {
                    num_dec = new int[dec_s.Length];
                }
                for (int i = 0; i < dec_s.Length; i++)
                {
                    num_dec[i] = int.Parse(dec_s[i]);
                }
                Array.Sort(num_dec);
            }
            catch
            {
                err += "【排除值】 不是数字";
                return false;
            }

            
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

            for (int i = 0; i < num_inc.Length; i++)
            {
                if (_isOutOf(num_inc[i], min, max))
                {
                    err += "  【包含值】超出范围【" + min.ToString() + "--" + max.ToString() + "】";
                    return false;
                }
            }

            for (int i = 0; i < num_dec.Length; i++)
            {
                if (_isOutOf(num_dec[i], min, max))
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
            for (int i = 0; i < 6; i++ )
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
}
