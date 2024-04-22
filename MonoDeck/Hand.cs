using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoDeck
{
    class Hand
    {
        private List<Card> _cards;
        private Vector2 _pos;

        private int _maxSize;

        private int _stepX;
        private int _startX;

        public int SelectedCard { get; set; }

        public bool IsFull => _cards.Count == _maxSize;
        public bool IsEmpty => _cards.Count == 0;

        public Hand(Vector2 pos, int maxSize)
        {
            _cards = new List<Card>();
            _pos = pos;

            _maxSize = maxSize;
            SelectedCard = -1;

            _stepX = 50;
        }

        public void Update(float deltaTime, Point mousePos)
        {
            SelectedCard = (mousePos.X - _startX) / _stepX;

            if (mousePos.Y < _pos.Y || SelectedCard >= _cards.Count)
                SelectedCard = -1;
        }

        public void Draw(SpriteBatch sb)
        {
            for (var i = 0; i < _cards.Count; i++)
            {
                if (SelectedCard == i)
                    _cards[i].Draw(sb, _cards[i].Pos + new Vector2(0, -25), FacingState.FaceUp, true);
                else
                    _cards[i].Draw(sb, FacingState.FaceUp, false);
            }
        }

        public string DebugInfo()
        {
            var tmp = "";
            foreach (var card in _cards)
                tmp += card.DebugInfo();

            return (tmp);
        }

        private void RefreshPositions()
        {
            _startX = (int) _pos.X - (_cards.Count / 2) * _stepX;

            for (var i = 0; i < _cards.Count; i++)
            {
                _cards[i].Pos = new Vector2(_startX + (_stepX * i), _pos.Y);
            }
        }

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

        public Card PullCard()
        {
            if (IsEmpty)
            {
                Debug.WriteLine("Attempt to draw from empty deck!");
                return null;
            }

            if (SelectedCard == -1)
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
