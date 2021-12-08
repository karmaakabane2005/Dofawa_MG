using System;
using System.Collections.Generic;
using System.Text;

namespace Dofawa_MenuStart
{
    public class MenuStart
    {
        public class MenuFunc
        {
            public static void StartIni()
            {
                //Dofawa_Settings.Singleton.Instance.PreferredBackBufferHeight = 1080;
                //Dofawa_Settings.Singleton.Instance.PreferredBackBufferWidth = 1920;
                StartGame();

            }
            public static void Update()
            {
                
            }
            public static void Draw()
            {

            }
            public static void Settings()
            {
                Dofawa_SceneManager.Singleton.Instance.currentSceneId = Dofawa_SceneManager.SceneFunctions.GetSceneByName("Sett" +
                    "ings").id;
            }
            public static void StartGame()
            {
                Dofawa_SceneManager.Singleton.Instance.currentSceneId = Dofawa_SceneManager.SceneFunctions.GetSceneByName("Game").id;
            }

        }
    }
}
