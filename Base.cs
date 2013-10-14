using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewLt
{
    public class NumCensus : IComparable
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
            get { return num; }
        }

        public int COUNT
        {
            get { return count; }
        }

        public int CompareTo(object obj)
        {
            NumCensus other = (NumCensus)obj;
            if (other.count < count)
            {
                return -1;
            }
            else if (other.count > count)
            {
                return 1;
            }
            else
            {
                return 0;
            }
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

        public static int bit_count(long u)
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

}
