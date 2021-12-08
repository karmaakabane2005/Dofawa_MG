/**
 * Auteur : Leandro Antunes & Anthony Marcacci
 * Date : 21-22
 * Projet : Dofawa Monogame
 * Details : Ini.cs, Toutes les variables qui sont initialisés lors du lancement du jeu ou d'une scène
 * Version : v1
 */
using System.Collections.Generic;
using Dofawa_Chars;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Dofawa_Level;
using Dofawa_MapChanger;
using Dofawa_Keyboard;
using Microsoft.Xna.Framework.Input;
using Dofawa_SceneManager;
using Dofawa_MiniMap;
using Dofawa_Inventory;
using Dofawa_Camera;
using Dofawa_Settings;
using Dofawa_Screen;
using Dofawa_Mouse;
using Dofawa_AllPlayerPB;
using Dofawa_FightMapItem;
using Dofawa_SkillTree;
using Dofawa_SkillTreeComponent;
using Dofawa_MenuStart;

namespace Dofawa_Ini
{
    /// <summary>
    /// Initialiser les vraiables
    /// </summary>
    public static class Ini
    {
        /// <summary>
        /// Fonction qui initialise toutes les variables au lancement du jeu
        /// </summary>
        public static void InitializeVariables()
        {




            // Crée la scène manager jeu
            new SceneManager("Game", new ScreenMouseActions(
                new MouseDragState(Vector2.Zero),
                new Camera(SettingsFunctions.GetCenterOfScreen(), 0.05f, new Vector2(0.1f, 4))),
                true);
            // Crée la scène manager Minimap
            new SceneManager("Minimap", new ScreenMouseActions(
                new MouseDragState(Vector2.Zero),
                new Camera(SettingsFunctions.GetCenterOfScreen(), 0.05f, new Vector2(0.1f, 4), true),
                true
                ));
            // Crée la scène manager Mort
            new SceneManager("Dead", new ScreenMouseActions(
                new MouseDragState(Vector2.Zero),
                new Camera(SettingsFunctions.GetCenterOfScreen(), 0.05f, new Vector2(0.1f, 4))));
            // Crée la scène manager Combat
            new SceneManager("Fight", new ScreenMouseActions(
                new MouseDragState(Vector2.Zero),
                new Camera(SettingsFunctions.GetCenterOfScreen(), 0.05f, new Vector2(0.1f, 4))));
            // Crée la scène manager Arbre de compétence
            new SceneManager("SkillTree", new ScreenMouseActions(
                new MouseDragState(Vector2.Zero),
                new Camera(SettingsFunctions.GetCenterOfScreen(), 0.05f, new Vector2(0.1f, 4), true),
                true));
            new SceneManager("Menu", new ScreenMouseActions(
                new MouseDragState(Vector2.Zero),
                new Camera(SettingsFunctions.GetCenterOfScreen(), 0.05f, new Vector2(0.1f, 4))));

            //on set la scene a Menu
            Dofawa_SceneManager.Singleton.Instance.currentSceneId = SceneFunctions.GetSceneByName("Menu").id;



            InventoryIni();
            // Crée le joueur
            Chars.PlayerIni(new Chars.Player(
                new Vector2( // Position
                    Dofawa_Settings.Singleton.Instance.PreferredBackBufferWidth / 2,
                    Dofawa_Settings.Singleton.Instance.PreferredBackBufferHeight / 2
                    ),
                1, //RoomIndex
                Dofawa_Tiles.Singleton.Instance.charImages[1],
                new Level(),
                new Chars.Equipment(Dofawa_Inventory.Singleton.Instance.items["Casque du débutant"], Dofawa_Inventory.Singleton.Instance.items["Plastron du débutant"], Dofawa_Inventory.Singleton.Instance.items["Jambières du débutant"], Dofawa_Inventory.Singleton.Instance.items["Bottes du débutant"]),
                new SkillTree()));
            MapIni();
            Dofawa_Inventory.InventoryFunctions.iniInventory();
            new AllPlayerPB(Dofawa_Chars.Singleton.Instance.players[0], new Point(30, 50));
            // Ini Minimap kba "M"
            new KeyboardAction(Keys.M, MiniMapFunctions.ChangeScene,new SceneManager[] {SceneFunctions.GetSceneByName("Minimap"), SceneFunctions.GetSceneByName("Game") });
            // Ini Skilltree kba "B"
            new KeyboardAction(Keys.B, SkillTreeFunctions.ChangeScene, new SceneManager[] { SceneFunctions.GetSceneByName("SkillTree"), SceneFunctions.GetSceneByName("Game") });
            // Ini Fight kba "F"
            new KeyboardAction(Keys.F, FMapFunctions.CreateFight, Dofawa_SceneManager.SceneFunctions.GetSceneByName("Game"));

            // Ini SkillComponent
            SkillTreeComponentIni();
            Dofawa_MenuStart.MenuStart.MenuFunc.StartIni();
        }
        /// <summary>
        /// Charge les images
        /// </summary>
        /// <param name="contentManager">Manager de contenu du jeu</param>
        /// <param name="path">chemin</param>
        /// <param name="images">Liste ou on va mettre les images</param>
        public static void LoadImages(ContentManager contentManager, string path, List<Texture2D> images)
        {
            DirectoryInfo dir = new DirectoryInfo(contentManager.RootDirectory + "/" + path);
            FileInfo[] files = dir.GetFiles("*.xnb", SearchOption.AllDirectories);
            foreach (FileInfo file in files)
            {
                string key = Path.GetFileNameWithoutExtension(file.Name);
                Texture2D texture = contentManager.Load<Texture2D>(path + key);
                images.Add(texture);
            }
        }
        /// <summary>
        /// Initialise la map
        /// </summary>
        public static void MapIni()
        {
            Dofawa_Tiles.Singleton.Instance.TotalTileH = 18;
            Dofawa_Tiles.Singleton.Instance.TotalTileW = 32;
            MapChanger.CreateMap("0", Dofawa_Tiles.Singleton.Instance.TotalTileW, Dofawa_Tiles.Singleton.Instance.TotalTileH, false, true, false, false);
        }
        /// <summary>
        /// Initialise l'arbre de compétence
        /// </summary>
        public static void SkillTreeComponentIni()
        {
            // parent 1
            new LifeSkillComponent(1, 25, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(0, 0), new Color(130, 130, 130), Color.White, "parent", null);
            // Layer 1
            new AttackSkillComponent(1, 5, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(200, -300), new Color(130, 130, 130), Color.White, "l1_atk1", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("parent", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new AttackSkillComponent(1, 5, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(200, -100), new Color(130, 130, 130), Color.White, "l1_atk2", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("parent", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new LifeSkillComponent(2, 60, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(200, 100), new Color(130, 130, 130), Color.White, "l1_pv1", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("parent", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new AttackSkillComponent(1, 5, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(200, 300), new Color(130, 130, 130), Color.White, "l1_atk3", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("parent", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            // Layer 2
            new AttackSkillComponent(2, 10, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(400, -350), new Color(130, 130, 130), Color.White, "l2_atk1", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("l1_atk1", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new LifeSkillComponent(2, 60, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(400, -250), new Color(130, 130, 130), Color.White, "l2_pv1", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("l1_atk1", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });

            new AttackSkillComponent(2, 10, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(400, -150), new Color(130, 130, 130), Color.White, "l2_atk2", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("l1_atk2", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new LifeSkillComponent(2, 60, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(400, -50), new Color(130, 130, 130), Color.White, "l2_pv2", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("l1_atk2", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });

            new AttackSkillComponent(2, 10, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(400, 50), new Color(130, 130, 130), Color.White, "l2_atk3", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("l1_pv1", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new LifeSkillComponent(2, 60, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(400, 150), new Color(130, 130, 130), Color.White, "l2_pv3", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("l1_pv1", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });

            new AttackSkillComponent(2, 10, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(400, 350), new Color(130, 130, 130), Color.White, "l2_atk4", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("l1_atk3", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new LifeSkillComponent(2, 60, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(400, 250), new Color(130, 130, 130), Color.White, "l2_pv4", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("l1_atk3", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            // Layer 3
            new AttackSkillComponent(3, 20, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(600, -200), new Color(130, 130, 130), Color.White, "l3_atk1", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("l2_atk1", Dofawa_Chars.Singleton.Instance.players[0].skillTree), SkillTreeFunctions.getComponentByTag("l2_atk2", Dofawa_Chars.Singleton.Instance.players[0].skillTree), SkillTreeFunctions.getComponentByTag("l2_pv1", Dofawa_Chars.Singleton.Instance.players[0].skillTree), SkillTreeFunctions.getComponentByTag("l2_pv2", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new LifeSkillComponent(3, 100, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(600, 200), new Color(130, 130, 130), Color.White, "l3_pv1", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("l2_atk3", Dofawa_Chars.Singleton.Instance.players[0].skillTree), SkillTreeFunctions.getComponentByTag("l2_atk4", Dofawa_Chars.Singleton.Instance.players[0].skillTree), SkillTreeFunctions.getComponentByTag("l2_pv3", Dofawa_Chars.Singleton.Instance.players[0].skillTree), SkillTreeFunctions.getComponentByTag("l2_pv4", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            // Layer 4
            new ActionPointSkillComponent(5, 1, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(800, 0), new Color(130, 130, 130), Color.White, "l4_pa1", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("l3_atk1", Dofawa_Chars.Singleton.Instance.players[0].skillTree), SkillTreeFunctions.getComponentByTag("l3_pv1", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });

            // parent 2
            new LifeSkillComponent(1, 25, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(1000, 0), new Color(130, 130, 130), Color.White, "2parent", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("l4_pa1", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            // Layer 2-1
            new AttackSkillComponent(1, 5, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(1200, -300), new Color(130, 130, 130), Color.White, "2l1_atk1", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("2parent", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new AttackSkillComponent(1, 5, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(1200, -100), new Color(130, 130, 130), Color.White, "2l1_atk2", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("2parent", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new LifeSkillComponent(2, 60, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(1200, 100), new Color(130, 130, 130), Color.White, "2l1_pv1", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("2parent", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new AttackSkillComponent(1, 5, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(1200, 300), new Color(130, 130, 130), Color.White, "2l1_atk3", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("2parent", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            // Layer 2-2
            new AttackSkillComponent(2, 10, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(1400, -350), new Color(130, 130, 130), Color.White, "2l2_atk1", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("2l1_atk1", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new LifeSkillComponent(2, 60, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(1400, -250), new Color(130, 130, 130), Color.White, "2l2_pv1", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("2l1_atk1", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });

            new AttackSkillComponent(2, 10, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(1400, -150), new Color(130, 130, 130), Color.White, "2l2_atk2", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("2l1_atk2", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new LifeSkillComponent(2, 60, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(1400, -50), new Color(130, 130, 130), Color.White, "2l2_pv2", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("2l1_atk2", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });

            new AttackSkillComponent(2, 10, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(1400, 50), new Color(130, 130, 130), Color.White, "2l2_atk3", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("2l1_pv1", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new LifeSkillComponent(2, 60, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(1400, 150), new Color(130, 130, 130), Color.White, "2l2_pv3", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("2l1_pv1", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });

            new AttackSkillComponent(2, 10, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(1400, 350), new Color(130, 130, 130), Color.White, "2l2_atk4", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("2l1_atk3", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new LifeSkillComponent(2, 60, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(1400, 250), new Color(130, 130, 130), Color.White, "2l2_pv4", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("2l1_atk3", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            // Layer 2-3
            new AttackSkillComponent(3, 20, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(1600, -200), new Color(130, 130, 130), Color.White, "2l3_atk1", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("2l2_atk1", Dofawa_Chars.Singleton.Instance.players[0].skillTree), SkillTreeFunctions.getComponentByTag("2l2_atk2", Dofawa_Chars.Singleton.Instance.players[0].skillTree), SkillTreeFunctions.getComponentByTag("2l2_pv1", Dofawa_Chars.Singleton.Instance.players[0].skillTree), SkillTreeFunctions.getComponentByTag("2l2_pv2", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new LifeSkillComponent(3, 100, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(1600, 200), new Color(130, 130, 130), Color.White, "2l3_pv1", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("2l2_atk3", Dofawa_Chars.Singleton.Instance.players[0].skillTree), SkillTreeFunctions.getComponentByTag("2l2_atk4", Dofawa_Chars.Singleton.Instance.players[0].skillTree), SkillTreeFunctions.getComponentByTag("2l2_pv3", Dofawa_Chars.Singleton.Instance.players[0].skillTree), SkillTreeFunctions.getComponentByTag("2l2_pv4", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            // Layer 2-4
            new ActionPointSkillComponent(5, 1, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(1800, 0), new Color(130, 130, 130), Color.White, "2l4_pa1", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("2l3_atk1", Dofawa_Chars.Singleton.Instance.players[0].skillTree), SkillTreeFunctions.getComponentByTag("2l3_pv1", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });

            // parent 3
            new LifeSkillComponent(1, 25, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(2000, 0), new Color(130, 130, 130), Color.White, "3parent", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("2l4_pa1", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            // Layer 3-1
            new AttackSkillComponent(1, 5, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(2200, -300), new Color(130, 130, 130), Color.White, "3l1_atk1", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("3parent", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new AttackSkillComponent(1, 5, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(2200, -100), new Color(130, 130, 130), Color.White, "3l1_atk2", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("3parent", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new LifeSkillComponent(2, 60, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(2200, 100), new Color(130, 130, 130), Color.White, "3l1_pv1", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("3parent", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new AttackSkillComponent(1, 5, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(2200, 300), new Color(130, 130, 130), Color.White, "3l1_atk3", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("3parent", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            // Layer 3-2
            new AttackSkillComponent(2, 10, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(2400, -350), new Color(130, 130, 130), Color.White, "3l2_atk1", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("3l1_atk1", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new LifeSkillComponent(2, 60, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(2400, -250), new Color(130, 130, 130), Color.White, "3l2_pv1", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("3l1_atk1", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });

            new AttackSkillComponent(2, 10, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(2400, -150), new Color(130, 130, 130), Color.White, "3l2_atk2", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("3l1_atk2", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new LifeSkillComponent(2, 60, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(2400, -50), new Color(130, 130, 130), Color.White, "3l2_pv2", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("3l1_atk2", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });

            new AttackSkillComponent(2, 10, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(2400, 50), new Color(130, 130, 130), Color.White, "3l2_atk3", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("3l1_pv1", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new LifeSkillComponent(2, 60, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(2400, 150), new Color(130, 130, 130), Color.White, "3l2_pv3", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("3l1_pv1", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });

            new AttackSkillComponent(2, 10, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(2400, 350), new Color(130, 130, 130), Color.White, "3l2_atk4", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("3l1_atk3", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new LifeSkillComponent(2, 60, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(2400, 250), new Color(130, 130, 130), Color.White, "3l2_pv4", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("3l1_atk3", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            // Layer 3-3
            new AttackSkillComponent(3, 20, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(2600, -200), new Color(130, 130, 130), Color.White, "3l3_atk1", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("3l2_atk1", Dofawa_Chars.Singleton.Instance.players[0].skillTree), SkillTreeFunctions.getComponentByTag("3l2_atk2", Dofawa_Chars.Singleton.Instance.players[0].skillTree), SkillTreeFunctions.getComponentByTag("3l2_pv1", Dofawa_Chars.Singleton.Instance.players[0].skillTree), SkillTreeFunctions.getComponentByTag("3l2_pv2", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new LifeSkillComponent(3, 100, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(2600, 200), new Color(130, 130, 130), Color.White, "3l3_pv1", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("3l2_atk3", Dofawa_Chars.Singleton.Instance.players[0].skillTree), SkillTreeFunctions.getComponentByTag("3l2_atk4", Dofawa_Chars.Singleton.Instance.players[0].skillTree), SkillTreeFunctions.getComponentByTag("3l2_pv3", Dofawa_Chars.Singleton.Instance.players[0].skillTree), SkillTreeFunctions.getComponentByTag("3l2_pv4", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            // Layer 3-4
            new ActionPointSkillComponent(5, 1, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(2800, 0), new Color(130, 130, 130), Color.White, "3l4_pa1", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("3l3_atk1", Dofawa_Chars.Singleton.Instance.players[0].skillTree), SkillTreeFunctions.getComponentByTag("3l3_pv1", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });

            // double parent 4
            new XpDropSkillComponent(10, 0.5f, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(3000, -200), new Color(99, 129, 235), new Color(20, 66, 227), "4parent_xp1", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("3l4_pa1", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new LifeSkillComponent(10, 300, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(3000, 200), new Color(99, 129, 235), new Color(20, 66, 227), "3parent_pv1", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("3l4_pa1", Dofawa_Chars.Singleton.Instance.players[0].skillTree) }, "Plus de vie", "Vous fabriquez une armure en peau de blob. Bizzarement c'est très résistant. + 300 point(s) de vie");

            // Layer 4-1
            new ActionPointSkillComponent(100, 3, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(3200, 0), new Color(99, 129, 235), new Color(20, 66, 227), "4l1_pa1", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("4parent_xp1", Dofawa_Chars.Singleton.Instance.players[0].skillTree), SkillTreeFunctions.getComponentByTag("3parent_pv1", Dofawa_Chars.Singleton.Instance.players[0].skillTree) },"Panoplie du sorcier", "Vous avez ramassé toutes les potions trouvés sur les mages que vous avez tuées et appris comment les utiliser. +3 point(s) d'action.");

            // Layer 4-2
            new LifeSkillComponent(15, 500, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(3600, -900), new Color(130, 130, 130), Color.White, "4l2_pv1", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("4l1_pa1", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new LifeSkillComponent(15, 500, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(3600, -700), new Color(130, 130, 130), Color.White, "4l2_pv2", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("4l1_pa1", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new LifeSkillComponent(15, 500, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(3600, -500), new Color(130, 130, 130), Color.White, "4l2_pv3", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("4l1_pa1", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new LifeSkillComponent(15, 500, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(3600, -300), new Color(130, 130, 130), Color.White, "4l2_pv4", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("4l1_pa1", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new LifeSkillComponent(15, 500, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(3600, -100), new Color(130, 130, 130), Color.White, "4l2_pv5", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("4l1_pa1", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new LifeSkillComponent(15, 500, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(3600, 100), new Color(130, 130, 130), Color.White, "4l2_pv6", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("4l1_pa1", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new LifeSkillComponent(15, 500, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(3600, 300), new Color(130, 130, 130), Color.White, "4l2_pv7", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("4l1_pa1", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new LifeSkillComponent(15, 500, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(3600, 500), new Color(130, 130, 130), Color.White, "4l2_pv8", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("4l1_pa1", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new LifeSkillComponent(15, 500, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(3600, 700), new Color(130, 130, 130), Color.White, "4l2_pv9", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("4l1_pa1", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new LifeSkillComponent(15, 500, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(3600, 900), new Color(130, 130, 130), Color.White, "4l2_pv10", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("4l1_pa1", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });

            // Layer 4-3
            new AttackSkillComponent(15, 75, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(3800, -900), new Color(130, 130, 130), Color.White, "4l3_atk1", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("4l2_pv1", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new AttackSkillComponent(15, 75, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(3800, -700), new Color(130, 130, 130), Color.White, "4l3_atk2", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("4l2_pv2", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new AttackSkillComponent(15, 75, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(3800, -500), new Color(130, 130, 130), Color.White, "4l3_atk3", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("4l2_pv3", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new AttackSkillComponent(15, 75, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(3800, -300), new Color(130, 130, 130), Color.White, "4l3_atk4", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("4l2_pv4", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new AttackSkillComponent(15, 75, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(3800, -100), new Color(130, 130, 130), Color.White, "4l3_atk5", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("4l2_pv5", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new AttackSkillComponent(15, 75, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(3800, 100), new Color(130, 130, 130), Color.White, "4l3_atk6", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("4l2_pv6", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new AttackSkillComponent(15, 75, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(3800, 300), new Color(130, 130, 130), Color.White, "4l3_atk7", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("4l2_pv7", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new AttackSkillComponent(15, 75, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(3800, 500), new Color(130, 130, 130), Color.White, "4l3_atk8", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("4l2_pv8", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new AttackSkillComponent(15, 75, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(3800, 700), new Color(130, 130, 130), Color.White, "4l3_atk9", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("4l2_pv9", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new AttackSkillComponent(15, 75, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(3800, 900), new Color(130, 130, 130), Color.White, "4l3_atk10", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("4l2_pv10", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });

            // Layer 4-4
            new XpDropSkillComponent(15, 0.75f, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(4000, -900), new Color(99, 129, 235), new Color(20, 66, 227), "4l4_xp1", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("4l3_atk1", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new XpDropSkillComponent(15, 0.75f, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(4000, -700), new Color(99, 129, 235), new Color(20, 66, 227), "4l4_xp2", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("4l3_atk2", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new XpDropSkillComponent(15, 0.75f, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(4000, -500), new Color(99, 129, 235), new Color(20, 66, 227), "4l4_xp3", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("4l3_atk3", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new XpDropSkillComponent(15, 0.75f, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(4000, -300), new Color(99, 129, 235), new Color(20, 66, 227), "4l4_xp4", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("4l3_atk4", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new XpDropSkillComponent(15, 0.75f, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(4000, -100), new Color(99, 129, 235), new Color(20, 66, 227), "4l4_xp5", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("4l3_atk5", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new XpDropSkillComponent(15, 0.75f, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(4000, 100), new Color(99, 129, 235), new Color(20, 66, 227), "4l4_xp6", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("4l3_atk6", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new XpDropSkillComponent(15, 0.75f, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(4000, 300), new Color(99, 129, 235), new Color(20, 66, 227), "4l4_xp7", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("4l3_atk7", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new XpDropSkillComponent(15, 0.75f, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(4000, 500), new Color(99, 129, 235), new Color(20, 66, 227), "4l4_xp8", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("4l3_atk8", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new XpDropSkillComponent(15, 0.75f, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(4000, 700), new Color(99, 129, 235), new Color(20, 66, 227), "4l4_xp9", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("4l3_atk9", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new XpDropSkillComponent(15, 0.75f, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(4000, 900), new Color(99, 129, 235), new Color(20, 66, 227), "4l4_xp10", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("4l3_atk10", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });

            // Layer ultime
            Dofawa_Chars.Singleton.Instance.players[0].skillTree.IniComponent("Omniscient", "Vos compétences sont incontrôlables. + 1 pa, + 1 pm, + 1 point de vision, + 5 Facteur d'xp, + 500 D'attaque, + 3500 points de vie", 250, new Point(5000, 0), Color.DarkRed, Color.Red, SkillTreeFunctions.BuyOmniscient, "ultimelayer", new List<SkillTree.Component>() {
                SkillTreeFunctions.getComponentByTag("4l4_xp1", Dofawa_Chars.Singleton.Instance.players[0].skillTree),
                SkillTreeFunctions.getComponentByTag("4l4_xp2", Dofawa_Chars.Singleton.Instance.players[0].skillTree),
                SkillTreeFunctions.getComponentByTag("4l4_xp3", Dofawa_Chars.Singleton.Instance.players[0].skillTree),
                SkillTreeFunctions.getComponentByTag("4l4_xp4", Dofawa_Chars.Singleton.Instance.players[0].skillTree),
                SkillTreeFunctions.getComponentByTag("4l4_xp5", Dofawa_Chars.Singleton.Instance.players[0].skillTree),
                SkillTreeFunctions.getComponentByTag("4l4_xp6", Dofawa_Chars.Singleton.Instance.players[0].skillTree),
                SkillTreeFunctions.getComponentByTag("4l4_xp7", Dofawa_Chars.Singleton.Instance.players[0].skillTree),
                SkillTreeFunctions.getComponentByTag("4l4_xp8", Dofawa_Chars.Singleton.Instance.players[0].skillTree),
                SkillTreeFunctions.getComponentByTag("4l4_xp9", Dofawa_Chars.Singleton.Instance.players[0].skillTree),
                SkillTreeFunctions.getComponentByTag("4l4_xp10", Dofawa_Chars.Singleton.Instance.players[0].skillTree)}, new Point(200, 200));

            // Special Layer
            new MouvmentPointSkillComponent(25, 1, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(1800, -2000), new Color(130, 130, 130), Color.White, "special_pm1", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("3l4_pa1", Dofawa_Chars.Singleton.Instance.players[0].skillTree), SkillTreeFunctions.getComponentByTag("2l4_pa1", Dofawa_Chars.Singleton.Instance.players[0].skillTree), SkillTreeFunctions.getComponentByTag("l4_pa1", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });
            new ReachSkillComponent(35, 1, Dofawa_Chars.Singleton.Instance.players[0].skillTree, new Point(1800, -2200), new Color(130, 130, 130), Color.White, "special_reach1", new List<SkillTree.Component>() { SkillTreeFunctions.getComponentByTag("special_pm1", Dofawa_Chars.Singleton.Instance.players[0].skillTree) });

        }
        /// <summary>
        /// Initialise l'inventaire
        /// </summary>
        public static void InventoryIni()
        {
            Dofawa_Inventory.Singleton.Instance.items.Add("Casque du débutant", new Inventory.Items("Casque du débutant", 0, new Inventory.ItemStats(3, 5, 0, 0), Inventory.Items.TypeArmure.Casque));
            Dofawa_Inventory.Singleton.Instance.items.Add("Plastron du débutant", new Inventory.Items("Plastron du débutant", 0, new Inventory.ItemStats(3, 5, 0, 0), Inventory.Items.TypeArmure.Plastron));
            Dofawa_Inventory.Singleton.Instance.items.Add("Jambières du débutant", new Inventory.Items("Jambières du débutant", 0, new Inventory.ItemStats(3, 5, 0, 0), Inventory.Items.TypeArmure.Jambieres));
            Dofawa_Inventory.Singleton.Instance.items.Add("Bottes du débutant", new Inventory.Items("Bottes du débutant", 0, new Inventory.ItemStats(3, 5, 0, 0), Inventory.Items.TypeArmure.Bottes));


            //Dofawa_Inventory.Singleton.Instance.itemDropables.Add(new Inventory.Items("le cheat du dev", 0, new Inventory.ItemStats(1000, 1000,60,60), Inventory.Items.TypeArmure.Casque));


            Dofawa_Inventory.Singleton.Instance.itemDropables.Add(new Inventory.Items("Coiffe du Comte Harebourg", 15, new Inventory.ItemStats(30, 30, 0, 0), Inventory.Items.TypeArmure.Casque));
            Dofawa_Inventory.Singleton.Instance.itemDropables.Add(new Inventory.Items("Plastron du Comte Harebourg", 15, new Inventory.ItemStats(40, 30, 0, 0), Inventory.Items.TypeArmure.Plastron));
            Dofawa_Inventory.Singleton.Instance.itemDropables.Add(new Inventory.Items("Jambières du Comte Harebourg", 15, new Inventory.ItemStats(40, 30, 0, 0), Inventory.Items.TypeArmure.Jambieres));
            Dofawa_Inventory.Singleton.Instance.itemDropables.Add(new Inventory.Items("Bottes du Comte Harebourg", 15, new Inventory.ItemStats(30, 30, 0, 1), Inventory.Items.TypeArmure.Bottes));


            Dofawa_Inventory.Singleton.Instance.itemDropables.Add(new Inventory.Items("Masque de l'Esprit Salvateur", 30, new Inventory.ItemStats(80, 20, 0, 0), Inventory.Items.TypeArmure.Casque));
            Dofawa_Inventory.Singleton.Instance.itemDropables.Add(new Inventory.Items("Plastron de l'Esprit Salvateur", 30, new Inventory.ItemStats(120, 40, 0, 0), Inventory.Items.TypeArmure.Plastron));
            Dofawa_Inventory.Singleton.Instance.itemDropables.Add(new Inventory.Items("Jambières de l'Esprit Salvateur", 30, new Inventory.ItemStats(120, 40, 0, 0), Inventory.Items.TypeArmure.Jambieres));
            Dofawa_Inventory.Singleton.Instance.itemDropables.Add(new Inventory.Items("Bottes de l'Esprit Salvateur", 30, new Inventory.ItemStats(80, 20, 0, 1), Inventory.Items.TypeArmure.Bottes));


            Dofawa_Inventory.Singleton.Instance.itemDropables.Add(new Inventory.Items("Masque de l'Esprit Malsain", 45, new Inventory.ItemStats(40, 80, 0, 0), Inventory.Items.TypeArmure.Casque));
            Dofawa_Inventory.Singleton.Instance.itemDropables.Add(new Inventory.Items("Plastron de l'Esprit Malsain", 45, new Inventory.ItemStats(60, 120, 1, 0), Inventory.Items.TypeArmure.Plastron));
            Dofawa_Inventory.Singleton.Instance.itemDropables.Add(new Inventory.Items("Jambières de l'Esprit Malsain", 45, new Inventory.ItemStats(60, 120, 0, 0), Inventory.Items.TypeArmure.Jambieres));
            Dofawa_Inventory.Singleton.Instance.itemDropables.Add(new Inventory.Items("Bottes de l'Esprit Malsain", 45, new Inventory.ItemStats(40, 80, 0, 1), Inventory.Items.TypeArmure.Bottes));


            Dofawa_Inventory.Singleton.Instance.itemDropables.Add(new Inventory.Items("Masque de Koutoulou", 60, new Inventory.ItemStats(80, 80, 1, 0), Inventory.Items.TypeArmure.Casque));
            Dofawa_Inventory.Singleton.Instance.itemDropables.Add(new Inventory.Items("Plastron de Koutoulou", 60, new Inventory.ItemStats(80, 120, 1, 0), Inventory.Items.TypeArmure.Plastron));
            Dofawa_Inventory.Singleton.Instance.itemDropables.Add(new Inventory.Items("Jambières de Koutoulou", 60, new Inventory.ItemStats(120, 120, 0, 0), Inventory.Items.TypeArmure.Jambieres));
            Dofawa_Inventory.Singleton.Instance.itemDropables.Add(new Inventory.Items("Bottes de Koutoulou", 60, new Inventory.ItemStats(120, 80, 0, 1), Inventory.Items.TypeArmure.Bottes));


            Dofawa_Inventory.Singleton.Instance.itemDropables.Add(new Inventory.Items("Masque du no-life", 200, new Inventory.ItemStats(8000, 800, 100, 100), Inventory.Items.TypeArmure.Casque));
            Dofawa_Inventory.Singleton.Instance.itemDropables.Add(new Inventory.Items("Plastron du no-life", 200, new Inventory.ItemStats(8000, 800, 100, 100), Inventory.Items.TypeArmure.Plastron));
            Dofawa_Inventory.Singleton.Instance.itemDropables.Add(new Inventory.Items("Jambières du no-life", 200, new Inventory.ItemStats(8000, 800, 100, 100), Inventory.Items.TypeArmure.Jambieres));
            Dofawa_Inventory.Singleton.Instance.itemDropables.Add(new Inventory.Items("Bottes du no-life", 200, new Inventory.ItemStats(8000, 800, 100, 100), Inventory.Items.TypeArmure.Bottes));

            foreach (var item in Dofawa_Inventory.Singleton.Instance.itemDropables)
            {
                Dofawa_Inventory.Singleton.Instance.items.Add(item.nameItem, item);
            }
            /*Dofawa_Inventory.Singleton.Instance.items.Add(Dofawa_Inventory.Singleton.Instance.itemDropables[0].nameItem, Dofawa_Inventory.Singleton.Instance.itemDropables[0]);*/
            new KeyboardAction(Keys.E, InventoryFunctions.IsToDrawInventory, SceneFunctions.GetSceneByName("Game"));
        }
        /// <summary>
        /// Charge une police
        /// </summary>
        /// <param name="content">Manager de contenu</param>
        /// <param name="name">chemin</param>
        /// <returns>SpriteFont contenant la police d'écriture</returns>
        public static SpriteFont LoadFont(ContentManager content, string name)
        {
            SpriteFont font = content.Load<SpriteFont>(name);
            Dofawa_Fonts.Singleton.Instance.fonts.Add(font);
            return font;
        }
    }
}
