using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;

namespace MonoDeck
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static readonly Random RNG = new Random();

        private MouseState ms_curr, ms_old;

        List<Texture2D> _allCardBacks;
        List<Texture2D> _allCardFaces;
        List<CardData> _allCardData;

        private Deck _testDeck;
        private List<Character> _weePeeps;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _allCardData = new List<CardData>
            {
                new (CardType.Club, CardColour.Black, CardRank.Royal, 1),
                new (CardType.Club, CardColour.Black, CardRank.Basic, 10),
                new (CardType.Club, CardColour.Black, CardRank.Royal, 2),
                new (CardType.Club, CardColour.Black, CardRank.Royal, 3),
                new (CardType.Club, CardColour.Black, CardRank.Royal, 4),
                new (CardType.Diamond, CardColour.Red, CardRank.Royal, 1),
                new (CardType.Diamond, CardColour.Red, CardRank.Basic, 10),
                new (CardType.Diamond, CardColour.Red, CardRank.Royal, 2),
                new (CardType.Diamond, CardColour.Red, CardRank.Royal, 3),
                new (CardType.Diamond, CardColour.Red, CardRank.Royal, 4),
                new (CardType.Heart, CardColour.Red, CardRank.Royal, 1),
                new (CardType.Heart, CardColour.Red, CardRank.Basic, 10),
                new (CardType.Heart, CardColour.Red, CardRank.Royal, 2),
                new (CardType.Heart, CardColour.Red, CardRank.Royal, 3),
                new (CardType.Heart, CardColour.Red, CardRank.Royal, 4),
                new (CardType.Spade, CardColour.Black, CardRank.Royal, 1),
                new (CardType.Spade, CardColour.Black, CardRank.Basic, 10),
                new (CardType.Spade, CardColour.Black, CardRank.Royal, 2),
                new (CardType.Spade, CardColour.Black, CardRank.Royal, 3),
                new (CardType.Spade, CardColour.Black, CardRank.Royal, 4),
            };
            
            _weePeeps = new List<Character>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _allCardBacks = new List<Texture2D>
            {
                Content.Load<Texture2D>("Cards/Backs/cardBack_blue1"),
                Content.Load<Texture2D>("Cards/Backs/cardBack_blue2"),
                Content.Load<Texture2D>("Cards/Backs/cardBack_blue3"),
                Content.Load<Texture2D>("Cards/Backs/cardBack_blue4"),
                Content.Load<Texture2D>("Cards/Backs/cardBack_blue5"),
                Content.Load<Texture2D>("Cards/Backs/cardBack_green1"),
                Content.Load<Texture2D>("Cards/Backs/cardBack_green2"),
                Content.Load<Texture2D>("Cards/Backs/cardBack_green3"),
                Content.Load<Texture2D>("Cards/Backs/cardBack_green4"),
                Content.Load<Texture2D>("Cards/Backs/cardBack_green5"),
                Content.Load<Texture2D>("Cards/Backs/cardBack_red1"),
                Content.Load<Texture2D>("Cards/Backs/cardBack_red2"),
                Content.Load<Texture2D>("Cards/Backs/cardBack_red3"),
                Content.Load<Texture2D>("Cards/Backs/cardBack_red4"),
                Content.Load<Texture2D>("Cards/Backs/cardBack_red5")
            };
            _allCardFaces = new List<Texture2D>
            {
                Content.Load<Texture2D>("Cards/Faces/cardClubsA"),
                Content.Load<Texture2D>("Cards/Faces/cardClubs10"),
                Content.Load<Texture2D>("Cards/Faces/cardClubsJ"),
                Content.Load<Texture2D>("Cards/Faces/cardClubsQ"),
                Content.Load<Texture2D>("Cards/Faces/cardClubsK"),
                Content.Load<Texture2D>("Cards/Faces/cardDiamondsA"),
                Content.Load<Texture2D>("Cards/Faces/cardDiamonds10"),
                Content.Load<Texture2D>("Cards/Faces/cardDiamondsJ"),
                Content.Load<Texture2D>("Cards/Faces/cardDiamondsQ"),
                Content.Load<Texture2D>("Cards/Faces/cardDiamondsK"),
                Content.Load<Texture2D>("Cards/Faces/cardHeartsA"),
                Content.Load<Texture2D>("Cards/Faces/cardHearts10"),
                Content.Load<Texture2D>("Cards/Faces/cardHeartsJ"),
                Content.Load<Texture2D>("Cards/Faces/cardHeartsQ"),
                Content.Load<Texture2D>("Cards/Faces/cardHeartsK"),
                Content.Load<Texture2D>("Cards/Faces/cardSpadesA"),
                Content.Load<Texture2D>("Cards/Faces/cardSpades10"),
                Content.Load<Texture2D>("Cards/Faces/cardSpadesJ"),
                Content.Load<Texture2D>("Cards/Faces/cardSpadesQ"),
                Content.Load<Texture2D>("Cards/Faces/cardSpadesK"),
            };

            Debug.Assert(_allCardFaces.Count == _allCardData.Count, "Card face count does not match card data count");

            _testDeck = new Deck(_allCardBacks[RNG.Next(_allCardBacks.Count)], new Vector2(_graphics.PreferredBackBufferWidth - 250, 25));
            for (var i = 0; i < _allCardData.Count; i++)
                _testDeck.AddCard(_allCardFaces[i], _allCardData[i]);
            _testDeck.Shuffle();

            _weePeeps.Add(new Character(Content.Load<Texture2D>("charsheet_chroma"), Content.Load<Texture2D>("charsheet_overlay"), new Vector2(20, 70), new Point(3, 2), Color.Red));
            _weePeeps.Add(new Character(Content.Load<Texture2D>("charsheet_chroma"), Content.Load<Texture2D>("charsheet_overlay"), new Vector2(300, 50), new Point(3, 2), Color.DimGray));
            _weePeeps.Add(new Character(Content.Load<Texture2D>("charsheet_chroma"), Content.Load<Texture2D>("charsheet_overlay"), new Vector2(170, 170), new Point(3, 2), Color.DodgerBlue));
        }

        protected override void Update(GameTime gameTime)
        {
            var dT = (float)gameTime.ElapsedGameTime.TotalSeconds;
            ms_curr = Mouse.GetState();
            
            _testDeck.Update(dT, ms_curr.Position);

            foreach (var peep in _weePeeps)
                peep.Update(dT, ms_curr.Position);

            if (_testDeck.Hover() && ms_curr.LeftButton == ButtonState.Pressed && ms_old.LeftButton == ButtonState.Released)
                _testDeck.DiscardCard(_testDeck.PullCard());

            ms_old = ms_curr;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGreen);

            _spriteBatch.Begin();
            _testDeck.Draw(_spriteBatch);

            foreach (var peep in _weePeeps)
                peep.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
