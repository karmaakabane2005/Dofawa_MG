/**
 * Auteur : Leandro Antunes & Anthony Marcacci
 * Date : 21-22
 * Projet : Dofawa Monogame
 * Details : Mouse.cs, Composant souris.
 * Version : v1
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Dofawa_Camera;

namespace Dofawa_Mouse
{
    /// <summary>
    /// Super globales de la souris
    /// </summary>
    public class MouseVars
    {
        // Vrai si cliqué sinon faux
        public bool isClicked;
        // Vrai si cliqué depuis 2 frames sinon faux
        public bool isHolded;
    }
    /// <summary>
    /// Obtient un état de souris calculant la position entre la position de la souris lors du clique et la position actuelle
    /// </summary>
    public class MouseDragState
    {
        public Vector2 startMousePos;
        /// <summary>
        /// Construit un état de souris calculant la position entre la position de la souris lors du clique et la position actuelle
        /// </summary>
        /// <param name="startMousePos">La position de départ</param>
        public MouseDragState(Vector2 startMousePos)
        {
            this.startMousePos = startMousePos;
        }
        /// <summary>
        /// Obtient le changement de position depuis la construction et maintenant
        /// </summary>
        public Vector2 PosChange
        {
            get => startMousePos - Mouse.GetState().Position.ToVector2();
        }
    }
    /// <summary>
    /// Fonctions pour les souris
    /// </summary>
    public static class MouseFunctions
    {
        /// <summary>
        /// Update la souris pour obtenir si il est cliqué ou non
        /// </summary>
        public static void Update()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (!Dofawa_Mouse.Singleton.Instance.isHolded && !Dofawa_Mouse.Singleton.Instance.isClicked)
                {
                    Dofawa_Mouse.Singleton.Instance.isClicked = true;
                }
                else
                {
                    Dofawa_Mouse.Singleton.Instance.isHolded = true;
                }
            }
            else
            {
                Dofawa_Mouse.Singleton.Instance.isClicked = false;
                Dofawa_Mouse.Singleton.Instance.isHolded = false;
            }
        }
        /// <summary>
        /// Update la roulette de la souris ( Utile pour le scrolling ) et change l'étirement de la camera
        /// </summary>
        /// <param name="camera">La camera</param>
        public static void UpdateMouseWheel(Camera camera)
        {
            int scrollVal = Mouse.GetState().ScrollWheelValue;

            if (scrollVal > camera.previousScrollValue)
            {
                if (camera.scale + camera.scaleFactor <= camera.maxScale.Y)
                {
                    camera.scale += camera.scaleFactor;
                }
                else
                {
                    camera.scale = camera.maxScale.Y;
                }
            }
            if (scrollVal < camera.previousScrollValue)
            {
                if (camera.scale - camera.scaleFactor >= camera.maxScale.X)
                {
                    camera.scale -= camera.scaleFactor;
                }
                else
                {
                    camera.scale = camera.maxScale.X;
                }
            }
            camera.previousScrollValue = scrollVal;
        }
    }
    /// <summary>
    /// Crée l'instance pour les Super Globales
    /// </summary>
    public static class Singleton
    {
        public static MouseVars Instance { get; } = new MouseVars();
    }
}
