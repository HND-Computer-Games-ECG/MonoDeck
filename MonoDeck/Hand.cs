using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoDeck
{
    class Hand
    {
        private List<Card> _cards;
        public Vector2 Pos { get; set; }

        private int _maxSize;

        private int _stepX;
        private int _startX;

        public int SelectedCard { get; set; }

        public bool IsFull => _cards.Count == _maxSize;
        public bool IsEmpty => _cards.Count == 0;

        public Hand(Vector2 pos, int maxSize)
        {
            _cards = new List<Card>();
            Pos = pos;

            _maxSize = maxSize;
            SelectedCard = -1;

            _stepX = 50;
        }

        public void Update(float deltaTime, Point mousePos)
        {
            foreach (var card in _cards)
                card.Update(deltaTime, FacingState.FaceUp);

            if (_cards.Count > 0)
            {
                // The last card in the hand is an annoying edge case, so let's deal with it first...
                if (_cards[^1].Hover(mousePos))
                {
                    SelectedCard = _cards.Count - 1;
                }
                else
                {
                    // Every other card is just maths to find it's section...
                    SelectedCard = (mousePos.X - _startX) / _stepX;

                    if (mousePos.Y < Pos.Y || SelectedCard >= _cards.Count)
                        SelectedCard = -1;
                }
            }
            else
                SelectedCard = -1;

            RefreshPositions();
        }

        public void Draw(SpriteBatch sb)
        {
            // Go through and draw all the cards
            for (var i = 0; i < _cards.Count; i++)
            {
                // if the current card is the selected one, highlight it and move it up when it's drawn
                if (SelectedCard == i)
                    _cards[i].Draw(sb, _cards[i].Pos + new Vector2(0, -25), FacingState.FaceUp, true);
                else
                    _cards[i].Draw(sb, FacingState.FaceUp, false);
            }
        }

        public string DebugInfo()
        {
            var tmp = $"**Hand Info**:\nSelected: {SelectedCard}\n";
            foreach (var card in _cards)
                tmp += card.DebugInfo();

            return (tmp);
        }

        /// <summary>
        /// When a card is added or removed from the hand, call this function to tidy up the positions
        /// </summary>
        private void RefreshPositions()
        {
            _startX = (int) Pos.X - (_cards.Count / 2) * _stepX;

            for (var i = 0; i < _cards.Count; i++)
            {
                _cards[i].Pos = new Vector2(_startX + (_stepX * i), Pos.Y);
            }
        }

        /// <summary>
        /// Add the given card to the end of the hand and reposition
        /// </summary>
        /// <param name="newHandCard">New card to add</param>
        public void AddCard(Card newHandCard)
        {
            if (newHandCard == null)
            {
                Debug.WriteLine("Attempt to add null card to hand!");
                return;
            }

            if (_cards.Count >= _maxSize)
            {
                Debug.WriteLine("Attempt to add card to full hand! (Card ignored!)");
                return;
            }

            _cards.Add(newHandCard);
            RefreshPositions();
        }

        /// <summary>
        /// Remove the currently selected card from the hand
        /// </summary>
        /// <returns>The card that has been removed.</returns>
        public Card PullCard()
        {
            if (IsEmpty)
            {
                Debug.WriteLine("Attempt to draw from empty deck!");
                return null;
            }

            if (SelectedCard < 0)
            {
                Debug.WriteLine("Attempt to draw from outside deck bounds!");
                return null;
            }

            var tmp = _cards[SelectedCard];
            _cards.RemoveAt(SelectedCard);
            RefreshPositions();
            return tmp;
        }



    }
}
