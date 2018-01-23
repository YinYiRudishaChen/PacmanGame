using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Packman_Game
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        System.Windows.Forms.Timer enemytimer = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer pacmantimer = new System.Windows.Forms.Timer();
        Characters.Dots[] dots = new Characters.Dots[60];
        Characters.Block[] blocks = new Characters.Block[19];
        Characters.Pacman pack;
        Characters.Enemy enemy;
        int direction = 0;
        bool[] _check = new bool[2];
       

        private void button1_Click(object sender, EventArgs e)
        {
            this.PacmanGroupBox.Controls.Clear();
            this.PacmanGroupBox.Refresh();

            GameInitialization();
        }

        void GameInitialization()
        {
            direction = 0;
            label1.Text = "0";
            _check[0] = false;
            _check[1] = false;
            enemytimer.Interval = 300;
            enemytimer.Tick -= T_Tick;
            enemytimer.Tick += T_Tick;

            pacmantimer.Interval = 100;
            pacmantimer.Tick -= Pacmantimer_Tick;
            pacmantimer.Tick += Pacmantimer_Tick;

            pack = new Characters.Pacman(ref dots, ref blocks);
            enemy = new Characters.Enemy(pack, ref blocks);

            pack.Pacman_PointsChanged += new 
                Characters.Pacman_PointsChanged(pack_Pacman_PointsChanged);
            pack.Pacman_Messages += new 
                Characters.Pacman_Messages(pack_Pacman_Messages);

            pack.Location = new Point(100, 100);
            enemy.Location = new Point(440, 100);

            this.PacmanGroupBox.Controls.Add(pack);
            this.PacmanGroupBox.Controls.Add(enemy);

            LoadDots();
            LoadBlocks();
            enemytimer.Start();
            pacmantimer.Start();
        }

        private void Pacmantimer_Tick(object sender, EventArgs e)
        {
            switch (direction)
            {
                case 1:
                    pack.Move(Characters.MovementWay.Up);
                    break;
                case 2:
                    pack.Move(Characters.MovementWay.Down);
                    break;
                case 3:
                    pack.Move(Characters.MovementWay.Left);
                    break;
                case 4:
                    pack.Move(Characters.MovementWay.Right);
                    break;
                default:
                    break;
            }
            if (pack.Location == enemy.Location)
            {
                pack.Catched(enemy);
            }
            if (_check[0] == false)
            {
                if (40 >= pack.Location.X && 40 <= (pack.Location.X + (pack.Width / 3)) && 80 >= pack.Location.Y && 80 <= ((pack.Height / 3) + pack.Location.Y))
                {
                    enemy.Location = new Point(440, 100);
                    _check[0] = true;
                }
            }
            if (_check[1] == false)
            {
                if (480 >= pack.Location.X && 480 <= (pack.Location.X + (pack.Width / 3)) && 440 >= pack.Location.Y && 440 <= ((pack.Height / 3) + pack.Location.Y))
                {
                    enemy.Location = new Point(440, 100);
                    _check[1] = true;
                }
            }
        }

        private void T_Tick(object sender, EventArgs e)
        {
            if (enemy.Location.X > pack.Location.X 
                && !enemy.IsBlock(Characters.MovementWay.Left))
            {
                enemy.Move(Characters.MovementWay.Left);
            }
            else if (enemy.Location.Y > pack.Location.Y 
                && !enemy.IsBlock(Characters.MovementWay.Up))
            {
                enemy.Move(Characters.MovementWay.Up);
            }
            else if (enemy.Location.X < pack.Location.X 
                && !enemy.IsBlock(Characters.MovementWay.Right))
            {
                enemy.Move(Characters.MovementWay.Right);
            }
            else if (enemy.Location.Y < pack.Location.Y 
                && !enemy.IsBlock(Characters.MovementWay.Down))
            {
                enemy.Move(Characters.MovementWay.Down);
            }
        }

        /*
        private void T_Tick(object sender, EventArgs e)
        {
            if (enemy.Location.X > pack.Location.X)
            {
                if (enemy.IsBlock(Characters.MovementWay.Left))
                {
                    if (enemy.Location.Y > pack.Location.Y)
                    {
                        if (!enemy.IsBlock(Characters.MovementWay.Up))
                            enemy.Move(Characters.MovementWay.Up);
                    }
                    else if(!enemy.IsBlock(Characters.MovementWay.Down))
                        enemy.Move(Characters.MovementWay.Down);
                }
                else
                    enemy.Move(Characters.MovementWay.Left);
            }
            else
               if (enemy.Location.Y > pack.Location.Y)
            {
                if (enemy.IsBlock(Characters.MovementWay.Up))
                {
                    if (enemy.Location.X < pack.Location.X)
                    {
                        if (!enemy.IsBlock(Characters.MovementWay.Right))
                            enemy.Move(Characters.MovementWay.Right);
                    }
                    else if (!enemy.IsBlock(Characters.MovementWay.Left))
                        enemy.Move(Characters.MovementWay.Left);
                }
                else
                    enemy.Move(Characters.MovementWay.Up);
            }
            else
               if (enemy.Location.X < pack.Location.X)
            {
                if (enemy.IsBlock(Characters.MovementWay.Right))
                {
                    if (enemy.Location.Y > pack.Location.Y)
                    {
                        if (!enemy.IsBlock(Characters.MovementWay.Up))
                            enemy.Move(Characters.MovementWay.Up);
                    }
                    else if (!enemy.IsBlock(Characters.MovementWay.Down))
                        enemy.Move(Characters.MovementWay.Down);
                }
                else
                    enemy.Move(Characters.MovementWay.Right);
            }
            else
                 if (enemy.Location.Y < pack.Location.Y)
            {
                if (enemy.IsBlock(Characters.MovementWay.Down))
                {
                    if (enemy.Location.X > pack.Location.X)
                    {
                        if (!enemy.IsBlock(Characters.MovementWay.Left))
                            enemy.Move(Characters.MovementWay.Left);
                    }
                    else if (!enemy.IsBlock(Characters.MovementWay.Right))
                        enemy.Move(Characters.MovementWay.Right);
                }
                else
                    enemy.Move(Characters.MovementWay.Down);
            }
        }
        */

        void LoadBlocks()
        {
            for (int i = 0; i < blocks.Length; i++)
                blocks[i] = new Characters.Block();

            blocks[15] = new Characters.Block(560, 20, new Point(20, 20));
            blocks[16] = new Characters.Block(20, 480, new Point(20, 20));
            blocks[17] = new Characters.Block(20, 480, new Point(560, 20));
            blocks[18] = new Characters.Block(560, 20, new Point(20, 480));

            blocks[0] = new Characters.Block(100, 20, new Point(80, 80));
            blocks[1] = new Characters.Block(20, 100, new Point(80, 80));

            blocks[2] = new Characters.Block(100, 20, new Point(420, 80));
            blocks[3] = new Characters.Block(20, 100, new Point(500, 80));

            blocks[9] = new Characters.Block(20, 100, new Point(80, 320));
            blocks[10] = new Characters.Block(100, 20, new Point(80, 420));

            blocks[11] = new Characters.Block(100, 20, new Point(420, 420));
            blocks[12] = new Characters.Block(20, 100, new Point(500, 320));

            ////////////////////////////////////////////////

            blocks[4] = new Characters.Block(20, 100, new Point(260, 100));
            blocks[5] = new Characters.Block(20, 100, new Point(320, 100));

            blocks[6] = new Characters.Block(160, 20, new Point(220, 240));

            blocks[7] = new Characters.Block(20, 100, new Point(260, 300));
            blocks[8] = new Characters.Block(20, 100, new Point(320, 300));

            blocks[13] = new Characters.Block(20, 60, new Point(160, 220));
            blocks[14] = new Characters.Block(20, 60, new Point(420, 220));

            this.PacmanGroupBox.Controls.AddRange(blocks);
        }

        void LoadDots()
        {
            // Top
            for (int i = 0; i < 12; i++)
            {
                dots[i] = new Characters.Dots();
                dots[i].Location = new Point(40 + i * 40, 40);
            }

            // Left
            for (int i = 12; i < 22; i++)
            {
                dots[i] = new Characters.Dots();
                dots[i].Location = new Point(40, 40 + (i - 11) * 40);
            }
            dots[12].Dot_Color = Color.BlueViolet;

            // Bottom
            for (int i = 22; i < 34; i++)
            {
                dots[i] = new Characters.Dots();
                dots[i].Location = new Point(40 + (i - 21) * 40, 440);
            }
            dots[32].Dot_Color = Color.BlueViolet;

            // Right
            for (int i = 34; i < 44; i++)
            {
                dots[i] = new Characters.Dots();
                dots[i].Location = new Point(520, 40 + (i - 34) * 40);
            }

            //Middle Upper Row
            for (int i = 44; i < 49; i++)
            {
                dots[i] = new Characters.Dots();
                dots[i].Location = new Point((i - 39) * 40, 200);
            }

            //Middle Lower Row
            for (int i = 49; i < 54; i++)
            {
                dots[i] = new Characters.Dots();
                dots[i].Location = new Point((i - 44) * 40, 260);
            }

            //Middle Upper Column
            for (int i = 54; i < 57; i++)
            {
                dots[i] = new Characters.Dots();
                dots[i].Location = new Point(280, (i - 52) * 40);
            }

            //Middle Lower Column
            for (int i = 57; i < 60; i++)
            {
                dots[i] = new Characters.Dots();
                dots[i].Location = new Point(280, 260 + (i - 56) * 40);
            }

            this.PacmanGroupBox.Controls.AddRange(dots);
        }

        void pack_Pacman_Messages(object sender, string messages)
        {
            enemytimer.Stop();
            pacmantimer.Stop();
            MessageBox.Show(messages);
            button1_Click(sender, null);
        }

        void pack_Pacman_PointsChanged(object sender, int totalPoints)
        {
            label1.Text = totalPoints.ToString();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    direction = 1;
                    break;

                case Keys.S:
                    direction = 2;
                    break;

                case Keys.A:
                    direction = 3;
                    break;

                case Keys.D:
                    direction = 4;
                    break;
            }
        }
    }
}