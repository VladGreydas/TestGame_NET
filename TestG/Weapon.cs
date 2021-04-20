using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestG
{
    [Serializable]
    public class Weapon
    {
        public enum WeaponType
        {
            MACHINE_GUN = 0,
            LIGHT_CANNON,
            HEAVY_CANNON,
            ROCKETS,
            FLAMETHROWER,
            MORTAR,
            LASER
        }
        public string Name { get; set; }
        public int DMG { get; set; }
        public WeaponType Weapon_Type { get; set; }
        public int Price { get; set; }
        //private int LVL;
        public int UpgradeCost { get; set; }
        public Weapon()
        {

        }
        public Weapon(int ID)
        {
            Name = "Milly-1";
            //LVL = 0;
            DMG = 100;
            Weapon_Type = WeaponType.MACHINE_GUN;
            Price = 250;
            UpgradeCost = 100;
        }
        public Weapon(string name, int dmg, WeaponType type, int price)
        {
            //LVL = 0;
            Name = name;
            DMG = dmg;
            Weapon_Type = type;
            Price = price;
        }
        public void GetWeaponStats()
        {
            Console.WriteLine("--Weapon Name:   " + Name + ";");
            //Console.WriteLine("--Weapon LVL:    " + LVL + ";");
            Console.WriteLine("--Weapon Type:   " + Weapon_Type + ";");
            Console.WriteLine("--Weapon Damage: " + DMG + ";");
            Console.WriteLine(" ");
        }
        //public void LVL_Up()
        //{
        //    LVL++;
        //    DMG += 25 * LVL;
        //    UpgradeCost += 100 * LVL;
        //}
        //public int Upgrade(int money)
        //{
        //    Console.WriteLine("Upgrade " + Name + "?" + " 'Y'es/'N'o"
        //    + "\nUpgrade cost: " + UpgradeCost);
        //    string Answer = Console.ReadLine();
        //    if (Answer == "Y")
        //    {
        //        if (money >= UpgradeCost)
        //        {
        //            money -= UpgradeCost;
        //            LVL_Up();
        //            Console.WriteLine("Upgrade complete! \nNow " + Name + " is LVL " + LVL);
        //            Console.ReadLine();
        //        }
        //        else Console.WriteLine("Not enough money...");
        //        Console.ReadLine();
        //    }
        //    return money;
        //}
    }
}
