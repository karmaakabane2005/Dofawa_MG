/**
 * Auteur : Leandro Antunes & Anthony Marcacci
 * Date : 21-22
 * Projet : Dofawa Monogame
 * Details : Draw.cs, Tout les scripts de dessins.
 * Version : v1
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Dofawa_Maps;
using System.Diagnostics;
using Dofawa_MiniMap;
using Dofawa_Settings;
using Dofawa_MapChanger;

namespace Dofawa_Draw
{
    /// <summary>
    /// Enumérateur pour aligner un rectangle
    /// </summary>
    public enum Alignment
    {
        Center,
        TopLeft,
        BottomLeft,
        TopRight,
        BottomRight,
        CenterTop,
        CenterBottom,
        CenterRight,
        CenterLeft
    }
    /// <summary>
    /// Class d'aide pour dessiner un texte
    /// </summary>
    public class DrawString
    {
        public string text;
        public Vector2 pos;
        public Color color;
        public Alignment align;
        /// <summary>
        /// Construit un texte
        /// </summary>
        /// <param name="text">le texte</param>
        /// <param name="pos">la position</param>
        /// <param name="color">la couleur</param>
        /// <param name="align">L'alignement</param>
        public DrawString(string text, Vector2 pos, Color color, Alignment align)
        {
            this.text = text;
            this.pos = pos;
            this.color = color;
            this.align = align;
        }
        /// <summary>
        /// Dessine le texte
        /// </summary>
        /// <param name="spriteFont">Police d'écriture</param>
        /// <param name="_spriteBatch">SpriteBatch du jeu</param>
        public void Draw(SpriteFont spriteFont, SpriteBatch _spriteBatch)
        {
            Vector2 size = spriteFont.MeasureString(text);
            Vector2 newPos = DrawFunctions.AlignForm(align, size,pos);
            _spriteBatch.DrawString(spriteFont, text, newPos, color);
        }
    }
    /// <summary>
    /// Fonctions de dessins
    /// </summary>
    public static class DrawFunctions
    {
        /// <summary>
        /// Aligne une position
        /// </summary>
        /// <param name="align">L'alignement</param>
        /// <param name="size">La taille</param>
        /// <param name="pos">La position</param>
        /// <returns></returns>
        public static Vector2 AlignForm(Alignment align, Vector2 size, Vector2 pos)
        {
            Vector2 origin = size * 0.5f;
            Vector2 newPos = pos;

            switch (align)
            {
                case Alignment.Center:
                    newPos += -origin;
                    break;
                case Alignment.TopRight:
                    newPos += new Vector2(-size.X, 0);
                    break;
                case Alignment.BottomLeft:
                    newPos += new Vector2(0, -size.Y);
                    break;
                case Alignment.BottomRight:
                    newPos += -size;
                    break;
                case Alignment.CenterTop:
                    newPos += new Vector2(-origin.X, 0);
                    break;
                case Alignment.CenterBottom:
                    newPos += new Vector2(-origin.X, -size.Y);
                    break;
                case Alignment.CenterRight:
                    newPos += new Vector2(-size.X, -origin.Y);
                    break;
                case Alignment.CenterLeft:
                    newPos += new Vector2(0, -origin.Y);
                    break;
            }
            return newPos.ToPoint().ToVector2();
        }
    }
    /// <summary>
    /// Dessin de la scène jeu
    /// </summary>
    public static class GameDraw
    {
        /// <summary>
        /// Dessine les characters
        /// </summary>
        /// <param name="_spriteBatch">SpriteBatch du jeu</param>
        /// <param name="font">Police d'écriture</param>
        public static void DrawPlayers(SpriteBatch _spriteBatch, SpriteFont font)
        {
            foreach (var character in Dofawa_Chars.Singleton.Instance.players)
            {
                if (character.isSpawned)
                {
                    character.Draw(_spriteBatch);
                }
            }
            foreach (var character in Dofawa_Chars.Singleton.Instance.ally)
            {
                if (character.isSpawned)
                {
                    character.Draw(_spriteBatch);
                }
            }
            foreach (var character in Dofawa_Chars.Singleton.Instance.enemys)
            {
                if (character.isSpawned)
                {
                    character.Draw(_spriteBatch);
                }
            }
        }
        /// <summary>
        /// Dessine la map
        /// </summary>
        /// <param name="_spriteBatch">SpriteBatch du jeu</param>
        public static void mapDraw(SpriteBatch _spriteBatch)
        {
            Vector2 posMap = Vector2.Zero;
            Maps.Map map = Dofawa_Maps.Singleton.Instance.maps[Dofawa_Maps.Singleton.Instance.Maplevel];
            for (int y = 0; y < Dofawa_Tiles.Singleton.Instance.TotalTileW; y++)
            {
                for (int x = 0; x < Dofawa_Tiles.Singleton.Instance.TotalTileH; x++)
                {
                    posMap.X = Dofawa_Tiles.Singleton.Instance.TileWPx * y;
                    posMap.Y = Dofawa_Tiles.Singleton.Instance.TileHPx * x;
                    _spriteBatch.Draw(Dofawa_Tiles.Singleton.Instance.images[map.map[x, y].spriteId], posMap, null, Color.White, 0, Vector2.Zero, Dofawa_Tiles.Singleton.Instance.TileHPx / 8f, SpriteEffects.None, 0);
                }
            }
        }
        /// <summary>
        /// Dessine le texte
        /// </summary>
        /// <param name="_spriteBatch">SpriteBatch du jeu</param>
        /// <param name="font">Police d'écriture du jeu</param>
        public static void textDraw(SpriteBatch _spriteBatch, SpriteFont font)
        {
            string mapId = Dofawa_Maps.Singleton.Instance.Maplevel;
            string nbFight = "Combats restants : " + (3 - Dofawa_Maps.Singleton.Instance.maps[mapId].nbFightCleared).ToString();
            string text = "Salle " + MapChanger.GetMainMapId(mapId) + Environment.NewLine;
            if (mapId.Contains('-'))
            {
                text += "Sous-salle " + MapChanger.GetSubMapId(mapId) + Environment.NewLine + (MapChanger.GetUpDownMapIndicator(mapId) == "U" ? "Haut" : "Bas");
            }
            _spriteBatch.DrawString(font, nbFight, new Vector2(20, Dofawa_Settings.Singleton.Instance.PreferredBackBufferHeight - 160).ToPoint().ToVector2(), Color.White);
            _spriteBatch.DrawString(font, text, new Vector2(20, Dofawa_Settings.Singleton.Instance.PreferredBackBufferHeight - 130).ToPoint().ToVector2(), Color.White);
            // Money draw
            _spriteBatch.DrawString(font, Dofawa_Chars.Singleton.Instance.players[0].level.skillPoint.ToString() + " P", new Vector2(30, 110), Color.DodgerBlue);
        }
        /// <summary>
        /// Dessine la minimap
        /// </summary>
        /// <param name="_spriteBatch">SpriteBatch du jeu</param>
        /// <param name="graphicsDeviceManager">Manager graphique du jeu</param>
        public static void DrawMM(SpriteBatch _spriteBatch, GraphicsDeviceManager graphicsDeviceManager)
        {
            MiniMapFunctions.Draw(_spriteBatch, graphicsDeviceManager);
        }
    }
}
