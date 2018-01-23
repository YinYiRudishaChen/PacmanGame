using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace Packman_Game.Characters
{
    public delegate void Pacman_Movement(object sender, System.Drawing.Point location);
    public delegate void Pacman_PointsChanged(object sender, int totalPoints);
    public delegate void Pacman_Messages(object sender, string messages);

    public class Pacman : System.Windows.Forms.Control, ICharacter
    {
        //Events
        public event Pacman_Movement Pacman_Movement;
        public event Pacman_PointsChanged Pacman_PointsChanged;
        public event Pacman_Messages Pacman_Messages;

        //Fields
        CharacterType m_Type = CharacterType.Packman;
        int m_Speed = 20;
        int m_TotalPoints = 0;
        private Dots[] _dots = null;
        private Block[] _blocks = null;
        private bool _catched = false;
        private MovementWay _movement = MovementWay.Right;

        //Constructors
        public Pacman()
        {
            this.Width = this.Height = 40;
        }
        public Pacman(ref Dots[] dots)
            : this()
        {
            _dots = dots;
            this.Pacman_Movement += new Characters.Pacman_Movement(Pacman_Pacman_Movement);
        }
        public Pacman(ref Dots[] dots, ref Block[] blocks)
            : this(ref dots)
        {
            _blocks = blocks;
        }
        public Pacman(ref Block[] blocks)
            : this()
        {
            _blocks = blocks;
        }

        //Attributes
        public CharacterType Type
        {
            get
            {
                return m_Type;
            }
        }
        public int Speed
        {
            get
            {
                return m_Speed;
            }
            set
            {
                m_Speed = value;
            }
        }
        public int TotalPoints
        {
            get
            {
                return m_TotalPoints;
            }
            set
            {
                m_TotalPoints = value;
                if (Pacman_PointsChanged != null)
                    Pacman_PointsChanged(this, value);
            }
        }

        //ReadOnly Attributes
        public Dots[] Dots
        {
            get { return _dots; }
        }
        public Block[] Blocks
        {
            get { return _blocks; }
        }
        public MovementWay Movement
        {
            get { return _movement; }
        }

        //Methods
        void Pacman_Pacman_Movement(object sender, System.Drawing.Point location)
        {
            for (int i = 0; i <= _dots.Length - 1; i++)
            {
                if (_dots[i] == null)
                    continue;

                if (_dots[i].Location.X >= location.X && _dots[i].Location.X <= (location.X + (this.Width/3)) && _dots[i].Location.Y >= location.Y && _dots[i].Location.Y <= ((this.Height/3)+ location.Y))
                {
                    (sender as Characters.Pacman).TotalPoints += _dots[i].Points;
                    _dots[i].Dispose();
                    _dots[i] = null;
                }
            }
            if ((_dots.Where(d => d != null).Count() < 1))
            {
                if (Pacman_Messages != null)
                    Pacman_Messages(this, "You win !!");
            }
        }
        public bool IsBlock(MovementWay Movement)
        {
            bool result = false;

            Point loc = new Point();
            loc.X = this.Location.X;
            loc.Y = this.Location.Y;

            if (Movement == MovementWay.Right)
                loc.X += this.Width;

            for (int i = 0; i <= _blocks.Length -1; i++)
            {
                if (_blocks[i] == null)
                    continue;

                switch (Movement)
                {
                    case MovementWay.Right:
                        if (loc.X == _blocks[i].Location.X)
                        {
                            if (loc.Y >= (_blocks[i].Location.Y - Speed) && loc.Y <= (_blocks[i].Location.Y + _blocks[i].Height - Speed))
                            {
                                result = true;
                                break;
                            }
                        }
                        break;
                    case MovementWay.Left:
                        if (loc.X == (_blocks[i].Location.X + _blocks[i].Width))
                        {
                            if (loc.Y >= (_blocks[i].Location.Y - Speed)&& loc.Y <= (_blocks[i].Location.Y + _blocks[i].Height - Speed))
                            {
                                result = true;
                                break;
                            }
                        }
                        break;
                    case MovementWay.Up:
                        if (loc.Y == (_blocks[i].Location.Y + _blocks[i].Height))
                        {
                            if (loc.X >= (_blocks[i].Location.X - Speed) && loc.X <= (_blocks[i].Location.X + _blocks[i].Width - Speed))
                            {
                                result = true;
                                break;
                            }
                        }
                        break;
                    case MovementWay.Down:
                        if ((loc.Y + this.Height) == _blocks[i].Location.Y)
                        {
                            if (loc.X >= (_blocks[i].Location.X - Speed) && loc.X <= (_blocks[i].Location.X + _blocks[i].Width -Speed))
                            {
                                result = true;
                                break;
                            }
                        }
                        break;
                }                
            }
            return result;
        }
        public new void Move(MovementWay way)
        {
            if (_catched)
                return;
            _movement = way;
            OnPaint(new System.Windows.Forms.PaintEventArgs(this.CreateGraphics(), this.ClientRectangle));
            switch (way)
            {
                case MovementWay.Up:
                    if (!IsBlock(_movement))
                    {
                        this.Location = new System.Drawing.Point(this.Location.X, this.Location.Y - Speed);
                    }
                    break;
                case MovementWay.Down:
                    if (!IsBlock(_movement))
                    {
                        this.Location = new System.Drawing.Point(this.Location.X, this.Location.Y + Speed);
                    }
                    break;
                case MovementWay.Left:
                    if (!IsBlock(_movement))
                    {
                        this.Location = new System.Drawing.Point(this.Location.X - Speed, this.Location.Y);
                    }
                    break;
                case MovementWay.Right:
                    if (!IsBlock(_movement))
                    {
                        this.Location = new System.Drawing.Point(this.Location.X + Speed, this.Location.Y);
                    }
                    break;
            }
            if (_dots != null)
            {
                Pacman_Movement(this, this.Location);
                return;
            }
            if (Pacman_Movement != null)
                Pacman_Movement(this, this.Location);
        }
        private void FillRegion()
        {
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, this.Width, this.Height);
            switch (_movement)
            {
                case MovementWay.Right:
                    path.AddPie(0, 0, this.Width, this.Height, 310, 100);
                    break;
                case MovementWay.Left:
                    path.AddPie(0, 0, this.Width, this.Height, 130, 100);
                    break;
                case MovementWay.Up:
                    path.AddPie(0, 0, this.Width, this.Height, 220, 100);
                    break;
                case MovementWay.Down:
                    path.AddPie(0, 0, this.Width, this.Height, 40, 100);
                    break;
            }
            this.Region = new System.Drawing.Region(path);
        }
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            DrawCharacter.Draw(ref e, Type, _movement);
            FillRegion();
            base.OnPaint(e);
        }    
        public virtual void Catched(Enemy sender)
        {
            Graphics g = this.CreateGraphics();

            g.FillEllipse(System.Drawing.Brushes.Red, 0, 0, Width, Height);
            g.FillEllipse(System.Drawing.Brushes.Black, 20, 10, 5, 5);
            g.FillEllipse(System.Drawing.Brushes.Transparent, 35, 20, 10, 5);

            _catched = true;

            if (Pacman_Messages != null)
            {
                Pacman_Messages(this, "Pacman has been catched by an enemy.");                
            }
        }        
    }
}
