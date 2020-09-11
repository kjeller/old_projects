using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Platformer_Slutprojekt
{
    class DownloadLevelState : GameState
    {
        // Used to establish connection to ftp server
        WebRequestFTP ftpRequest;

        Texture2D btnGreen;
        Texture2D btnHoverGreen;

        SpriteFont font;

        bool IsConnected;

        public override void LoadContent()
        {
            ContentManager Content = gameStateManager.Game.Content;

            // Creates row for buttons
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

            // Gets path to appdata/Roaming directory
            string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),  "PenguinPlatformer\\CustomLevels");

            // Creates directory with penguin platformer if it doesn't exist
            Directory.CreateDirectory(dir);

            // Will contain names of every txt file local
            List<string> localFileNames = new List<string>();

            // Will contain names of every txt file remote
            List<string>ftpFileNames = new List<string>();

            // Create new request
            ftpRequest = new WebRequestFTP();

            // Establish connection and fetch names of files in ftp
            ftpFileNames = ftpRequest.EstablishConnection("localhost", "guest", "");

            // Gets names of already downloaded txt files without path
            foreach (string fileName in Directory.EnumerateFiles(dir, "*", SearchOption.AllDirectories).Select(Path.GetFileName))
            {
                // Adds filenames to list
                if (fileName.Contains(".txt") && fileName != null)
                {
                    localFileNames.Add(fileName);
                }   
            }

            if(!(ftpFileNames[0] == "No Connection to FTP"))
            {
                // Creates a button for each string in filenames
                foreach (string text in ftpFileNames)
                {
                    // Action tied to method that downloads level from ftp
                    Action action = delegate { ftpRequest.DownloadLevel(text); };
                    Texture2D defaultTexture = gameStateManager.defaultButtonTexture;
                    Texture2D hoverTexture = gameStateManager.defaultHoverButtonTexture;

                    // Compare every button text to every local file name
                    foreach (string localFileName in localFileNames)
                    {
                        // If remote and local file exists
                        if (localFileName == text)
                        {
                            defaultTexture = gameStateManager.greenButtonTexture;
                            hoverTexture = gameStateManager.greenHoverButtonTexture;

                            // Add event tied to starting downloaded level
                            action = delegate {
                                // Removes current gameplay states
                                gameStateManager.Remove(new GameplayState(null, 0));

                                // Adds new gameplaystates
                                gameStateManager.AddState(new GameplayState(Path.Combine(dir, text), -1));
                                gameStateManager.ChangeState(typeof(GameplayState));
                            };
                        }
                    }
                    // Create new buttons
                    GUI.CreateButtonInRow(text, defaultTexture, hoverTexture, action);
                }
            }
            else
            {
                GUI.CreateButton(ftpFileNames[0], gameStateManager.ubuntuFont, gameStateManager.defaultButtonTexture, gameStateManager.defaultHoverButtonTexture, new Vector2(500, 60), new Vector2(
                    gameStateManager.Game.GraphicsDevice.Viewport.Width / 2,
                    gameStateManager.Game.GraphicsDevice.Viewport.Height / 2 - 70), delegate { });
            }

            // Creates new button in row that returns to main menu
            GUI.CreateButtonInRow("Return to Menu", null, null, delegate { gameStateManager.ChangeState(typeof(MainMenuState)); });

            // State is now ready to update
            isLoaded = true;
        }


        public override void Update(GameTime gameTime)
        {
            HandleKeyboardInput();
            HandleMouseInput();

            // Returns to main menu
            if (keyboardState.IsKeyDown(Keys.Escape) && oldKeyboardState.IsKeyUp(Keys.Escape))
            {
                gameStateManager.ChangeState(typeof(MainMenuState));
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = gameStateManager.Game.Services.GetService<SpriteBatch>();

            spriteBatch.Begin();

            // Draw every button
            foreach (Button button in GUI.buttons)
            {
                button.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
