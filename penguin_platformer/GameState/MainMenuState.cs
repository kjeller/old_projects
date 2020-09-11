using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Platformer_Slutprojekt
{
    class MainMenuState : GameState
    {

        TileManager tileManager;

        Player player;

        string[] textButton = new string[4];

        public override void Initialize()
        {
            // Create new row for buttons
            GUI.CreateRow(
                gameStateManager.ubuntuFont,
                gameStateManager.defaultButtonTexture,
                gameStateManager.defaultHoverButtonTexture,
                new Vector2(
                    gameStateManager.Game.GraphicsDevice.Viewport.Width / 2,
                    gameStateManager.Game.GraphicsDevice.Viewport.Height / 2),
                new Vector2(500, 60),
                new Vector2(0, 20)
                );

            tileManager = new TileManager("Levels/menu.txt");
            player = new Player("Kjelle", TileManager.playerSpawnTile);

            textButton[0] = "Start New Game";
            textButton[1] = "Continue Game";
            textButton[2] = "Download more levels";
            textButton[3] = "Exit Game";


            // Adds buttons from list based on how many strings there are in array
            foreach (string text in textButton)
            {
                // Create new button
                GUI.CreateButtonInRow(text, gameStateManager.defaultButtonTexture, gameStateManager.defaultHoverButtonTexture, null);
            }

            // Uses delegate to set action in button to changestate to gameplaystate
            // Remove current gameplaystate based on type
            GUI.buttons[0].SetEvent(delegate {
                gameStateManager.Remove(new GameplayState(null, 0));
                gameStateManager.AddState(new GameplayState("Levels/level_", 0));
                gameStateManager.ChangeState(typeof(GameplayState));
            });

            // Changes back to gameplay state and game continues to run
            GUI.buttons[1].SetEvent(delegate { gameStateManager.ChangeState(typeof(GameplayState)); });

            // Resets downloadstate and adds new downloadstate which makes sure player is able to try to connect to ftp again
            // TODO: Add a retry to connect button on downloadlevelstate instead
            GUI.buttons[2].SetEvent(delegate {
                gameStateManager.Remove(new DownloadLevelState());
                gameStateManager.AddState(new DownloadLevelState()); gameStateManager.ChangeState(typeof(DownloadLevelState));
            });

            // Exit button closes application
            GUI.buttons[3].SetEvent(delegate { gameStateManager.Game.Exit(); });

            base.Initialize();
        }

        public override void LoadContent()
        {
            ContentManager Content = gameStateManager.Game.Content;

            tileManager.LoadContent(Content);
            player.LoadContent(gameStateManager.playerTexture);

            // State is now ready to update
            isLoaded = true;
        }

        public override void Update(GameTime gameTime)
        {
            // Mouse is visible
            gameStateManager.Game.IsMouseVisible = true;

            player.Update(gameTime);

            // Handle mouse input and updates mouse state
            HandleMouseInput();

            // Updates the buttons in list
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = gameStateManager.Game.Services.GetService<SpriteBatch>();

            spriteBatch.Begin();

            player.Draw(spriteBatch);

            tileManager.Draw(spriteBatch);

            GUI.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
