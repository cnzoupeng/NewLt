using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
using System.Xml;
using System.Threading;


namespace NewLt
{
    public class NetFile
    {
        public byte[] StructToBytes(object structObj)
        {
            int size = Marshal.SizeOf(structObj);
            byte[] bytes = new byte[size];
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(structObj, structPtr, false);
            Marshal.Copy(structPtr, bytes, 0, size);
            Marshal.FreeHGlobal(structPtr);
            return bytes;
        }

        public object BytesToStruct(byte[] bytes, Type type)
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

        public delegate void OnData(int count);

        public string dnsGetIp(string host)
        {
            string ipRet = null;
            PingReply reply;
            Ping pingSender = new Ping();
            bool testSuccess = false;
            IPHostEntry IPinfo = Dns.GetHostEntry(host);
            foreach (IPAddress ipAddr in IPinfo.AddressList)
            {
                for (int i = 0; i < 3; i++)
                {
                    reply = pingSender.Send(ipAddr);
                    if (reply.Status == IPStatus.Success)
                    {
                        ipRet = ipAddr.ToString();
                        testSuccess = true;
                        break;
                    }
                }

                if (testSuccess)
                {
                    break;
                }                
            }

            return ipRet;
        }

        public int get(string host, ushort port, string fileName, string savePath, ref string err, OnData onData)
        {
            byte[] byteHead;
            byte[] byteData;
            string pathFile = savePath;
            string fullPath = savePath;
            if (fullPath[fullPath.Length - 1] != '\\')
            {
                fullPath += "\\";
            }
            else
            {
                pathFile = savePath.Substring(0, savePath.Length - 1);
            }
            fullPath += fileName;

            try
            {
                if (File.Exists(pathFile))
                {
                    File.Delete(pathFile);
                }

                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }

                TcpClient tclient = new TcpClient();
                tclient.Connect(host, port);
                NetworkStream stream = tclient.GetStream();
                ReqFileData req = new ReqFileData();
                int sendLen = Marshal.SizeOf(req);
                sendLen += fileName.Length + 1;

                byte[] byteFileName = Encoding.ASCII.GetBytes(fileName);
                byte[] byteSend = new byte[sendLen];
                byteFileName.CopyTo(byteSend, Marshal.SizeOf(req));
                byteSend[sendLen - 1] = 0;

                //req.req.fileName = byteSend;                
                req.head.key = 0;
                req.head.magic = IPAddress.HostToNetworkOrder((int)LtNet.LT_MAGIC);
                req.head.job = IPAddress.HostToNetworkOrder((int)JobId.JOB_FILE);
                req.head.len = IPAddress.HostToNetworkOrder(sendLen);
                req.req.len = IPAddress.HostToNetworkOrder(byteFileName.Length + 1);
                byte[] byteHeadSend = StructToBytes(req);
                byteHeadSend.CopyTo(byteSend, 0);

                stream.Write(byteSend, 0, sendLen);
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
                    err = "更新数据失败：\n";
                    return -1;
                }

                res = (ResData)BytesToStruct(byteHead, res.GetType());
                res.head.key = IPAddress.NetworkToHostOrder(res.head.key);
                res.head.magic = IPAddress.NetworkToHostOrder(res.head.magic);
                res.head.job = IPAddress.NetworkToHostOrder(res.head.job);
                res.head.len = IPAddress.NetworkToHostOrder(res.head.len);
                res.result = IPAddress.NetworkToHostOrder(res.result);
                if (res.head.magic != LtNet.LT_MAGIC || res.head.job != (int)JobId.JOB_FILE || res.head.len < headSize)
                {
                    stream.Close();
                    tclient.Close();
                    err = "更新数据失败：错误的响应\n";
                    return -1;
                }

                if (res.result != 0)
                {
                    stream.Close();
                    tclient.Close();
                    err = "更新数据失败：服务器错误\n";
                    return -1;
                }

                if (res.head.len == headSize)
                {
                    err = "更新数据失败：文件为空\n";
                    stream.Close();
                    tclient.Close();
                    return -1;
                }

                int dataLen = res.head.len - headSize;
                byteData = new byte[LtNet.RCV_BUF_LEN];
                int recvLen = 0;

                StreamWriter objWriter = new StreamWriter(fullPath, false);
                ASCIIEncoding encoding = new ASCIIEncoding();

                //读取文件内容
                while (recvLen < dataLen)
                {
                    int recvCount = stream.Read(byteData, 0, (int)LtNet.RCV_BUF_LEN);
                    if (recvCount < 0)
                    {
                        stream.Close();
                        tclient.Close();
                        err = "更新数据失败：\n";
                        return -1;
                    }
                    else if (recvCount == 0)
                    {
                        break;
                    }

                    if (onData != null)
                    {
                        onData(recvCount);
                    }
                    
                    recvLen += recvCount;
                    objWriter.BaseStream.Write(byteData, 0, recvCount);
                }

