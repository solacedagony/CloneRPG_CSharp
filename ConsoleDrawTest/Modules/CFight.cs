using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using Utility;
using Defines;

namespace CloneRPG
{
    class CFight : IModule
    {
        CModuleManager moduleManager;
        FightGameState fightGameState;
        InputMenuState inputMenuState;
        List<string> menuList = new List<string>();
        List<string> buffer = new List<string>();

        CPlayer player = null;
        List<CPlayer> enemyList = null;

        int playerStatsX = 0;
        int playerStatsY = 0;

        int enemyStatsX = 35;
        int enemyStatsY = 0;

        int menuX = 0;
        int menuY = 6;

        int bufferX = 0;
        int bufferY = 15;

        const int xOffset = 15;

        const int maxBufferCount = 9;

        const ConsoleKey KEY_ATTACK = ConsoleKey.D1;
        const ConsoleKey KEY_SKILL = ConsoleKey.D2;
        const ConsoleKey KEY_MAGIC = ConsoleKey.D3;
        const ConsoleKey KEY_ITEM = ConsoleKey.D4;
        const ConsoleKey KEY_RUN = ConsoleKey.D5;

        const int blocksY = 3;
        const int maxBlocks = 10;
        int playerBlocks = 0;
        List<int> enemyBlockList = null;

        enum FightGameState
        {
            FIGHT,
            VICTORY,
            DEFEAT
        }

        enum InputMenuState
        {
            ATTACKLIST,
            ENEMYLIST
        }

        public CFight( CModuleManager moduleManagerArg )
        {
            moduleManager = moduleManagerArg;

            enemyList = new List<CPlayer>();
        }

        public void draw()
        {
            // Clear screen
            Console.Clear();

            switch( fightGameState )
            {
                case FightGameState.FIGHT:
                    fight();
                    break;

                case FightGameState.VICTORY:
                    victory();
                    break;

                case FightGameState.DEFEAT:
                    defeat();
                    break;
            }
        }

        private void fight()
        {
            printPlayerStats();
            printEnemyStats();
            printInputMenu();
            printBuffer();
            updateTurnBlocks( true );

            processInput();
        }

        private void printPlayerStats()
        {
            int y = playerStatsY;

            Console.ResetColor();
            Console.SetCursorPosition( playerStatsX, y );
            Console.Write( moduleManager.player.name );
            y += 1;

            Console.SetCursorPosition( playerStatsX, y );
            Console.Write( "HP: " + (int)moduleManager.player.hp + "/" + (int)moduleManager.player.hpMax );
            y += 1;

            Console.SetCursorPosition( playerStatsX, y );
            Console.Write( "MP: " + (int)moduleManager.player.mp + "/" + (int)moduleManager.player.mpMax );
        }

        private void printEnemyStats()
        {
            Console.ResetColor();

            for( int i = 0 ; i < enemyList.Count() ; i++ )
            {
                int y = enemyStatsY;

                Console.SetCursorPosition( enemyStatsX + (xOffset * i), y );
                Console.Write( enemyList[i].name );
                y += 1;

                Console.SetCursorPosition( enemyStatsX + (xOffset * i), y );
                Console.Write( "HP: " + (int)enemyList[i].hp + "/" + (int)enemyList[i].hpMax );
                y += 1;

                Console.SetCursorPosition( enemyStatsX + (xOffset * i), y );
                Console.Write( "MP: " + (int)enemyList[i].mp + "/" + (int)enemyList[i].mpMax );
            }
        }

        private void printInputMenu()
        {
            menuList.Clear();
            if( inputMenuState == InputMenuState.ATTACKLIST )
            {
                menuList.Add( "1. Attack       " );
                menuList.Add( "2. Skill        " );
                menuList.Add( "3. Magic        " );
                menuList.Add( "4. Item         " );
                menuList.Add( "5. Run          " );
            }
            else if( inputMenuState == InputMenuState.ENEMYLIST )
            {
                for( int i = 0 ; i < 5 ; i++ )
                {
                    if( i < enemyList.Count() )
                    {
                        if( enemyList[i].isAlive() )
                        {
                            menuList.Add( (i + 1) + ": " + enemyList[i].name );
                        }
                        else
                        {
                            menuList.Add( "X: " + enemyList[i].name );
                        }
                    }
                    else
                    {
                        menuList.Add( "                " );
                    }
                }
            }

            int y = menuY;

            for( int i = 0 ; i < menuList.Count() ; i++ )
            {
                Console.SetCursorPosition( menuX, y );
                Console.Write( menuList[i] );
                y += 1;
            }
        }

