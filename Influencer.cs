using System.Drawing;

namespace Project1;

public abstract class Influencer
{
    public abstract V2 GetForce(CandyVpt candyParticle);
    //public abstract void Render(Graphics g, Size space);
        
    public abstract void SetActive(bool active);

}