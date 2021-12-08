/**
 * Auteur : Leandro Antunes & Anthony Marcacci
 * Date : 21-22
 * Projet : Dofawa Monogame
 * Details : SceneActions.cs, Ordres d'update et de draw structurés sous forms de scènes.
 * Version : v1
 */
using Dofawa_Draw;
using Dofawa_Update;
using Dofawa_Animation;
using Dofawa_Keyboard;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Dofawa_Inventory;
using Dofawa_Camera;
using Dofawa_SceneManager;
using Dofawa_MenuStart;

namespace Dofawa_SceneActions
{
    /// <summary>
    /// Ordres d'update et de draw structurés sous forms de scènes.
    /// </summary>
    public static class SceneActions
    {
        /// <summary>
        /// Dessine le jeu sous une scène et des fonctions multi-scènes.
        /// </summary>
        /// <param name="_spriteBatch">SpriteBatch du jeu</param>
        /// <param name="font">Police d'écriture</param>
        /// <param name="graphicsDeviceManager">Manager graphique du jeu</param>
        /// <param name="camera">Camera de la scène</param>
        public static void Draw(SpriteBatch _spriteBatch, SpriteFont font, GraphicsDeviceManager graphicsDeviceManager, Camera camera)
        {
            SceneManager scene = SceneFunctions.GetSceneById(Dofawa_SceneManager.Singleton.Instance.currentSceneId);
            if (scene.name == "Game")
            {
                _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, camera.ViewMatrix);
                GameDraw.mapDraw(_spriteBatch);
                GameDraw.DrawPlayers(_spriteBatch, font);
                GameDraw.textDraw(_spriteBatch, font);
                InventoryFunctions.drawInventory(_spriteBatch, graphicsDeviceManager, Dofawa_Fonts.Singleton.Instance.fonts[1]);
                Dofawa_AllPlayerPB.Singleton.Instance.lifePBDict[Dofawa_Chars.Singleton.Instance.players[0]].Draw(font, graphicsDeviceManager, _spriteBatch);
                if (Dofawa_Maps.Singleton.Instance.Maplevel == "0")
                {
                    _spriteBatch.DrawString(font, "Appuyez sur \"F\" pour commencer le combat de cette salle.", DrawFunctions.AlignForm(Alignment.Center, font.MeasureString("Appuyez sur \"F\" pour commencer le combat de cette salle."), new Vector2(Dofawa_Settings.SettingsFunctions.GetCenterOfScreen().X, 100)),Color.White);
                }
                _spriteBatch.End();
            }
            else if (scene.name == "Minimap")
            {
                _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, camera.ViewMatrix);
                GameDraw.DrawMM(_spriteBatch, graphicsDeviceManager);
                _spriteBatch.End();
            }
            else if (scene.name == "Dead")
            {
                _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, camera.ViewMatrix);
                SpriteFont bigFont = Dofawa_Fonts.Singleton.Instance.fonts[2];

                string[] text = new string[] { 
                "Tu es mort",
                "Appuye sur ESC pour recommencer"
                };

                _spriteBatch.DrawString(bigFont, text[0], DrawFunctions.AlignForm(Alignment.Center, bigFont.MeasureString(text[0]),Dofawa_Settings.SettingsFunctions.GetCenterOfScreen() - new Vector2(0, 50)), Color.White);
                _spriteBatch.DrawString(bigFont, text[1], DrawFunctions.AlignForm(Alignment.Center, bigFont.MeasureString(text[1]), Dofawa_Settings.SettingsFunctions.GetCenterOfScreen() + new Vector2(0, 50)), Color.White);
                _spriteBatch.End();
            }
            else if (scene.name == "Fight")
            {
                _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, camera.ViewMatrix);
                GameDraw.mapDraw(_spriteBatch);
                _spriteBatch.End();
                Dofawa_FightMapItem.Singleton.Instance.fmap.Draw(Dofawa_Fonts.Singleton.Instance.fonts[2], _spriteBatch, graphicsDeviceManager, camera);
            }
            else if (scene.name == "SkillTree")
            {
                Dofawa_Chars.Singleton.Instance.players[0].skillTree.Draw(Dofawa_Fonts.Singleton.Instance.fonts[4], font, _spriteBatch, graphicsDeviceManager ,scene.screenMouseActions.camera);
            }
            else if (scene.name == "Menu")
            {
                MenuStart.MenuFunc.Draw();
            }
        }
        /// <summary>
        /// Update le jeu sous une scène et des fonctions multi-scènes.
        /// </summary>
        /// <param name="gameTime">Temps du jeu</param>
        /// <param name="viewport">Le viewport du Manager grapgique du jeu</param>
        public static void Update(GameTime gameTime, Viewport viewport)
        {
            Dofawa_Time.Singleton.Instance.UpdateDeltaTime(gameTime);
            Dofawa_Mouse.MouseFunctions.Update();
            KeyboardFunction.Update();
            
            SceneManager scene = SceneFunctions.GetSceneById(Dofawa_SceneManager.Singleton.Instance.currentSceneId);
            if (scene.name == "Game")
            {
                InventoryFunctions.updateInventory();
                GameUpdate.UpdatePlayers();
                AnimationFunctions.updateAnimation(gameTime);
            }
            else if (scene.name == "Minimap")
            {

            }
            else if (scene.name == "Dead")
            {
                
            }
            else if (scene.name == "Fight")
            {
                Dofawa_FightMapItem.Singleton.Instance.fmap.Update();
                Dofawa_Chars.Singleton.Instance.players[0].VerifyPv();
            }
            else if (scene.name == "SkillTree")
            {
                Dofawa_Chars.Singleton.Instance.players[0].skillTree.Update(SceneFunctions.GetSceneByName("SkillTree").screenMouseActions.camera,Dofawa_Chars.Singleton.Instance.players[0].level);
            }
            else if (scene.name == "Menu")
            {
                MenuStart.MenuFunc.Update();
            }
            Dofawa_Chars.Singleton.Instance.players[0].level.VerifyLevel();
            scene.screenMouseActions.Update();
            scene.screenMouseActions.camera.Update(viewport);
        }
    }
}
