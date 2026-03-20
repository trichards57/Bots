using System.Numerics;

namespace Bots;

internal readonly record struct Bot(
    int Id,
    Vector2 Position,
    Vector2 Velocity,
    float Radius = 5.0f)
{
    public float Angle => MathF.Atan2(Velocity.Y, Velocity.X);
    public float Speed => Velocity.Length();
    public float Mass => 1.0f;
};

