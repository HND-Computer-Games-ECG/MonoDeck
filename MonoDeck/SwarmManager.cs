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

        public int Update(float deltaTime)
        {
            var killList = 0;
            for (var i = _swarm.Count - 1; i >= 0; i--)
            {
                if (_swarm[i].State == SwarmPartState.Dead)
                {
                    _swarm.RemoveAt(i);
                    killList++;
                }
                else
                    _swarm[i].Update(deltaTime);
            }

            return killList;
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

        public void Launch()
        {
            if (_swarm.Count == 0)
                return;

            switch (_type)
            {
                case SwarmType.Cloud:
                    // trigger one to fall.
                    _swarm[Game1.RNG.Next(_swarm.Count)].State = SwarmPartState.Attacking;
                    break;
                case SwarmType.Orbital:
                    foreach (var particle in _swarm)
                        particle.State = SwarmPartState.Attacking;
                    break;
            }
        }
    }
}
