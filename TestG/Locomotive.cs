using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TestG
{
    public class Locomotive
    {
        public enum TypeFuel
        {
            Coal,
            Diesel,
            Electricity,
            Uranium
        }
        public string Name { get; set; }
        public int Weight { get; set; }
        public int Power { get; set; }
        public int Armor { get; set; }
        public int MaxArmor { get; set; }
        public TypeFuel Type_Fuel { get; set; }
        public int Fuel { get; set; }
        public int Fuelcap { get; set; }
        public int Price { get; set; }
        public int LVL { get; set; }
        public int UpgradeCost { get; set; }
        public Locomotive()
        {

        }
        public Locomotive(int ID)
        {
            Name = "Steamy Joe";
            Weight = 3500;
            Power = 250;
            Armor = MaxArmor = 500;
            Type_Fuel = TypeFuel.Coal;
            Fuel = 10;
            Fuelcap = 10;
            Price = 500;
            LVL = ID;
            UpgradeCost = 100;
        }
        public Locomotive(string _name, int _weight, int _power, int _armor, TypeFuel fuel, int _fuelcap, int _price)
        {
            LVL = 0;
            Name = _name;
            Weight = _weight;
            Power = _power;
            Armor = MaxArmor = _armor;
            Type_Fuel = fuel;
            Fuelcap = _fuelcap;
            Price = _price;
            UpgradeCost = 100;
        }
        public void GetLocoStats()
        {
            Console.WriteLine("Locomotive:");
            Console.WriteLine("-Name:           " + Name + ";");
            Console.WriteLine("-LVL:            " + LVL + ";");
            Console.WriteLine("-Weight:         " + Weight + ";");
            Console.WriteLine("-Power:          " + Power + ";");
            Console.WriteLine("-Armor:          " + Armor + ";");
            Console.WriteLine("-Fuel Type:      " + Type_Fuel + ";");
            Console.WriteLine("-Fuel :          " + Fuel + "/" + Fuelcap + ";");
            Console.WriteLine(" ");
        }
        public void LVL_Up()
        {
            LVL++;
            Power += 50 * LVL;
            MaxArmor += 100 * LVL;
            Fuelcap += 2 * LVL;
            UpgradeCost += 100 * LVL;
        }

        public int Upgrade(int money)
        {
            Console.WriteLine("Upgrade " + Name + "?" + " 'Y'es/'N'o"
            + "\nUpgrade cost: " + UpgradeCost);
            string Answer = Console.ReadLine();
            if(Answer == "Y")
            {
                if(money >= UpgradeCost)
                {
                    money -= UpgradeCost;
                    LVL_Up();
                    Console.WriteLine("Upgrade complete! \nNow " + Name + " is LVL " + LVL);
                    Console.ReadLine();
                }
                else Console.WriteLine("Not enough money...");
                Console.ReadLine();
            }
            return money;
        }
    }
}
