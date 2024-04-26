using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MonoDeck
{
    class SwarmManager
    {
        private List<SwarmParticle> _swarm;
        private Vector2 _pos;
        private Vector2 _range;

        public SwarmManager(Vector2 pos, Vector2 range)
        {
            _swarm = new List<SwarmParticle>();
            _pos = pos;
            _range = range;
        }

        public void Update(float deltaTime)
        {
            foreach (SwarmParticle particle in _swarm)
            {
                particle.Update(deltaTime);
            }
        }

        public void Draw(SpriteBatch sB)
        {
            foreach (SwarmParticle particle in _swarm)
            {
                particle.Draw(sB);
            }
        }

        public void AddToSwarm(SwarmParticle newPart)
        {
            newPart.AnchorPos = _pos;
            newPart.Pos = newPart.AnchorPos + new Vector2(Game1.RNG.Next((int) _range.X) - _range.X/2, Game1.RNG.Next((int) _range.Y) - _range.Y / 2);

            newPart.Rotation = Game1.RNG.NextSingle() * MathHelper.TwoPi;
            _swarm.Add(newPart);
        }
    }
}
