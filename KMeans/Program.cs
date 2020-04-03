using System;
using System.IO;

namespace KMeans
{
    class Program
    {
        static void Main(string[] args)
        {
            double[][] data = POBIERZ("iris.txt");
            int numClasses = 3;
            double[] nowy = new double[] { 6.3, 3.0, 4.6, 0.5 };
            Console.WriteLine($"Predictor values: {nowy[0]}, {nowy[1]}, {nowy[2]}, {nowy[4]} ");
            int k = 10;
            Console.WriteLine($"With k = {k}");
            string predicted = Classify(nowy, data, numClasses, k);
            Console.WriteLine("Predicted class = " + predicted);

        }

        static double[][] POBIERZ(string nazwa)
        {
            string[] lines = File.ReadAllLines(@"iris.txt");// zwraca tablicę o
            //długości równej ilości wierszy w pliku tekstowym
            double[][] data = new double[lines.Length][];//stworzenie tablicy tablic od długości równej długości tablicy wierszy
            for (int i = 0; i < lines.Length; i++)//
            {
                string[] tmp = lines[i].Split(",");//funkcja split zwraca tablicę,
                //Console.WriteLine(tmp);
                data[i] = new double[tmp.Length + 2];//tworzymy w każdym wierszu tablicy tablicę o długości o dwa dłuższej niż długość pierwotna, ponieważ będziemy kodować
                //każdą nazwę za pomocą trzech liczb
                for (int j = 0; j < tmp.Length - 1; j++)
                {
                    //Console.WriteLine(tmp[j]);
                    data[i][j] = Convert.ToDouble(tmp[j].Replace(".", ","));//konwersja danych na typ double, oprócz tego zamiana kropek na przecinki
                    //( w notacji polskiej używa się przecinków do liczb zmiennoprzecinkowych, a nie kropek
                    //Console.WriteLine(data[i][j]);
                }
                if (tmp[4] == "Iris-setosa")//konwersja nazw na liczby;
                {
                    data[i][4] = 0;
                    data[i][5] = 0;
                    data[i][6] = 1;
                }
                else if (tmp[4] == "Iris-versicolor")
                {
                    data[i][4] = 0;
                    data[i][5] = 1;
                    data[i][6] = 0;
                }
                else if (tmp[4] == "Iris-virginica")
                {
                    data[i][4] = 1;
                    data[i][5] = 0;
                    data[i][6] = 0;
                }
            }
            return data;

        }
        static string Classify(double[] nowy,
          double[][] data, int numClasses, int k)
        { 
            int n = data.Length;
            IndexAndDistance[] info = new IndexAndDistance[n];
            for (int i = 0; i < n; ++i)
            {
                IndexAndDistance curr = new IndexAndDistance();//tworzymy nowy element
                double dist = Distance(nowy, data[i]);
                curr.idx = i;//przypisujemy mu indeks
                curr.dist = dist;//przypisujemy dystans
                info[i] = curr;// wpisujemy go do tablicy elementów
            }
            Array.Sort(info);  // sortujemy (przeładowanie metody compare to)
            Console.WriteLine("Nearest / Distance / Class");//?
            Console.WriteLine("==========================");
            for (int i = 0; i < k; ++i)
            {
                string c = Dekoduj((int)data[info[i].idx][4], (int)data[info[i].idx][5], (int)data[info[i].idx][6]);
                string dist = info[i].dist.ToString("F3");
                Console.WriteLine("( " + data[info[i].idx][0] +" | " + data[info[i].idx][1]+ " | " + data[info[i].idx][2]+ " | " + data[info[i].idx][3] + " )  :  " + dist + "        " + c);
            }
            string result = Vote(info, data, numClasses, k);
            return result;
        }

        static string Vote(IndexAndDistance[] info,
          double[][] data, int numClasses, int k)
        {
            int[] votes = new int[numClasses];  // One cell per class
            for (int i = 0; i < k; ++i)
            {       // Just first k
                int idx = info[i].idx;            // Which train item
                string c = Dekoduj((int)data[info[i].idx][4], (int)data[info[i].idx][5], (int)data[info[i].idx][6]);   // Class in last cell
                if (c == "Iris-setosa")//konwersja nazw na liczby;
                {
                    votes[0]++;
                }
                else if (c == "Iris-versicolor")
                {
                    votes[1]++;
                }
                else if (c == "Iris-virginica")
                {
                    votes[2]++;
                }
            }
            int mostVotes = 0;
            int classWithMostVotes = 0;
            string nazwa = "";
            for (int j = 0; j < numClasses; ++j)
            {
                if (votes[j] > mostVotes)
                {
                    mostVotes = votes[j];
                    classWithMostVotes = j;
                }

            }
            if (classWithMostVotes == 0)
            {
                nazwa = "Iris-setosa";
            }
            else if (classWithMostVotes == 1)
            {
                nazwa = "Iris-versicolor";
            }
            else if (classWithMostVotes == 2)
            {
                nazwa = "Iris-virginica";
            }
            return nazwa;
        }


        static double Distance(double[] nowy,
          double[] data)
        {
            double sum = 0.0;
            for (int i = 0; i < nowy.Length; ++i)
                sum += (nowy[i] - data[i]) * (nowy[i] - data[i]);
            return Math.Sqrt(sum);
        }

        static string Dekoduj(int a, int b, int c)
        {
            string nazwa = "";
            if(a == 0)
            {
                if(b == 0)
                {
                    nazwa = "Iris-setosa";
                }
                else if(b == 1)
                {
                    nazwa = "Iris-versicolor";
                }
            }
            else if(a == 1)
            {
                nazwa = "Iris-virginica";
            }
            return nazwa;
        }
    } // Program class
    public class IndexAndDistance : IComparable<IndexAndDistance>
    {
        public int idx;  // Index of a training item
        public double dist;  // To unknown
                             // Need to sort these to find k closest
        public int CompareTo(IndexAndDistance other)
        {
            if (this.dist < other.dist) return -1;
            else if (this.dist > other.dist) return +1;
            else return 0;
        }
    }

}
