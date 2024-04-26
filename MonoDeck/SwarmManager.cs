using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

namespace MonoDeck
{
    enum SwarmType
    {
        Cloud,
        Orbital
    }

    class SwarmManager
    {
        private List<SwarmParticle> _swarm;

        public Vector2 Pos { get; set; }
        public Vector2 Range { get; set; }

        private SwarmType _type;

        public SwarmManager(Vector2 pos, Vector2 range, SwarmType type)
        {
            _swarm = new List<SwarmParticle>();
            Pos = pos;
            Range = range;

            _type = type;
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

        public void AddToSwarm(Texture2D tex, int amount)
        {
            switch (_type)
            {
                case SwarmType.Cloud:
                    for (var i = 0; i < amount; i++)
                        _swarm.Add(new CloudSwarmParticle(tex, Pos, Range.Length()/2));
                    break;
                case SwarmType.Orbital:
                    for (var i = 0; i < amount; i++)
                        _swarm.Add(new OrbitalSwarmParticle(tex, Pos, Range.Length()/2));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
