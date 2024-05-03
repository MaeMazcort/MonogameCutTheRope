using System.Drawing;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Project1;

public class WindInfluencer : Influencer
{
    public V2 Position { get; private set; }
    public V2 Direction { get; private set; }
    public float Strength { get; private set; }
    public float Velocity { get; private set; }
    public int DirectionDegrees { get; private set; }
        
    public bool IsActive { get; set; }
    public int Radius = 20;
        
    public WindInfluencer(V2 position, float strength, float velocity, Rectangle canvasSize)
    {
        Position = position;
        Strength = strength;
        Velocity = velocity;
        SetDirectionBasedOnPosition(canvasSize);
    }
        
    public override V2 GetForce(CandyVpt candyParticle)
    {
        V2 displacement = candyParticle.Pos - Position;
        float distance = displacement.Length(); 
            
        //Avoid division by 0
        if (distance == 0 || Direction.Length() == 0) return new V2(0, 0);
            
        //Normalize the direction vector
        V2 normalizedDirection = Direction / Direction.Length();
            
        //Calculate the force magnitude
        float forceMagnitude = (Strength / (distance * distance)) * Velocity;
            
        //Apply the force in the wind direction
        return normalizedDirection * forceMagnitude;
    }
        
    private void SetDirectionBasedOnPosition(Rectangle canvasSize)
    {
        // Divide the canvas in 2 vertically, set direction based on the side
        DirectionDegrees = Position.X < canvasSize.Width / 2 ? 0 : 180;
        // Convert degrees to vector
        Direction = DirectionDegrees == 0 ? new V2(1, 0) : new V2(-1, 0);
    }

    public override void SetActive(bool active)
    {
        IsActive = active;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
    
    public void Activate()
    {
        IsActive = true;
    }
}