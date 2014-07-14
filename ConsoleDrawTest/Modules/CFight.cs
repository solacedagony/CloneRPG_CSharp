using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Utility;

namespace CloneRPG
{
    class CFight
    {
        CModuleManager moduleManager;
        FightGameState fightGameState;

        List<string> buffer = new List<string>();

        CPlayer enemy;

        int playerStatsX = 0;
        int playerStatsY = 0;

        int enemyStatsX = 20;
        int enemyStatsY = 0;

        int menuX = 0;
        int menuY = 6;

        int inputX = 0;
        int inputY = 13;

        int bufferX = 0;
        int bufferY = 15;

        const int maxBufferCount = 9;

        const ConsoleKey KEY_ATTACK = ConsoleKey.D1;
        const ConsoleKey KEY_SKILL = ConsoleKey.D2;
        const ConsoleKey KEY_MAGIC = ConsoleKey.D3;
        const ConsoleKey KEY_ITEM = ConsoleKey.D4;
        const ConsoleKey KEY_RUN = ConsoleKey.D5;

        enum FightGameState
        {
            FIGHT,
            VICTORY,
            DEFEAT
        }

        public CFight(CModuleManager moduleManagerArg)
        {
            moduleManager = moduleManagerArg;
            fightGameState = FightGameState.FIGHT;
        }

        public void draw()
        {
            // Clear screen
            Console.Clear();

            switch(fightGameState)
            {
                case FightGameState.FIGHT:
                    {
                        fight();
                    }
                    break;

                case FightGameState.VICTORY:
                    {
                        victory();
                    }
                    break;

                case FightGameState.DEFEAT:
                    {
                        defeat();
                    }
                    break;
            }
            
        }

        private void fight()
        {
            printPlayerStats();
            printEnemyStats(enemy);
            printStaticText();
            printBuffer();

            processInput(); 
        }

        private void printPlayerStats()
        {
            int y = playerStatsY;

            Console.SetCursorPosition(playerStatsX, y);
            Console.Write(moduleManager.player.name);
            y += 1;

            Console.SetCursorPosition(playerStatsX, y);
            Console.Write("HP: " + (int)moduleManager.player.hp + "/" + (int)moduleManager.player.hpMax);
            y += 1;

            Console.SetCursorPosition(playerStatsX, y);
            Console.Write("HP: " + (int)moduleManager.player.mp + "/" + (int)moduleManager.player.mpMax);
        }

        private void printEnemyStats(CPlayer enemy)
        {
            if (enemy == null)
            {
                return;
            }
            else
            {
                int y = enemyStatsY;

                Console.SetCursorPosition(enemyStatsX, y);
                Console.Write(enemy.name);
                y += 1;

                Console.SetCursorPosition(enemyStatsX, y);
                Console.Write("HP: " + (int)enemy.hp + "/" + (int)enemy.hpMax);
                y += 1;

                Console.SetCursorPosition(enemyStatsX, y);
                Console.Write("HP: " + (int)enemy.mp + "/" + (int)enemy.mpMax);
            }
        }

        private void printStaticText()
        {
            List<string> menuList = new List<string>();
            menuList.Add("1. Attack");
            menuList.Add("2. Skill");
            menuList.Add("3. Magic");
            menuList.Add("4. Item");
            menuList.Add("5. Run");

            int y = menuY;

            for (int i = 0; i < menuList.Count(); i++)
            {
                Console.SetCursorPosition(menuX, y);
                Console.Write(menuList[i]);
                y += 1;
            }

            // TODO: Create box around buffer
        }

        private void processInput()
        {
            Console.SetCursorPosition(inputX, inputY);
            Console.Write("Input: ");

            ConsoleKeyInfo keyInfo = Console.ReadKey(false);

            switch (keyInfo.Key)
            {
                case KEY_ATTACK:
                    {
                        // Attack
                        fightAttack();
                    }
                    break;
                case KEY_SKILL:
                    {
                        // Skill
                    }
                    break;
                case KEY_MAGIC:
                    {
                        // Magic
                    }
                    break;
                case KEY_ITEM:
                    {
                        // Item
                    }
                    break;
                case KEY_RUN:
                    {
                        // Run
                    }
                    break;
            }
        }

        private void fightAttack()
        {
            double playerAttack = moduleManager.player.calculateAttack();
            double enemyAttack = enemy.calculateAttack();

            if (playerAttack > 0)
            {
                this.appendBuffer(moduleManager.player.name + " hits " + enemy.name + " for " + playerAttack + " damage.");
                enemy.hp -= playerAttack;
            }
            else
            {
                this.appendBuffer(moduleManager.player.name + " swings but misses.");
            }

            // Check if enemy is dead
            if( enemy.hp > 0)
            {
                if (enemyAttack > 0)
                {
                    this.appendBuffer(enemy.name + " hits " + moduleManager.player.name + " for " + enemyAttack + " damage.");
                    moduleManager.player.hp -= enemyAttack;
                }
                else
                {
                    this.appendBuffer(enemy.name + " swings but misses.");
                }

                this.appendBuffer("");
            }
            else
            {
                fightGameState = FightGameState.VICTORY;
            }

            if( moduleManager.player.hp <= 0)
            {
                fightGameState = FightGameState.DEFEAT;
            }
        }

        private void printBuffer()
        {
            for (int i = 0; i < buffer.Count; i++)
            {
                Console.SetCursorPosition(bufferX, bufferY + i);
                Console.Write(buffer[i]);
            }
        }

        private void appendBuffer( string data )
        {
            if( buffer.Count() >= maxBufferCount)
            {
                buffer.RemoveAt(buffer.Count()-1);
            }

            buffer.Insert(0,data);
        }

        private void victory()
        {
            // Print fight information
            int y = 0;
            Console.SetCursorPosition(0, y);
            Console.Write("You were victorious!");
            y += 2;

            // Gain XP from fight
            Console.SetCursorPosition(0, y);
            Console.Write("XP: " + moduleManager.player.xp + " -> ");
            moduleManager.player.xp += enemy.xp;
            Console.WriteLine(moduleManager.player.xp);
            y+= 2;

            // Wait for button press
            Utility.Interaction.pressAnyKeyToContinue(0, y);

            // Reset fight variables
            resetFightScene();

            // Check if level, otherwise go to map
            if( moduleManager.player.checkXPForLevel() )
            {
                moduleManager.switchModule(CModuleManager.ModuleType.Level);
            }
            else
            {
                moduleManager.switchModule(CModuleManager.ModuleType.Map);
            }
        }

        private void defeat()
        {
            int y = 0;
            Console.SetCursorPosition(0, y);
            Console.Write("You were defeated...");
            y += 2;

            Utility.Interaction.pressAnyKeyToContinue(0, y);
           
            // TODO: Death state clean up?
            
            // Reset fight variables
            resetFightScene();

            // Reset player
            moduleManager.player = null;

            moduleManager.switchModule(CModuleManager.ModuleType.MainMenu);
        }

        public void setEnemy(CPlayer enemyArg)
        {
            this.enemy = enemyArg;
        }

        private void resetFightScene()
        {
            // Reset fight variables
            fightGameState = FightGameState.FIGHT;
            buffer.Clear();
            enemy = null;
        }
    }
}
