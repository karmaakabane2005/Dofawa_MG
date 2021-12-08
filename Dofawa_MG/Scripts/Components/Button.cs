/**
 * Auteur : Leandro Antunes & Anthony Marcacci
 * Date : 21-22
 * Projet : Dofawa Monogame
 * Details : Button.cs, Composoant boutton.
 * Version : v1
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Dofawa_Camera;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dofawa_Button
{
    public class Button
    {
        public Point pos;
        public Point size;
        public Texture2D image;
        public Action onClickFunction;
        
        /// <summary>
        /// Crée un boutton avec une méthode donné
        /// </summary>
        /// <param name="pos">Position du boutton</param>
        /// <param name="size">taille du boutton</param>
        /// <param name="onClickFunction">Méthode éxécuté quand le boutton est cliqué</param>
        /// <param name="image">Texture du boutton</param>
        public Button(Point pos, Point size, Action onClickFunction = null, Texture2D image = null)
        {
            this.pos = pos;
            this.onClickFunction = onClickFunction;
            this.size = size;
            this.image = image;
        }
        /// <summary>
        /// Rergarde si le bouton est cliqué
        /// </summary>
        /// <param name="camera">Camera pour pouvoir calculer la position réele de la souris</param>
        /// <returns>Vrai si cliqué Faux sinon</returns>
        public bool Click(Camera camera)
        {
            Vector2 mousePosWorldSpace = Vector2.Transform(Mouse.GetState().Position.ToVector2(), Matrix.Invert(camera.ViewMatrix));
            if (Dofawa_Mouse.Singleton.Instance.isClicked
                && !Dofawa_Mouse.Singleton.Instance.isHolded
                && mousePosWorldSpace.X >= pos.X
                && mousePosWorldSpace.X <= pos.X + size.X
                && mousePosWorldSpace.Y >= pos.Y
                && mousePosWorldSpace.Y <= pos.Y + size.Y)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Rergarde si le bouton est cliqué
        /// </summary>
        /// <returns>Vrai si cliqué Faux sinon</returns>
        public bool Click()
        {
            if (Dofawa_Mouse.Singleton.Instance.isClicked
                && !Dofawa_Mouse.Singleton.Instance.isHolded
                && Mouse.GetState().X >= pos.X
                && Mouse.GetState().X <= pos.X + size.X
                && Mouse.GetState().Y >= pos.Y
                && Mouse.GetState().Y <= pos.Y + size.Y)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Rergarde si le bouton est cliqué en fonction d'une position et non de la souris
        /// </summary>
        /// <returns>Vrai si cliqué Faux sinon</returns>
        public bool Click(int posx, int posy)
        {
            if (Dofawa_Mouse.Singleton.Instance.isClicked
            && !Dofawa_Mouse.Singleton.Instance.isHolded
            && posx >= pos.X
            && posx <= pos.X + size.X
            && posy >= pos.Y
            && posy <= pos.Y + size.Y)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Update le boutton
        /// </summary>
        /// <param name="camera">Camera de la scène</param>
        public void Update(Camera camera)
        {
            if (Click(camera))
            {
                // ?.Invoke() = Verifie si onClickFunction est pas null et si il ne l'est pas alors appelle la méthode donné.
                onClickFunction?.Invoke();
            }
        }
        /// <summary>
        /// Dessine le boutton
        /// </summary>
        /// <param name="_spriteBatch">SpriteBatch du jeu</param>
        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(image, new Rectangle(pos.X,pos.Y,size.X,size.Y), Color.White);
        }
    }
}
