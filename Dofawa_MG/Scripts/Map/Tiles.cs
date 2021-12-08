/**
 * Auteur : Leandro Antunes & Anthony Marcacci
 * Date : 21-22
 * Projet : Dofawa Monogame
 * Details : Tiles.cs, Fonctions relatives aux cases de la map.
 * Version : v1
 */
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dofawa_Tiles
{
    /// <summary>
    /// Fonctions de cases
    /// </summary>
    public static class TilesFunctions
    {
        /// <summary>
        /// Convert tile position to pixel position
        /// </summary>
        /// <param name="tileX">The nth tile in horizontal column by starting by 0</param>
        /// <param name="tileY">The nth tile in vertical column by starting by 0</param>
        /// <returns>The pixel position</returns>
        public static Vector2 TileToPixel(int tileX, int tileY)
        {
            Vector2 res = new Vector2
            {
                X = Singleton.Instance.TileWPx * tileX,
                Y = Singleton.Instance.TileHPx * tileY
            };
            return res;
        }
        /// <summary>
        /// Convert pixel position to tile position
        /// </summary>
        /// <remarks>If the tile position returns a decimal number (ex. 1.6) the result will be cast to int (ex. 1.6 => 1)</remarks>
        /// <param name="pxWidth">The number of pixel in width</param>
        /// <param name="pxHeight">The number of pixel in height</param>
        /// <returns>The tile position strating by 0</returns>
        public static Vector2 PixelToTile(int pxWidth, int pxHeight)
        {
            Vector2 res = new Vector2
            {
                X = (int)Math.Floor((float)pxWidth / Singleton.Instance.TileWPx),
                Y = (int)Math.Floor((float)pxHeight / Singleton.Instance.TileHPx)
            };
            return res;
        }
    }

    /// <summary>
    /// Super globales de cases
    /// </summary>
    public class Tiles
    {
        public List<Texture2D> images = new List<Texture2D>();
        public List<Texture2D> charImages = new List<Texture2D>();
        public List<Texture2D> enemyImages = new List<Texture2D>();
        public List<Texture2D> mmImages = new List<Texture2D>();
        public int TotalTileW = 32;
        public int TotalTileH = 18;
        /// <summary>
        /// taille en longueur (px) d'une case
        /// </summary>
        public int TileWPx
        {
            get => Dofawa_Settings.Singleton.Instance.PreferredBackBufferWidth / TotalTileW;
        }
        /// <summary>
        /// taille en hauteur (px) d'une case
        /// </summary>
        public int TileHPx
        {
            get => Dofawa_Settings.Singleton.Instance.PreferredBackBufferHeight / TotalTileH;
        }
    }
    /// <summary>
    /// Donne l'instance des super globales de cases
    /// </summary>
    public static class Singleton
    {
        public static Tiles Instance { get; } = new Tiles();
    }
}
