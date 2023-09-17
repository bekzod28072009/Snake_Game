using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Snake_Game
{
    public partial class Classic : Form
    {
        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();
        Random rand = new Random();

        int maxWidth;
        int maxHeight;

        int score;
        int heightScore;

        bool goLeft, goRight, goDown, goUp;

        public Classic()
        {
            InitializeComponent();

            new Settings();
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Left && Settings.direction != "right")
            {
                goLeft = true;
            }
            if (e.KeyCode == Keys.Right && Settings.direction != "left")
            {
                goRight = true;
            }
            if (e.KeyCode == Keys.Down && Settings.direction != "up")
            {
                goDown = true;
            }
            if (e.KeyCode == Keys.Up && Settings.direction != "down")
            {
                goUp = true;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if (e.KeyCode == Keys.Down)
            {
                goDown = false;
            }
            if (e.KeyCode == Keys.Up)
            {
                goUp = false;
            }
        }

        private void StartGame(object sender, EventArgs e)
        {
            RestartGame();
        }

        private void TakeSnapShot(object sender, EventArgs e)
        {
            Label caption = new Label();
            caption.Text = "I scored " + score + " and my Hightscore is " + heightScore + " on the Snake Game from Robotics Lab";
            caption.Font = new Font("Ariel", 12, FontStyle.Bold);
            caption.ForeColor = Color.Purple;
            caption.AutoSize = false;
            caption.Width = picCanvas.Width;
            caption.Height = 30;
            caption.TextAlign = ContentAlignment.MiddleCenter;
            picCanvas.Controls.Add(caption);

            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.FileName = "Snake Game SmapShot Roboyics Lab";
            fileDialog.DefaultExt = "jpg";
            fileDialog.Filter = "JPG Image File | *.jpg";
            fileDialog.ValidateNames = true;

            if(fileDialog.ShowDialog() == DialogResult.OK)
            {
                int width = Convert.ToInt32(picCanvas.Width);
                int height = Convert.ToInt32(picCanvas.Height);
                Bitmap btm = new Bitmap(width, height);
                picCanvas.DrawToBitmap(btm, new Rectangle(0, 0, width, height));
                btm.Save(fileDialog.FileName, ImageFormat.Jpeg);
                picCanvas.Controls.Remove(caption);
            }
        }

        private void GameTimerEvent(object sender, EventArgs e)
        {
            if (goLeft)
            {
                Settings.direction = "left";
            }
            if (goRight)
            {
                Settings.direction = "right";
            }
            if (goDown)
            {
                Settings.direction = "down";
            }
            if (goUp)
            {
                Settings.direction = "up";
            }

            for(int i = Snake.Count - 1; i >= 0; i--)
            {
                if(i == 0)
                {
                    switch(Settings.direction)
                    {
                        case "left":
                            Snake[i].X--;
                            break;
                        case "right":
                            Snake[i].X++;
                            break;
                        case "down":
                            Snake[i].Y++;
                            break;
                        case "up":
                            Snake[i].Y--;
                            break;
                    }

                    if (Snake[i].X < 0)
                    {
                        Snake[i].X = maxWidth;
                    }
                    if (Snake[i].X > maxWidth)
                    {
                        Snake[i].X = 0;
                    }

                    if (Snake[i].Y < 0)
                    {
                        Snake[i].Y = maxHeight;
                    }
                    if (Snake[i].Y > maxHeight)
                    {
                        Snake[i].Y = 0;
                    }

                    if (Snake[i].X == food.X && Snake[i].Y == food.Y)
                    {
                        EatFood();
                    }

                    for(int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y)
                        {
                            GameOver();
                        }
                    }
                }

                else
                {
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }

            picCanvas.Invalidate();
        }

        private void UpdatePictureBoxGraphics(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            Brush snakeColor;

            for(int i = 0; i < Snake.Count; i++)
            {
                if(i == 0)
                {
                    snakeColor = Brushes.Black;
                }
                else
                {
                    snakeColor = Brushes.DarkGreen;
                }

                canvas.FillEllipse(snakeColor, new Rectangle
                    (
                    Snake[i].X * Settings.Width,
                    Snake[i].Y * Settings.Heigth,

                    Settings.Width, Settings.Heigth
                    ));
            }

            canvas.FillEllipse(Brushes.DarkRed, new Rectangle
                    (
                    food.X * Settings.Width,
                    food.Y * Settings.Heigth,

                    Settings.Width, Settings.Heigth
                    ));
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            StartPage startPage = new StartPage();
            startPage.Show();
            this.Hide();
        }

        private void RestartGame()
        {
            maxWidth = picCanvas.Width / Settings.Width - 1;
            maxHeight = picCanvas.Height / Settings.Heigth - 1;

            Snake.Clear();

            startButton.Enabled = false;
            snapButton.Enabled = false;
            score = 0;
            txtScore.Text = "Score : " + score;

            Circle head = new Circle { X = 10, Y = 5 };
            Snake.Add(head);

            for (int i = 0; i < 10; i++)
            {
                Circle body = new Circle();
                Snake.Add(body);
            }

            food = new Circle { X = rand.Next(2, maxWidth), Y = rand.Next(2, maxHeight) };

            GameTimer.Start();

        }

        private void EatFood()
        {
            score += 1;

            txtScore.Text = "Score : " + score;

            Circle body = new Circle()
            {
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y
            };

            Snake.Add(body);

            food = new Circle { X = rand.Next(2, maxWidth), Y = rand.Next(2, maxHeight) };
        }

        private void GameOver()
        {
            GameTimer.Stop();
            startButton.Enabled = true;
            snapButton.Enabled = true;

            if(score > heightScore)
            {
                heightScore = score;

                txtHightScore.Text = "Hight Score : " + Environment.NewLine + heightScore;
                txtHightScore.ForeColor = Color.Maroon;
                txtHightScore.TextAlign = ContentAlignment.MiddleCenter;
            }
        }

    }
}
