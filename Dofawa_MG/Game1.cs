/**
 * Auteur : Leandro Antunes & Anthony Marcacci
 * Date : 21-22
 * Projet : Dofawa Monogame
 * Details : Game1.cs, Fonctionnement du jeu fichier d'éxecution de tout les autres fichiers.
 * Version : v1
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;
using Dofawa_Ini;
using Dofawa_SceneActions;
using Dofawa_Camera;
using Dofawa_Screen;
using Dofawa_Mouse;
using Dofawa_SceneManager;

namespace Dofawa_MG
{
    public class Game1 : Game
    {
        public GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;
        public Texture2D cercleStyle;
        public SpriteFont font;
        public Camera CurrentCamera
        {
            get => SceneFunctions.GetSceneById(Dofawa_SceneManager.Singleton.Instance.currentSceneId).screenMouseActions.camera;
        }
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
            Ini.InitializeVariables();
            _graphics.PreferredBackBufferHeight = Dofawa_Settings.Singleton.Instance.PreferredBackBufferHeight;
            _graphics.PreferredBackBufferWidth = Dofawa_Settings.Singleton.Instance.PreferredBackBufferWidth;
            _graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Ini.LoadImages(Content, "Tiles/Colored/", Dofawa_Tiles.Singleton.Instance.images);
            Ini.LoadImages(Content, "Tiles/Char/", Dofawa_Tiles.Singleton.Instance.charImages);
            Ini.LoadImages(Content, "UI/FightUI/", Dofawa_FightMapItem.Singleton.Instance.fightUI);
            Ini.LoadImages(Content, "Tiles/Enemys/", Dofawa_Tiles.Singleton.Instance.enemyImages);
            Ini.LoadImages(Content, "UI/InventoryUI/", Dofawa_Inventory.Singleton.Instance.UiInventory);
            font = Ini.LoadFont(Content, "Font");
            Ini.LoadFont(Content, "FontInventory");
            Ini.LoadFont(Content, "BigFont");
            Ini.LoadFont(Content, "UIFont");
            Ini.LoadFont(Content, "MediumFont");
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            SceneActions.Update(gameTime, _graphics.GraphicsDevice.Viewport);
            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            SceneActions.Draw(_spriteBatch, font, _graphics, CurrentCamera);
            base.Draw(gameTime);
        }
    }
}
