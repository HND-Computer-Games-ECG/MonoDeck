using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoDeck
{
    class SwarmParticle
    {
        public Vector2 AnchorPos { get; set; }
        public Vector2 Pos { get; set; }

        protected Texture2D _tex;
        
        public float Rotation { get; set; }
        protected Vector2 _pivot;
        private float _rotSpeed;

        public SwarmParticle(Texture2D tex)
        {
            AnchorPos = Vector2.Zero;
            Pos = Vector2.Zero;
            _tex = tex;

            Rotation = Game1.RNG.NextSingle() * MathHelper.TwoPi;
            _pivot = tex.Bounds.Center.ToVector2();
            _rotSpeed = (Game1.RNG.NextSingle() < 0.5f ? -1 : 1) * ((Game1.RNG.NextSingle() * 6) + 2);
        }

        public virtual void Update(float deltaTime)
        {
            Rotation = (Rotation + _rotSpeed * deltaTime) % MathHelper.TwoPi;
        }

        public virtual void Draw(SpriteBatch sB)
        {
            sB.Draw(_tex, Pos, null, Color.White * 0.8f, Rotation, _pivot, 1, SpriteEffects.None, 0);
        }
    }

    /*class OrbitalSwarmParticle : SwarmParticle
    {
        private float _orbitalRotation;
        private float _orbitalSpeed;
        private Vector2 _orbitalPos;
        private float _range;

        public OrbitalSwarmParticle(Texture2D tex, Vector2 pos, float rotSpeed, float orbitalSpeed, float range) : base(tex, pos, rotSpeed)
        {
            _orbitalRotation = Game1.RNG.NextSingle() * MathHelper.TwoPi;
            _orbitalSpeed = orbitalSpeed;
            _range = range;
        }

        public override void Update(float deltaTime)
        {
            _orbitalRotation += _orbitalSpeed * deltaTime;

            _orbitalPos = new Vector2(MathF.Cos(_orbitalRotation) * _range, MathF.Sin(_orbitalRotation));

            base.Update(deltaTime);
        }

        public override void Draw(SpriteBatch sB)
        {
            sB.Draw(_tex, _orbitalPos, null, Color.White, Rotation, _pivot, 1, SpriteEffects.None, 0);
        }
    }*/
}
