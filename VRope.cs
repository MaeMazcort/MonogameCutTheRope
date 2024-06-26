﻿// Stauts: Completed
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace Project1
{
    public class VRope
    {
        private int numPoints;
        public List<VptBase> Points;
        public List<VStk> Sticks;
        public int Level { get; set; }

        public VRope(VptBase startVpt, VptBase endVpt, int numPoints, int level)
        {
            this.numPoints = numPoints;
            Points = new List<VptBase>();
            Sticks = new List<VStk>();
            Level = level;

            // Create points along the rope, excluding the actual start and end positions for the rope
            for (var i = 1; i < numPoints - 1; i++)
            {
                var t = (float)i / (numPoints - 1);
                var pos = new V2(startVpt.Pos.X + t * (endVpt.Pos.X - startVpt.Pos.X), startVpt.Pos.Y + t * (endVpt.Pos.Y - startVpt.Pos.Y));
                Points.Add(new VptBase(pos.X, pos.Y, i, false, Color.SaddleBrown, 0.01f, startVpt.Level));
            }

            // Create sticks between consecutive rope points
            for (var i = 0; i < Points.Count - 1; i++)
            {
                Sticks.Add(new VStk(Points[i], Points[i + 1]));
            }

            // Connect the startVpt to the first rope point and endVpt to the last rope point with sticks.
            // Check if there are points in the rope to connect.
            if (Points.Any())
            {
                Sticks.Add(new VStk(startVpt, Points.First()));
                Sticks.Add(new VStk(Points.Last(), endVpt));
            }
        }
        public void Update(Rectangle space) {
            // Render each stick
            foreach (var stick in Sticks)
                stick.Update();

            // Render each point
            foreach (var point in Points)
                point.Update(space);

        }

        public void Render(SpriteBatch spriteBatch, Rectangle space, Camera camera)
        {
            // Render each stick
            foreach (var stick in Sticks)
                stick.Render(spriteBatch, camera);

            // Render each point
            foreach (var point in Points)
                point.Render(spriteBatch, camera);
        }

        public void DeleteEntireRope()
        {
            Sticks.Clear();
        }
    }
}
