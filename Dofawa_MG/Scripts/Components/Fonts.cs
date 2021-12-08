/**
 * Auteur : Leandro Antunes & Anthony Marcacci
 * Date : 21-22
 * Projet : Dofawa Monogame
 * Details : Fonts.cs, Super globals des fonts du jeu.
 * Version : v1
 */
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using System.Collections.Generic;

namespace Dofawa_Fonts
{
    /// <summary>
    /// Super globales des police d'écritures
    /// </summary>
    public class Fonts
    {
        public List<SpriteFont> fonts = new List<SpriteFont>();
    }
    /// <summary>
    /// Crée une instance pour les Super globales
    /// </summary>
    public static class Singleton
    {
        public static Fonts Instance { get; } = new Fonts();
    }
}