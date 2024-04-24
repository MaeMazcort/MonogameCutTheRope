using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    [Serializable]
    public struct VertexPositionColor : IVertexType
    {
        public Vector3 Position { get; set; }
        public Color Color { get; set; }

        public VertexPositionColor(Vector3 position, Color color)
        {
            Position = position;
            Color = color;
        }                

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return new VertexDeclaration(new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0), new VertexElement(sizeof(float) * 3, VertexElementFormat.Color, VertexElementUsage.Color, 0)); }
        }
    }
}
