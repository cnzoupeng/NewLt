using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Net.Sockets;
using System.Net;

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

        public static byte[] StructToBytes(object structObj)
        {
            int size = Marshal.SizeOf(structObj);
            byte[] bytes = new byte[size];
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(structObj, structPtr, false);
            Marshal.Copy(structPtr, bytes, 0, size);
            Marshal.FreeHGlobal(structPtr);
            return bytes;
        }

        public static object BytesToStruct(byte[] bytes, Type type)
        {
            int size = Marshal.SizeOf(type);
            if (size > bytes.Length)
            {
                return null;
            }

            IntPtr structPtr = Marshal.AllocHGlobal(size);
            Marshal.Copy(bytes, 0, structPtr, size);
            object obj = Marshal.PtrToStructure(structPtr, type);
            Marshal.FreeHGlobal(structPtr);
            return obj;
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

            Thread th2 = new Thread(LtData.get_new);
            th2.Start(this);

            return 0;
        }

        public static void get_new(object ltd)
        {
            LtData ltData = (LtData)ltd;

            int tryTimes = 3;
            int start = 2003001;
            int stop = 2020999;
            byte[] byteHead;
            byte[] byteData;


            if (ltData.ltSet.Count > 0)
            {
                start = ((LottoryItem)ltData.ltSet[ltData.ltSet.Count - 1]).id;
                start += 1;
            }
        RETRY:
            try
            {
                TcpClient tclient = new TcpClient();
                tclient.Connect("42.121.193.75", 5000);
                NetworkStream stream = tclient.GetStream();

                ReqHisData req = new ReqHisData();
                req.head.key = 0;
                req.head.magic = IPAddress.HostToNetworkOrder((int)LtNet.LT_MAGIC);
                req.head.job = IPAddress.HostToNetworkOrder((int)JobId.JOB_HISTORY);
                req.head.len = IPAddress.HostToNetworkOrder(Marshal.SizeOf(req));
                req.req.start = IPAddress.HostToNetworkOrder(start);
                req.req.stop = IPAddress.HostToNetworkOrder(stop);

                stream.Write(StructToBytes(req), 0, Marshal.SizeOf(req));
                stream.Flush();

                //读取头部
                ResData res = new ResData();
                int headSize = Marshal.SizeOf(res);
                byteHead = new byte[headSize];
                int count = stream.Read(byteHead, 0, headSize);
                if (count < headSize)
                {
                    stream.Close();
                    tclient.Close();
                    MessageBox.Show("更新数据失败：\n");
                    return;
                }

                res = (ResData)BytesToStruct(byteHead, res.GetType());
                res.head.key = IPAddress.NetworkToHostOrder(res.head.key);
                res.head.magic = IPAddress.NetworkToHostOrder(res.head.magic);
                res.head.job = IPAddress.NetworkToHostOrder(res.head.job);
                res.head.len = IPAddress.NetworkToHostOrder(res.head.len);
                res.result = IPAddress.NetworkToHostOrder(res.result);
                if (res.head.magic != LtNet.LT_MAGIC || res.head.job != (int)JobId.JOB_HISTORY || res.head.len < headSize)
                {
                    stream.Close();
                    tclient.Close();
                    MessageBox.Show("更新数据失败：错误的响应\n");
                    return;
                }

                if (res.result != 0)
                {
                    stream.Close();
                    tclient.Close();
                    MessageBox.Show("更新数据失败：服务器错误\n");
                    return;
                }

                if (res.head.len == headSize)
                {
                    stream.Close();
                    tclient.Close();
                    return;
                }

                int dataLen = res.head.len - headSize;
                byteData = new byte[dataLen];
                int recvLen = 0;

                while (recvLen < dataLen)
                {
                    int recvCount = stream.Read(byteData, recvLen, dataLen - recvLen);
                    if (recvCount <= 0)
                    {
                        stream.Close();
                        tclient.Close();
                        MessageBox.Show("更新数据失败：\n");
                        return;
                    }
                    recvLen += recvCount;
                }


            }
            catch (System.Exception ex)
            {
                tryTimes--;
                if (tryTimes > 0)
                {
                    goto RETRY;
                }

                //MessageBox.Show("更新数据失败：" + ex.Message);
                return;
            }

            StreamWriter objWriter = new StreamWriter(DEFINE.HISTORY_FILE, true);
            ASCIIEncoding encoding = new ASCIIEncoding();
            string strLt = encoding.GetString(byteData);
            int itemAdd = 0;

            //objWriter.Write(strLt);

            string[] items = strLt.Split('\n');
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].Length < 10)
                {
                    continue;
                }

                LottoryItem item = new LottoryItem();
                if (string_to_item(items[i], item) == 0)
                {
                    ltData.ltSet.Add(item);
                    //if (i < (items.Length - 1))
                    {
                        items[i] += "\n";
                    }
                    objWriter.Write(items[i]);
                    itemAdd++;
                }
            }
            objWriter.Close();

            if (itemAdd > 0)
            {
                Thread.Sleep(1000);
                ltData.form.otherUpdateDatas();
            }
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
}
