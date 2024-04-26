using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
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

    /// <summary>
    /// A little peep for the world...
    /// </summary>
    class Character
    {
        private static List<Color> levelColours = new List<Color>
        {
            Color.DodgerBlue,
            Color.Gold,
            Color.MediumPurple
        };

        private SwarmManager _cloudSwarm, _orbitalSwarm;

        private Texture2D _baseTxr, _overlayTxr;
        private Texture2D _emptyHeartTxr, _fullHeartTxr;
        private Texture2D _armourChromaTxr, _armourOverlayTxr;

        private List<Rectangle> _StatQuarters;

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
        public int Level;
        public int HPMax;
        private int _hp;

        public int HP
        {
            get => _hp;
            set => _hp = MathHelper.Clamp(value, 0, HPMax);
        }
        private int _armour;

        /// <summary>
        /// Character Constructor
        /// </summary>
        /// <param name="baseTxr">The greyscale texture containing the body (affected by the bodyTint)</param>
        /// <param name="overlayTxr">The texture containing the overlay (screen, buttons) (affected by the mouseover)</param>
        /// <param name="pos">The base position of the character on screen</param>
        /// <param name="cells">How the texture spritesheets are divided up (grid dimensions</param>
        /// <param name="bodyTint">What colour is the body</param>
        public Character(Texture2D baseTxr, Texture2D overlayTxr, 
            List<Texture2D> uiTxrs, Vector2 pos, Point cells)
        {

            var frameSize = baseTxr.Bounds.Size / cells;
            _cloudSwarm = new SwarmManager(pos + frameSize.ToVector2() / 2, frameSize.ToVector2(), SwarmType.Cloud);
            _orbitalSwarm = new SwarmManager(pos + frameSize.ToVector2() / 2, frameSize.ToVector2(), SwarmType.Orbital);

            _baseTxr = baseTxr;
            _overlayTxr = overlayTxr;
            _emptyHeartTxr = uiTxrs[0];
            _fullHeartTxr = uiTxrs[1];
            _armourChromaTxr = uiTxrs[2];
            _armourOverlayTxr = uiTxrs[3];

            _StatQuarters = new List<Rectangle>()
            {
                new Rectangle(0,                      0,                       _emptyHeartTxr.Width/2, _emptyHeartTxr.Height/2),
                new Rectangle(_emptyHeartTxr.Width/2, 0,                       _emptyHeartTxr.Width/2, _emptyHeartTxr.Height/2),
                new Rectangle(0,                      _emptyHeartTxr.Height/2, _emptyHeartTxr.Width/2, _emptyHeartTxr.Height/2),
                new Rectangle(_emptyHeartTxr.Width/2, _emptyHeartTxr.Height/2, _emptyHeartTxr.Width/2, _emptyHeartTxr.Height/2),
            };

            Pos = pos;
            _currPos = pos;
            _rect = new Rectangle(Pos.ToPoint(), new Point(baseTxr.Width / cells.X, baseTxr.Height / cells.Y));

            Level = 0;
            CardAffinity = CardColour.None;
            switch (CardAffinity)
            {
                case CardColour.Black:
                    _bodyTint = Color.DimGray;
                    break;
                case CardColour.Red:
                    _bodyTint = Color.Red;
                    break;
                default:
                    _bodyTint = levelColours[Level];
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

            HPMax = 16;
            _hp = 12;
            _armour = 0;
        }

        public void Update(float deltaTime, Point mousePos)
        {
            _cloudSwarm.Update(deltaTime);
            _orbitalSwarm.Update(deltaTime);

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

            _cloudSwarm.Draw(sB);
            _orbitalSwarm.Draw(sB);

            if (_screenTint != Color.White)
                return;

            DrawStats(sB);
        }

        private void DrawStats(SpriteBatch sB)
        {
            int i;
            int healthY = (int)_currPos.Y;
            for (i = 0; i < _hp/4; i++)
                sB.Draw(_fullHeartTxr, new Vector2(_currPos.X + i * (_fullHeartTxr.Width - 1), healthY), Color.White * 0.9f);
            int quarters = _hp % 4;

            if (quarters != 0)
            {
                int j;
                for (j = 0; j < quarters; j++)
                {
                    sB.Draw(_fullHeartTxr,
                        new Vector2(_currPos.X + i * (_fullHeartTxr.Width - 1) + _StatQuarters[0].Width * (j % 2),
                            healthY + _StatQuarters[0].Height * (j / 2)),
                        _StatQuarters[j],
                        Color.White * 0.9f);
                }

                for (; j < 4; j++)
                {
                    sB.Draw(_emptyHeartTxr,
                        new Vector2(_currPos.X + i * (_fullHeartTxr.Width - 1) + _StatQuarters[0].Width * (j % 2),
                            healthY + _StatQuarters[0].Height * (j / 2)),
                        _StatQuarters[j],
                        Color.White * 0.9f);
                }

                i++;
            }
            for (; i < HPMax / 4; i++)
                sB.Draw(_emptyHeartTxr, new Vector2(_currPos.X + i * (_fullHeartTxr.Width - 1), healthY),
                    Color.White * 0.9f);

            for (i = 0; i < _armour; i++)
            {
                sB.Draw(_armourChromaTxr, new Vector2(_currPos.X + i * _armourChromaTxr.Width, healthY - 15), _bodyTint * 0.9f);
                sB.Draw(_armourOverlayTxr, new Vector2(_currPos.X + i * _armourOverlayTxr.Width, healthY - 15),
                    Color.White * 0.9f);
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

        public void GainLevel(CardColour levelingCardColour)
        {
            if (Level == levelColours.Count)
            {
                Debug.WriteLine("Peep is already max level when assigned a level up!");
                return;
            }

            Level++;
            HPMax += 4;

            if (Level == levelColours.Count)
                CardAffinity = levelingCardColour;

            switch (CardAffinity)
            {
                case CardColour.Black:
                    _bodyTint = Color.DimGray;
                    break;
                case CardColour.Red:
                    _bodyTint = Color.Red;
                    break;
                default:
                    _bodyTint = levelColours[Level];
                    break;
            }
        }

        public void GainArmour()
        {
            _armour++;
        }

        public void GainCloudSwarm(Texture2D tex, int amount = 1)
        {
            _cloudSwarm.AddToSwarm(tex, amount);
        }

        public void GainOrbitalSwarm(Texture2D tex, int amount = 1)
        {
            _orbitalSwarm.AddToSwarm(tex, amount);
        }

        public void GainHealth(int amount = 1)
        {
            HP += amount;
        }
    }
}
