using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SharpDX.XAudio2;

namespace MonoDeck
{
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
        Red
    }

    enum CardRank
    {
        Basic,
        Royal
    }

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
            return ($"Type: {Type}\nColour: {Colour}\nRank: {Rank}\nValue:{Value}");
        }
    }

    class Card
    {
        Texture2D _frontTex;
        Texture2D _backTex;

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

        CardData Data;

        public Card(Vector2 pos, Texture2D frontTex, Texture2D backTex, CardData data)
        {
            Pos = pos;
            _rect = new Rectangle(pos.ToPoint(), frontTex.Bounds.Size);
            Highlight = false;

            _frontTex = frontTex;
            _backTex = backTex;
            Data = data;
        }

        public void Draw(SpriteBatch sb, FacingState facingState, bool highlight)
        {
            Draw(sb, Pos, facingState, highlight);
        }

        public void Draw(SpriteBatch sb, Vector2 position, FacingState facingState, bool highlight)
        {
            sb.Draw(facingState == FacingState.FaceUp ? _frontTex : _backTex, position, highlight ? Color.White : Color.LightGray);
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