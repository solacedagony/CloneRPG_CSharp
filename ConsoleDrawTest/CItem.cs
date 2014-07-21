using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloneRPG
{
    class CItem
    {
        public enum ItemType
        {
            NONE,
            QUEST,
            CONSUMABLE,
            WEAPON,
            ARMOR
        };

        public enum EquipablePosition
        {
            NONE,
            HEAD,
            CHEST,
            ARMS,
            LEGS,
            FINGER,
            NECK
        };

        public enum DamageType
        {
            NONE,
            BLUNT,
            PIERCE,
            MAGIC,
            EXPLOSIVE
        };

        // Properties all items must 
        public string name = "";
        public int cost = -1;
        public ItemType itemType = ItemType.NONE;
        public int quantity = 0;

        // Weapon/armor
        public EquipablePosition equipablePosition = EquipablePosition.NONE;
        public double armor = -1;
        public double strengthModifier = -1;
        public double dexterityModifier = -1;
        public double intelligenceModifier = -1;
        public double absorb = -1;
        public DamageType damageType = DamageType.NONE;
        public double damageMin = -1;
        public double damageMax = -1;

        // Potions
        public double hpModifier = -1;
        public double mpModifier = -1;
        public double damageModifier = -1;
        public double armorModifier = -1;

        // Quest Item
        public int questID = -1;

        public void use()
        {

        }
    }
}
