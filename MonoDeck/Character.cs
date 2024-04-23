using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoDeck
{
    enum CharState
    {
        Idle = 0,
        Happy,
        Sad,
        Walking,
        Jumping
    }

    class Character
    {
        protected Texture2D _baseTxr, _overlayTxr;

        public Vector2 Pos { get; }
        private Rectangle _rect;

        private Color _bodyTint, _screenTint;

        CharState _cState;
        private float _moodTimer;

        private float _walkPace;
        private int _walkStep;

        private Vector2 _currPos;
        private Vector2 _velocity;

        private readonly List<Rectangle> _srcCells;

        public CardColour CardAffinity;

        /// <summary>
        /// Character Constructor
        /// </summary>
        /// <param name="baseTxr">The greyscale texture containing the body (affected by the bodyTint)</param>
        /// <param name="overlayTxr">The texture containing the overlay (screen, buttons) (affected by the mouseover)</param>
        /// <param name="pos">The base position of the character on screen</param>
        /// <param name="cells">How the texture spritesheets are divided up (grid dimensions</param>
        /// <param name="bodyTint">What colour is the body</param>
        public Character(Texture2D baseTxr, Texture2D overlayTxr, Vector2 pos, Point cells, CardColour cardAffinity)
        {
            _baseTxr = baseTxr;
            _overlayTxr = overlayTxr;

            Pos = pos;
            _currPos = pos;
            _rect = new Rectangle(Pos.ToPoint(), new Point(baseTxr.Width / cells.X, baseTxr.Height / cells.Y));

            CardAffinity = cardAffinity;
            switch (CardAffinity)
            {
                case CardColour.Black:
                    _bodyTint = Color.DimGray;
                    break;
                case CardColour.Red:
                    _bodyTint = Color.Red;
                    break;
                default:
                    _bodyTint = Color.DodgerBlue;
                    break;
            }

            _screenTint = Color.LightGray;

            _cState = CharState.Idle;

            
            _velocity = Vector2.Zero;

            var cellSize = new Point(baseTxr.Width / cells.X, baseTxr.Height / cells.Y);
            _srcCells = new List<Rectangle>();
            for (var y = 0; y < cells.Y; y++)
                for (var x = 0; x < cells.X; x++)
                    _srcCells.Add(new Rectangle(new Point(x * cellSize.X, y * cellSize.Y), cellSize));

            _walkPace = 0.25f;
        }

        public void Update(float deltaTime, Point mousePos)
        {
            _rect.Location = _currPos.ToPoint();
            _screenTint = _rect.Contains(mousePos) ? Color.White : Color.LightGray;

            if (_currPos.Y < Pos.Y)
            {
                _cState = CharState.Jumping;
                _velocity.Y += (50 * deltaTime);
                _currPos += _velocity;
                return;
            }

            _currPos.Y = Pos.Y;
            _velocity.Y = 0;

            switch (_cState)
            {
                case CharState.Idle:
                    return;
                case CharState.Walking:
                {
                    _walkPace -= deltaTime;
                    if (_walkPace < 0)
                    {
                        _walkStep = _walkStep == 0 ? 1 : 0;
                        _walkPace = 0.25f;
                    }
                    return;
                }
                case CharState.Happy:
                case CharState.Sad:
                case CharState.Jumping:
                default:
                    break;
            }

            if (_moodTimer > 0)
            {
                _moodTimer -= deltaTime;
            }
            else
            {
                _moodTimer = 0;
                _cState = CharState.Idle;
            }
        }

        public void Draw(SpriteBatch sB)
        {
            switch (_cState)
            {
                case CharState.Idle:
                case CharState.Happy:
                case CharState.Sad:
                    sB.Draw(_baseTxr, Pos, _srcCells[(int) _cState], _bodyTint);
                    sB.Draw(_overlayTxr, Pos, _srcCells[(int) _cState], _screenTint);
                    break;
                case CharState.Walking:
                    sB.Draw(_baseTxr, Pos, _srcCells[3+_walkStep], _bodyTint);
                    sB.Draw(_overlayTxr, Pos, _srcCells[3+_walkStep], _screenTint);
                    break;
                case CharState.Jumping:
                    sB.Draw(_baseTxr, _currPos, _srcCells[5], _bodyTint);
                    sB.Draw(_overlayTxr, _currPos, _srcCells[5], _screenTint);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public string DebugInfo()
        {
            return ($"Root: {Pos}\ndeltaV: {_currPos}\nState: {_cState}\nVel:{_velocity}\n");
        }

        public bool Hover()
        {
            return _screenTint == Color.White;
        }

        public void SetMood(CharState newMood, float duration)
        {
            _cState = newMood;
            _moodTimer = duration;
        }

        public void Jump(float deltaTime)
        {
            _velocity.Y = -800 * deltaTime;
            _currPos += _velocity;
        }
    }
}
