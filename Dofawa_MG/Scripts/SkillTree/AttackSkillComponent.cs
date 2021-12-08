/**
 * Auteur : Leandro Antunes & Anthony Marcacci
 * Date : 21-22
 * Projet : Dofawa Monogame
 * Details : AttackSkillComponent.cs, Squelette pré-rempli d'un composant d'un arbre de compétence. Celui-ci est pré-rempli par rapport à l'attaque
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
    /// Squelette pré-rempli d'un composant d'un arbre de compétence. Celui-ci est pré-rempli par rapport à l'attaque
    /// </summary>
    class AttackSkillComponent
    {
        public int cost;
        public int atkToAdd;
        public SkillTree.Component component;
        /// <summary>
        /// Construit squelette pré-rempli d'un composant d'un arbre de compétence. Celui-ci est pré-rempli par rapport à l'attaque
        /// </summary>
        /// <param name="cost">le prix</param>
        /// <param name="atkToAdd">les points a ajouter</param>
        /// <param name="parent">le parent</param>
        /// <param name="pos">la position</param>
        /// <param name="nobuyedColor">la couleur quand on la pas acheté</param>
        /// <param name="buyedColor">la couleur quand on la acheté</param>
        /// <param name="tag">le tag</param>
        /// <param name="childOf">les parents</param>
        /// <param name="title">le titre</param>
        /// <param name="desc">la description</param>
        public AttackSkillComponent(int cost, int atkToAdd, SkillTree parent, Point pos, Color nobuyedColor, Color buyedColor, string tag, List<SkillTree.Component> childOf, string title = null, string desc = null)
        {
            this.cost = cost;
            this.atkToAdd = atkToAdd;
            string tmptitle = "Plus d'attaque";
            string tmpdesc = "Vous avez trouvé une épée parterre dans la salle. +" + atkToAdd + " d'attaque.";
            if (title != null)
            {
                tmptitle = title;
            }
            if (desc != null)
            {
                tmpdesc = desc;
            }
            this.component = parent.IniComponent(tmptitle, tmpdesc, cost, pos, nobuyedColor, buyedColor, AddAtk, tag, childOf);
        }
        /// <summary>
        /// Ajoute les points
        /// </summary>
        public void AddAtk()
        {
            Dofawa_Chars.Singleton.Instance.players[0].bonusatk += atkToAdd;
            Dofawa_Chars.Singleton.Instance.players[0].atk += atkToAdd;
        }
    }
}
