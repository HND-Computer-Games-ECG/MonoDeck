using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace MonoDeck
{
    // Add jokers to take top discard       -   done

    // Hand limit based on combined value of cards?

    // A Jump next launch                   - done
    // J Armours in suit - armour protects against lethal damage
    // Q Heal All                           - done
    // K Upgrade                            - done

    // Unplayed cards get worse

    // Spades add to that cloud and launch orbitals     - done
    // Diamonds add to all orbitals         - done
    // Clubs add to all clouds              - done
    // Hearts Heal                          - done

    // Clouds drop one per card play        - done

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static readonly Random RNG = new Random();

        private MouseState ms_curr, ms_old;

        // Variables for data management
        List<Texture2D> _allCardBacks;
        List<Texture2D> _allCardFaces;
        private List<Texture2D> _allMiniCardFaces;
        List<CardData> _allCardData;

        List<Texture2D> _particleCards;
        List<Texture2D> _characterUISprites;

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
                new (0, CardType.Club, CardColour.Black, CardRank.Court, 1),
                new (1, CardType.Club, CardColour.Black, CardRank.Basic, 2),
                new (2, CardType.Club, CardColour.Black, CardRank.Basic, 3),
                new (3, CardType.Club, CardColour.Black, CardRank.Basic, 4),
                new (4, CardType.Club, CardColour.Black, CardRank.Basic, 5),
                new (5, CardType.Club, CardColour.Black, CardRank.Basic, 6),
                new (6, CardType.Club, CardColour.Black, CardRank.Basic, 7),
                new (7, CardType.Club, CardColour.Black, CardRank.Basic, 8),
                new (8, CardType.Club, CardColour.Black, CardRank.Basic, 9),
                new (9, CardType.Club, CardColour.Black, CardRank.Basic, 10),
                new (10, CardType.Club, CardColour.Black, CardRank.Court, 2),
                new (11, CardType.Club, CardColour.Black, CardRank.Court, 3),
                new (12, CardType.Club, CardColour.Black, CardRank.Court, 4),
                new (13, CardType.Diamond, CardColour.Red, CardRank.Court, 1),
                new (14, CardType.Diamond, CardColour.Red, CardRank.Basic, 2),
                new (15, CardType.Diamond, CardColour.Red, CardRank.Basic, 3),
                new (16, CardType.Diamond, CardColour.Red, CardRank.Basic, 4),
                new (17, CardType.Diamond, CardColour.Red, CardRank.Basic, 5),
                new (18, CardType.Diamond, CardColour.Red, CardRank.Basic, 6),
                new (19, CardType.Diamond, CardColour.Red, CardRank.Basic, 7),
                new (20, CardType.Diamond, CardColour.Red, CardRank.Basic, 8),
                new (21, CardType.Diamond, CardColour.Red, CardRank.Basic, 9),
                new (22, CardType.Diamond, CardColour.Red, CardRank.Basic, 10),
                new (23, CardType.Diamond, CardColour.Red, CardRank.Court, 2),
                new (24, CardType.Diamond, CardColour.Red, CardRank.Court, 3),
                new (25, CardType.Diamond, CardColour.Red, CardRank.Court, 4),
                new (26, CardType.Heart, CardColour.Red, CardRank.Court, 1),
                new (27, CardType.Heart, CardColour.Red, CardRank.Basic, 2),
                new (28, CardType.Heart, CardColour.Red, CardRank.Basic, 3),
                new (29, CardType.Heart, CardColour.Red, CardRank.Basic, 4),
                new (30, CardType.Heart, CardColour.Red, CardRank.Basic, 5),
                new (31, CardType.Heart, CardColour.Red, CardRank.Basic, 6),
                new (32, CardType.Heart, CardColour.Red, CardRank.Basic, 7),
                new (33, CardType.Heart, CardColour.Red, CardRank.Basic, 8),
                new (34, CardType.Heart, CardColour.Red, CardRank.Basic, 9),
                new (35, CardType.Heart, CardColour.Red, CardRank.Basic, 10),
                new (36, CardType.Heart, CardColour.Red, CardRank.Court, 2),
                new (37, CardType.Heart, CardColour.Red, CardRank.Court, 3),
                new (38, CardType.Heart, CardColour.Red, CardRank.Court, 4),
                new (39, CardType.Spade, CardColour.Black, CardRank.Court, 1),
                new (40, CardType.Spade, CardColour.Black, CardRank.Basic, 2),
                new (41, CardType.Spade, CardColour.Black, CardRank.Basic, 3),
                new (42, CardType.Spade, CardColour.Black, CardRank.Basic, 4),
                new (43, CardType.Spade, CardColour.Black, CardRank.Basic, 5),
                new (44, CardType.Spade, CardColour.Black, CardRank.Basic, 6),
                new (45, CardType.Spade, CardColour.Black, CardRank.Basic, 7),
                new (46, CardType.Spade, CardColour.Black, CardRank.Basic, 8),
                new (47, CardType.Spade, CardColour.Black, CardRank.Basic, 9),
                new (48, CardType.Spade, CardColour.Black, CardRank.Basic, 10),
                new (49, CardType.Spade, CardColour.Black, CardRank.Court, 2),
                new (50, CardType.Spade, CardColour.Black, CardRank.Court, 3),
                new (51, CardType.Spade, CardColour.Black, CardRank.Court, 4),
                new (52, CardType.None, CardColour.Black, CardRank.Court, 5),
                new (53, CardType.None, CardColour.Red, CardRank.Court, 5),
            };

            // Create a "hand" structure to hold player's cards
            _playerHand = new Hand(new Vector2(_graphics.PreferredBackBufferWidth/2 - 35, _graphics.PreferredBackBufferHeight - 75), 5);

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
                Content.Load<Texture2D>("Cards/Faces/cardClubs2"),
                Content.Load<Texture2D>("Cards/Faces/cardClubs3"),
                Content.Load<Texture2D>("Cards/Faces/cardClubs4"),
                Content.Load<Texture2D>("Cards/Faces/cardClubs5"),
                Content.Load<Texture2D>("Cards/Faces/cardClubs6"),
                Content.Load<Texture2D>("Cards/Faces/cardClubs7"),
                Content.Load<Texture2D>("Cards/Faces/cardClubs8"),
                Content.Load<Texture2D>("Cards/Faces/cardClubs9"),
                Content.Load<Texture2D>("Cards/Faces/cardClubs10"),
                Content.Load<Texture2D>("Cards/Faces/cardClubsJ"),
                Content.Load<Texture2D>("Cards/Faces/cardClubsQ"),
                Content.Load<Texture2D>("Cards/Faces/cardClubsK"),
                Content.Load<Texture2D>("Cards/Faces/cardDiamondsA"),
                Content.Load<Texture2D>("Cards/Faces/cardDiamonds2"),
                Content.Load<Texture2D>("Cards/Faces/cardDiamonds3"),
                Content.Load<Texture2D>("Cards/Faces/cardDiamonds4"),
                Content.Load<Texture2D>("Cards/Faces/cardDiamonds5"),
                Content.Load<Texture2D>("Cards/Faces/cardDiamonds6"),
                Content.Load<Texture2D>("Cards/Faces/cardDiamonds7"),
                Content.Load<Texture2D>("Cards/Faces/cardDiamonds8"),
                Content.Load<Texture2D>("Cards/Faces/cardDiamonds9"),
                Content.Load<Texture2D>("Cards/Faces/cardDiamonds10"),
                Content.Load<Texture2D>("Cards/Faces/cardDiamondsJ"),
                Content.Load<Texture2D>("Cards/Faces/cardDiamondsQ"),
                Content.Load<Texture2D>("Cards/Faces/cardDiamondsK"),
                Content.Load<Texture2D>("Cards/Faces/cardHeartsA"),
                Content.Load<Texture2D>("Cards/Faces/cardHearts2"),
                Content.Load<Texture2D>("Cards/Faces/cardHearts3"),
                Content.Load<Texture2D>("Cards/Faces/cardHearts4"),
                Content.Load<Texture2D>("Cards/Faces/cardHearts5"),
                Content.Load<Texture2D>("Cards/Faces/cardHearts6"),
                Content.Load<Texture2D>("Cards/Faces/cardHearts7"),
                Content.Load<Texture2D>("Cards/Faces/cardHearts8"),
                Content.Load<Texture2D>("Cards/Faces/cardHearts9"),
                Content.Load<Texture2D>("Cards/Faces/cardHearts10"),
                Content.Load<Texture2D>("Cards/Faces/cardHeartsJ"),
                Content.Load<Texture2D>("Cards/Faces/cardHeartsQ"),
                Content.Load<Texture2D>("Cards/Faces/cardHeartsK"),
                Content.Load<Texture2D>("Cards/Faces/cardSpadesA"),
                Content.Load<Texture2D>("Cards/Faces/cardSpades2"),
                Content.Load<Texture2D>("Cards/Faces/cardSpades3"),
                Content.Load<Texture2D>("Cards/Faces/cardSpades4"),
                Content.Load<Texture2D>("Cards/Faces/cardSpades5"),
                Content.Load<Texture2D>("Cards/Faces/cardSpades6"),
                Content.Load<Texture2D>("Cards/Faces/cardSpades7"),
                Content.Load<Texture2D>("Cards/Faces/cardSpades8"),
                Content.Load<Texture2D>("Cards/Faces/cardSpades9"),
                Content.Load<Texture2D>("Cards/Faces/cardSpades10"),
                Content.Load<Texture2D>("Cards/Faces/cardSpadesJ"),
                Content.Load<Texture2D>("Cards/Faces/cardSpadesQ"),
                Content.Load<Texture2D>("Cards/Faces/cardSpadesK"),
                Content.Load<Texture2D>("Cards/Faces/cardBlackJoker"),
                Content.Load<Texture2D>("Cards/Faces/cardRedJoker"),
            };
            _allMiniCardFaces = new List<Texture2D>()
            {
                Content.Load<Texture2D>("Cards/Minis/card_clubs_A"),
                Content.Load<Texture2D>("Cards/Minis/card_clubs_02"),
                Content.Load<Texture2D>("Cards/Minis/card_clubs_03"),
                Content.Load<Texture2D>("Cards/Minis/card_clubs_04"),
                Content.Load<Texture2D>("Cards/Minis/card_clubs_05"),
                Content.Load<Texture2D>("Cards/Minis/card_clubs_06"),
                Content.Load<Texture2D>("Cards/Minis/card_clubs_07"),
                Content.Load<Texture2D>("Cards/Minis/card_clubs_08"),
                Content.Load<Texture2D>("Cards/Minis/card_clubs_09"),
                Content.Load<Texture2D>("Cards/Minis/card_clubs_10"),
                Content.Load<Texture2D>("Cards/Minis/card_clubs_J"),
                Content.Load<Texture2D>("Cards/Minis/card_clubs_Q"),
                Content.Load<Texture2D>("Cards/Minis/card_clubs_K"),
                Content.Load<Texture2D>("Cards/Minis/card_diamonds_A"),
                Content.Load<Texture2D>("Cards/Minis/card_diamonds_02"),
                Content.Load<Texture2D>("Cards/Minis/card_diamonds_03"),
                Content.Load<Texture2D>("Cards/Minis/card_diamonds_04"),
                Content.Load<Texture2D>("Cards/Minis/card_diamonds_05"),
                Content.Load<Texture2D>("Cards/Minis/card_diamonds_06"),
                Content.Load<Texture2D>("Cards/Minis/card_diamonds_07"),
                Content.Load<Texture2D>("Cards/Minis/card_diamonds_08"),
                Content.Load<Texture2D>("Cards/Minis/card_diamonds_09"),
                Content.Load<Texture2D>("Cards/Minis/card_diamonds_10"),
                Content.Load<Texture2D>("Cards/Minis/card_diamonds_J"),
                Content.Load<Texture2D>("Cards/Minis/card_diamonds_Q"),
                Content.Load<Texture2D>("Cards/Minis/card_diamonds_K"),
                Content.Load<Texture2D>("Cards/Minis/card_hearts_A"),
                Content.Load<Texture2D>("Cards/Minis/card_hearts_02"),
                Content.Load<Texture2D>("Cards/Minis/card_hearts_03"),
                Content.Load<Texture2D>("Cards/Minis/card_hearts_04"),
                Content.Load<Texture2D>("Cards/Minis/card_hearts_05"),
                Content.Load<Texture2D>("Cards/Minis/card_hearts_06"),
                Content.Load<Texture2D>("Cards/Minis/card_hearts_07"),
                Content.Load<Texture2D>("Cards/Minis/card_hearts_08"),
                Content.Load<Texture2D>("Cards/Minis/card_hearts_09"),
                Content.Load<Texture2D>("Cards/Minis/card_hearts_10"),
                Content.Load<Texture2D>("Cards/Minis/card_hearts_J"),
                Content.Load<Texture2D>("Cards/Minis/card_hearts_Q"),
                Content.Load<Texture2D>("Cards/Minis/card_hearts_K"),
                Content.Load<Texture2D>("Cards/Minis/card_spades_A"),
                Content.Load<Texture2D>("Cards/Minis/card_spades_02"),
                Content.Load<Texture2D>("Cards/Minis/card_spades_03"),
                Content.Load<Texture2D>("Cards/Minis/card_spades_04"),
                Content.Load<Texture2D>("Cards/Minis/card_spades_05"),
                Content.Load<Texture2D>("Cards/Minis/card_spades_06"),
                Content.Load<Texture2D>("Cards/Minis/card_spades_07"),
                Content.Load<Texture2D>("Cards/Minis/card_spades_08"),
                Content.Load<Texture2D>("Cards/Minis/card_spades_09"),
                Content.Load<Texture2D>("Cards/Minis/card_spades_10"),
                Content.Load<Texture2D>("Cards/Minis/card_spades_J"),
                Content.Load<Texture2D>("Cards/Minis/card_spades_Q"),
                Content.Load<Texture2D>("Cards/Minis/card_spades_K"),
                Content.Load<Texture2D>("Cards/Minis/card_joker_black"),
                Content.Load<Texture2D>("Cards/Minis/card_joker_red"),

            };
            _particleCards = new List<Texture2D>
            {
                Content.Load<Texture2D>("Cards/ParticleCards/Club"),
                Content.Load<Texture2D>("Cards/ParticleCards/Diamond"),
                Content.Load<Texture2D>("Cards/ParticleCards/Heart"),
                Content.Load<Texture2D>("Cards/ParticleCards/Spade")
            };
            _characterUISprites = new List<Texture2D>
            {
                Content.Load<Texture2D>("Cards/ParticleCards/blank"),
                Content.Load<Texture2D>("Cards/ParticleCards/Heart"),
                Content.Load<Texture2D>("shield_chroma"),
                Content.Load<Texture2D>("shield_overlay")
            };

            // The card faces need to match up with the card data set up in Initialise or this won't work - so let's check for that
            Debug.Assert(_allCardFaces.Count == _allCardData.Count, "Card face count does not match card data count");
            Debug.Assert(_allCardFaces.Count == _allMiniCardFaces.Count, "Card face count does not match card data count");

            // Create a deck structure to hold all the cards - pick a random card back for it too.
            _testDeck = new Deck(_allCardBacks[RNG.Next(_allCardBacks.Count)], new Vector2(_graphics.PreferredBackBufferWidth - 250, 25));

            // Add one of each card to the deck
            for (var i = 0; i < _allCardData.Count; i++)
                _testDeck.AddCard(_allCardFaces[i], _allCardData[i]);

            // Draw opening hand
            _playerHand.AddCard(_testDeck.PullCard(1));
            _playerHand.AddCard(_testDeck.PullCard(14));
            _playerHand.AddCard(_testDeck.PullCard(27));
            _playerHand.AddCard(_testDeck.PullCard(40));

            // Shuffle the deck
            _testDeck.Shuffle();

            // Create the peeps and add them to the list for easy management
            _weePeeps.Add(new Character(Content.Load<Texture2D>("charsheet_chroma"),
                Content.Load<Texture2D>("charsheet_overlay_face"),
                Content.Load<Texture2D>("charsheet_overlay_dpad"),
                Content.Load<Texture2D>("charsheet_overlay_buttons_on"),
                Content.Load<Texture2D>("charsheet_overlay_buttons_off"),
                _characterUISprites, 
                new Vector2(50, 160), new Point(3, 2)));
            _weePeeps.Add(new Character(Content.Load<Texture2D>("charsheet_chroma"),
                Content.Load<Texture2D>("charsheet_overlay_face"),
                Content.Load<Texture2D>("charsheet_overlay_dpad"),
                Content.Load<Texture2D>("charsheet_overlay_buttons_on"),
                Content.Load<Texture2D>("charsheet_overlay_buttons_off"),
                _characterUISprites, 
                new Vector2(200, 250), new Point(3, 2)));
            _weePeeps.Add(new Character(Content.Load<Texture2D>("charsheet_chroma"),
                Content.Load<Texture2D>("charsheet_overlay_face"),
                Content.Load<Texture2D>("charsheet_overlay_dpad"),
                Content.Load<Texture2D>("charsheet_overlay_buttons_on"),
                Content.Load<Texture2D>("charsheet_overlay_buttons_off"),
                _characterUISprites, 
                new Vector2(330, 100), new Point(3, 2)));
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
                    // Trying to re-hand a card...
                    if (_playerHand.SelectedCard != -1)
                    {
                        Debug.WriteLine($"Rehanding card: {_cursorCard.DebugInfo()}");
                        _playerHand.AddCard(_cursorCard);
                        _cursorCard = null;
                    }

                    // Trying to discard a card
                    if (_testDeck.Hover(CardPile.Discard))
                    {
                        Debug.WriteLine($"Discarding card: {_cursorCard.DebugInfo()}");
                        _testDeck.DiscardCard(_cursorCard);
                        _cursorCard = null;
                    }

                    for (var i = 0; i < _weePeeps.Count; i++)
                    {
                        // Trying to play a card on a peep
                        if (_weePeeps[i].Hover() && CheckLegalPlay(i))
                        {
                            foreach (var peep in _weePeeps)
                                peep.LaunchCloudSwarm();

                            Debug.WriteLine($"Playing card on peep {i}: {_cursorCard.DebugInfo()}");
                            ProcessCursorCard(i, dT);

                            if (_cursorCard.Data.Type != CardType.None)     // Cardtype.none is a joker, so it's destroyed instead of going into peep hands and discards
                            {
                                var freeCards = _weePeeps[i].GainCard(_cursorCard.Data);
                                for (var j = 0; j < freeCards; j++)
                                {
                                    if (!_playerHand.IsFull && !_testDeck.IsEmpty)
                                        _playerHand.AddCard(_testDeck.PullCard());
                                }
                                _testDeck.DiscardCard(_cursorCard);
                            }
                            _cursorCard = null;
                        }
                    }
                }
                else
                // We're ready to pick up a card
                {
                    // Trying to draw a card, but check if the hand is full or the deck is empty first!
                    if (_testDeck.Hover(CardPile.Draw) && !_playerHand.IsFull && !_testDeck.IsEmpty)
                    {
                        Debug.WriteLine($"Drawing card to hand.");
                        foreach (var peep in _weePeeps)
                            peep.LaunchCloudSwarm();
                        _playerHand.AddCard(_testDeck.PullCard());
                    }

                    // Trying to shuffle the discards back in the deck, but check if there *is* discards to shuffle back in first!
                    if (_testDeck.Hover(CardPile.Discard) && !_testDeck.IsDiscardEmpty)
                        _testDeck.MergeAndReshuffle();

                    // Trying to pick up a card...
                    if (_playerHand.SelectedCard != -1)
                    {
                        Debug.Write($"Taking card {_playerHand.SelectedCard} from hand -> ");
                        _cursorCard = _playerHand.PullCard();
                    }
                }
            }

            // Cycle the discards
            if (_testDeck.Hover(CardPile.Discard))
            {
                if (ms_curr.ScrollWheelValue < ms_old.ScrollWheelValue)
                    _testDeck.SurfaceDiscard();
                else if (ms_curr.ScrollWheelValue > ms_old.ScrollWheelValue)
                    _testDeck.SinkDiscard();
            }
                
            #endregion

            if (_playerHand.IsEmpty && _cursorCard == null)
            {
                for (int i = 0; i < 4; i++)
                    _playerHand.AddCard(_testDeck.PullCard());

                foreach (var peep in _weePeeps)
                {
                    peep.LaunchCloudSwarm();
                    peep.LaunchCloudSwarm();
                }
            }

            // Store what the mouse WAS doing
            ms_old = ms_curr;

            base.Update(gameTime);
        }

        private bool CheckLegalPlay(int activePeep)
        {
            bool legalPlay = true;

            if (_cursorCard.Data.Rank == CardRank.Court)
            {
                if (_cursorCard.Data.Value == (int)CourtCards.Queen && _weePeeps[activePeep].HP >= _weePeeps[activePeep].HPMax)
                {
                    Debug.WriteLine("Cannot play a queen on a peep with full HP.");
                    legalPlay = false;
                }

                if (_cursorCard.Data.Value == (int)CourtCards.King && _weePeeps[activePeep].CardAffinity != CardColour.None)
                {
                    Debug.WriteLine("Cannot play a king on a peep that's already at max upgrade.");
                    legalPlay = false;
                }

                if (_cursorCard.Data.Type == CardType.None && _testDeck.IsDiscardEmpty)
                {
                    Debug.WriteLine("Cannot play a joker on an empty discard pile.");
                    legalPlay = false;
                }
            }

            return legalPlay;
        }

        private void ProcessCursorCard(int activePeep, float deltaTime)
        {
            if (_cursorCard.Data.Rank == CardRank.Court)
            {
                switch (_cursorCard.Data.Value)
                {
                    case (int)CourtCards.Ace:
                        _weePeeps[activePeep].SetJump();
                        break;
                    case (int)CourtCards.Jack:
                        _weePeeps[activePeep].GainArmour();
                        foreach (var peep in _weePeeps)
                        {
                            if (peep.CardAffinity == _cursorCard.Data.Colour)
                                peep.GainArmour();
                        }
                        break;
                    case (int)CourtCards.Queen:
                        if (_weePeeps[activePeep].HP < _weePeeps[activePeep].HPMax)
                        {
                            _weePeeps[activePeep].HP = _weePeeps[activePeep].HPMax;
                            foreach (var peep in _weePeeps)
                                peep.HP = Math.Max(peep.HP, peep.HPMax/2);
                        }
                        break;
                    case (int)CourtCards.King:
                        if (_weePeeps[activePeep].CardAffinity == CardColour.None)
                        {
                            _weePeeps[activePeep].GainLevel(_cursorCard.Data.Colour);
                        }
                        break;
                    case (int)CourtCards.Joker:
                        _playerHand.AddCard(_testDeck.PullDiscard());
                        break;
                }
            }

            int portion;
            switch (_cursorCard.Data.Type)
            {
                case CardType.Club:
                    // Cloud swarm to value with splash
                    portion = _cursorCard.Data.Value / 2;
                    _weePeeps[activePeep].GainCloudSwarm(_particleCards[(int) _cursorCard.Data.Type], portion);
                    for (var i = 0; i < _cursorCard.Data.Value - portion; i++)
                        _weePeeps[(activePeep + i) % _weePeeps.Count].GainCloudSwarm(_particleCards[(int) _cursorCard.Data.Type]);
                    break;
                case CardType.Diamond:
                    // Orbital swarm to value with splash
                    portion = _cursorCard.Data.Value / 2;
                    _weePeeps[activePeep].GainOrbitalSwarm(_particleCards[(int) _cursorCard.Data.Type], portion);
                    for (var i = 0; i < _cursorCard.Data.Value - portion; i++)
                        _weePeeps[(activePeep + i) % _weePeeps.Count].GainOrbitalSwarm(_particleCards[(int) _cursorCard.Data.Type]);
                    break;
                case CardType.Heart:
                    // Heal to value
                    _weePeeps[activePeep].GainHealth(_cursorCard.Data.Value);
                    foreach (var peep in _weePeeps)
                    {
                        if (peep.CardAffinity == _cursorCard.Data.Colour)
                            peep.GainHealth(_cursorCard.Data.Value);
                    }
                    break;
                case CardType.Spade:
                        // Cloud swarm the spades to value
                        _weePeeps[activePeep].GainCloudSwarm(_particleCards[(int) _cursorCard.Data.Type], _cursorCard.Data.Value);
                        foreach (var peep in _weePeeps)
                            peep.LaunchOrbitalSwarm();
                    break;
                case CardType.None:
                    Debug.WriteLine($"Got a {(CourtCards) _cursorCard.Data.Value}");
                    break;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGreen);

            _spriteBatch.Begin();

            _testDeck.Draw(_spriteBatch);

            foreach (var peep in _weePeeps)
                peep.Draw(_spriteBatch, _allMiniCardFaces);

            _playerHand.Draw(_spriteBatch);

            _cursorCard?.DrawMini(_spriteBatch, ms_curr.Position, FacingState.FaceUp, true);

#if DEBUG
            //_spriteBatch.DrawString(_debugFont, _cursorCard == null ? "_cursorCard is null" : _cursorCard.DebugInfo(), ms_curr.Position.ToVector2() + Vector2.One, Color.Black);
            //_spriteBatch.DrawString(_debugFont, _cursorCard == null ? "_cursorCard is null" : _cursorCard.DebugInfo(), ms_curr.Position.ToVector2(), Color.White);

            //_spriteBatch.DrawString(_debugFont, _playerHand.DebugInfo(), Vector2.One, Color.Black);
            //_spriteBatch.DrawString(_debugFont, _playerHand.DebugInfo(), Vector2.Zero, Color.White);

            //_spriteBatch.DrawString(_debugFont, _testDeck.DebugInfo(), _testDeck.Pos + Vector2.One, Color.Black);
            //_spriteBatch.DrawString(_debugFont, _testDeck.DebugInfo(), _testDeck.Pos, Color.White);

            //foreach (var peep in _weePeeps)
            //{
            //    _spriteBatch.DrawString(_debugFont, peep.DebugInfo(), peep.Pos + Vector2.One, Color.Black);
            //    _spriteBatch.DrawString(_debugFont, peep.DebugInfo(), peep.Pos, Color.White);
            //}
#endif

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
