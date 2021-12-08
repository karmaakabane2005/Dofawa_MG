/**
 * Auteur : Leandro Antunes & Anthony Marcacci
 * Date : 21-22
 * Projet : Dofawa Monogame
 * Details : Keyboard.cs, Composant clavier.
 * Version : v1
 */
using Microsoft.Xna.Framework.Input;
using Dofawa_SceneManager;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Dofawa_Keyboard
{
    public class KeyboardAction
    {
        public Keys key;
        // Vrai si la touche est appuyé
        public bool keypressed;
        public Action func;
        // Vrai si l'action est active
        public bool isActive;
        /// <summary>
        /// Vrai à partir de la deuxième frame que la touche est appuyé
        /// </summary>
        public bool IsHold
        {
            get => (GameKeyboard.IsPressed(key) && isActive);
        }
        /// <summary>
        /// Crée l'action de clavier
        /// </summary>
        /// <param name="key">La touche</param>
        /// <param name="func">La méthode éxécuté lorsque est la touche est appuyé</param>
        /// <param name="scene">La scène de l'action</param>
        /// <param name="isActive">Vrai si l'action est activé</param>
        /// <param name="keypressed">Vrai si la touche est appuyé</param>
        public KeyboardAction(Keys key, Action func, SceneManager scene, bool isActive = true, bool keypressed = false)
        {
            Ini(key, func, isActive, keypressed);
            Dofawa_Keyboard.Singleton.Instance.keyboardStateDict[scene].TryAdd(key, this);
        }
        /// <summary>
        /// Crée l'action de clavie
        /// </summary>
        /// <param name="key">La touche</param>
        /// <param name="func">La méthode éxécuté lorsque est la touche est appuyé</param>
        /// <param name="scenes">Las scènes de l'action</param>
        /// <param name="isActive">Vrai si l'action est activé</param>
        /// <param name="keypressed">Vrai si la touche est appuyé</param>
        public KeyboardAction(Keys key, Action func, SceneManager[] scenes, bool isActive = true, bool keypressed = false)
        {
            Ini(key, func, isActive, keypressed);
            foreach (SceneManager scene in scenes)
            {
                Dofawa_Keyboard.Singleton.Instance.keyboardStateDict[scene].TryAdd(key, this);
            }
        }
        /// <summary>
        /// Initialise les variables
        /// </summary>
        /// <param name="key">La touche</param>
        /// <param name="func">La méthode éxécuté lorsque est la touche est appuyé</param>
        /// <param name="isActive">Vrai si l'action est activé</param>
        /// <param name="keypressed">Vrai si la touche est appuyé</param>
        private void Ini(Keys key, Action func, bool isActive, bool keypressed )
        {
            this.key = key;
            this.func = func;
            this.isActive = isActive;
            this.keypressed = keypressed;
        }
        /// <summary>
        /// Obtient l'état du clavier et verifie si la touche est appuyé. Si c'est la première frame alors execute la fonction
        /// </summary>
        public void KeyStateAndExe()
        {
            if (GameKeyboard.GetState().IsKeyDown(key) && !keypressed)
            {
                keypressed = true;
                // ?.Invoke() = Verifie si onClickFunction est pas null et si il ne l'est pas alors appelle la méthode donné.
                func?.Invoke();
            }
            if (GameKeyboard.GetState().IsKeyUp(key))
            {
                keypressed = false;
            }
        }
    }
    public class KeyboardFunction
    {
        /// <summary>
        /// Update les actions de claviers actives
        /// </summary>
        public static void Update()
        {
            Dictionary<Keys, KeyboardAction> dict = Dofawa_Keyboard.Singleton.Instance.keyboardStateDict[SceneFunctions.GetSceneById(Dofawa_SceneManager.Singleton.Instance.currentSceneId)];
            foreach (KeyboardAction kba in dict.Values)
            {
                kba.KeyStateAndExe();
            }
        }
        /// <summary>
        /// Supprime une action de clavier
        /// </summary>
        /// <param name="keyboardAction">l'action de clavier à supprimer</param>
        public static void RemoveKeyboardAction(KeyboardAction keyboardAction)
        {
            var dict = Dofawa_Keyboard.Singleton.Instance.keyboardStateDict;
            foreach (var subdict in dict.Values)
            {
                foreach (KeyboardAction kba in subdict.Values)
                {
                    if (kba == keyboardAction)
                    {
                        subdict.Remove(kba.key);
                    }
                }
            }
        }
    }
    public class GameKeyboard
    {
        static KeyboardState currentKeyState;
        static KeyboardState previousKeyState;

        /// <summary>
        /// Obtient l'état du clavier
        /// </summary>
        /// <returns>L'état du clavier</returns>
        public static KeyboardState GetState()
        {
            previousKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();
            return currentKeyState;
        }

        /// <summary>
        /// Verifie si la touche est appuyé
        /// </summary>
        /// <param name="key">la touche</param>
        /// <returns>Vrai si la touche est appuyé sinon faux</returns>
        public static bool IsPressed(Keys key)
        {
            return currentKeyState.IsKeyDown(key);
        }

        /// <summary>
        /// Verifie si la touche à été pressé la première fois ( 1ère frame )
        /// </summary>
        /// <param name="key">La touche</param>
        /// <returns>Vrai si la touche est pressé et que c'est la première frame sinon Faux</returns>
        public static bool HasBeenPressed(Keys key)
        {
            return currentKeyState.IsKeyDown(key) && !previousKeyState.IsKeyDown(key);
        }
    }
    /// <summary>
    /// Super globales du clavier
    /// </summary>
    public class KeyboardVars
    {
        public IDictionary<SceneManager, Dictionary<Keys, KeyboardAction>> keyboardStateDict = new Dictionary<SceneManager, Dictionary<Keys, KeyboardAction>>();
    }

    /// <summary>
    /// Crée l'instance pour les Super globales
    /// </summary>
    public static class Singleton
    {
        public static KeyboardVars Instance { get; } = new KeyboardVars();
    }
}
