using Bots;
using Raylib_cs;
using System.Numerics;

const int Width = 1250;
const int Height = 1250;
const int StartAgentCount = 100;

Raylib.InitWindow(Width, Height, "My Bots");
Raylib.SetTargetFPS(60);

var world = new World(Width, Height, StartAgentCount);

while (!Raylib.WindowShouldClose())
{
    var dt = (float)Math.Min(Raylib.GetFrameTime(), 0.05);
    
    world.Update(dt);

    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.Black);

    foreach (var b in world.Bots)
    {
        Raylib.DrawCircle((int)b.Position.X, (int)b.Position.Y, b.Radius, Color.Green);

        var normVel = Vector2.Normalize(b.Velocity);
        var tip = b.Position + normVel * b.Radius * 2;

        Raylib.DrawLine((int)b.Position.X, (int)b.Position.Y, (int)tip.X, (int)tip.Y, Color.White);
    }

    Raylib.DrawFPS(0, 0);

    Raylib.EndDrawing();
}

Raylib.CloseWindow();