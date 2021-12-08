/**
 * Auteur : Leandro Antunes & Anthony Marcacci
 * Date : 21-22
 * Projet : Dofawa Monogame
 * Details : Level.cs
 * Version : v1
 */
using System;

namespace Dofawa_Level
{
    /// <summary>
    /// class level contenant le level(int), l'xp(int), les skillPoint(int) et XpRequired(int)
    /// </summary>
    public class Level
    {
        public int level;
        public int xp;
        public int skillPoint;
        public int XpRequired 
        {
            get => (int)(1d / 4 * Math.Pow(level, 2) + 5);
        }
        /// <summary>
        /// crée un type level regroupant les 2 parmètres
        /// </summary>
        /// <param name="level"></param>
        /// <param name="xp"></param>
        public Level(int level = 0, int xp = 0)
        {
            this.level = level;
            this.xp = xp;
        }
        /// <summary>
        /// fonctions VerifyLevel servant a tester si le player peut monter en niveau
        /// </summary>
        public void VerifyLevel()
        {
            if(xp >= XpRequired)
            {
                xp -= XpRequired;
                level++;
                skillPoint++;
                VerifyLevel();
            }
        }
    }
}
