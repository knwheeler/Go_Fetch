using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Go_Fetch
{
    public partial class MainForm : Form
    {
        public static bool rightKeyDown = false;
        public static bool leftKeyDown = false;
        public readonly int lives = 3;
        public readonly int score = 0; 
        public readonly string livestring = "YYY";
        

        Dog dog;
        TreatDropper treatDropper; 

        public MainForm()
        {
            InitializeComponent();
            dog = new Dog(this);
            treatDropper = new TreatDropper(this, dog);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

            

        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            dog.ChangeDirection(e.KeyCode);
            if (e.KeyCode == Keys.Right)
            {
                rightKeyDown = true; 
                dog.StartRunning();
            }
            else if (e.KeyCode == Keys.Left)
            {
                leftKeyDown = true; 
                dog.StartRunning(); 
            }

        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                rightKeyDown = false;
                dog.StopRunning();
                
            }
            else if(e.KeyCode == Keys.Left)
            {
                leftKeyDown = false; 
                dog.StopRunning();
            }
        }


    }
}
