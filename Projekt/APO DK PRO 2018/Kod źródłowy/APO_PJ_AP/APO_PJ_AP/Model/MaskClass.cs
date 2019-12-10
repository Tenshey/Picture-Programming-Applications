namespace APO_PJ_AP.Model
{
    public class MaskClass
    {
        public int[,] Values;
        public int[,] Values2;
        public int columnCount;
        public int rowCount;

        public int getSumMaskValues()
        {
            int sum = 0;
            foreach (int value in Values)
            {
                sum += value;
            }
            return sum;
        }

        public MaskClass() { }

        public MaskClass(int radioButtonValue)
        {
            switch (radioButtonValue)
            {
                case 1:
                    Values = new int[3, 3];
                    columnCount = 3;
                    rowCount = 3;
                    break;
                case 2:
                    Values = new int[3, 5];
                    columnCount = 3;
                    rowCount = 5;
                    break;
                case 3:
                    Values = new int[5, 3];
                    columnCount = 5;
                    rowCount = 3;
                    break;
                case 4:
                    Values = new int[5, 5];
                    columnCount = 5;
                    rowCount = 5;
                    break;
                case 5:
                    Values = new int[7, 7];
                    columnCount = 7;
                    rowCount = 7;
                    break;
            }
            if(Values != null)
            {
                for (int i = 0; i < columnCount; i++)
                    for (int j = 0; j < rowCount; j++)
                        Values[i, j] = 1;
            }
        }
    }
}