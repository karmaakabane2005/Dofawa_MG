/**
 * Auteur : Leandro Antunes & Anthony Marcacci
 * Date : 21-22
 * Projet : Dofawa Monogame
 * Details : Time.cs, Super globales donnant le temps.
 * Version : v1
 */
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dofawa_Time
{
    /// <summary>
    /// Super globales de temps
    /// </summary>
    public class Time
    {
        public float deltaTime = 0f;
        /// <summary>
        /// Obtient le temps delta
        /// </summary>
        /// <param name="gameTime">Temps du jeu</param>
        public void UpdateDeltaTime(GameTime gameTime)
        {
            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
    /// <summary>
    /// Donne l'instance des super globales du temps
    /// </summary>
    public static class Singleton
    {
        public static Time Instance { get; } = new Time();
    }
}
