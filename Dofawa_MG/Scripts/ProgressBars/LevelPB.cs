/**
 * Auteur : Leandro Antunes & Anthony Marcacci
 * Date : 21-22
 * Projet : Dofawa Monogame
 * Details : LevelPB.cs, Barre de progression du niveau.
 * Version : v1
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Dofawa_Chars;
using Dofawa_Draw;
using Dofawa_Progressbar;

namespace Dofawa_Level
{
    /// <summary>
    /// Barrde de progression du niveau.
    /// </summary>
    public class LevelPB
    {
        // Couleurs des niveaux.
        public IDictionary<int, Color> colorlevel = new Dictionary<int, Color>()
        {
            {0, Color.White },
            {25, Color.Sienna },
            {50, new Color(200,200,200) },
            {75, Color.Gold },
            {100, new Color(43,185,254) },
            {125, Color.PaleTurquoise },
            {150, new Color(163, 92, 243) },
            {175, new Color(250, 15, 71) },
            {200, new Color(175, 71, 184) }
        };
        public ProgressBar progressBar;
        public Rectangle levelRect;
        public Chars.Player player;
        public int border = 4;
        /// <summary>
        /// Obtient la couleur en fonction de la key int
        /// </summary>
        /// <param name="val">Niveau du joueur. Donne la couleur qui dont la clé englobe ce paramètre</param>
        /// <returns>La couleur</returns>
        public Color GetColor(int val)
        {
            Color tmpclr = Color.White;
            foreach (KeyValuePair<int, Color> kvp in colorlevel)
            {
                if (val >= kvp.Key)
                {
                    tmpclr = kvp.Value;
                }
                else
                {
                    break;
                }
            }
            return tmpclr;
        }
        /// <summary>
        /// Construit la barre de progression du niveau
        /// </summary>
        /// <param name="progressBar">Squelette de barre de progression</param>
        /// <param name="levelRect">Rectangle du niveau</param>
        /// <param name="player">joueur</param>
        public LevelPB(ProgressBar progressBar, Rectangle levelRect, Chars.Player player)
        {
            this.levelRect = levelRect;
            this.progressBar = progressBar;
            this.player = player;
        }
        /// <summary>
        /// Dessine la barre de progression des points de vie
        /// </summary>
        /// <param name="font">Police d'écriture</param>
        /// <param name="graphicsDeviceManager">Manager graphique du jeu</param>
        /// <param name="_spriteBatch">SpriteBatch du jeu</param>
        /// <param name="percent">Pourcentage. Dessine la barre de progression en fonction de ce paramètre.</param>
        public void Draw(SpriteFont font, GraphicsDeviceManager graphicsDeviceManager, SpriteBatch _spriteBatch, Percent percent)
        {
            Draw(font, graphicsDeviceManager, _spriteBatch, percent, levelRect);
        }
        /// <summary>
        /// Dessine la barre de progression des points de vie
        /// </summary>
        /// <param name="font">Police d'écriture</param>
        /// <param name="graphicsDeviceManager">Manager graphique du jeu</param>
        /// <param name="_spriteBatch">SpriteBatch du jeu</param>
        /// <param name="percent">Pourcentage. Dessine la barre de progression en fonction de ce paramètre.</param>
        /// <param name="levelRect">le rectangle qui contient le niveau</param>
        public void Draw(SpriteFont font, GraphicsDeviceManager graphicsDeviceManager, SpriteBatch _spriteBatch, Percent percent, Rectangle levelRect)
        {
            Dofawa_Forms.Forms.DrawRectangle(levelRect, GetColor(player.level.level), graphicsDeviceManager, _spriteBatch, Alignment.TopLeft);
            Dofawa_Forms.Forms.DrawRectangle(new Rectangle(levelRect.Center, levelRect.Size - new Point(border * 2)), Color.SteelBlue, graphicsDeviceManager, _spriteBatch, Alignment.Center);
            progressBar.Draw(graphicsDeviceManager, _spriteBatch, percent);
            _spriteBatch.DrawString(font, player.level.level.ToString(), DrawFunctions.AlignForm(Alignment.Center, font.MeasureString(player.level.level.ToString()), levelRect.Center.ToVector2()), Color.White);
        }
    }
}
