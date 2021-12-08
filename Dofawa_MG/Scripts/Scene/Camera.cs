/**
 * Auteur : Leandro Antunes & Anthony Marcacci
 * Date : 21-22
 * Projet : Dofawa Monogame
 * Details : Camera.cs, Camera d'une scène.
 * Version : v1
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Dofawa_Keyboard;
using Dofawa_Mouse;

namespace Dofawa_Camera
{
    /// <summary>
    /// Camera d'une scène
    /// </summary>
    public class Camera
    {
        public Vector2 position;
        Matrix viewMatrix;
        public float scale = 1f;
        public float scaleFactor;
        public int previousScrollValue = 0;
        // Vrai si la camera peut être zoomé
        public bool canBeZoom;
        public Vector2 maxScale;
        /// <summary>
        /// Construit la camera d'une scène.
        /// </summary>
        /// <param name="position">position de la camera</param>
        /// <param name="scaleFactor">facteur d'étirement de la camera (de combien monter par scroll ?)</param>
        /// <param name="maxScale">Intervale minimale et maximale de l'étirement</param>
        /// <param name="canBeZoom">Est-ce que la camera peut être zoomé ?</param>
        public Camera(Vector2 position, float scaleFactor, Vector2 maxScale, bool canBeZoom = false)
        {
            this.position = position;
            this.scaleFactor = scaleFactor;
            this.canBeZoom = canBeZoom;
            this.maxScale = maxScale;
        }
        /// <summary>
        /// Obtient la matrice de la camera
        /// </summary>
        public Matrix ViewMatrix
        {
            get => viewMatrix;
        }
        /// <summary>
        /// Update la camera en fonction du viewport
        /// </summary>
        /// <param name="view">le viewport</param>
        public void Update(Viewport view)
        {
            if (canBeZoom)
                MouseFunctions.UpdateMouseWheel(this);
            viewMatrix = Matrix.CreateTranslation(new Vector3(-position, 0)) *
                Matrix.CreateScale(new Vector3(scale)) *
                Matrix.CreateTranslation(new Vector3(view.Width / 2, view.Height / 2 , 0));
        }
    }
}
