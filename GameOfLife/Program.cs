using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GameOfLife
{
    internal class Program
    {
        [STAThread]
        static void Main()
        {
            Thread thread1 = new Thread(() =>
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                GameOfLifeArray gameOfLifeArray = new GameOfLifeArray();
                Application.Run(new Form1(gameOfLifeArray));
            });

            Thread thread2 = new Thread(() =>
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                GameOfLifeDict gameOfLifeDict = new GameOfLifeDict();
                Application.Run(new Form1(gameOfLifeDict));
            });

            thread1.SetApartmentState(ApartmentState.STA);
            thread2.SetApartmentState(ApartmentState.STA);

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();
        }
    }
}