        private void processInput()
        {
            bool keyPressed = false;
            while( !keyPressed )
            {
                // Check if a turn is available
                if( moduleManager.player.isPlayerReady() )
                {
                    if( Console.KeyAvailable )
                    {
                        keyPressed = true;
                    }
                }
                else
                {
                    while( Console.KeyAvailable )
                    {
                        Console.ReadKey( true );
                    }
                }

                for( int i = 0 ; i < enemyList.Count() ; i++ )
                {
                    if( enemyList[i].hp > 0 )
                    {
                        if( enemyList[i].isPlayerReady() )
                        {
                            enemyAttack( i );
                            return;
                        }
                    }
                }

                updateTurnBlocks( false );
            }

            ConsoleKeyInfo keyInfo = Console.ReadKey( true );

            if( inputMenuState == InputMenuState.ATTACKLIST )
            {
                switch( keyInfo.Key )
                {
                    case KEY_ATTACK:
                        inputMenuState = InputMenuState.ENEMYLIST;
                        printInputMenu();
                        break;
                    case KEY_SKILL:
                        break;
                    case KEY_MAGIC:
                        break;
                    case KEY_ITEM:
                        break;
                    case KEY_RUN:
                        break;
                    case ConsoleKey.Escape:
                        System.Environment.Exit( 0 );
                        break;
                    default:
                        break;
                }
            }
            else if( inputMenuState == InputMenuState.ENEMYLIST )
            {
                // Check if valid enemy chosen
                // 1 is minimum choice, should be first enemy
                int enemyIndex = keyInfo.Key - ConsoleKey.D1;
                if( enemyIndex >= 0 &&
                    enemyIndex < enemyList.Count() )
                {
                    if( enemyList[enemyIndex].isAlive() )
                    {
                        playerAttack( enemyIndex );

                        inputMenuState = InputMenuState.ATTACKLIST;
                        printInputMenu();
                    }
                }
            }
        }

        private void updateTurnBlocks( bool forceUpdate )
        {
            // Calculate player blocks
            if( moduleManager.player.computeReadyTime() == 0 )
            {
                return;
            }

            int currentPlayerBlocks = Convert.ToInt32( (moduleManager.player.getPlayerElapsedTime() / moduleManager.player.computeReadyTime()) * maxBlocks );
            if( (currentPlayerBlocks != playerBlocks) ||
                forceUpdate )
            {
                playerBlocks = currentPlayerBlocks;

                // Draw
                Console.SetCursorPosition( playerStatsX, blocksY );
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write( "[" );
                for( int blocks = 0 ; blocks < maxBlocks ; blocks++ )
                {
                    if( blocks < playerBlocks )
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write( "\x2588" );
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write( "\x2591" );
                    }
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write( "]" );
            }

            // Calculate enemy blocks
            for( int i = 0 ; i < enemyList.Count() ; i++ )
            {
                if( enemyList[i].hp > 0 )
                {
                    int currentEnemyBlocks = Convert.ToInt32( (enemyList[i].getPlayerElapsedTime() / enemyList[i].computeReadyTime()) * maxBlocks );
                    if( currentEnemyBlocks != enemyBlockList[i] ||
                        forceUpdate )
                    {
                        enemyBlockList[i] = currentEnemyBlocks;

                        // Draw
                        Console.SetCursorPosition( enemyStatsX + (xOffset * i), blocksY );
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write( "[" );
                        for( int blocks = 0 ; blocks < maxBlocks ; blocks++ )
                        {
                            if( blocks < enemyBlockList[i] )
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write( "\x2588" );
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.Write( "\x2591" );
                            }
                        }
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write( "]" );
                    }
                }
            }
        }

        private void playerAttack( int enemyIndex )
        {
            double playerAttack = moduleManager.player.calculateAttack();

            if( playerAttack > 0 )
            {
                this.appendBuffer( Colors.FGgreen + moduleManager.player.name + Colors.FGwhite + " hits " + Colors.FGred + enemyList[enemyIndex].name + Colors.FGwhite + " for " + (int)playerAttack + " damage." );
                enemyList[enemyIndex].hp -= playerAttack;
                if( enemyList[enemyIndex].isDead() )
                {
                    enemyList[enemyIndex].hp = 0;
                }
            }
            else
            {
                this.appendBuffer( Colors.FGgreen + moduleManager.player.name + Colors.FGwhite + " swings but misses." );
            }

            // Check if enemy is dead
            bool enemiesDefeated = true;
            for( int i = 0 ; i < enemyList.Count() ; i++ )
            {
                if( enemyList[i].hp > 0 )
                {
                    enemiesDefeated = false;
                    break;
                }
            }

            if( enemiesDefeated )
            {
                fightGameState = FightGameState.VICTORY;
            }

            moduleManager.player.resetPlayerTimer();
            printBuffer();
        }

