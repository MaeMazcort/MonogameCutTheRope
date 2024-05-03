using System.Drawing;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Project1;

public class WindInfluencer
{
    public V2 Position { get; private set; }
    public V2 Direction { get; private set; }
    public float Strength { get; private set; }
    public float Velocity { get; private set; }
    public int DirectionDegrees { get; private set; }
        
    public bool IsActive { get; set; }
    public float distance;
        
    public WindInfluencer(V2 position, float strength, float velocity, Rectangle canvasSize)
    {
        Position = position;
        Strength = strength;
        Velocity = velocity;
        SetDirectionBasedOnPosition(canvasSize);
    }

    public V2 GetForce(CandyVpt candy)
    {
        V2 distanceVec = candy.Pos - Position;
        distance = distanceVec.Length();
        V2 directionNorm = V2.NormalizeVector(Direction);

        float forceMagnitude = (Strength / (distance * distance)) * Velocity;
        return directionNorm * forceMagnitude;
    }

    private void SetDirectionBasedOnPosition(Rectangle canvasSize)
    {
        // Divide the canvas in 2 vertically, set direction based on the side
        DirectionDegrees = Position.X < canvasSize.Width / 2 ? 0 : 180;
        // Convert degrees to vector
        Direction = DirectionDegrees == 0 ? new V2(1, 0) : new V2(-1, 0);
    }

    public void SetActive(bool active)
    {
        IsActive = active;
    }
}