/**
 * Auteur : Leandro Antunes & Anthony Marcacci
 * Date : 21-22
 * Projet : Dofawa Monogame
 * Details : Screen.cs, Composant d'écran, Utilise pour le scrolling et le "Drag and drop".
 * Version : v1
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Dofawa_Camera;
using Dofawa_Mouse;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Dofawa_Screen
{
    /// <summary>
    /// Action de l'écran en fonction de la souris ( Scroll + "Drag and drop" )
    /// </summary>
    public class ScreenMouseActions
    {
        public MouseDragState mouseDragState;
        public Camera camera;
        // Est-ce que l'écran pour être bougé ( Ex: Minimap et SkillTree sont "bougeables" )
        public bool canBeDrag;
        // Vrai si le clic droit est appuyé
        bool leftClickPressed = false;
        Vector2 lastScreenPos = Vector2.Zero;
        /// <summary>
        /// Obtient le centre de l'écran
        /// </summary>
        Vector2 CenterPos
        {
            get => Dofawa_Settings.SettingsFunctions.GetCenterOfScreen() + lastScreenPos;
        }
        /// <summary>
        /// Construit les actions de l'écran en fonction de la souris
        /// </summary>
        /// <param name="mouseDragState">état de souris calculant la position entre la position de la souris lors du clique et la position actuelle</param>
        /// <param name="camera">Camera</param>
        /// <param name="canBeDrag">Vrai si l'écran peut être bougeable (Comme Minimap et Skilltree)</param>
        public ScreenMouseActions(MouseDragState mouseDragState, Camera camera, bool canBeDrag = false)
        {
            this.mouseDragState = mouseDragState;
            this.camera = camera;
            this.canBeDrag = canBeDrag;
        }
        /// <summary>
        /// Update l'écran
        /// </summary>
        public void Update()
        {
            if (canBeDrag)
            {
                MouseState m = Mouse.GetState();
                if (m.LeftButton == ButtonState.Pressed && !leftClickPressed)
                {
                    leftClickPressed = true;
                    mouseDragState.startMousePos = m.Position.ToVector2();
                }
                if (m.LeftButton == ButtonState.Released && leftClickPressed)
                {
                    leftClickPressed = false;
                    camera.position = mouseDragState.PosChange / camera.scale + CenterPos;
                    lastScreenPos += mouseDragState.PosChange / camera.scale;
                }
                if (leftClickPressed)
                {
                    camera.position = mouseDragState.PosChange / camera.scale + CenterPos;
                }
            }
        }
    }
}
