using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

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
    }

    class Card
    {
        Texture2D _frontTex;
        Texture2D _backTex;
        CardData Data;

        public Card(Texture2D frontTex, Texture2D backTex, CardData data)
        {
            _frontTex = frontTex;
            _backTex = backTex;
            Data = data;
        }

        public void DrawAt(SpriteBatch sb, Vector2 pos, FacingState facingState)
        {
            if (facingState == FacingState.FaceUp)
                sb.Draw(_frontTex, pos, Color.LightGray);
            else
                sb.Draw(_backTex, pos, Color.LightGray);
        }
    }
}