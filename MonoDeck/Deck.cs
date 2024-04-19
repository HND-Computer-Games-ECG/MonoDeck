using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoDeck
{
    class Deck
    {
        private Texture2D _backTex;
        private List<Card> _drawPile;
        private int _top;

        public Deck(Texture2D backTex)
        {
            _backTex = backTex;
            _drawPile = new List<Card>();
            _top = 0;
        }
    }
}
