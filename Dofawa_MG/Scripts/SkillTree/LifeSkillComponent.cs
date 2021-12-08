/**
 * Auteur : Leandro Antunes & Anthony Marcacci
 * Date : 21-22
 * Projet : Dofawa Monogame
 * Details : LifeSkillComponent.cs, Squelette pré-rempli d'un composant d'un arbre de compétence. Celui-ci est pré-rempli par rapport à la vie
 * Version : v1
 */
using Dofawa_SkillTree;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dofawa_SkillTreeComponent
{
    /// <summary>
    /// Squelette pré-rempli d'un composant d'un arbre de compétence. Celui-ci est pré-rempli par rapport à la vie
    /// </summary>
    class LifeSkillComponent
    {
        public int cost;
        public int lifeToAdd;
        public SkillTree.Component component;
        /// <summary>
        /// Construit squelette pré-rempli d'un composant d'un arbre de compétence. Celui-ci est pré-rempli par rapport à la vie
        /// </summary>
        /// <param name="cost">le prix</param>
        /// <param name="lifeToAdd">les points a ajouter</param>
        /// <param name="parent">le parent</param>
        /// <param name="pos">la position</param>
        /// <param name="nobuyedColor">la couleur quand on la pas acheté</param>
        /// <param name="buyedColor">la couleur quand on la acheté</param>
        /// <param name="tag">le tag</param>
        /// <param name="childOf">les parents</param>
        /// <param name="title">le titre</param>
        /// <param name="desc">la description</param>
        public LifeSkillComponent(int cost, int lifeToAdd, SkillTree parent, Point pos, Color nobuyedColor, Color buyedColor, string tag, List<SkillTree.Component> childOf, string title = null, string desc = null)
        {
            this.cost = cost;
            this.lifeToAdd = lifeToAdd;
            string tmptitle = "Plus de vie";
            string tmpdesc = "Vos combats vous ont rendu plus robuste ! +" + lifeToAdd + " de points de vie.";
            if (title != null)
            {
                tmptitle = title;
            }
            if (desc != null)
            {
                tmpdesc = desc;
            }
            this.component = parent.IniComponent(tmptitle, tmpdesc, cost, pos, nobuyedColor, buyedColor, AddLife, tag, childOf);
        }
        /// <summary>
        /// Ajoute les points
        /// </summary>
        public void AddLife()
        {
            Dofawa_Chars.Singleton.Instance.players[0].bonuspv += lifeToAdd;
            Dofawa_Chars.Singleton.Instance.players[0].pv += lifeToAdd;
        }
    }
}
