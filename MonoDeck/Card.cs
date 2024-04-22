using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

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

        public Vector2 Pos { get; set; }
        private Rectangle _rect;

        CardData Data;

        public Card(Vector2 pos, Texture2D frontTex, Texture2D backTex, CardData data)
        {
            Pos = pos;
            _rect = new Rectangle(pos.ToPoint(), frontTex.Bounds.Size);

            _frontTex = frontTex;
            _backTex = backTex;
            Data = data;
        }

        public void Draw(SpriteBatch sb, FacingState facingState)
        {
            if (facingState == FacingState.FaceUp)
                sb.Draw(_frontTex, Pos, Color.LightGray);
            else
                sb.Draw(_backTex, Pos, Color.LightGray);
        }

        public string DebugInfo()
        {
            return Data.DebugInfo();
        }
    }
}