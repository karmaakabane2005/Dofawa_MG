/**
 * Auteur : Leandro Antunes & Anthony Marcacci
 * Date : 21-22
 * Projet : Dofawa Monogame
 * Details : SceneManager.cs, Squelette et Fonctions d'une scene.
 * Version : v1
 */
using Dofawa_Screen;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Dofawa_Keyboard;

namespace Dofawa_SceneManager
{
    /// <summary>
    /// Squelette et Fonctions d'une scene.
    /// </summary>
    public class SceneManager
    {
        public int id;
        public string name;
        public ScreenMouseActions screenMouseActions;
        /// <summary>
        /// Create a sceneManger component
        /// </summary>
        /// <param name="name">The scene name</param>
        /// <param name="setToCurrent">The scene is set to current scene ?</param>
        public SceneManager(string name, ScreenMouseActions screenMouseActions, bool setToCurrent = false)
        {
            this.name = name;
            this.screenMouseActions = screenMouseActions;
            Dofawa_SceneManager.Singleton.Instance.sceneManagers.Add(this);
            id = Dofawa_SceneManager.Singleton.Instance.sceneManagers.Count - 1;
            if (setToCurrent)
            {
                Dofawa_SceneManager.Singleton.Instance.currentSceneId = id;
            }
            Dofawa_Keyboard.Singleton.Instance.keyboardStateDict.Add(this,new Dictionary<Keys,KeyboardAction>());
        }
        /// <summary>
        /// Mettre cette scène actif
        /// </summary>
        public void SetToCurrent()
        {
            Dofawa_SceneManager.Singleton.Instance.currentSceneId = id;
        }
    }
    /// <summary>
    /// Fonctions des scènes
    /// </summary>
    public class SceneFunctions
    {
        /// <summary>
        /// Obtient la scène avec l'id
        /// </summary>
        /// <param name="id">id de la scène</param>
        /// <returns>La scène</returns>
        public static SceneManager GetSceneById(int id)
        {
            return Dofawa_SceneManager.Singleton.Instance.sceneManagers[id];
        }
        /// <summary>
        /// Obtient la scène avec le nom
        /// </summary>
        /// <param name="name">nom de la scène</param>
        /// <returns>La scène</returns>
        public static SceneManager GetSceneByName(string name)
        {
            foreach (SceneManager sm in Dofawa_SceneManager.Singleton.Instance.sceneManagers)
            {
                if (sm.name == name)
                {
                    return sm;
                }
            }
            throw new Exception("Scene not found");
        }
    }
    /// <summary>
    /// Variables super globales des scènes
    /// </summary>
    public class SceneVars
    {
        public List<SceneManager> sceneManagers = new List<SceneManager>();
        public int currentSceneId = /*SceneFunctions.GetSceneByName("Menu").id*/0;
    }
    /// <summary>
    /// Donne l'instance des super globales des scènes
    /// </summary>
    public static class Singleton
    {
        public static SceneVars Instance { get; } = new SceneVars();
    }
}
