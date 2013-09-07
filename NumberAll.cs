

using System.Collections;
using System.ComponentModel;
namespace NewLt
{
    public class NumberAll
    {
        public int[] number = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17
                       , 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33,
                       1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16};
        
        public int[] map = new int[50];

        public NumberAll()
        {
            for (int i = 0; i < 50; i++ )
            {
                map[i] = 0;
            }
        }

        public int ID
        {
            get { return this.number[0]; }            
        }

        public int R1
        {
            get { return this.number[1]; }
        }

        public int R2
        {
            get { return this.number[2]; }
        }

        public int R3
        {
            get { return this.number[3]; }
        }

        public int R4
        {
            get { return this.number[4]; }
        }
        public int R5
        {
            get { return this.number[5]; }
        }
        public int R6
        {
            get { return this.number[6]; }
        }
        public int R7
        {
            get { return this.number[7]; }
        }
        public int R8
        {
            get { return this.number[8]; }
        }
        public int R9
        {
            get { return this.number[9]; }
        }
        public int R10
        {
            get { return this.number[10]; }
        }
        public int R11
        {
            get { return this.number[11]; }
        }
        public int R12
        {
            get { return this.number[12]; }
        }
        public int R13
        {
            get { return this.number[13]; }
        }
        public int R14
        {
            get { return this.number[14]; }
        }
        public int R15
        {
            get { return this.number[15]; }
        }
        public int R16
        {
            get { return this.number[16]; }
        }
        public int R17
        {
            get { return this.number[17]; }
        }
        public int R18
        {
            get { return this.number[18]; }
        }
        public int R19
        {
            get { return this.number[19]; }
        }
        public int R20
        {
            get { return this.number[20]; }
        }
        public int R21
        {
            get { return this.number[21]; }
        }
        public int R22
        {
            get { return this.number[22]; }
        }
        public int R23
        {
            get { return this.number[23]; }
        }
        public int R24
        {
            get { return this.number[24]; }
        }
        public int R25
        {
            get { return this.number[25]; }
        }
        public int R26
        {
            get { return this.number[26]; }
        }
        public int R27
        {
            get { return this.number[27]; }
        }
        public int R28
        {
            get { return this.number[28]; }
        }
        public int R29
        {
            get { return this.number[29]; }
        }
        public int R30
        {
            get { return this.number[30]; }
        }
        public int R31
        {
            get { return this.number[31]; }
        }
        public int R32
        {
            get { return this.number[32]; }
        }
        public int R33
        {
            get { return this.number[33]; }
        }


        public int B1
        {
            get { return this.number[34]; }
        }
        public int B2
        {
            get { return this.number[35]; }
        }
        public int B3
        {
            get { return this.number[36]; }
        }
        public int B4
        {
            get { return this.number[37]; }
        }
        public int B5
        {
            get { return this.number[38]; }
        }
        public int B6
        {
            get { return this.number[39]; }
        }
        public int B7
        {
            get { return this.number[40]; }
        }
        public int B8
        {
            get { return this.number[41]; }
        }
        public int B9
        {
            get { return this.number[42]; }
        }
        public int B10
        {
            get { return this.number[43]; }
        }
        public int B11
        {
            get { return this.number[44]; }
        }
        public int B12
        {
            get { return this.number[45]; }
        }
        public int B13
        {
            get { return this.number[46]; }
        }
        public int B14
        {
            get { return this.number[47]; }
        }
        public int B15
        {
            get { return this.number[48]; }
        }
        public int B16
        {
            get { return this.number[49]; }
        }


    }
    
    public class NumberAllBuild
    {
        public static int init_num(BindingList<NumberAll> listNum, BindingList<LottoryItem> listHis)
        {
            int[] map = new int[50];
            for (int i = 0; i < 50; i++)
            {
                map[i] = 0;
            }


            listNum.Clear();
            foreach (LottoryItem item in listHis)
            {
                for (int i = 0; i < 50; i++)
                {
                    map[i] ++;
                }

                NumberAll number = new NumberAll();
                number.number[0] = item.id;
                for (int i = 0; i < 49; i++ )
                {
                    number.number[i + 1] = map[i];
                }

                for (int i = 0; i < 6; i++) 
                {
                    number.number[item.red[i]] = item.red[i];
                    number.map[item.red[i] - 1] = 1;
                    map[item.red[i] - 1] = 0;
                }
                if (item.blue == 13)
                {
                    item.blue = 13;
                }

                number.number[33 + item.blue] = item.blue;
                number.map[33 + item.blue - 1] = 1;
                map[33 + item.blue - 1] = 0;                
                listNum.Add(number);

            }

            return listNum.Count;
        }
    }
    

}