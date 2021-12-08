/**
 * Auteur : Leandro Antunes & Anthony Marcacci
 * Date : 21-22
 * Projet : Dofawa Monogame
 * Details : LifePB.cs, Barre de progression de la vie du joueur.
 * Version : v1
 */
using Microsoft.Xna.Framework.Graphics;
using Dofawa_Chars;
using Dofawa_Progressbar;
using System.Collections.Generic;

namespace Dofawa_LifePB
{
    /// <summary>
    /// Barre de progression de la vie du joueur
    /// </summary>
    public class LifePB
    {
        public ProgressBar progressBar;
        /// <summary>
        /// Construit la barre de progression de la vie du joueur
        /// </summary>
        /// <param name="progressBar"></param>
        public LifePB(ProgressBar progressBar)
        {
            this.progressBar = progressBar;
        }
    }
}
