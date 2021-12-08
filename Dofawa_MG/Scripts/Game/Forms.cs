/**
 * Auteur : Leandro Antunes & Anthony Marcacci
 * Date : 21-22
 * Projet : Dofawa Monogame
 * Details : Forms.cs, Fonctions permettant de dessiner des formes.
 * Version : v1
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Dofawa_Draw;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dofawa_Forms
{
    /// <summary>
    /// Toutes les formes
    /// </summary>
    public class Forms
    {
        public static Texture2D rect;
        /// <summary>
        /// Dessine une ligne
        /// </summary>
        /// <param name="graphicsDeviceManager">Manager graphique du jeu</param>
        /// <param name="spriteBatch">SpriteBatch du jeu</param>
        /// <param name="begin">Point A</param>
        /// <param name="end">Point B</param>
        /// <param name="color">Couleur</param>
        /// <param name="width">épaisseur en px</param>
        public static void DrawLine(GraphicsDeviceManager graphicsDeviceManager,SpriteBatch spriteBatch, Vector2 begin, Vector2 end, Color color, int width = 1)
        {
            Rectangle r = new Rectangle((int)begin.X, (int)begin.Y, (int)(end - begin).Length() + width, width);
            Vector2 v = Vector2.Normalize(begin - end);
            float angle = (float)Math.Acos(Vector2.Dot(v, -Vector2.UnitX));
            if (begin.Y > end.Y) angle = MathHelper.TwoPi - angle;
            spriteBatch.Draw(CreateRectangle(graphicsDeviceManager), r, null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
        }
        /// <summary>
        /// Dessine un rectangle
        /// </summary>
        /// <param name="coords">Cordonnées</param>
        /// <param name="color">Couleur</param>
        /// <param name="_graphics">Manager graphique du jeu</param>
        /// <param name="_spriteBatch">SpriteBatch du jeu</param>
        /// <param name="align">Alignement</param>
        public static void DrawRectangle(Rectangle coords, Color color, GraphicsDeviceManager _graphics, SpriteBatch _spriteBatch,Alignment align)
        {
            if (rect == null)
            {
                rect = new Texture2D(_graphics.GraphicsDevice, 1, 1,false,SurfaceFormat.Color);
                Color[] c = new Color[1];
                c[0] = Color.FromNonPremultiplied(255, 255, 255, color.A);
                rect.SetData<Color>(c);
            }
            Vector2 alignPos = DrawFunctions.AlignForm(align, new Vector2(coords.Width, coords.Height), new Vector2(coords.X, coords.Y));
            _spriteBatch.Draw(rect, new Rectangle((int)alignPos.X,(int)alignPos.Y,coords.Width,coords.Height), color);
        }

        /// <summary>
        /// Crée une texture de rectangle (1 pixel)
        /// </summary>
        /// <param name="_graphics">Manager graphique du jeu</param>
        /// <returns>Une texture2D d'un pixel</returns>
        public static Texture2D CreateRectangle(GraphicsDeviceManager _graphics)
        {
            rect = new Texture2D(_graphics.GraphicsDevice, 1, 1);
            rect.SetData(new[] { Color.White });
            return rect;
        }
        /// <summary>
        /// Crée un cercle
        /// </summary>
        /// <param name="radius">rayon</param>
        /// <param name="_graphics">Manager graphique du jeu</param>
        /// <returns>Cercle (Texture2D)</returns>
        public static Texture2D createCircleText(int radius, GraphicsDeviceManager _graphics)
        {
            Texture2D texture = new Texture2D(_graphics.GraphicsDevice, radius, radius);
            Color[] colorData = new Color[radius * radius];

            float diam = radius / 2f;
            float diamsq = diam * diam;

            for (int x = 0; x < radius; x++)
            {
                for (int y = 0; y < radius; y++)
                {
                    int index = x * radius + y;
                    Vector2 pos = new Vector2(x - diam, y - diam);
                    if (pos.LengthSquared() <= diamsq)
                    {
                        colorData[index] = Color.White;
                    }
                    else
                    {
                        colorData[index] = Color.Transparent;
                    }
                }
            }

            texture.SetData(colorData);
            return texture;
        }
    }
}
