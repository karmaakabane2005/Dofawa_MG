/**
 * Auteur : Leandro Antunes & Anthony Marcacci
 * Date : 21-22
 * Projet : Dofawa Monogame
 * Details : XpDropSkillComponent.cs, Squelette pré-rempli d'un composant d'un arbre de compétence. Celui-ci est pré-rempli par rapport au facteur supplémentaire de l'experience reçu lors d'un combat terminé.
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
    /// Squelette pré-rempli d'un composant d'un arbre de compétence. Celui-ci est pré-rempli par rapport au facteur supplémentaire de l'experience reçu lors d'un combat terminé.
    /// </summary>
    class XpDropSkillComponent
    {
        public int cost;
        public float dropFactorToAdd;
        public SkillTree.Component component;
        /// <summary>
        /// Construit squelette pré-rempli d'un composant d'un arbre de compétence. Celui-ci est pré-rempli par rapport au facteur supplémentaire de l'experience reçu lors d'un combat terminé.
        /// </summary>
        /// <param name="cost">le prix</param>
        /// <param name="dropFactorToAdd">les points a ajouter</param>
        /// <param name="parent">le parent</param>
        /// <param name="pos">la position</param>
        /// <param name="nobuyedColor">la couleur quand on la pas acheté</param>
        /// <param name="buyedColor">la couleur quand on la acheté</param>
        /// <param name="tag">le tag</param>
        /// <param name="childOf">les parents</param>
        /// <param name="title">le titre</param>
        /// <param name="desc">la description</param>
        public XpDropSkillComponent(int cost, float dropFactorToAdd, SkillTree parent, Point pos, Color nobuyedColor, Color buyedColor, string tag, List<SkillTree.Component> childOf, string title = null, string desc = null)
        {
            this.cost = cost;
            this.dropFactorToAdd = dropFactorToAdd;
            string tmptitle = "Facteur d'experience augmenté";
            string tmpdesc = "Vos ennemis sont utiles désormais. Vous les analysez et commprenez le fonctionement de vos sorts. Facteur d'xp reçu +" + dropFactorToAdd + ".";
            if (title != null)
            {
                tmptitle = title;
            }
            if (desc != null)
            {
                tmpdesc = desc;
            }
            this.component = parent.IniComponent(tmptitle, tmpdesc, cost, pos, nobuyedColor, buyedColor, AddXpDrop, tag, childOf);
        }
        /// <summary>
        /// Ajoute les points
        /// </summary>
        public void AddXpDrop()
        {
            Dofawa_Chars.Singleton.Instance.players[0].xpDropFactor += dropFactorToAdd;
        }
    }
}
