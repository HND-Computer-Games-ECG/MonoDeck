using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MonoDeck
{
    class Particle
    {
        private Texture2D _txr;
        private Vector2 _pos;

        private float _ttl;
        public bool IsDead => (_ttl < 0);

        private Vector2 _velocity;
        private float _wibbliness;

        private Color _tint;

        private float _rotation;
        private float _rotationSpeed;
        private Vector2 _pivot;

        private Vector2 _scale;

        public Particle(Texture2D txr, Vector2 pos, float ttl, Vector2 velocity, float wibbliness, Color tint, float rotationSpeed, Vector2 scale)
        {
            _txr = txr;
            _pos = pos;
            _ttl = ttl;

            _velocity = velocity;
            _wibbliness = wibbliness;

            _tint = tint;
            
            _rotation = Game1.RNG.NextSingle() * MathHelper.TwoPi;
            _rotationSpeed = rotationSpeed;
            _pivot = _txr.Bounds.Size.ToVector2()/2;
            
            _scale = scale;
        }

        public void Update(float dt)
        {
            _pos += (_velocity * dt) + new Vector2(MathF.Sin(_rotation) * _wibbliness, 0);
            _rotation += _rotationSpeed * dt;

            _scale *= 1.002f;
            _tint *= 0.995f;

            if (_ttl > 0)
                _ttl -= dt;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(_txr, _pos, null, _tint, _rotation, _pivot * _scale, _scale, SpriteEffects.None, 0);
        }
    }

    class ParticleEmitter
    {
        protected Texture2D _txr;
        public Vector2 Pos { get; set; }

        protected List<Particle> _particles;

        public ParticleEmitter(Texture2D txr, Vector2 pos)
        {
            _txr = txr;
            Pos = pos;
            _particles = new List<Particle>();
        }

        public virtual void Update(float dt, Vector2 pos)
        {
            Pos = pos;
            for (var i = _particles.Count - 1; i >= 0; i--)
            {
                if (_particles[i].IsDead)
                    _particles.RemoveAt(i);
                else
                    _particles[i].Update(dt);
            }
        }

        public void Draw(SpriteBatch sb)
        {
            Draw(sb, Pos);
        }

        public virtual void Draw(SpriteBatch sb, Vector2 pos)
        {
            Pos = pos;
            foreach (var particle in _particles)
                particle.Draw(sb);
#if DEBUG
            sb.Draw(Game1._debugPixel, new Rectangle(Pos.ToPoint(), new Point(2)), Color.Blue);
#endif

        }

        public void Kill()
        {
            _particles.Clear();
        }
    }

    class SteamerEmitter : ParticleEmitter
    {
        private int _particleCapacity;
        private Vector2 _spawnZoneSize;
        private bool _stopped;

        public SteamerEmitter(Texture2D txr, Vector2 pos, int particleCapacity, Vector2 spawnZoneSize) : base(txr, pos)
        {
            _particleCapacity = particleCapacity;
            _spawnZoneSize = spawnZoneSize;
            _stopped = false;
        }

        public override void Update(float dt, Vector2 pos)
        {
            base.Update(dt, pos);

            if (_particles.Count >= _particleCapacity || _stopped) 
                return;

            _particles.Add(new Particle(
                _txr, 
                Pos + (new Vector2(Game1.RNG.NextSingle() - 0.5f, Game1.RNG.NextSingle() - 0.5f) * _spawnZoneSize), 
                3 + Game1.RNG.NextSingle(),
                new Vector2(0, -30-Game1.RNG.Next(20)), 
                0.5f, 
                Color.White, 
                3, 
                Vector2.One
            ));
        }
        public override void Draw(SpriteBatch sb, Vector2 pos)
        {
            base.Draw(sb, pos);

#if DEBUG
            var tmp = new Rectangle((Pos - _spawnZoneSize/2).ToPoint(), _spawnZoneSize.ToPoint());
            sb.Draw(Game1._debugPixel, tmp, Color.Blue * 0.25f);
#endif
        }

        public void Stop()
        {
            _stopped = true;
        }

        public void Start()
        {
            _stopped = false;
        }
    }
}
