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
   enum GameParameters
    {
        lives = 3, 
        score = 0, 
        ScoreVal = 5
    }

    public partial class MainForm : Form
    {
        public static bool rightKeyDown = false;
        public static bool leftKeyDown = false;
        public static int lives = (int)GameParameters.lives;
        public static int score = (int)GameParameters.score;
        public static System.Windows.Forms.Label lLives, lScore, lPoints, lGameOver;
        public static System.Windows.Forms.Button bRetry; 

        Dog dog;
        TreatDropper treatDropper; 

        public MainForm()
        {
            InitializeComponent();

            AddControls();

            dog = new Dog(this);
            treatDropper = new TreatDropper(this, dog);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
 
            
        }

       

        public static void UpdateLives()
        {

            string liveString = " ";
            for (int i = 0; i < lives; i++)
            {
                liveString = liveString + "Y";
            }

            lLives.Text = liveString; 
        }

        public static void UpdateScore()
        {
            string scoreString = ""; 

            for (int i = score.ToString().Length; i < 4; i++)
            {
                scoreString = scoreString + "0"; 
            }

            scoreString = scoreString + score.ToString();

            lPoints.Text = scoreString; 

        }

        public static void GameOver()
        {

            lGameOver.Visible = true;
            bRetry.Visible = true;
            bRetry.Enabled = true; 
        }

        private void bRetry_Click(object sender, EventArgs e)
        {
            lGameOver.Visible = false;
            bRetry.Visible = false;
            bRetry.Enabled = false;

            lives = (int)GameParameters.lives;
            score = (int)GameParameters.score;
            UpdateLives();
            UpdateScore();

            dog.Reset(); 
            treatDropper.ResetTreat();
            treatDropper.DropTreat(); 
        }

        private void AddControls()
        {
            lLives = new Label
            {
                Font = new Font("Webdings", 12),
                ForeColor = System.Drawing.Color.Red,
                Location = new Point(this.Width - 100, 10),
                AutoSize = true,
                Text = " ", 
                Visible = true
            };

            

            lScore = new Label
            {
                Font = new Font("Impact", 12),
                ForeColor = System.Drawing.Color.Red,
                Location = new Point(10, 10),
                AutoSize = true,
                Text = "Score:",
                Visible = true
            };

            lPoints = new Label
            {
                Font = new Font("Impact", 12),
                ForeColor = System.Drawing.Color.Red,
                Location = new Point(60, 10),
                AutoSize = true,
                Text = "0000",
                Visible = true
            };

            lGameOver = new Label
            {
                Text = "GAME OVER",
                Font = new Font("Impact", 25),
                ForeColor = Color.Red,
                Width = 200,
                Height = 55,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point((this.Width / 2) - 100, (this.Height / 2) - 55),
                Visible = false
            };

            bRetry = new Button
            {
                Text = "Retry",
                Visible = false,
                Enabled = false,
                Width = 100,
                Height = 50,
                Location = new Point((this.Width / 2) - 50, (lGameOver.Location.Y + 70))
            };

            this.Controls.Add(lLives);
            this.Controls.Add(lScore);
            this.Controls.Add(lPoints);
            this.Controls.Add(lGameOver);
            this.Controls.Add(bRetry);
            bRetry.Click += new EventHandler(bRetry_Click); 
            UpdateLives();

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
