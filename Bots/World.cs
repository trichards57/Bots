using System.Collections.Immutable;
using System.Numerics;
using System.Reflection.Metadata;

namespace Bots;

internal class World
{
    private const int MaxSpeed = 120;
    private ImmutableList<Bot> bots;

    public World(int width, int height, int agentCount)
    {
        const int minSpeed = 20;
        const int minEnergy = 500;
        const int maxEnergy = 5000;

        Width = width;
        Height = height;

        bots = [..Enumerable.Range(0, agentCount)
            .Select(i => {
                var angle = Random.Shared.NextSingle() * 2 * (float)Math.PI;
                var speed = minSpeed + Random.Shared.NextSingle() * (MaxSpeed - minSpeed);
                var vel = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * speed;

                return new Bot(
                i,
                Vector2.Create(Random.Shared.NextSingle() * width, Random.Shared.NextSingle() * height),
                vel,
                minEnergy + Random.Shared.NextSingle() * (maxEnergy - minEnergy));
            })];
    }

    public IImmutableList<Bot> Bots => bots;
    public int Height { get; }
    public int Width { get; }

    public void Update(float dt)
    {
        var oldBots = bots;

        bots = [..oldBots.AsParallel().Select(b =>
        {
            var vel = b.Velocity;

            foreach (var c in oldBots.Where(d=>d.Id != b.Id))
            {
                var dist = Vector2.Distance(b.Position, c.Position);
                var dn = Vector2.Normalize(b.Position - c.Position);
                var vrel = Vector2.Dot((b.Velocity - c.Velocity), dn);
                if (float.IsNaN(vrel)) continue;

                if (vrel < 0 && dist > float.Epsilon && dist < (b.Radius + c.Radius))
                {
                    var j = (-2 * b.Mass * c.Mass * vrel)/(b.Mass + c.Mass);
                    if (float.IsNaN(j) ) continue;

                    vel += (j * dn)/b.Mass;
                }
            }

            if (vel.Length() > MaxSpeed)
            {
                vel = Vector2.Normalize(vel) * MaxSpeed;
            }

            var nb = b with
            {
                Position = b.Position + vel * dt,
                Velocity = vel,
            };

            if (nb.Position.X < 0) nb = nb with { Position = nb.Position with { X = nb.Position.X + Width } };
            if (nb.Position.X > Width) nb = nb with { Position = nb.Position with { X = nb.Position.X - Width } };
            if (nb.Position.Y < 0) nb = nb with { Position = nb.Position with { Y = nb.Position.Y + Height } };
            if (nb.Position.Y > Height) nb = nb with { Position = nb.Position with { Y = nb.Position.Y - Height } };

            return nb;
        })];
    }
}