using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Utility;

namespace CloneRPG
{
    public enum PlayerClass
    {
        WARRIOR,
        THIEF,
        MAGE
    }

    class CPlayer
    {
        List<double> xpToLevel = new List<double>();

        public string name;
        public int mapPosX;
        public int mapPosY;

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

        public List<CItem> inventory;

        public CPlayer()
        {
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

            playerClass = PlayerClass.WARRIOR;

            calculateXPToLevel();

            inventory = new List<CItem>();
        }

        public double calculateAttack()
        {
            double hitChance = this.calculateHitChance();
            if( hitChance < 5)
            {
                return 0;
            }
            double statModifier = (this.strength * 2.0) + (this.dexterity) +  (this.intelligence*.3);
            //double weaponModifier = 

            double randomness = RandomNumberGenerator.generateRandomNumber(3, 5);

            double chance = statModifier * randomness;

            return chance;
        }

        public double calculateHitChance()
        {
            double statModifier = (this.dexterity * 3.0) + (this.strength) + (this.intelligence * .3);
            //double weaponModifier = 

            double randomness = RandomNumberGenerator.generateRandomNumber(0,5);

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
            if( xp >= xpToLevel[level] )
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
    }
}
