/**
 * Auteur : Leandro Antunes & Anthony Marcacci
 * Date : 21-22
 * Projet : Dofawa Monogame
 * Details : ProgressBar.cs, Squelette d'une barre de progression et du calcul d'un pourcentage.
 * Version : v1
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Dofawa_Forms;
using Dofawa_Draw;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dofawa_Progressbar
{
    /// <summary>
    /// Squelette d'une barre de progression et du calcul d'un pourcentage.
    /// </summary>
    public class ProgressBar
    {
        public float percent;
        public Color color;
        public Point pos;
        public Point size;
        public int border = 2;
        /// <summary>
        /// Origine de la barrde de progression
        /// </summary>
        public Vector2 SizeOrigin => new Vector2(size.X, size.Y) / 2;
        /// <summary>
        /// Construit la barre progression
        /// </summary>
        /// <param name="color">Couleur de la barre</param>
        /// <param name="pos">Position de la barre</param>
        /// <param name="size">Taille de la barre</param>
        /// <param name="percent">Pourcentage. C'est ce paramètre qui est relatif a la valeur de la barre de progression</param>
        public ProgressBar(Color color, Point pos, Point size, float percent = 0f)
        {
            this.percent = percent;
            this.color = color;
            this.size = size;
            this.pos = pos;
        }
        /// <summary>
        /// Dessine la barre de progression
        /// </summary>
        /// <param name="graphicsDeviceManager">Manager graphique du jeu</param>
        /// <param name="_spriteBatch">SpriteBatch du jeu</param>
        /// <param name="percent">Pourcentage. Dessine la barre de progression en fonction de ce paramètre.</param>
        public void Draw(GraphicsDeviceManager graphicsDeviceManager, SpriteBatch _spriteBatch, Percent percent)
        {
            Draw(graphicsDeviceManager, _spriteBatch, percent, pos, size, color, Color.White, border);
        }
        /// <summary>
        /// Dessine la barre de progression
        /// </summary>
        /// <param name="graphicsDeviceManager">Manager graphique du jeu</param>
        /// <param name="_spriteBatch">SpriteBatch du jeu</param>
        /// <param name="percent">Pourcentage. Dessine la barre de progression en fonction de ce paramètre.</param>
        /// <param name="pos">Position de la barre de progression</param>
        /// <param name="size">Taille de la barre de progression</param>
        /// <param name="SpecialColor">Couleur de la barre de progression</param>
        /// <param name="borderColor">Couleur de la bordure de la barre de progression</param>
        /// <param name="border">épaisseur de la bordure de la barre de progression</param>
        public void Draw(GraphicsDeviceManager graphicsDeviceManager, SpriteBatch _spriteBatch, Percent percent, Point pos, Point size, Color SpecialColor, Color borderColor, int border = 2)
        {
            Forms.DrawRectangle(new Rectangle(pos, new Point(size.X, size.Y)), borderColor, graphicsDeviceManager, _spriteBatch, Alignment.TopLeft);
            Forms.DrawRectangle(new Rectangle(pos + new Point(border), new Point(size.X - border * 2, size.Y - border * 2)), Color.Black, graphicsDeviceManager, _spriteBatch, Alignment.TopLeft);
            Forms.DrawRectangle(new Rectangle(pos + new Point(border), new Point((int)((size.X - border * 2) / 100f * percent.PercentVal), size.Y - border * 2)), SpecialColor, graphicsDeviceManager, _spriteBatch, Alignment.TopLeft);
        }
        /// <summary>
        /// Dessine la barre de progression
        /// </summary>
        /// <param name="graphicsDeviceManager">Manager graphique du jeu</param>
        /// <param name="_spriteBatch">SpriteBatch du jeu</param>
        /// <param name="percent">Pourcentage. Dessine la barre de progression en fonction de ce paramètre.</param>
        /// <param name="pos">Position de la barre de progression</param>
        /// <param name="size">Taille de la barre de progression</param>
        /// <param name="borderColor">Couleur de la bordure de la barre de progression</param>
        /// <param name="border">épaisseur de la bordure de la barre de progression</param>
        public void Draw(GraphicsDeviceManager graphicsDeviceManager, SpriteBatch _spriteBatch, Percent percent, Point pos, Point size, Color borderColor, int border = 2)
        {
            Draw(graphicsDeviceManager, _spriteBatch, percent, pos, size, color, borderColor, border);
        }
    }
    /// <summary>
    /// Pourcentage qui sert a calculer la distance de la barre de progression
    /// </summary>
    public class Percent
    {
        public int minval;
        public int maxval;
        public int val;
        /// <summary>
        /// distance entre min et max
        /// </summary>
        public int Range => maxval - minval;
        /// <summary>
        /// Valeur en pourcentage
        /// </summary>
        public float PercentVal => (float)val / Range * 100;
        /// <summary>
        /// Construit un pourcentage qui sert a calculer la distance de la barre de progression
        /// </summary>
        /// <param name="minval">valeur minimale</param>
        /// <param name="maxval">valeur maximale</param>
        /// <param name="val">valeur actuel</param>
        public Percent(int minval, int maxval, int val)
        {
            this.minval = minval;
            this.maxval = maxval;
            this.val = val;
        }
    }
}
