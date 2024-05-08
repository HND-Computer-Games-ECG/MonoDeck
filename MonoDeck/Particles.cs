using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoDeck
{
    class Particle
    {
        private Texture2D _txr;
        private Vector2 _pos;
        private Vector2 _velocity;
        private float _wibbliness;
        private Color _tint;
        private float _rotation;
        private float _rotationSpeed;
        private Vector2 _pivot;
        private Vector2 _scale;

        public Particle(Texture2D txr, Vector2 Pos, Vector2 Velocity, float Wibbliness, Color Tint, float RotationSpeed, Vector2 scale)
        {
            _txr = txr;
            _pos = Pos;
            _velocity = Velocity;
            _wibbliness = Wibbliness;
            _tint = Tint;
            _rotation = Game1.RNG.NextSingle() * MathHelper.TwoPi;
            _rotationSpeed = RotationSpeed;
            _pivot = _txr.Bounds.Size.ToVector2()/2;
            _scale = scale;
        }

        public void Update(float dt)
        {
            _pos += _velocity * dt + new Vector2(MathF.Sin(_rotation) * _wibbliness, 0);
            _rotation += _rotationSpeed * dt;

            _scale *= 1.002f;
            _tint *= 0.995f;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(_txr, _pos, null, _tint, _rotation, _pivot * _scale, _scale, SpriteEffects.None, 0);
        }
    }

    class ParticleEmitter
    {

    }
}
