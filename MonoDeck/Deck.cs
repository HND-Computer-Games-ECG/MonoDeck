using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace MonoDeck
{
    enum CardPile
    {
        Draw,
        Discard
    }

    class Deck
    {
        private Texture2D _backTex;
        private List<Card> _drawPile;
        private List<Card> _discardPile;

        public Vector2 Pos { get; private set; }

        private Rectangle _drawpileRect, _discardRect;

        private bool _drawpileHighlight, _discardHighlight;

        public bool IsEmpty => _drawPile.Count == 0;
        public bool IsDiscardEmpty => _discardPile.Count == 0;

        public Deck(Texture2D backTex, Vector2 pos)
        {
            _backTex = backTex;
            _drawPile = new List<Card>();
            _discardPile = new List<Card>();

            Pos = pos;
            _drawpileRect = new Rectangle(pos.ToPoint(), backTex.Bounds.Size);
            _discardRect = new Rectangle(pos.ToPoint() + new Point(_backTex.Width + 16, 0), backTex.Bounds.Size);
        }

        public void Update(float deltaTime, Point mousePos)
        {
            _drawpileHighlight = _drawpileRect.Contains(mousePos);
            _discardHighlight = _discardRect.Contains(mousePos);

            foreach (var card in _drawPile)
                card.Update(deltaTime, FacingState.FaceDown);

            foreach (var card in _discardPile)
                card.Update(deltaTime, FacingState.FaceUp);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(_backTex, _drawpileRect, Color.White * 0.1f);
            sb.Draw(_backTex, _discardRect, Color.White * 0.1f);
            
            for (var i = 0; i < _drawPile.Count; i++)
            {
                sb.Draw(_backTex, Pos + new Vector2(i/4, i/2), _drawpileHighlight ? Color.White : Color.LightGray);
            }

            foreach (var card in _discardPile)
                card.Draw(sb, FacingState.FaceUp, _discardHighlight);
        }

        public string DebugInfo()
        {
            return ($"Pos:{Pos}\nDrawpile:{_drawPile.Count}\nDiscards:{_discardPile.Count}\n");
        }

        public void DebugDumpDeck()
        {
            foreach (var card in _drawPile)
            {
                Debug.WriteLine(card.DebugInfo());
            }
        }

        public bool Hover(CardPile cardPile)
        {
            return cardPile == CardPile.Draw ? _drawpileHighlight : _discardHighlight;
        }


        #region Functions that affect the draw pile
        /// <summary>
        /// Add a card to the deck using the deck's back
        /// </summary>
        /// <param name="frontTex">Art for the front of the card</param>
        /// <param name="data">The play data of the card</param>
        public void AddCard(Texture2D frontTex, CardData data, Texture2D particle)
        {
            AddCard(frontTex, _backTex, data, particle);
        }

        /// <summary>
        /// Add a card to the deck using a (potentially) different back
        /// </summary>
        /// <param name="frontTex">Art for the front of the card</param>
        /// <param name="backTex">Art for the back of the card</param>
        /// <param name="data">The play data of the card</param>
        public void AddCard(Texture2D frontTex, Texture2D backTex, CardData data, Texture2D particle)
        {
            _drawPile.Add(new Card(Pos, frontTex, backTex, data, particle));
        }

        /// <summary>
        /// Remove (draw) a card from the deck.
        /// </summary>
        /// <param name="location">Where to draw from, defaults to the top of the deck.</param>
        /// <returns>The removed card</returns>
        public Card PullCard(int location = 0)
        {
            if (IsEmpty)
            {
                Debug.WriteLine("Attempt to draw from empty deck!");
                return null;
            }

            if (_drawPile.Count <= location)
            {
                Debug.WriteLine("Attempt to draw from outside deck bounds!");
                return null;
            }

            var tmp = _drawPile[location];
            _drawPile.RemoveAt(location);
            return tmp;
        }

        /// <summary>
        /// Shuffle the cards currently in the drawPile
        /// </summary>
        public void Shuffle()
        {
            for (var i = _drawPile.Count - 1; i > 0; i--)
            {
                var j = Game1.RNG.Next(i + 1);
                (_drawPile[i], _drawPile[j]) = (_drawPile[j], _drawPile[i]);
            }
        }
        #endregion

        #region Functions that affect the discard pile
        /// <summary>
        /// Add the given card to the discard pile
        /// </summary>
        /// <param name="newDiscard">The card to add to the discard pile</param>
        public void DiscardCard(Card newDiscard)
        {
            if (newDiscard == null)
            {
                Debug.WriteLine("Attempt to add null card to discards!");
                return;
            }

            // Set the new discard to the discard cardPile's location plus a little shoogle to make it look cool
            newDiscard.Pos = _discardRect.Location.ToVector2() + new Vector2(Game1.RNG.Next(-4, 4), Game1.RNG.Next(-4, 4));

            _discardPile.Add(newDiscard);
        }
        #endregion

        /// <summary>
        /// Take the discard pile, add it to the draw pile and reshuffle.
        /// </summary>
        public void MergeAndReshuffle()
        {
            _drawPile.AddRange(_discardPile);
            _discardPile.Clear();
            Shuffle();
        }
    }
}
