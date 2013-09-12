namespace NewLt
{
    public class DEFINE
    {
        public const uint RED_ITEM_COUNT = 6;
        public const string HISTORY_FILE = "history.txt";
        public const string WEB_DATA_URL = "http://42.121.193.75";

        
    }

    public class SelectRec
    {
        public int row_begin = -1;
        public int row_end = -1;
        public int col_begin = -1;
        public int col_end = -1;
        public int col_map = 0;
    }

    //
    public class LtNet
    {
        public const uint LT_MAGIC = 0x56ADF65B;
    }

    enum JobId
    {
	    JOB_MIN = 900001,
	    JOB_HISTORY = JOB_MIN,
	    JOB_VERSION,
	    JOB_FILE,
	    JOB_MAX = JOB_FILE
    };

    public struct CommHead
    {
        public int key;
        public int magic;
        public int job;
        public int len;
    }

    public struct ReqHis
    {
        public int start;
        public int stop;
    }

    public struct ReqHisData
    {
        public CommHead head;
        public ReqHis req;
    }

    public struct ResData
    {
        public CommHead head;
        public int result;
    }
}
