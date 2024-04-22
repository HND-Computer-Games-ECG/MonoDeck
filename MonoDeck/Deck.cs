using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace MonoDeck
{
    class Deck
    {
        private Texture2D _backTex;
        private List<Card> _drawPile;
        private List<Card> _discardPile;

        private Vector2 _pos;
        private Rectangle _rect;

        private bool _highLight;

        public Deck(Texture2D backTex, Vector2 pos)
        {
            _backTex = backTex;
            _drawPile = new List<Card>();
            _discardPile = new List<Card>();

            _pos = pos;
            _rect = new Rectangle(pos.ToPoint(), backTex.Bounds.Size);
        }

        public void Update(float deltaTime, Point mousePos)
        {
            _highLight = _rect.Contains(mousePos);
        }

        public void Draw(SpriteBatch sb)
        {
            for (var i = 0; i < _drawPile.Count; i++)
            {
                sb.Draw(_backTex, _pos + new Vector2(i/4, i/2), _highLight ? Color.White : Color.LightGray);
            }


            foreach (var card in _discardPile)
                card.Draw(sb, FacingState.FaceUp);
        }

        public string DebugInfo()
        {
            return ($"Pos:{_pos}\nDrawpile:{_drawPile.Count}\nDiscards:{_discardPile.Count}\n");
        }

        public void DebugDumpDeck()
        {
            foreach (var card in _drawPile)
            {
                Debug.WriteLine(card.DebugInfo());
            }
        }

        public bool Hover()
        {
            return _highLight;
        }

        public void AddCard(Texture2D frontTex, CardData data)
        {
            AddCard(frontTex, _backTex, data);
        }

        public void AddCard(Texture2D frontTex, Texture2D backTex, CardData data)
        {
            _drawPile.Add(new Card(_pos, frontTex, backTex, data));
        }

        public Card PullCard(int location = 0)
        {
            if (_drawPile.Count == 0)
            {
                Debug.WriteLine("Attempt to draw from empty deck!");
                return null;
            }

            if (_drawPile.Count <= 0)
            {
                Debug.WriteLine("Attempt to draw from outside deck bounds!");
                return null;
            }

            var tmp = _drawPile[location];
            _drawPile.RemoveAt(location);
            return tmp;
        }

        public void DiscardCard(Card newDiscard)
        {
            if (newDiscard == null)
            {
                Debug.WriteLine("Attempt to add null card to discards!");
                return;
            }

            // Set the new discard to the discard pile's location plus a little shoogle to make it look cool
            newDiscard.Pos = _pos + new Vector2(_backTex.Width + 16, 0) + new Vector2(Game1.RNG.Next(-4, 4), Game1.RNG.Next(-4, 4));

            _discardPile.Add(newDiscard);
        }

        public void Shuffle()
        {
            for (var i = _drawPile.Count - 1; i > 0; i--)
            {
                var j =  Game1.RNG.Next(i + 1);
                (_drawPile[i], _drawPile[j]) = (_drawPile[j], _drawPile[i]);
            }
        }
    }
}
