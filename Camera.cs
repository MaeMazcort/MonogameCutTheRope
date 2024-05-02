using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    public class Camera
    {
        public V2 position {  get; private set; }

        // Init the camera at this position
        public Camera(V2 startPosition)
        {
            position = startPosition;
        }

        // Update the camera to centralize it in a point
        public void Follow(V2 targetPos, int screenW, int screenH)
        {
            position = new V2(targetPos.X - screenW / 2, targetPos.Y - screenH / 2);
        }

        public void ClampToArea(int width, int height, int xW, int yH)
        {
            position.X = Math.Clamp(position.X, 0, width - xW);
            position.Y = Math.Clamp(position.Y, 0, height - yH);
        }
    }
}
