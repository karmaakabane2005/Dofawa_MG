/**
 * Auteur : Leandro Antunes & Anthony Marcacci
 * Date : 21-22
 * Projet : Dofawa Monogame
 * Details : Update.cs, Quelques Fonctions utilisé dans le update.
 * Version : v1
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace Dofawa_Update
{
    /// <summary>
    /// Update la scène Game
    /// </summary>
    class GameUpdate
    {
        /// <summary>
        /// Update les characters
        /// </summary>
        public static void UpdatePlayers()
        {
            foreach (var character in Dofawa_Chars.Singleton.Instance.players)
            {
                character.Update();
            }
            foreach (var character in Dofawa_Chars.Singleton.Instance.ally)
            {
                character.Update();
            }
            foreach (var character in Dofawa_Chars.Singleton.Instance.enemys)
            {
                character.Update();
            }
        }
    }
}
