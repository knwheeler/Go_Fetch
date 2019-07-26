using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace Go_Fetch
{
    class TreatDropper
    {
        public readonly PictureBox pbTreat;
        private Bitmap myTreat;
        private List<Bitmap> _treatList; 
        private Random r = new Random();
        private Dog _dog;
        private int _formWidth; 


        public TreatDropper(Form form, Dog dog)
        {
            _dog = dog;


            List<Bitmap> treatList = new List<Bitmap>();

            treatList.Add(Go_Fetch.Properties.Resources.Treat_Taco);
            treatList.Add(Go_Fetch.Properties.Resources.Treat_TurkeyLeg);
            treatList.Add(Go_Fetch.Properties.Resources.Treat_Burger);

            _treatList = treatList; 

            pbTreat = new PictureBox
            {
                Name = "pbTreat",
            };

            _formWidth = form.ClientSize.Width; 

            ResetTreat(); 

            form.Controls.Add(pbTreat);
            pbTreat.BringToFront();
            dog.pbDog.BringToFront();

            DropTreat();
        }

        private void ResetTreat()
        {
            myTreat = _treatList[r.Next(0, _treatList.Count)];

            pbTreat.Image = myTreat;
            pbTreat.Size = new System.Drawing.Size(myTreat.Width, myTreat.Height);
            pbTreat.Location = new Point(r.Next(0, _formWidth), 15 - myTreat.Height);
           
        }
       
        //using a backgroundworker to ensure the treat keeps falling while the dog is moving 
        public void DropTreat()
        {

            

            BackgroundWorker dropWorker = new BackgroundWorker();
            dropWorker.DoWork += Fall;
            dropWorker.WorkerReportsProgress = true;
            dropWorker.WorkerSupportsCancellation = true; 
            dropWorker.ProgressChanged += dropWorker_ProgressChanged;
            dropWorker.RunWorkerCompleted += dropWorker_runWorkerCompleted;

            dropWorker.RunWorkerAsync(); 
        }

        private void Fall(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker fallWorker = (BackgroundWorker)sender; 
            
            while (pbTreat.Location.Y < (_dog.pbDog.Location.Y - _dog.pbDog.Height))
            {

                fallWorker.ReportProgress(pbTreat.Location.Y);
                System.Threading.Thread.Sleep(r.Next(5, 20));
            }
            
        }

        private void dropWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int yloc = pbTreat.Location.Y;
            yloc += 1;
            pbTreat.Location = new Point(pbTreat.Location.X, yloc);
            pbTreat.Refresh();
        }

        private void dropWorker_runWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {


            
            if ((_dog.pbDog.Location.X > pbTreat.Location.X) && (_dog.pbDog.Location.X < pbTreat.Location.X + pbTreat.Width))
            {
                MainForm.score += MainForm.ScoreVal ;
            }
            else if ((_dog.pbDog.Location.X + _dog.pbDog.Width > pbTreat.Location.X) && (_dog.pbDog.Location.X + _dog.pbDog.Width < pbTreat.Location.X + pbTreat.Width))
            {
                MainForm.score += MainForm.ScoreVal; 
            }
            else if ((_dog.pbDog.Location.X + _dog.pbDog.Width / 2 > pbTreat.Location.X) && (_dog.pbDog.Location.X + _dog.pbDog.Width / 2 < pbTreat.Location.X + pbTreat.Width))
            {
                MainForm.score += MainForm.ScoreVal;
            }
            else
            {
                MainForm.lives -= 1;
            }
            
            MainForm.UpdateLives();
            MainForm.UpdateScore();

            if (MainForm.lives > 0)
            {
                ResetTreat();
                DropTreat(); 
            }
            else
            {
                MainForm.GameOver(); 
            }


       


        }

    }
}
