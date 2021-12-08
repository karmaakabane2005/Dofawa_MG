/**
 * Auteur : Leandro Antunes & Anthony Marcacci
 * Date : 21-22
 * Projet : Dofawa Monogame
 * Details : ActionPointSkillComponent.cs, Squelette pré-rempli d'un composant d'un arbre de compétence. Celui-ci est pré-rempli par rapport aux points d'action du joueur
 * Version : v1
 */
using Microsoft.Xna.Framework;
using Dofawa_SkillTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dofawa_SkillTreeComponent
{
    /// <summary>
    /// Squelette pré-rempli d'un composant d'un arbre de compétence. Celui-ci est pré-rempli par rapport aux points d'action du joueur
    /// </summary>
    class ActionPointSkillComponent
    {
        public int cost;
        public int paToAdd;
        public SkillTree.Component component;
        /// <summary>
        /// Construit squelette pré-rempli d'un composant d'un arbre de compétence. Celui-ci est pré-rempli par rapport aux points d'action du joueur
        /// </summary>
        /// <param name="cost">le prix</param>
        /// <param name="paToAdd">les points a ajouter</param>
        /// <param name="parent">le parent</param>
        /// <param name="pos">la position</param>
        /// <param name="nobuyedColor">la couleur quand on la pas acheté</param>
        /// <param name="buyedColor">la couleur quand on la acheté</param>
        /// <param name="tag">le tag</param>
        /// <param name="childOf">les parents</param>
        /// <param name="title">le titre</param>
        /// <param name="desc">la description</param>
        public ActionPointSkillComponent(int cost, int paToAdd, SkillTree parent, Point pos, Color nobuyedColor, Color buyedColor, string tag, List<SkillTree.Component> childOf, string title = null, string desc = null)
        {
            this.cost = cost;
            this.paToAdd = paToAdd;
            string tmptitle = "Plus de point(s) d'action";
            string tmpdesc = "Vous avez analysé vos ennemis et appris leurs points faibles. + " + paToAdd + " point(s) d'action.";
            if (title != null)
            {
                tmptitle = title;
            }
            if (desc != null)
            {
                tmpdesc = desc;
            }
            this.component = parent.IniComponent(tmptitle, tmpdesc, cost, pos, nobuyedColor, buyedColor, AddPa, tag, childOf);
        }
        /// <summary>
        /// Ajoute les points
        /// </summary>
        public void AddPa()
        {
            Dofawa_Chars.Singleton.Instance.players[0].bonuspa += paToAdd;
            Dofawa_Chars.Singleton.Instance.players[0].actionPoint += paToAdd;
        }
    }
}
