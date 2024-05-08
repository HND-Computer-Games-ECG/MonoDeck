using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SharpDX.XAudio2;

namespace MonoDeck
{
    #region card enums
    enum FacingState
    {
        FaceUp,
        FaceDown
    }

    enum CardType
    {
        Club,
        Diamond,
        Heart,
        Spade,
    }

    enum CardColour
    {
        Black,
        Red,
        None
    }

    enum CardRank
    {
        Basic,
        Royal
    }
    #endregion

    /// <summary>
    /// The gameplay information about the card.
    /// </summary>
    struct CardData
    {
        public CardType Type;
        public CardColour Colour;
        public CardRank Rank;
        public int Value;

        public CardData(CardType ctype, CardColour colour, CardRank rank, int value)
        {
            Type = ctype; 
            Colour = colour; 
            Rank = rank; 
            Value = value;
        }

        public string DebugInfo()
        {
            return ($"**Card Info**:\nType: {Type}\nColour: {Colour}\nRank: {Rank}\nValue:{Value}\n");
        }
    }

    class Card
    {
        // Front and back art of the card
        Texture2D _frontTex;
        Texture2D _backTex;

        Texture2D _particle;
        SteamerEmitter _particleEmitter;

        private Vector2 _pos;
        public Vector2 Pos 
        {
            get => _pos;
            set
            {
                _pos = value;
                _rect.Location = value.ToPoint();
            }
        }
        private Rectangle _rect;

        public bool Highlight { get; set; }

        public CardData Data { get; private set; }

        public Card(Vector2 pos, Texture2D frontTex, Texture2D backTex, CardData data, Texture2D particle)
        {
            Pos = pos;
            _rect = new Rectangle(pos.ToPoint(), frontTex.Bounds.Size);
            Highlight = false;

            _frontTex = frontTex;
            _backTex = backTex;
            Data = data;

            _particle = particle;
            _particleEmitter = new SteamerEmitter(particle, _rect.Center.ToVector2(), data.Value, 
                new Vector2(frontTex.Width/2, frontTex.Height/4));
        }

        public void Update(float deltaTime, FacingState facing)
        {
            if (facing == FacingState.FaceUp)
                _particleEmitter.Update(deltaTime, _rect.Center.ToVector2());
        }

        public void Draw(SpriteBatch sb, FacingState facingState, bool highlight)
        {
            Draw(sb, Pos, facingState, highlight);
        }

        public void Draw(SpriteBatch sb, Vector2 position, FacingState facingState, bool highlight)
        {
            sb.Draw(facingState == FacingState.FaceUp ? _frontTex : _backTex, position, highlight ? Color.White : Color.LightGray);

            if (facingState == FacingState.FaceUp)
            {
                _particleEmitter.Start();
                _particleEmitter.Draw(sb);
            }
        }

        public void DrawMini(SpriteBatch sb, Point position, FacingState facingState, bool highlight)
        {
            Pos = position.ToVector2();
            Rectangle rect = _rect;
            rect.Size = _rect.Size / new Point(2);
            rect.Location = position;

            sb.Draw(facingState == FacingState.FaceUp? _frontTex : _backTex, 
                rect, 
                null,
                highlight ? Color.White : Color.LightGray
            );

            if (facingState == FacingState.FaceUp)
            {
                _particleEmitter.Stop();
                _particleEmitter.Draw(sb);
            }
        }

        public string DebugInfo()
        {
            return Data.DebugInfo();
        }

        public bool Hover(Point mousePos)
        {
            return _rect.Contains(mousePos);
        }
    }
}