using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

using Utility;

namespace CloneRPG
{
    class CModuleManager
    {
        public enum ModuleType
        {
            MainMenu,
            NewGame,
            LoadGame,
            Map,
            Fight,
            Level,
            Exit
        };

        public const string gameName = "CloneRPG";
        public const int versionMajor = 1;
        public const int versionMinor = 1;

        private readonly string logFilename;
        private const string logDirectory = "Logs";
        private const string playersDirectory = "Players";
        private const string itemDirectory = "Items";
        private const string NPCDirectory = "NPCs";
        private const string areaDirectory = "Areas";

        // Console constants
        const int consoleWidth = 100;
        const int consoleHeight = 40;

        private ModuleType currentModule;
        private IntPtr consoleHandle;

        // Modules
        CMainMenu mainMenu = null;
        CNewGame newGame = null;
        CLoadGame loadGame = null;
        public CMap map = null;
        public CFight fight = null;
        CLevel level = null;

        // Player specific
        public CPlayer player = null;

        // Game elements
        List<CItem> items = null;
        List<CPlayer> npcs = null;

        // Module
        CModule currentModule;

        // Constructor
        public CModuleManager()
        {
            currentModule = ModuleType.MainMenu;

            // Setup modules
            mainMenu = new CMainMenu(this);
            newGame = new CNewGame(this);
            loadGame = new CLoadGame(this);
            map = new CMap(this);
            fight = new CFight(this);
            level = new CLevel(this);

            // Get window handle
            this.consoleHandle = Process.GetCurrentProcess().MainWindowHandle;

            // Setup console
            Console.Title = CModuleManager.gameName + " v" + CModuleManager.versionMajor + "." + CModuleManager.versionMinor;
            Console.ResetColor();
            Console.Clear();
            Console.SetWindowSize(CModuleManager.consoleWidth, CModuleManager.consoleHeight);
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            // Setup log filename
            logFilename = gameName + "_" + DateTime.Now.Hour + "." + DateTime.Now.Minute + "." + DateTime.Now.Second + ".txt";

            // Create directory if it doesn't exist
            Directory.CreateDirectory(logDirectory);
            Directory.CreateDirectory(playersDirectory);
            Directory.CreateDirectory(itemDirectory);
            Directory.CreateDirectory(NPCDirectory);

            // Load all weapons/armor/monsters/etc from file
            Utility.FileIO.loadItems(ref items, itemDirectory);
            Utility.FileIO.loadNPCs(ref npcs, NPCDirectory);
        }

        public void mainLoop()
        {
            Log("Starting " + gameName + " v" + versionMajor + "." + versionMinor);

            while (!currentModule.Equals(ModuleType.Exit))
            {
                this.runCurrentModule();
            }
            Log("Exiting.");
            Log("");
        }

        // Run each loop
        private void runCurrentModule()
        {
            if (currentModule.Equals(ModuleType.MainMenu))
            {
                // Run main menu function
                mainMenu.draw();
            }
            else if (currentModule.Equals(ModuleType.NewGame))
            {
                // Run main menu function
                newGame.draw();
            }
            else if (currentModule.Equals(ModuleType.LoadGame))
            {
                loadGame.draw();
            }
            else if (currentModule.Equals(ModuleType.Map))
            {
                map.draw();
            }
            else if (currentModule.Equals(ModuleType.Fight))
            {
                fight.draw();
            }
            else if (currentModule.Equals(ModuleType.Level))
            {
                level.draw();
            }
            else if (currentModule.Equals(ModuleType.Exit))
            {
                // Exit game
            }
        }

        public void switchModule(ModuleType newModule)
        {
            this.currentModule = newModule;
        }

        public void Log(string data)
        {
            using (StreamWriter file = File.AppendText("Logs/" + logFilename))
            {
                string output = DateTime.Now.ToString() + ": " + data;
                file.WriteLine(output);
            }
        }
    }
}
