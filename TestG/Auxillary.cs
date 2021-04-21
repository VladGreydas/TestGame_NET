using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestG
{
    [Serializable]
    public class Resource
    {
        public enum Resource_Type
        {
            WOOD = 0,
            STONE,
            COAL,
            IRON_ORE,
            COPPER_ORE,
            OIL_BARREL,
            URANIUM_ORE,
            IRON_PLATE,
            COPPER_PLATE,
            STONE_BRICK,
            LEAD_ORE,
            LEAD_PLATE,
            WATER_BARREL,
            ELECTRONIC_CIRCUIT,
            GASOLINE,
            DIESEL_BARREL,
            PLASTIC_BAR,
            METAL_SCRAP,
            TEXTILE,
            GOLD_BAR
        }
        public int ID;      //Resource       0   1   2   3   4   5  6   7   8   9   10  11  12  13   14  15  16  17  18  19
        public static float[] res_price = { 20, 30, 50, 30, 30, 50, 60, 50, 50, 40, 30, 50, 10, 100, 80, 80, 60, 20, 40, 1000 };
        public float Price { get; set; }
        public Resource_Type Res { get; set; }
        public Resource()
        {

        }
        public Resource(int index)
        {
            ID = index;
            Array value = Enum.GetValues(typeof(Resource_Type));
            Res = (Resource_Type)value.GetValue(index);
            Price = res_price[index];
        }
        public string GetResName()
        {
            return Res.ToString();
        }
        ~Resource()
        {

        }
    }

    public class Enemy
    {
        private readonly Random random1 = new Random();
        public Train EnemyTrain { get; set; }
        public List<(Resource, int)> Inventory { get; set; }
        public int LootEXP { get; set; }
        public Enemy()
        {
            EnemyTrain = new Train(1);           
            int i = 0;
            LootEXP = random1.Next(10, 101);
            Inventory = new List<(Resource, int)>();
            while (i < random1.Next(5, 16))
            {
                int index = random1.Next(1, 11);
                Add_Res(index);
                i++;
            }
        }
        public void Add_Res(int index)
        {
            int amount = random1.Next(1, 11);
            (Resource, int) Item = ((new Resource(index), amount));
            if (Inventory.Count < EnemyTrain.GetTotalCapacity())
            {
                foreach ((Resource, int) position in Inventory)
                {
                    if (position.Item1.GetResName() == Item.Item1.GetResName())
                    {
                        int Iindex = Inventory.BinarySearch(position);
                        Item.Item2 += Inventory[Iindex].Item2;
                        Inventory[index] = Item;
                        Item = (null, 0);
                    }
                }
                if (Item != (null, 0))
                {
                    Inventory.Add((new Resource(index), amount));
                }
            }
        }
        public void Attack(Train PlayerTrain) 
        {
            int AttackIndex = FindWeakPoint(PlayerTrain);
            List<(WeaponWagon, int)> CurrentWeaponWagons = EnemyTrain.GetWeaponWagons();
            int TotalAttack = 0;
            foreach ((WeaponWagon, int) wagon in CurrentWeaponWagons)
            {
                foreach(Weapon w in wagon.Item1.Weapons)
                {
                    TotalAttack += w.DMG;
                }
            }
            PlayerTrain.Wagons[AttackIndex].Armor -= TotalAttack;
            CheckDestroyedWagons();
        }
        public void CheckDestroyedWagons()
        {
            for(int i = 0; i < EnemyTrain.Wagons.Count; i++)
            {
                if(EnemyTrain.Wagons[i].Armor <= 0)
                {
                    EnemyTrain.Wagons.RemoveAt(i);
                }
            }
        }
        public int FindWeakPoint(Train T)
        {
            int WeakPointIndex = 0;
            int WeakArmor = 10000;
            for(int i = 0; i < T.Wagons.Count; i++)
            {
                if (T.Wagons[i].Armor < WeakArmor)
                {
                    WeakArmor = T.Wagons[i].Armor;
                    WeakPointIndex = i;
                }
            }
            return WeakPointIndex;
        }
        ~Enemy()
        {

        }
    }
}