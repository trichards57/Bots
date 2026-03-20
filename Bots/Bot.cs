using System.Numerics;

namespace Bots;

internal readonly record struct Bot(
    int Id,
    Vector2 Position,
    Vector2 Velocity,
    float Energy)
{
    const int StandardMass = 1000; // Stored energy required for a bot to have a mass of 1.0f.
    const int StandardRadius = 5; // Radius of a bot with 1.0f mass.

    private static int RadiusFromMass(float mass) => (int)(StandardRadius * MathF.Sqrt(mass));
    private static int MassFromEnergy(float energy) => (int)(energy / StandardMass);

    public float Angle => MathF.Atan2(Velocity.Y, Velocity.X);
    public float Speed => Velocity.Length();
    public float Mass => MassFromEnergy(Energy);
    public float Radius => RadiusFromMass(Mass);
};