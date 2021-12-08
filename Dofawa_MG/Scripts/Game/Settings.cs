/**
 * Auteur : Leandro Antunes & Anthony Marcacci
 * Date : 21-22
 * Projet : Dofawa Monogame
 * Details : Settings.cs, Super globales et fonctions realtif à la configuration.
 * Version : v1
 */
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Dofawa_Settings
{
    /// <summary>
    /// Fonctions des paramètres
    /// </summary>
    public static class SettingsFunctions
    {
        /// <summary>
        /// Obtient les cordonnées du centre de l'écran
        /// </summary>
        /// <returns>cor données du centre de l'écran</returns>
        public static Vector2 GetCenterOfScreen()
        {
            return new Vector2(Dofawa_Settings.Singleton.Instance.PreferredBackBufferWidth/2,Dofawa_Settings.Singleton.Instance.PreferredBackBufferHeight/2); ;
        }
    }
    /// <summary>
    /// Paramètres de mouvement (touches)
    /// </summary>
    public class MoveSettings
    {
        public Keys up = Keys.W;
        public Keys right = Keys.D;
        public Keys down = Keys.S;
        public Keys left = Keys.A;
    }
    /// <summary>
    /// Super globales de paramètres
    /// </summary>
    public class Settings
    {
        public int PreferredBackBufferHeight = 1080/*720*/;
        public int PreferredBackBufferWidth = 1920/*1280*/;
        public MoveSettings moveSettings = new MoveSettings();
    }
    /// <summary>
    /// Donne l'instance des super globales de paramètres
    /// </summary>
    public static class Singleton
    {
        public static Settings Instance { get; } = new Settings();
    }
}
