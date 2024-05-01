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
        None
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
        Court
    }

    enum CourtCards
    {
        Ace = 1,
        Jack,
        Queen,
        King,
        Joker
    }
    #endregion

    /// <summary>
    /// The gameplay information about the card.
    /// </summary>
    struct CardData
    {
        public int Idx;
        public CardType Type;
        public CardColour Colour;
        public CardRank Rank;
        public int Value;

        public CardData(int idx, CardType ctype, CardColour colour, CardRank rank, int value)
        {
            Idx = idx;
            Type = ctype; 
            Colour = colour; 
            Rank = rank; 
            Value = value;
        }

        public string DebugInfo()
        {
            return ($"**Card {Idx} Info**:\tType: {Type}\tColour: {Colour}\tRank: {Rank}\tValue:{Value}\n");
        }
    }

    class Card
    {
        // Front and back art of the card
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

        public CardData Data { get; private set; }

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

        public void DrawMini(SpriteBatch sb, Point position, FacingState facingState, bool highlight)
        {
            Rectangle rect = _rect;
            rect.Size = _rect.Size / new Point(2);
            rect.Location = position;

            sb.Draw(facingState == FacingState.FaceUp? _frontTex : _backTex, 
                rect, 
                null,
                highlight ? Color.White : Color.LightGray,
                0,
                rect.Size.ToVector2() / 2,
                SpriteEffects.None,
                0
            );
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