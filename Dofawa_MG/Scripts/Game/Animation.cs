/**
 * Auteur : Leandro Antunes & Anthony Marcacci
 * Date : 21-22
 * Projet : Dofawa Monogame
 * Details : Animation.cs
 * Version : v1
 */
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Dofawa_Keyboard;

namespace Dofawa_Animation
{
    /// <summary>
    /// classe contenant toute les fonctions de l'animations
    /// </summary>
    public static class AnimationFunctions
    {
        /// <summary>
        /// fonction qui crée une list de texture2D
        /// </summary>
        /// <returns name="list">List de texture2D utile a l'animation du sprite</returns>
        public static List<Texture2D> getSprite()
        {
            List<Texture2D> list = new List<Texture2D>();
            list.Add(Dofawa_Tiles.Singleton.Instance.charImages[Dofawa_Tiles.Singleton.Instance.charImages.Count]);
            list.Add(Dofawa_Tiles.Singleton.Instance.charImages[Dofawa_Tiles.Singleton.Instance.charImages.Count-1]);
            return list;
        }
        /// <summary>
        /// fonctions qui update l'animation
        /// </summary>
        /// <param name="gameTime"></param>
        public static void updateAnimation(GameTime gameTime)
        {
            
            List<Texture2D> textureMove = new List<Texture2D>();
            textureMove.Add(Dofawa_Tiles.Singleton.Instance.charImages[2]);
            textureMove.Add(Dofawa_Tiles.Singleton.Instance.charImages[3]);
            List<Texture2D> textureStand = new List<Texture2D>();
            textureStand.Add(Dofawa_Tiles.Singleton.Instance.charImages[0]);
            textureStand.Add(Dofawa_Tiles.Singleton.Instance.charImages[1]);
            Dofawa_Animation.Singleton.Instance.timeElapsedBetweenLastFrame += gameTime.ElapsedGameTime.TotalSeconds*10;
            if (GameKeyboard.IsPressed(Dofawa_Settings.Singleton.Instance.moveSettings.up) || GameKeyboard.IsPressed(Dofawa_Settings.Singleton.Instance.moveSettings.right) ||
              GameKeyboard.IsPressed(Dofawa_Settings.Singleton.Instance.moveSettings.down) || GameKeyboard.IsPressed(Dofawa_Settings.Singleton.Instance.moveSettings.left))
            {
                
                if (Dofawa_Animation.Singleton.Instance.timeElapsedBetweenLastFrame >= 4)
                {
                    Dofawa_Animation.Singleton.Instance.timeElapsedBetweenLastFrame = 0;
                    Dofawa_Chars.Singleton.Instance.players[0].sprite = Dofawa_Chars.Singleton.Instance.players[0].sprite == textureMove[0] ? textureMove[1] : textureMove[0];
                }
            }
            else
            {
                Dofawa_Chars.Singleton.Instance.players[0].sprite = textureStand[1];

                /*if (Dofawa_Animation.Singleton.Instance.timeElapsedBetweenLastFrame >= 10)
                {
                    Dofawa_Animation.Singleton.Instance.timeElapsedBetweenLastFrame = 0;
                    Dofawa_Chars.Singleton.Instance.players[0].sprite = Dofawa_Chars.Singleton.Instance.players[0].sprite == textureStand[0] ? textureStand[1] : textureStand[0];
                }*/
            }
            
        }

    }
    /// <summary>
    /// class contenant toutes les variables
    /// </summary>
    public class AnimationVars
    {
       public double timeElapsedBetweenLastFrame = 0d;
    }
    /// <summary>
    /// classes permetant d'accéder dynamiquement au variables de la class AnimationVars si celles ci sont public
    /// </summary>
    public static class Singleton
    {
        public static AnimationVars Instance { get; } = new AnimationVars();
    }
}
