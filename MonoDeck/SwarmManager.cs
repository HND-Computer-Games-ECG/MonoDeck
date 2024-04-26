using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

namespace MonoDeck
{
    class SwarmManager
    {
        private List<SwarmParticle> _swarm;
        public Vector2 Pos { get; set; }
        public Vector2 Range { get; set; }

        public SwarmManager(Vector2 pos, Vector2 range)
        {
            _swarm = new List<SwarmParticle>();
            Pos = pos;
            Range = range;
        }

        public void Update(float deltaTime)
        {
            foreach (var particle in _swarm)
            {
                particle.Update(deltaTime);
            }
        }

        public void Draw(SpriteBatch sB)
        {
            foreach (var particle in _swarm)
            {
                particle.Draw(sB);
            }
        }

        public void AddToSwarm(Texture2D tex)
        {
            //_swarm.Add(new ScatterSwarmParticle(tex, Pos, Range));
            _swarm.Add(new OrbitalSwarmSwarmParticle(tex, Pos, Range.Length()/2));
        }
    }
}
