using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using System.Windows.Input;
using System.Windows.Threading; 



namespace Go_Fetch
{
    
    enum Direction
    {
        Right, 
        Left
    }

    class Dog
    {
        private Bitmap _myDog = Go_Fetch.Properties.Resources.LDog;
        public readonly PictureBox pbDog;
        private List<Bitmap> _runBitmapsL, _runBitmapsR, _wagBitmapsL, _wagBitmapsR;
        private static System.Windows.Threading.DispatcherTimer _runTimer, _wagTimer;
        private Direction direction = Direction.Left;

        public Dog(Form form)
        {
            //putting Run and Wag on their own timers so they will not interrupt the treatDropper
            _runTimer = new System.Windows.Threading.DispatcherTimer();
            _runTimer.Tick += Run; 
            _runTimer.Interval = new TimeSpan(20);

            _wagTimer = new System.Windows.Threading.DispatcherTimer();
            _wagTimer.Tick += Wag;
            _wagTimer.Interval = new TimeSpan(20);

            


            _runBitmapsL = new List<Bitmap>();
            _runBitmapsR = new List<Bitmap>();
            _wagBitmapsL = new List<Bitmap>();
            _wagBitmapsR = new List<Bitmap>(); 

            _runBitmapsL.Add(Go_Fetch.Properties.Resources.LSprint1);
            _runBitmapsL.Add(Go_Fetch.Properties.Resources.LSprint2);
            _runBitmapsL.Add(Go_Fetch.Properties.Resources.LSprint3);
            _runBitmapsL.Add(Go_Fetch.Properties.Resources.LSprint4);
            _runBitmapsL.Add(Go_Fetch.Properties.Resources.LSprint5);

            _runBitmapsR.Add(Go_Fetch.Properties.Resources.RSprint1);
            _runBitmapsR.Add(Go_Fetch.Properties.Resources.RSprint2);
            _runBitmapsR.Add(Go_Fetch.Properties.Resources.RSprint3);
            _runBitmapsR.Add(Go_Fetch.Properties.Resources.RSprint4);
            _runBitmapsR.Add(Go_Fetch.Properties.Resources.RSprint5);

            _wagBitmapsL.Add(Go_Fetch.Properties.Resources.LSit1);
            _wagBitmapsL.Add(Go_Fetch.Properties.Resources.LSit2);
            _wagBitmapsL.Add(Go_Fetch.Properties.Resources.LSit3);
            _wagBitmapsL.Add(Go_Fetch.Properties.Resources.LSit4);

            _wagBitmapsR.Add(Go_Fetch.Properties.Resources.RSit1);
            _wagBitmapsR.Add(Go_Fetch.Properties.Resources.RSit2);
            _wagBitmapsR.Add(Go_Fetch.Properties.Resources.RSit3);
            _wagBitmapsR.Add(Go_Fetch.Properties.Resources.RSit4);


            //using a tag on pbDog so that we can check to see what image pbDog is set to and move to the next for each timer tick
            pbDog = new PictureBox
            {
                Name = "pbDog",
                Image = _myDog,
                Tag = 0,
                Size = new System.Drawing.Size(_myDog.Width, _myDog.Height),
                Location = new Point(form.ClientSize.Width / 2, form.ClientSize.Height - _myDog.Height)
            };
           
            form.Controls.Add(pbDog);
            pbDog.BringToFront();
        }


        public void ChangeDirection(Keys key)
        {
            if (key == Keys.Right)
            {
                direction = Direction.Right;
            }
            else if (key == Keys.Left)
            {
                direction = Direction.Left;
            }
        }

        public void StartRunning()
        {


            if (direction == Direction.Right && pbDog.Location.X >= Go_Fetch.MainForm.ActiveForm.ClientSize.Width - (pbDog.Width * 1.25))
            {
                Bounce(-1, 0);
            }
            else if (direction == Direction.Left && pbDog.Location.X <= (pbDog.Width * 0.25))
            {
                Bounce(1, 0);
            }
            else
            {
                _runTimer.Start();
            }


        }

        public void StopRunning()
        {

            if (MainForm.leftKeyDown == false && MainForm.rightKeyDown == false)
            {
                _runTimer.Stop();
                pbDog.Tag = 0;
                _wagTimer.Start();
            }

        }


        //overloading UpdateImage method so that it can be used regardless of whether the dog is moving or stationary
        private void UpdateImage(Bitmap bitmap, int tag, Point point)
        {
            pbDog.Location = point;
            pbDog.Image = bitmap;
            pbDog.Height = bitmap.Height;
            pbDog.Width = bitmap.Width;
            pbDog.Tag = tag; 
            System.Threading.Thread.Sleep(50);
            pbDog.Refresh();
        }

        private void UpdateImage(Bitmap bitmap, int tag, int i)
        {
            pbDog.Image = bitmap;
            pbDog.Height = bitmap.Height;
            pbDog.Width = bitmap.Width;
            pbDog.Tag = tag; 
            System.Threading.Thread.Sleep(i);
            pbDog.Refresh();
        }
        


        private void Run(Object source, EventArgs e)
        {

            int increment = 0;
            int tag = 0; 
            List<Bitmap> runBitmaps = null;

            if (direction == Direction.Left)
            {
                increment = -4;
                runBitmaps = _runBitmapsL;
            }
            else
            {
                increment = 4;
                runBitmaps = _runBitmapsR;
            }

            if (Int32.Parse(pbDog.Tag.ToString()) < runBitmaps.Count - 1)
            {
                tag = Int32.Parse(pbDog.Tag.ToString()) + 1; 
            }
            else
            {
                tag = 0; 
            }

            _myDog = runBitmaps[tag];
            UpdateImage(_myDog, tag,  new Point(pbDog.Location.X + increment, pbDog.Location.Y));


            
        }

        private void Wag(object sender, EventArgs e)
        {
            List<Bitmap> wagBitmaps = null;
            int tag = 0; 

            if (direction == Direction.Left)
            {
                wagBitmaps = _wagBitmapsL;
            }
            else
            {
                wagBitmaps = _wagBitmapsR;
            }


            if (Int32.Parse(pbDog.Tag.ToString()) < wagBitmaps.Count - 1)
            {
                tag = Int32.Parse(pbDog.Tag.ToString()) + 1;
                _myDog = wagBitmaps[tag];
                UpdateImage(_myDog, tag, 100);
            }
            else
            {
                _wagTimer.Stop();
            }

           
        }

        private void Bounce(int x, int y)
        {
            _runTimer.Stop();
            int xloc = pbDog.Location.X;
            int yloc = pbDog.Location.Y;
         
            for (int i = 0; i < 10; i++)
            {
                xloc += x;
                yloc += y;
                pbDog.Location = new Point(xloc, yloc);
                System.Threading.Thread.Sleep(10);
            }

            for (int i = 0; i < 10; i++)
            {
                xloc -= x;
                yloc -= y;
                pbDog.Location = new Point(xloc, yloc);
                System.Threading.Thread.Sleep(10);
            }

        }

  
    }
}
