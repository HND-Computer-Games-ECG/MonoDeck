﻿using System;
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

        // Variables for data management
        List<Texture2D> _allCardBacks;
        List<Texture2D> _allCardFaces;
        List<CardData> _allCardData;

        // Actual game world stuff
        private Deck _testDeck;
        private Hand _playerHand;
        private Card _cursorCard;

        private List<Character> _weePeeps;

#if DEBUG
        private SpriteFont _debugFont;
#endif

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // Set up the card info
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

            // Create a "hand" structure to hold player's cards
            _playerHand = new Hand(new Vector2(_graphics.PreferredBackBufferWidth/2 - 70, _graphics.PreferredBackBufferHeight - 75), 7);

            // Cursor starts not "holding" a card
            _cursorCard = null;

            // List to hold game characters
            _weePeeps = new List<Character>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

#if DEBUG
            _debugFont = Content.Load<SpriteFont>("Arial08");
#endif

            // Preload all the art into lists (this would be better in a spritesheet, but this is easier...)
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

            // The card faces need to match up with the card data set up in Initialise or this won't work - so let's check for that
            Debug.Assert(_allCardFaces.Count == _allCardData.Count, "Card face count does not match card data count");

            // Create a deck structure to hold all the cards - pick a random card back for it too.
            _testDeck = new Deck(_allCardBacks[RNG.Next(_allCardBacks.Count)], new Vector2(_graphics.PreferredBackBufferWidth - 250, 25));

            // Add one of each card to the deck
            for (var i = 0; i < _allCardData.Count; i++)
                _testDeck.AddCard(_allCardFaces[i], _allCardData[i]);

            // Shuffle the deck
            _testDeck.Shuffle();

            // Create the peeps and add them to the list for easy management
            _weePeeps.Add(new Character(Content.Load<Texture2D>("charsheet_chroma"), Content.Load<Texture2D>("charsheet_overlay"), new Vector2(50, 160), new Point(3, 2), CardColour.Red));
            _weePeeps.Add(new Character(Content.Load<Texture2D>("charsheet_chroma"), Content.Load<Texture2D>("charsheet_overlay"), new Vector2(330, 100), new Point(3, 2), CardColour.Black));
            _weePeeps.Add(new Character(Content.Load<Texture2D>("charsheet_chroma"), Content.Load<Texture2D>("charsheet_overlay"), new Vector2(200, 250), new Point(3, 2), CardColour.None));
        }

        protected override void Update(GameTime gameTime)
        {
            // Make a local variable so we don't have to type gametime.Elapsed...blahblahblah each time
            var dT = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Get what the mouse is doing
            ms_curr = Mouse.GetState();
            
            // Update the deck and hand
            _testDeck.Update(dT, ms_curr.Position);
            _playerHand.Update(dT, ms_curr.Position);

            // Update the peeps
            foreach (var peep in _weePeeps)
                peep.Update(dT, ms_curr.Position);

            #region Process player clicks
            if (ms_curr.LeftButton == ButtonState.Pressed && ms_old.LeftButton == ButtonState.Released)
            {
                // if we're already holding a card
                if (_cursorCard != null)
                {
                    // Trying to discard a card
                    if (_testDeck.Hover(CardPile.Discard))
                    {
                        _testDeck.DiscardCard(_cursorCard);
                        _cursorCard = null;
                    }

                    foreach (var peep in _weePeeps)
                    {
                        // if player has clicked a card on a peep
                        if (peep.Hover())
                        {
                            // if the colour of the card is the peep's favourite
                            if (peep.CardAffinity == _cursorCard.Data.Colour)
                            {
                                if (_cursorCard.Data.Rank == CardRank.Royal)
                                    peep.Jump(dT);
                                else
                                    peep.SetMood(CharState.Happy, 2);
                            }
                            else
                            {
                                if (_cursorCard.Data.Rank == CardRank.Royal)
                                    peep.SetMood(CharState.Happy, 2);
                                else
                                    peep.SetMood(CharState.Sad, 2);
                            }

                            // Regardless of reaction, discard the card
                            _testDeck.DiscardCard(_cursorCard);
                            _cursorCard = null;
                        }
                    }

                }
                else
                // We're ready to pick up a card
                {
                    // Trying to draw a card, but check if the hand is full or the deck is empty first!
                    if (_testDeck.Hover(CardPile.Draw) && !_playerHand.IsFull && !_testDeck.IsEmpty)
                        _playerHand.AddCard(_testDeck.PullCard());

                    // Trying to shuffle the discards back in the deck, but check if there *is* discards to shuffle back in first!
                    if (_testDeck.Hover(CardPile.Discard) && !_testDeck.IsDiscardEmpty)
                        _testDeck.MergeAndReshuffle();

                    // Trying to pick up a card...
                    if (_playerHand.SelectedCard != -1)
                        _cursorCard = _playerHand.PullCard();
                }
            }
            #endregion

            // Store what the mouse WAS doing
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

            _playerHand.Draw(_spriteBatch);

            if (_cursorCard != null)
                _cursorCard.DrawMini(_spriteBatch, ms_curr.Position, FacingState.FaceUp, true);

#if DEBUG
            _spriteBatch.DrawString(_debugFont, _cursorCard == null ? "_cursorCard is null" : _cursorCard.DebugInfo(), ms_curr.Position.ToVector2() + Vector2.One, Color.Black);
            _spriteBatch.DrawString(_debugFont, _cursorCard == null ? "_cursorCard is null" : _cursorCard.DebugInfo(), ms_curr.Position.ToVector2(), Color.White);

            _spriteBatch.DrawString(_debugFont, _playerHand.DebugInfo(), Vector2.One, Color.Black);
            _spriteBatch.DrawString(_debugFont, _playerHand.DebugInfo(), Vector2.Zero, Color.White);

            _spriteBatch.DrawString(_debugFont, _testDeck.DebugInfo(), _testDeck.Pos + Vector2.One, Color.Black);
            _spriteBatch.DrawString(_debugFont, _testDeck.DebugInfo(), _testDeck.Pos, Color.White);

            foreach (var peep in _weePeeps)
            {
                _spriteBatch.DrawString(_debugFont, peep.DebugInfo(), peep.Pos + Vector2.One, Color.Black);
                _spriteBatch.DrawString(_debugFont, peep.DebugInfo(), peep.Pos, Color.White);
            }
#endif

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
