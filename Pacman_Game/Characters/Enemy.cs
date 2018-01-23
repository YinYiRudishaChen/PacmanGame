using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Packman_Game.Characters
{
    public class Enemy : System.Windows.Forms.Control, ICharacter
    {
        //Fields
        CharacterType m_Type = CharacterType.Enemy;
        int m_Speed = 20;
        private Block[] _blocks = null;

        //Constructors
        public Enemy()
        {
            this.Height = this.Width = 40;
        }
        public Enemy(Characters.Pacman pacman,ref Block[] blocks)
            :this()
        {
            _blocks = blocks;
        }

        //Attributes
        public CharacterType Type
        {
            get { return m_Type; }
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

        //ReadOnly Attributes
        public Block[] Blocks
        {
            get { return _blocks; }
        }

        //Methods
        public bool IsBlock(MovementWay _Movement)
        {
            bool result = false;

            Point loc = new Point();
            loc.X = this.Location.X;
            loc.Y = this.Location.Y;

            if (_Movement == MovementWay.Right)
                loc.X += this.Width;

            for (int i = 0; i <= _blocks.Length - 1; i++)
            {
                if (_blocks[i] == null)
                    continue;

                switch (_Movement)
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
                            if (loc.Y >= (_blocks[i].Location.Y - Speed) && loc.Y <= (_blocks[i].Location.Y + _blocks[i].Height - Speed))
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
                            if (loc.X >= (_blocks[i].Location.X - Speed) && loc.X <= (_blocks[i].Location.X + _blocks[i].Width - Speed))
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
            switch (way)
            {
                case MovementWay.Up:
                    this.Location = new System.Drawing.Point(this.Location.X, this.Location.Y - Speed);
                    break;

                case MovementWay.Down:
                    this.Location = new System.Drawing.Point(this.Location.X, this.Location.Y + Speed);
                    break;

                case MovementWay.Left:
                    this.Location = new System.Drawing.Point(this.Location.X - Speed, this.Location.Y);
                    break;

                case MovementWay.Right:
                    this.Location = new System.Drawing.Point(this.Location.X + Speed, this.Location.Y);
                    break;
            }
            return;
        }
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            DrawCharacter.Draw(ref e, Type);

            base.OnPaint(e);
        }
    }
}