        private void enemyAttack( int enemyIndex )
        {
            double enemyAttack = enemyList[enemyIndex].calculateAttack();

            if( enemyAttack > 0 )
            {
                this.appendBuffer( Colors.FGred + enemyList[enemyIndex].name + Colors.FGwhite + " hits " + Colors.FGgreen + moduleManager.player.name + Colors.FGwhite + " for " + (int)enemyAttack + " damage." );
                moduleManager.player.hp -= enemyAttack;
            }
            else
            {
                this.appendBuffer( Colors.FGred + enemyList[enemyIndex].name + Colors.FGwhite + " swings but misses." );
            }

            if( moduleManager.player.hp <= 0 )
            {
                fightGameState = FightGameState.DEFEAT;
            }

            enemyList[enemyIndex].resetPlayerTimer();
            printBuffer();
        }

        private void printBuffer()
        {
            Console.ForegroundColor = ConsoleColor.White;
            for( int i = 0 ; i < buffer.Count ; i++ )
            {
                Console.SetCursorPosition( bufferX, bufferY + i );
                Utility.StringColorizer.colorizeString( buffer[i] );
            }
        }

        private void appendBuffer( string data )
        {
            if( buffer.Count() >= maxBufferCount )
            {
                buffer.RemoveAt( buffer.Count() - 1 );
            }

            buffer.Insert( 0, data );
        }

        private void victory()
        {
            // Print fight information
            int y = 0;
            Console.SetCursorPosition( 0, y );
            Console.Write( "You were victorious!" );
            y += 2;

            // Gain XP from fight
            Console.SetCursorPosition( 0, y );
            Console.Write( "XP: " + moduleManager.player.xp + " -> " );

            for( int i = 0 ; i < enemyList.Count() ; i++ )
            {
                moduleManager.player.xp += enemyList[i].xp;
            }
            Console.WriteLine( moduleManager.player.xp );
            y += 2;

            // Wait for button press
            Utility.Interaction.pressAnyKeyToContinue( 0, y );

            // Reset fight variables
            resetFightScene();

            // Check if level, otherwise go to map
            if( moduleManager.player.checkXPForLevel() )
            {
                moduleManager.switchModule( CModuleManager.ModuleType.Level );
            }
            else
            {
                moduleManager.switchModule( CModuleManager.ModuleType.Map );
            }
        }

        private void defeat()
        {
            int y = 0;
            Console.SetCursorPosition( 0, y );
            Console.Write( "You were defeated..." );
            y += 2;

            Utility.Interaction.pressAnyKeyToContinue( 0, y );

            // TODO: Death state clean up?

            // Reset fight variables
            resetFightScene();

            // Reset player
            moduleManager.player = null;

            moduleManager.switchModule( CModuleManager.ModuleType.MainMenu );
        }

        public void setEnemy( CPlayer enemyArg )
        {

        }

        private void resetFightScene()
        {
            // Reset fight variables
            fightGameState = FightGameState.FIGHT;
            buffer.Clear();
        }

        public void initialize()
        {
            moduleManager.player.resetPlayerTimer();

            fightGameState = FightGameState.FIGHT;
            inputMenuState = InputMenuState.ATTACKLIST;

            enemyList.Clear();
            CPlayer enemy = new CPlayer( moduleManager );
            enemy.name = "Orc1";
            enemy.strength = 1;
            enemy.dexterity = 1;
            enemy.intelligence = 1;
            enemy.hp = 100;
            enemy.hpMax = enemy.hp;
            enemy.mp = 0;
            enemy.mpMax = enemy.mp;
            enemy.level = 1;
            enemy.id = 0;
            enemy.isNPC = true;
            enemy.xp = 5;
            enemy.resetPlayerTimer();
            enemyList.Add( enemy );

            enemy = new CPlayer( moduleManager );
            enemy.name = "Orc2";
            enemy.strength = 1;
            enemy.dexterity = 1;
            enemy.intelligence = 1;
            enemy.hp = 100;
            enemy.hpMax = enemy.hp;
            enemy.mp = 0;
            enemy.mpMax = enemy.mp;
            enemy.level = 1;
            enemy.id = 0;
            enemy.isNPC = true;
            enemy.xp = 5;
            enemy.resetPlayerTimer();
            enemyList.Add( enemy );

            enemy = new CPlayer( moduleManager );
            enemy.name = "Orc3";
            enemy.strength = 1;
            enemy.dexterity = 1;
            enemy.intelligence = 1;
            enemy.hp = 100;
            enemy.hpMax = enemy.hp;
            enemy.mp = 0;
            enemy.mpMax = enemy.mp;
            enemy.level = 1;
            enemy.id = 0;
            enemy.isNPC = true;
            enemy.xp = 5;
            enemy.resetPlayerTimer();
            enemyList.Add( enemy );

            enemy = null;

            enemyBlockList = new List<int>();

            // Add block lists for each enemy
            for( int i = 0 ; i < enemyList.Count() ; i++ )
            {
                enemyBlockList.Add( -1 );
                enemyList[i].resetPlayerTimer();
            }
        }

        public void destroy()
        {

        }
    }
}
