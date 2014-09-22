using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using Utility;

namespace CloneRPG
{
    public enum PlayerClass
    {
        WARRIOR,
        THIEF,
        MAGE
    }

    public enum NPCClass
    {
        SHOP,
        BOSS,
        DIALOG
    }

    class CPlayer
    {
        CModuleManager moduleManager;
        List<double> xpToLevel = new List<double>();

        // Interal data
        public double id;
        public string name;
        public int mapPosX;
        public int mapPosY;

        // Player data
        public double hp;
        public double hpMax;
        public double mp;
        public double mpMax;

        public double strength;
        public double dexterity;
        public double intelligence;

        public double xp;
        public int level;

        public PlayerClass playerClass;

        // Inventory and gear
        public List<CItem> inventory;
        public CItem weaponLeft;
        public CItem weaponRight;
        public CItem head;
        public CItem chest;
        public CItem arms;
        public CItem legs;
        public CItem fingerLeft;
        public CItem fingerRight;
        public CItem neck;

        public bool isNPC;
        public NPCClass npcType;
        public string mapSymbol;

        public Stopwatch swElapsed = null;

        // Statistics tracking
        public const int damageDealt_Melee = 0;
        public const int damageDealt_Skill = 1;
        public const int damageDealt_Magic = 2;

        public const int damageReceived_Melee = 3;
        public const int damageReceived_Skill = 4;
        public const int damageReceived_Magic = 5;

        public const int gold_Received = 6;
        public const int gold_Spent = 7;

        public const int potions_Consumed = 8;

        Dictionary<int, double> statistics_Record;
		
        public CPlayer(CModuleManager moduleManagerArg )
        {
            moduleManager = moduleManagerArg;

            // Both PCs and NPCs
            name = "";
            mapPosX = 1;
            mapPosY = 1;

            hp = 0;
            hpMax = 0;
            mp = 0;
            mpMax = 0;

            strength = 0;
            dexterity = 0;
            intelligence = 0;

            xp = 0;
            level = 1;

            calculateXPToLevel();

            inventory = new List<CItem>();
            weaponLeft = null;
            weaponRight = null;
            head = null;
            chest = null;
            arms = null;
            legs = null;
            fingerLeft = null;
            fingerRight = null;
            neck = null;

            isNPC = true;

            swElapsed = new Stopwatch();
            swElapsed.Start();

            // PCs only
            playerClass = PlayerClass.WARRIOR;

            // NPCs only
            npcType = NPCClass.DIALOG;
            mapSymbol = "D";
        }

        public double calculateAttack()
        {
            double hitChance = this.calculateHitChance();
            if (hitChance < 5)
            {
                return 0;
            }
            double statModifier = (this.strength * 2.0) + (this.dexterity) + (this.intelligence * .3);
            //double weaponModifier = 

            double randomness = (double)RandomNumberGenerator.generateRandomNumber(30, 50)/10.0;

            double chance = statModifier * randomness;

            return chance;
        }

        public double calculateHitChance()
        {
            double statModifier = (this.dexterity * 3.0) + (this.strength) + (this.intelligence * .3);
            //double weaponModifier = 

            double randomness = RandomNumberGenerator.generateRandomNumber(0, 5);

            double chance = statModifier * randomness;

            return chance;
        }

        private void calculateXPToLevel()
        {
            // Add 0's so comparison can be direct
            xpToLevel.Add(0);
            xpToLevel.Add(0);

            for (int i = 2; i < 100; i++)
            {
                xpToLevel.Add(((double)xpToLevel[xpToLevel.Count() - 1] * 1.15) + (4.0 * i));
            }
        }

        public bool checkXPForLevel()
        {
            if (xp >= xpToLevel[level])
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public double xpUntilNextLevel()
        {
            return (xpToLevel[level] - xp);
        }

        public void addItem(CItem newItem )
        {
            if( newItem != null)
            {
                inventory.Add(newItem);
            }
            else
            {
                string errorMessage = this.GetType().ToString() + ":" + System.Reflection.MethodBase.GetCurrentMethod().ToString() + ": newItem is null. Cannot continue.";
                moduleManager.Log(errorMessage);
                throw new Exception(errorMessage);
            }
        }
		  public double computeReadyTime()
        {
            // Return value in ms, make sure to multiply by 1000
            //double readyTime = ((1 / Math.Log( dexterity + 2 )) * 5);
            double readyTime = (((1 / Math.Log( dexterity + 25.0 )) * 8.0) * ((1750.0 - dexterity) / 1000.0))*1000.0;
            return readyTime;
        }

        public bool isPlayerReady()
        {

            if( swElapsed.ElapsedMilliseconds >= computeReadyTime() )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public long getPlayerElapsedTime()
        {
            return swElapsed.ElapsedMilliseconds;
        }

        public void resetPlayerTimer()
        {
            swElapsed.Reset();
            swElapsed.Start();
        }

        public bool isDead()
        {
            if( hp <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool isAlive()
        {
            return !isDead();
        }

        public bool isNPCVisible()
        {
            if( npcType == NPCClass.BOSS)
            {
                if( isAlive())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if( npcType == NPCClass.SHOP)
            {
                return true;
            }
            else if( npcType == NPCClass.DIALOG)
            {
                return true;
            }

            return false;
        }
    }
}
