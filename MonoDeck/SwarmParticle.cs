﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoDeck
{
    class SwarmParticle
    {
        public Vector2 Pos { get; set; }
        protected Vector2 _anchorPos;
        protected Vector2 _anchorOffset;

        protected Texture2D _tex;

        public float Rotation { get; set; }
        protected Vector2 _pivot;
        private float _rotSpeed;

        public SwarmParticle(Texture2D tex, Vector2 pos)
        {
            _tex = tex;
            Pos = pos;
            _anchorPos = pos;
            _anchorOffset = Vector2.Zero;

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
            Pos = _anchorPos + _anchorOffset;
            sB.Draw(_tex, Pos, null, Color.White * 0.8f, Rotation, _pivot, 1, SpriteEffects.None, 0);
        }
    }

    class ScatterSwarmParticle : SwarmParticle
    {
        public ScatterSwarmParticle(Texture2D tex, Vector2 anchor, Vector2 range) : base(tex, anchor)
        {
            _anchorPos = anchor;
            _anchorOffset = new Vector2(Game1.RNG.Next((int) range.X) - range.X/2, Game1.RNG.Next((int) range.Y) - range.Y / 2);
        }
    }

    class OrbitalSwarmParticle : SwarmParticle
    {
        protected float _orbitalRotation;
        protected float _orbitalSeed;

        protected float _orbitalSpeed;
        protected float _range;

        public OrbitalSwarmParticle(Texture2D tex, Vector2 anchor, float range) : base(tex, anchor)
        {
            _anchorPos = anchor;

            _orbitalSeed = _orbitalRotation = Game1.RNG.NextSingle() * MathHelper.TwoPi;
            _orbitalSpeed = (Game1.RNG.NextSingle() < 0.5f ? -1 : 1) * ((Game1.RNG.NextSingle() * 2) + 1);
            _range = range;

            _anchorOffset = new Vector2(MathF.Cos(_orbitalRotation), MathF.Sin(_orbitalRotation)) * _range;
        }

        public override void Update(float deltaTime)
        {
            _orbitalRotation = (_orbitalRotation + _orbitalSpeed * deltaTime) % MathHelper.TwoPi;
            _anchorOffset = new Vector2(MathF.Cos(_orbitalRotation), MathF.Sin(_orbitalRotation)) * (_range + MathF.Sin(_orbitalRotation * 2 + _orbitalSeed) * 8);

            base.Update(deltaTime);
        }
    }

    class CloudSwarmParticle : OrbitalSwarmParticle
    {
        public CloudSwarmParticle(Texture2D tex, Vector2 anchor, float range) : base(tex, anchor, range)
        {
            _anchorOffset = new Vector2(MathF.Cos(_orbitalRotation) * _range/2, MathF.Sin(_orbitalRotation) * _range/8 - (_range + MathF.Cos(_orbitalRotation * 3 + _orbitalSeed) * 2));
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            _anchorOffset = new Vector2(MathF.Cos(_orbitalRotation) * _range/2 , MathF.Sin(_orbitalRotation) * _range/8 - (_range + MathF.Cos(_orbitalRotation * 3 + _orbitalSeed) * 2));
        }
    }
}
