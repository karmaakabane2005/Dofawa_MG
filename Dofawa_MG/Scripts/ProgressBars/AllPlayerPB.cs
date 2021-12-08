/**
 * Auteur : Leandro Antunes & Anthony Marcacci
 * Date : 21-22
 * Projet : Dofawa Monogame
 * Details : AllPlayerPB.cs, Toutes les données du joueur sous forme de barres de progression relatives à un pourcentage.
 * Version : v1
 */
using Microsoft.Xna.Framework;
using Dofawa_Chars;
using Dofawa_Level;
using Dofawa_LifePB;
using Dofawa_Progressbar;
using Dofawa_Draw;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Dofawa_AllPlayerPB
{
    /// <summary>
    /// Toutes les données du joueur sous forme de barres de progression relatives à un pourcentage.
    /// </summary>
    public class AllPlayerPB
    {
        public LevelPB levelPB;
        public LifePB lifePB;
        public Chars.Player player;
        /// <summary>
        /// Construit toutes les données du joueur sous forme de barres de progression relatives à un pourcentage.
        /// </summary>
        /// <param name="player">joueur</param>
        /// <param name="pos">la position de la barre de progression</param>
        public AllPlayerPB(Chars.Player player, Point pos)
        {
            this.player = player;
            lifePB = new LifePB(new ProgressBar(Color.OrangeRed, pos + new Point(60 , 5), new Point(100,15)));
            levelPB = new LevelPB(new ProgressBar(Color.RoyalBlue, pos + new Point(60, 30), new Point(50,15)), new Rectangle(pos, new Point(50)),player);
            Dofawa_AllPlayerPB.Singleton.Instance.lifePBDict.Add(player, this);
        }
        /// <summary>
        /// Dessine la barre de progression
        /// </summary>
        /// <param name="font">Police d'écriture</param>
        /// <param name="graphicsDeviceManager">Manager graphique du jeu</param>
        /// <param name="_spriteBatch">SpriteBatch du jeu</param>
        public void Draw(SpriteFont font, GraphicsDeviceManager graphicsDeviceManager, SpriteBatch _spriteBatch)
        {
            lifePB.progressBar.Draw(graphicsDeviceManager, _spriteBatch, new Percent(0,player.maxPv,player.pv));
            levelPB.Draw(font, graphicsDeviceManager, _spriteBatch, new Percent(0,player.level.XpRequired,player.level.xp));
        }
    }
    /// <summary>
    /// Super globales des barres de progression du joueur
    /// </summary>
    public class AllPlayerPBVars
    {
        public IDictionary<Chars.Player, AllPlayerPB> lifePBDict = new Dictionary<Chars.Player, AllPlayerPB>();
    }
    /// <summary>
    /// Donne l'instance des super globales des barres de progression du joueur
    /// </summary>
    public static class Singleton
    {
        public static AllPlayerPBVars Instance { get; } = new AllPlayerPBVars();
    }
}
