using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TestG
{
    [Serializable]
    public class Wagon
    {
        public string Name { get; set; }
        public int Weight { get; set; }
        public double Price { get; set; }
        public int Armor { get; set; }
        public int MaxArmor { get; set; }
        public int LVL { get; set; }
        public int UpgradeCost { get; set; }
        public List<(Resource, int)> UpgradeRequirements { get; set; }
        public Wagon()
        {

        }
        public virtual bool RequirementEquals(int LVL, Player P)
        {
            List<bool> results = new List<bool>();
            bool Res = false;
            for(int i = 0; i < UpgradeRequirements.Count; i++)
            {
                for(int j = 0; j < P.ResInventory.Count; j++)
                {
                    if(UpgradeRequirements[i].Item1.GetResName() == P.ResInventory[j].Item1.GetResName())
                    {
                        if(P.ResInventory[j].Item2 >= UpgradeRequirements[i].Item2)
                        {
                            results.Add(true);
                        }
                    }
                }
                if (results.Count == i) results.Add(false);
            }
            if(results.Count > 1)
            {
                Res = results[0];
                for(int i = 1; i < results.Count; i++)
                {
                    Res &= results[i];
                }
            }
            else
            {
                Res = results[0];
            }
            return Res;
        }
        public virtual void GetWagonStats(int index)
        {
            Console.WriteLine("Wagon No." + index.ToString() + ":");
            Console.WriteLine("-Wagon name:     " + Name + ";");
            Console.WriteLine("-Wagon LVL:      " + LVL + ";");
            Console.WriteLine("-Wagon weight:   " + Weight + ";");
            Console.WriteLine("-Wagon armor:    " + Armor + ";");
        }
        public virtual int GetSpec()
        {
            return 1;
        }
        public virtual void Modify()
        {

        }
        public virtual int Upgrade(int money, Player P)
        {
            int[] Res_Arr = { 0, 1, 9, 7, 8, 11, 13, 16, 6, 19 };
            Console.WriteLine("Upgrade " + Name + "?" + " 'Y'es/'N'o"
            + "\nUpgrade cost: " + UpgradeCost);
            foreach (var position in UpgradeRequirements)
            {
                Console.WriteLine("Resource: " + position.Item1.GetResName() + "; Amount = " + position.Item2.ToString());
            }
            string Answer = Console.ReadLine();
            if (Answer == "Y")
            {
                if (money >= UpgradeCost && RequirementEquals(LVL, P))
                {
                    money -= UpgradeCost;
                    Withdrawal(P);
                    LVL_Up();
                    Console.WriteLine("Upgrade complete! \nNow " + Name + " is LVL " + LVL);
                    Console.ReadLine();
                }
                else Console.WriteLine("Not enough money and/or resources...");
                Console.ReadLine();
            }
            return money;
        }
        public virtual void Withdrawal(Player P)
        {
            for (int i = 0; i < UpgradeRequirements.Count; i++)
            {
                for (int j = 0; j < P.ResInventory.Count; j++)
                {
                    if (UpgradeRequirements[i].Item1.GetResName() == P.ResInventory[j].Item1.GetResName())
                    {
                        if (P.ResInventory[j].Item2 >= UpgradeRequirements[i].Item2)
                        {
                            (Resource, int) Item = (null, 0);
                            Item = P.ResInventory[j];
                            Item.Item2 -= UpgradeRequirements[i].Item2;
                            P.ResInventory[j] = Item;
                            break;
                        }
                    }
                }
            }
        }
        public virtual void LVL_Up()
        {
            
            LVL++;
            UpgradeCost += 100 * LVL;
            MaxArmor += 100 * LVL;
        }
    }
    [Serializable]
    public class CargoWagon : Wagon
    {
        public int Capacity { get; set; }
        public CargoWagon()
        {

        }
        public CargoWagon(int ID)
        {
            LVL = ID;
            Name = "Cargo-1";
            UpgradeRequirements = new List<(Resource, int)>();
            UpgradeRequirements.Add((new Resource(0), 2));
            Weight = 1500;
            Price = 250;
            Armor = MaxArmor = 250;
            Capacity = 10;
            UpgradeCost = 100;
        }
        public CargoWagon(string _name, int _weight, double _price, int _armor, int _capacity)
        {
            Name = _name;
            Weight = _weight;
            Price = _price;
            Armor = MaxArmor = _armor;
            Capacity = _capacity;
            UpgradeCost = 100;
        }
        public override void GetWagonStats(int index)
        {
            base.GetWagonStats(index);
            Console.WriteLine("-Wagon Type:     Cargo");
            Console.WriteLine("-Wagon Capacity: " + Capacity.ToString() + ";");
            Console.WriteLine(" ");
        }
        public override int GetSpec()
        {
            base.GetSpec();
            return Capacity;
        }
        public override void LVL_Up()
        {
            base.LVL_Up();
            Capacity += 2 + LVL;
        }
    }
    [Serializable]
    public class WeaponWagon : Wagon
    {
        public int Weapon_slots { get; set; }
        public int Ammo_cap { get; set; }
        public List<Weapon> Weapons { get; set; }
        public WeaponWagon()
        {

        }
        public WeaponWagon(int ID)
        {
            Name = "Weapon-1";
            LVL = ID;
            UpgradeRequirements = new List<(Resource, int)>();
            UpgradeRequirements.Add((new Resource(0), 2));
            Weight = 2500;
            Price = 375;
            Armor = MaxArmor = 500;
            Weapon_slots = 1;
            Ammo_cap = 150;
            UpgradeCost = 100;
            Weapons = new List<Weapon>
            {
                new Weapon(0)
            };
        }
        public WeaponWagon(string _name, int _weight, double _price, int _armor, int _weapon_slots, int _ammo_cap)
        {
            Name = _name;
            Weight = _weight;
            Price = _price;
            Armor = MaxArmor = _armor;
            Weapon_slots = _weapon_slots;
            Ammo_cap = _ammo_cap;
            Weapons = new List<Weapon>();
            while(Weapons.Count != Weapon_slots)
            {
                Weapons.Add(new Weapon());
            }
        }

        public override void GetWagonStats(int index)
        {
            base.GetWagonStats(index);
            Console.WriteLine("-Wagon Type:     Weapon Wagon");
            Console.WriteLine("-Weapon Slots:   " + Weapon_slots + ";");
            Console.WriteLine("-Ammo Capacity:  " + Ammo_cap + ";");
            Console.WriteLine(" ");
            index = 1;
            foreach(Weapon weapon in Weapons)
            {
                if(weapon != null)
                {
                    Console.WriteLine("No." + index);
                    weapon.GetWeaponStats();
                    index++;
                }               
            }
        }

        public override int GetSpec()
        {
            base.GetSpec();
            return GetSlotIndex();
        }

        public override void LVL_Up()
        {
            base.LVL_Up();
            Ammo_cap += 50 * LVL;
            if(LVL % 3 == 0 && Weapon_slots < 5)
            {
                Weapon_slots++;
                Weapons.Add(new Weapon());
            }
        }

        public int GetSlotIndex()
        {
            int index = 0;
            if(Weapons.Count != Weapon_slots)
            {
                return Weapons.Count;
            }
            return index;
        }
    }
    [Serializable]
    public class AdditWagon : Wagon
    {
        enum AddType
        {
            RESEARCH,
            AMMOBOX,
            LIVING
        }
    }

}
