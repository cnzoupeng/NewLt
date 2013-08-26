using System;
using System.IO;
using System.Collections;
using System.Windows.Forms;

namespace NewLt
{
    public class LottoryItem
    {
        public int id = 0;
        public int[] red = { 0, 0, 0, 0, 0, 0 };
        public int blue;
        public int sum_red;
        public int sum_all;
        public int ac;
        public int sd;
        public int even_num;
        public int odd_num;
        public int miss_row;
        public int map_miss_row;
        public int miss_col;
        public int map_miss_col;
        public long map;
        public long mapFilter;
        public int[] allNums;
        public string odd_even_str = null;

        public const int NUMS = 29;


        public static string[] names = { "ID", "R1", "R2", "R3", "R4", "R5", "R6", "BL",
                                           "SUM_RED", "SUM_ALL", "AC", "SD", "EVEN_NUM", "ODD_NUM", "EO",
                                           "MR", "MR1", "MR2", "MR3", "MR4", "MR5", "MR6",
                                           "MC", "MC1", "MC2", "MC3", "MC4", "MC5", "MC6" };
        public static int ItemId(string name)
        {
            int index = -1;
            for (int i = 0; i < names.Length; i++ )
            {
                if (names[i] == name)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
        public int get_item_by_index(int index)
        {
            if (index >= 0 && index < NUMS)
            {
                return allNums[index];
            }
            return -1;
        }

        public LottoryItem()
        {
            allNums = new int[NUMS];
        }

        public static int bit_one_count(long u)
        {
            var ret = 0;
            while (u != 0)
            {
                u = (u & (u - 1));
                ret++;
            }
            return ret;
        }

        public void init()
        {
            Array.Sort(red);

            calc_ac();
            calc_sandu();
            calc_odd_even();
            calc_miss_row();
            calc_miss_col();


            allNums[0] = id;
            for (var i = 0; i < 6; i++ )
            {
                allNums[i + 1] = red[i];
            }
            allNums[7] = blue;
            allNums[8] = sum_red;
            allNums[9] = sum_all;
            allNums[10] = ac;
            allNums[11] = sd;
            allNums[12] = even_num;
            allNums[13] = odd_num;
            allNums[14] = 0;
            allNums[15] = miss_row;
            allNums[16] = MR1;
            allNums[17] = MR2;
            allNums[18] = MR3;
            allNums[19] = MR4;
            allNums[20] = MR5;
            allNums[21] = MR6;
            allNums[22] = map_miss_col;
            allNums[23] = MC1;
            allNums[24] = MC2;
            allNums[25] = MC3;
            allNums[26] = MC4;
            allNums[27] = MC5;
            allNums[28] = MC6;
        }


        public int calc_ac()
        {
            var acmap = 0;
            var one = 1;
            var ac = 0;
            for (var i = 0; i < 5; i++)
            {
                for (var j = i + 1; j < 6; j++)
                {
                    ac = Math.Abs((int)red[j] - (int)red[i]);
                    acmap |= one << (ac - 1);
                }
            }

            this.ac = LottoryItem.bit_one_count(acmap) - 5;
            return this.ac;
        }

        public int calc_sandu()
        {
            var max = 0;
            for (var i = 1; i < 34; i++)
            {
                var min = 0xFF;
                var cal = 0;
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
            sd = (int)max;
            return sd;
        }

        public int calc_odd_even()
        {
            var even_num = 0;
            odd_even_str = new string(' ', 1);
            odd_even_str = string.Empty;

            for (var i = 0; i < 6; i++ )
            {
                if (red[i] % 2 != 0)
                {
                    even_num++;
                    odd_even_str += "奇";
                }
                else
                {
                    odd_even_str += "偶";
                }
            }
            this.even_num = even_num;
            odd_num = 6 - even_num;
            return 0;
        }

        public int calc_miss_row()
        {
            var bit1 = 1;
            var missBase = 0x3F;
            map_miss_row = 0;
            miss_row = 0;
            for (var i = 0; i < 6; i++)
            {
                if ((map & missBase) != 0)
                {
                    map_miss_row |= (int)(bit1 << i);
                }
                else
                {
                    miss_row++;
                }
                missBase <<= 6;
            }

            return miss_row;
        }

        public int calc_miss_col()
        {
            var bit1 = 1;
            long missBase = 0x555;
            map_miss_col = 0;
            miss_col = 0;

            for (var i = 0; i < 3; i++)
            {
                if ((map & missBase) != 0)
                {
                    map_miss_col |= bit1 << i;
                }
                else
                {
                    miss_col++;
                }

                missBase = missBase << 1;
                if ((map & missBase) != 0)
                {
                    map_miss_col |= bit1 << (i + 3);
                }
                else
                {
                    miss_col++;
                }
                missBase <<= 11;
            }

            return miss_col;
        }

        public int ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        public int R1
        {
            get
            {
                return red[0];
            }
            set
            {
                red[0] = value;
            }
        }

        public int R2
        {
            get
            {
                return red[1];
            }
            set
            {
                red[1] = value;
            }
        }

        public int R3
        {
            get
            {
                return red[2];
            }
            set
            {
                red[2] = value;
            }
        }

        public int R4
        {
            get
            {
                return red[3];
            }
            set
            {
                red[3] = value;
            }
        }

        public int R5
        {
            get
            {
                return red[4];
            }
            set
            {
                red[4] = value;
            }
        }

        public int R6
        {
            get
            {
                return red[5];
            }
            set
            {
                red[5] = value;
            }
        }

        public int BL
        {
            get
            {
                return blue;
            }
            set
            {
                blue = value;
            }
        }

        public int SUM_RED
        {
            get
            {
                return sum_red;
            }
            set
            {
                sum_red = value;
            }
        }

        public int SUM_ALL
        {
            get
            {
                return sum_all;
            }
            set
            {
                sum_all = value;
            }
        }

        public int AC
        {
            get
            {
                return ac;
            }
            set
            {
                ac = value;
            }
        }

        public int SD
        {
            get
            {
                return sd;
            }
            set
            {
                sd = value;
            }
        }

        public int EVEN_NUM
        {
            get
            {
                return even_num;
            }
            set
            {
                even_num = value;
            }
        }

        public int ODD_NUM
        {
            get
            {
                return odd_num;
            }
            set
            {
                odd_num = value;
            }
        }

        public string EVEN_ODD
        {
            get
            {
                return odd_even_str;
            }
            set
            {
                odd_even_str = value;
            }
        }

        public int MR
        {
            get
            {
                return miss_row;
            }
            set
            {
                miss_row = value;
            }
        }

        public int MC
        {
            get
            {
                return miss_col;
            }
            set
            {
                miss_col = value;
            }
        }

        public int MC1
        {
            get
            {
                return (int)((map_miss_col & 0x1) > 0 ? 0 : 1);
            }
        }
        public int MC2
        {
            get
            {
                return (int)((map_miss_col & 0x2) > 0 ? 0 : 2);
            }
        }
        public int MC3
        {
            get
            {
                return (int)((map_miss_col & 0x4) > 0 ? 0 : 3);
            }
        }
        public int MC4
        {
            get
            {
                return (int)((map_miss_col & 0x8) > 0 ? 0 : 4);
            }
        }
        public int MC5
        {
            get
            {
                return (int)((map_miss_col & 0x10) > 0 ? 0 : 5);
            }
        }
        public int MC6
        {
            get
            {
                return (int)((map_miss_col & 0x20) > 0 ? 0 : 6);
            }
        }


        public int MR1
        {
            get
            {
                return (int)((map_miss_row & 0x1) > 0 ? 0 : 1);
            }
        }
        public int MR2
        {
            get
            {
                return (int)((map_miss_row & 0x2) > 0 ? 0 : 2);
            }
        }
        public int MR3
        {
            get
            {
                return (int)((map_miss_row & 0x4) > 0 ? 0 : 3);
            }
        }
        public int MR4
        {
            get
            {
                return (int)((map_miss_row & 0x8) > 0 ? 0 : 4);
            }
        }
        public int MR5
        {
            get
            {
                return (int)((map_miss_row & 0x10) > 0 ? 0 : 5);
            }
        }
        public int MR6
        {
            get
            {
                return (int)((map_miss_row & 0x20) > 0 ? 0 : 6);
            }
        }
    }

    public class LtFileOper
    {
        public int string_to_item(string str, LottoryItem item)
        {
            var bit1 = 1;
            var strItem = str.Split('\t');
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
        public int load_item_from_file(string fileName, ArrayList ltSet)
        {
            var objReader = new StreamReader(fileName);
            var sLine = string.Empty;
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

        public int write_item_to_file(string fileName, string itemStr)
        {
            var objWriter = new StreamWriter(fileName, false);
            objWriter.Write(itemStr);
            objWriter.Close();
            return 0;
        }
    }

    public class Num_Choose
    {
        public string[] column = { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };

        public Num_Choose(int c1, int c2, int c3, int c4, int c5, int c6)
        {
            var val = new int[] { c1, c2, c3, c4, c5, c6 };
            for (var i = 0; i < 6; i++ )
            {
                if (val[i] >= 0)
                {
                    column[i] = val[i].ToString();
                }
            }
        }

        public string NUM_COL_1
        {
            get
            {
                return column[0];
            }
        }
        public string NUM_COL_2
        {
            get
            {
                return column[1];
            }
        }
        public string NUM_COL_3
        {
            get
            {
                return column[2];
            }
        }
        public string NUM_COL_4
        {
            get
            {
                return column[3];
            }
        }
        public string NUM_COL_5
        {
            get
            {
                return column[4];
            }
        }
        public string NUM_COL_6
        {
            get
            {
                return column[5];
            }
        }
    }
    public class EvenOdd_Choose
    {
        public string[] column = { "偶", "偶", "偶", "偶", "偶", "偶" };
        public string NUM_COL_1
        {
            get
            {
                return column[0];
            }
        }
        public string NUM_COL_2
        {
            get
            {
                return column[1];
            }
        }
        public string NUM_COL_3
        {
            get
            {
                return column[2];
            }
        }
        public string NUM_COL_4
        {
            get
            {
                return column[3];
            }
        }
        public string NUM_COL_5
        {
            get
            {
                return column[4];
            }
        }
        public string NUM_COL_6
        {
            get
            {
                return column[5];
            }
        }
    } 
}