                stream.Close();
                tclient.Close();
                objWriter.Close();
            }
            catch (System.Exception ex)
            {
                err = "更新数据失败：" + ex.Message;
                return -1;
            }

            return 0;
        }
    }

    public class FileItem
    {
        public int size;
        public string md5;
        public string name;
    }

    public class UpCheck
    {
        public static NetFile netFile;
        public static string err;
        public static MainForm mform;
        public static bool needUp;

        public UpCheck(MainForm form)
        {
            mform = form;
            err = null;
            needUp = false;
            netFile = new NetFile();
        }

        public static int get_up_list(List<FileItem> upList)
        {
            XmlDocument xmlRemote = new XmlDocument();
            XmlDocument xmlLocal = new XmlDocument();
            List<FileItem> listRmt = new List<FileItem>();
            List<FileItem> listLocal = new List<FileItem>();
            int rmt_ver_main = 0;
            int rmt_ver_sub = 0;
            int lcl_ver_main = 0;
            int lcl_ver_sub = 0;

            string work_dir = Directory.GetCurrentDirectory();
            string up_dir = work_dir + "\\" + LtNet.UPDATA_DIR;
            string netErr = "";
            string rmtXmlFile = up_dir + "\\version.xml";
            string lclXmlFile = work_dir + "\\version.xml";

            //获取远端版本文件
            if (netFile.get(LtNet.REMOTE_HOST, LtNet.REMOTE_PORT, LtNet.VERSION_FILE, up_dir, ref netErr, null) < 0)
            {
                err = "获取版本文件失败: " + netErr;
                return -1;
            }

            //加载远端版本文件
            try
            {
                xmlRemote.Load(rmtXmlFile);
                XmlNodeList topM = xmlRemote.DocumentElement.ChildNodes;
                foreach (XmlElement elm in topM)
                {
                    if (elm.Name.ToLower() == "version")
                    {
                        rmt_ver_main = Int32.Parse(elm.GetAttribute("major"));
                        rmt_ver_sub = Int32.Parse(elm.GetAttribute("sub"));
                    }
                    if (elm.Name.ToLower() == "files")
                    {
                        foreach (XmlElement elmFile in elm)
                        {
                            if (elmFile.Name.ToLower() == "file")
                            {
                                FileItem fItem = new FileItem();
                                fItem.size = Int32.Parse(elmFile.GetAttribute("size"));
                                fItem.md5 = elmFile.GetAttribute("md5");
                                fItem.name = elmFile.GetAttribute("name");
                                listRmt.Add(fItem);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                err = "加载远程版本文件失败：" + ex.Message;
                return -1;
            }

            //加载本地版本文件
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
                    if (elm.Name.ToLower() == "files")
                    {
                        foreach (XmlElement elmFile in elm)
                        {
                            if (elmFile.Name.ToLower() == "file")
                            {
                                FileItem fItem = new FileItem();
                                fItem.size = Int32.Parse(elmFile.GetAttribute("size"));
                                fItem.md5 = elmFile.GetAttribute("md5");
                                fItem.name = elmFile.GetAttribute("name");
                                listLocal.Add(fItem);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                err = "加载本地版本文件失败：" + ex.Message;
                return -1;
            }

            upList.Clear();
//             if (false)
//             {
//                 if (rmt_ver_main == lcl_ver_main && rmt_ver_sub == lcl_ver_sub)
//                 {
//                     err = "已经是最新版本: " + rmt_ver_main.ToString();
//                     err += "." + rmt_ver_sub.ToString();
//                     return 0;
//                 }
//             }

            bool existFile = false;
            foreach (FileItem newItem in listRmt)
            {
                existFile = false;
                foreach (FileItem oldItem in listLocal)
                {
                    if (oldItem.name == newItem.name)
                    {
                        if (oldItem.size == newItem.size && oldItem.md5 == newItem.md5)
                        {
                            existFile = true;
                            break;
                        }
                    }
                }

                if (!existFile)
                {
                    upList.Add(newItem);
                }
            }

            return 0;
        }

        public void doCheck()
        {
            Thread th2 = new Thread(UpCheck.check);
            th2.Start(this);
        }

        public static void check(object ltd)
        {
            List<FileItem> upList = new List<FileItem>();
            Thread.Sleep(1000);
            if (get_up_list(upList) < 0)
            {
                mform.upCheckNotify();
                return;
            }

            if (upList.Count > 0)
            {
                needUp = true;
                mform.upCheckNotify();
            }
        }
    }
}
