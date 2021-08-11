using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace QwertyMod
{
    class TextureBuilder
    {
        //this function creates an array of Texture2Ds that 'progress' from start to end
        public static Texture2D[] TransitionFrames(Texture2D start, Texture2D end, int steps)
        {
            Texture2D[] output = new Texture2D[steps];
            int width = start.Width;
            int height = start.Height;
            PixelPack startPack = new PixelPack(start);
            PixelPack endPack = new PixelPack(end);
            PixelPack transition = PixelPack.SetupTransition(startPack, endPack);
            output[0] = start;
            output[steps - 1] = end;
            for(int i = 1; i < steps-1; i++)
            {
                transition.movePixels(1f / (float)(steps - 1));
                output[i] = transition.Print(width, height);
            }
            return output;
            
            
        }
        class PixelMover
        {
            public Vector2 position;
            Vector2 startPosition;
            Vector2 endPosition;
            public Color color;
            Color startColor;
            Color endColor;
            public PixelMover(Vector2 position, Color color)
            {
                this.position = startPosition = position;
                this.color = startColor = color;
            }
            public PixelMover(Vector2 position, Vector2 endPosition, Color color, Color endColor)
            {
                this.position = startPosition = position;
                this.color = startColor = color;
                this.endPosition = endPosition;
                this.endColor = endColor;
            }
            public void move(float scale)
            {
                position += (endPosition - startPosition) * scale;
                color.A += (byte)Math.Round((endColor.A - startColor.A) * scale);
                color.R += (byte)Math.Round((endColor.R - startColor.R) * scale);
                color.G += (byte)Math.Round((endColor.G - startColor.G) * scale);
                color.B += (byte)Math.Round((endColor.B - startColor.B) * scale);
            }
        }
        class PixelPack
        {
            List<PixelMover> pixels = new List<PixelMover>();
            public PixelPack()
            {

            }
            public PixelPack(Texture2D texture)
            {
                Color[] dataColors = new Color[texture.Width * texture.Height]; //Color array
                texture.GetData(dataColors);
                int width = texture.Width;
                for(int c =0; c < dataColors.Length; c++)
                {
                    if(dataColors[c].A != 0)
                    {
                        pixels.Add(new PixelMover(new Vector2(c % width, c / width), dataColors[c]));
                    }
                }
            }
            public static PixelPack SetupTransition(PixelPack start, PixelPack end)
            {
                PixelPack newPack = new PixelPack();
                foreach(PixelMover startPixel in start.pixels)
                {
                    Vector2 closest = end.pixels[0].position;
                    Vector2 position = startPixel.position;
                    Color endColor = end.pixels[0].color;
                    foreach (PixelMover endPixel in end.pixels)
                    {
                        if((endPixel.position - position).Length() < (closest - position).Length())
                        {
                            closest = endPixel.position;
                            endColor = endPixel.color;
                        }
                    }
                    newPack.pixels.Add(new PixelMover(startPixel.position, closest, startPixel.color, endColor));
                }
                foreach (PixelMover endPixel in end.pixels)
                {
                    Vector2 closest = start.pixels[0].position;
                    Vector2 position = endPixel.position;
                    Color startColor = start.pixels[0].color;
                    foreach (PixelMover startPixel in start.pixels)
                    {
                        if ((startPixel.position - position).Length() < (closest - position).Length())
                        {
                            closest = startPixel.position;
                            startColor = startPixel.color;
                        }
                    }
                    PixelMover newOne = new PixelMover(closest, endPixel.position, startColor, endPixel.color);
                    if (!newPack.pixels.Contains(newOne))
                    {
                        newPack.pixels.Add(newOne);
                    }
                    
                }
                return newPack;
            }
            public void movePixels(float scale)
            {
                for(int i = 0; i < pixels.Count; i++)
                {
                    pixels[i].move(scale);
                }
            }
            public Texture2D Print(int width, int height)
            {
                
                Color[] dataColors = new Color[width * height];
                foreach(PixelMover pixel in pixels)
                {
                    int x = (int)Math.Round(pixel.position.X);
                    int y = (int)Math.Round(pixel.position.Y);
                    dataColors[x + y * width] = pixel.color;
                }
                Texture2D d = new Texture2D(Main.graphics.GraphicsDevice, width, height);
                d.SetData(0, null, dataColors, 0, width * height);
                return d;
            }
        }

    }
}
