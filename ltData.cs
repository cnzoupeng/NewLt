
using System.IO;
using System.Collections;
using System.Windows.Forms;
using System.Net;
using System.Text;
using System.Threading;

namespace NewLt
{
    public class LtData
    {
        public ArrayList ltSet;

        public LtData()
        {
            ltSet = new ArrayList();
        }

        public static int string_to_item(string str, LottoryItem item)
        {
            long bit1 = 1;
            string[] strItem = str.Split(' ');
            if (strItem.Length > 7)
            {
                item.map = 0;
                item.id = int.Parse(strItem[0]);
                for (int i = 0; i < DEFINE.RED_ITEM_COUNT; i++)
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
            string sLine = "";
            StreamReader objReader = new StreamReader(history_file);

            while (sLine != null)
            {
                sLine = objReader.ReadLine();
                if (sLine != null && !sLine.Equals(""))
                {
                    LottoryItem item = new LottoryItem();
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

            Thread th2 = new Thread(LtData.update_history);
            th2.Start(this);

            return 0;
        }

        public static void update_history(object ltd)
        {
            LtData ltData = (LtData)ltd;

            string url = DEFINE.WEB_DATA_URL + "/ssqhis/";
            if (ltData.ltSet.Count == 0)
            {
                url += "2003001";
            }
            else
            {
                LottoryItem item = (LottoryItem)ltData.ltSet[ltData.ltSet.Count - 1];
                url += (item.id + 1).ToString();
            }
            url += "/2040001";

            string strResult = "";
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 3000;
                request.Headers.Set("Pragma", "no-cache");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream streamReceive = response.GetResponseStream();
                Encoding encoding = Encoding.GetEncoding("GB2312");
                StreamReader streamReader = new StreamReader(streamReceive, encoding);
                strResult = streamReader.ReadToEnd();
                streamReader.Close();

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }


            string[] items = strResult.Split('\n');
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

            StreamWriter objWriter = new StreamWriter(DEFINE.HISTORY_FILE, true);
            for (int i = 1; i < items.Length; i++)
            {
                if (items[i] == "")
                {
                    continue;
                }               

                LottoryItem item = new LottoryItem();
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

            return;
        }
    }
}