// Status: Completed
using System.Drawing;

namespace Project1
{
    public class StartVpt : VptBase
    {
        public StartVpt(float x, float y, int id, int level) : base(x, y, id, true, Color.CornflowerBlue, 10f, level)
        {
            Level = level;
        }
    }
}
