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
        private List<Bitmap> treatList; 
        private Random r = new Random();
        private Dog _dog; 

        public TreatDropper(Form form, Dog dog)
        {
            _dog = dog; 

            List<Bitmap> treatList = new List<Bitmap>();

            treatList.Add(Go_Fetch.Properties.Resources.Treat_Taco);
            treatList.Add(Go_Fetch.Properties.Resources.Treat_TurkeyLeg);
            treatList.Add(Go_Fetch.Properties.Resources.Treat_Burger);

            myTreat = treatList[r.Next(0, treatList.Count)];

            pbTreat = new PictureBox
            {
                Name = "pbTreat",
                Image = myTreat,
                Size = new System.Drawing.Size(myTreat.Width, myTreat.Height),
                Location = new Point(r.Next(0, form.ClientSize.Width), 15 - myTreat.Height)
            };

            form.Controls.Add(pbTreat);
            pbTreat.BringToFront();
            dog.pbDog.BringToFront();

            DropTreat();
        }

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

        }

    }
}
