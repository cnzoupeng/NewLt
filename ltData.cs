using System.IO;
using System.Collections;
using System.Windows.Forms;
using System.Net;
using System.Text;
using System.Threading;
using System.ComponentModel;

namespace NewLt
{
    public class LtData
    {
        public ArrayList ltSet;

        public LtData()
        {
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
            var bit1 = 1;
            var strItem = str.Split(' ');
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
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 3000;
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
                MessageBox.Show(ex.Message);
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
                MessageBox.Show("更新数据 " + (items.Length - 2).ToString() + "条\n");
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
            get
            {
                return num;
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
}
