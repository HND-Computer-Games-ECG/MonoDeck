using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace MonoDeck
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        List<Texture2D> _allCardBacks;
        List<Texture2D> _allCardFaces;
        List<CardData> _allCardData;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
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
            _allCardData = new List<CardData>()
            {
                new CardData(CardType.Club, CardColour.Black, CardRank.Royal, 1),
                new CardData(CardType.Club, CardColour.Black, CardRank.Basic, 10),
                new CardData(CardType.Club, CardColour.Black, CardRank.Royal, 2),
                new CardData(CardType.Club, CardColour.Black, CardRank.Royal, 3),
                new CardData(CardType.Club, CardColour.Black, CardRank.Royal, 4),
                new CardData(CardType.Diamond, CardColour.Red, CardRank.Royal, 1),
                new CardData(CardType.Diamond, CardColour.Red, CardRank.Basic, 10),
                new CardData(CardType.Diamond, CardColour.Red, CardRank.Royal, 2),
                new CardData(CardType.Diamond, CardColour.Red, CardRank.Royal, 3),
                new CardData(CardType.Diamond, CardColour.Red, CardRank.Royal, 4),
                new CardData(CardType.Heart, CardColour.Red, CardRank.Royal, 1),
                new CardData(CardType.Heart, CardColour.Red, CardRank.Basic, 10),
                new CardData(CardType.Heart, CardColour.Red, CardRank.Royal, 2),
                new CardData(CardType.Heart, CardColour.Red, CardRank.Royal, 3),
                new CardData(CardType.Heart, CardColour.Red, CardRank.Royal, 4),
                new CardData(CardType.Spade, CardColour.Black, CardRank.Royal, 1),
                new CardData(CardType.Spade, CardColour.Black, CardRank.Basic, 10),
                new CardData(CardType.Spade, CardColour.Black, CardRank.Royal, 2),
                new CardData(CardType.Spade, CardColour.Black, CardRank.Royal, 3),
                new CardData(CardType.Spade, CardColour.Black, CardRank.Royal, 4),
            };

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.ForestGreen);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
