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
            HelpMap,
            InventoryMap,
            InventoryFight,
            Shop,
            Fight,
            Level,
            Exit
        };

        public const string gameName = "CloneRPG";
        public const int versionMajor = 1;
        public const int versionMinor = 2;

        private readonly string logFilename;
        private const string logDirectory = "Logs";
        private const string playersDirectory = "Players";
        private const string itemDirectory = "Items";
        private const string NPCDirectory = "NPCs";
        private const string areaDirectory = "Areas";

        // Console constants
        const int consoleWidth = 100;
        const int consoleHeight = 40;

        private IntPtr consoleHandle;
        private bool exitGame;

        // Modules
        CMainMenu mainMenu = null;
        CNewGame newGame = null;
        CLoadGame loadGame = null;
        public CMap map = null;
        public CFight fight = null;
        CLevel level = null;
        CInventoryMap inventoryMap = null;
        CHelpMap helpMap = null;
        CShop shop = null;

        // Player specific
        public CPlayer player = null;

        // Game elements
        public CItemManager itemManager = null;
        List<CPlayer> npcs = null;

        private IModule currentModule = null;

        // Constructor
        public CModuleManager()
        {
            // Setup modules
            mainMenu = new CMainMenu(this);
            newGame = new CNewGame(this);
            loadGame = new CLoadGame(this);
            map = new CMap(this);
            fight = new CFight(this);
            level = new CLevel(this);
            inventoryMap = new CInventoryMap(this);
            helpMap = new CHelpMap(this);
            shop = new CShop(this);

            player = new CPlayer(this);
            itemManager = new CItemManager(this);

            // Initialize currentModule
            currentModule = mainMenu;
            currentModule.initialize();

            // Get window handle
            consoleHandle = Process.GetCurrentProcess().MainWindowHandle;
            
            // Initialize exitGame
            exitGame = false;

            // Setup console
            Console.Title = CModuleManager.gameName + " v" + CModuleManager.versionMajor + "." + CModuleManager.versionMinor;
            Console.ResetColor();
            Console.Clear();
            Console.SetWindowSize(CModuleManager.consoleWidth, CModuleManager.consoleHeight);
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            // Setup log filename
            logFilename = gameName + "_" + DateTime.Now.Hour + "." + DateTime.Now.Minute + "." + DateTime.Now.Second + ".txt";

            // Create directory if it doesn't exist so that files can be read/saved from these locations
            Directory.CreateDirectory(logDirectory);
            Directory.CreateDirectory(playersDirectory);
            Directory.CreateDirectory(itemDirectory);
            Directory.CreateDirectory(NPCDirectory);

            // Load all weapons/armor/monsters/etc from file
            Utility.FileIO.loadNPCs(ref npcs, NPCDirectory);
            if( itemManager.loadItems(itemDirectory))
            {
                Log("Successfully loaded items from: " + itemDirectory);
            }
            else
            {
                string errorMessage = this.GetType().ToString() + ":" + System.Reflection.MethodBase.GetCurrentMethod().ToString() + ": Failed to load items. Cannot continue.";
                Log(errorMessage);
                throw new Exception(errorMessage);
            }
        }

        public void mainLoop()
        {
            Log("Starting " + gameName + " v" + versionMajor + "." + versionMinor);

            while (!exitGame)
            {
                this.runCurrentModule();
            }

            Log("Exiting.");
            Log("");
        }

        // Run each loop
        private void runCurrentModule()
        {
            if (currentModule != null)
            {
                currentModule.draw();
            }
            else
            {
                string errorMessage = this.GetType().ToString() + ":" + System.Reflection.MethodBase.GetCurrentMethod().ToString() + ": Current module is null. Cannot continue.";
                Log(errorMessage);
                throw new Exception(errorMessage);
            }
        }

        public void switchModule(ModuleType newModule)
        {
            currentModule.destroy();

            if( newModule.Equals(ModuleType.MainMenu))
            {
                currentModule = mainMenu;
            }
            else if (newModule.Equals(ModuleType.NewGame))
            {
                currentModule = newGame;
            }
            else if (newModule.Equals(ModuleType.LoadGame))
            {
                currentModule = loadGame;
            }
            else if (newModule.Equals(ModuleType.Map))
            {
                currentModule = map;
            }
            else if (newModule.Equals(ModuleType.Fight))
            {
                currentModule = fight;
            }
            else if (newModule.Equals(ModuleType.Level))
            {
                currentModule = level;
            }
            else if (newModule.Equals(ModuleType.InventoryMap))
            {
                currentModule = inventoryMap;
            }
            else if (newModule.Equals(ModuleType.HelpMap))
            {
                currentModule = helpMap;
            }
            else if (newModule.Equals(ModuleType.InventoryFight))
            {
                //currentModule = inventoryFight;
            }
            else if( newModule.Equals(ModuleType.Exit))
            {
                exitGame = true;
            }
            else
            {
                // Invalid game module to switch to
                string errorMessage = this.GetType().ToString() + ":" + System.Reflection.MethodBase.GetCurrentMethod().ToString() + ": Received undefined type.";
                Log(errorMessage);
                throw new Exception(errorMessage);
            }

            // Check for a null module 
            if (currentModule != null)
            {
                currentModule.initialize();
            }
            else
            {
                string errorMessage = this.GetType().ToString() + ":" + System.Reflection.MethodBase.GetCurrentMethod().ToString() + ": Current module is null. Cannot continue.";
                Log(errorMessage);
                throw new Exception(errorMessage);
            }
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
