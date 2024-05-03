using System.Drawing;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Project1;

public interface IParticle
{
    void Update(Rectangle space);
    void ApplyForce(V2 force);  
}