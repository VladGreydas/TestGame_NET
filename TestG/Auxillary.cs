using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestG
{  
    public interface IBuff
    {   
        int Apply(Player P);
        void OnTurns(int StartTurn, int CurrentTurn, Player P);
        void Reset(Player P);
    }

    [Serializable]
    public class Buff : IBuff
    {
        public enum BuffDurationType
        {
            Permanent,
            Turn_based
        }
        public enum BuffEffectType
        {
            ATK = 1,
            CRIT,
            DEF,
            GOLD,
            EXP,
            Resourse,
            Auxillary
        }
        public enum BuffTarget
        {
            Single = 1,
            Group,
            All,
        }
        public static int Duration;
        public bool IsPositive;
        public bool IsPermanent;
        public BuffEffectType buffEfType;
        public BuffTarget buffTarget;
        public Effect buffEffect;
        public string buffCode; 
        // BuffCode = {Type, Duration, EffectType, Target, Effect, Add(Turn Duration)}
        // Example: 001120 => permanent buff increasing ATK by 15pts
        public Buff()
        {

        }
        public Buff(string BuffCode)
        {
            buffCode = BuffCode;
            IsPositive = buffCode[0] == 0;
            IsPermanent = buffCode[1] == 0;
            buffEfType = (BuffEffectType)buffCode[2];
            buffTarget = (BuffTarget)buffCode[3];
            buffEffect = new Effect(buffCode[4]);
            if (IsPermanent) Duration = 0;
            else Duration = buffCode[5];
        }
        public int Apply(Player P)
        {
            if (IsPermanent)
            {
                if (buffCode[4] > 4)
                {
                    buffEffect.ChangebyValue(P, buffCode[4], 1);
                }
                else buffEffect.ChangebyPercent(P, buffCode[4], 1);
                return 0;
            }
            else
            {
                if (buffCode[4] > 4)
                {
                    buffEffect.ChangebyValue(P, buffCode[4], 1);
                }
                else buffEffect.ChangebyPercent(P, buffCode[4], 1);
                return Duration;
            }
            
        }

        public void OnTurns(int StartTurn, int CurrentTurn, Player P)
        {
            if (!IsPermanent)
            {
                if (StartTurn + Duration >= CurrentTurn)
                {
                    Reset(P);
                    return;
                }
            }
            else
            {
                return;
            }
        }

        public void Reset(Player P)
        {
            if (buffCode[4] > 4)
            {
                buffEffect.ResetbyValue(P, buffCode[4], 1);
            }
            else buffEffect.ResetbyPercent(P, buffCode[4], 1);
        }
    }

    [Serializable]
    public class Effect
    {
        public Effect(int EffectCode)
        {
            switch (EffectCode) {
                default: { break; }
            }
        }
        public void ChangebyValue(Player P, int OpCode, int TypeCode)
        {
            //TODO: Player/Enemy ATK Modifier
            switch (OpCode)
            {
                case 0: { break; } // +5
                case 1: { break; } // +10
                case 2: { break; } // +15
                case 3: { break; } // +20
                case 4: { break; } // +30
                    //...
                default:
                    break;
            }
        }
        public void ChangebyPercent(Player P, int OpCode, int TypeCode)
        {
            //TODO: Player/Enemy ATK Modifier
            switch (OpCode)
            {
                case 0: { break; } // +1%
                case 1: { break; } // +2%
                case 2: { break; } // +5%
                case 3: { break; } // +10%
                case 4: { break; } // +25%
                    //...
                default:
                    break;
            }
        }
        public void ResetbyValue(Player P, int OpCode, int TypeCode)
        {
            //TODO: Player/Enemy ATK Modifier Reset
            switch (OpCode)
            {
                case 0: { break; } // +5
                case 1: { break; } // +10
                case 2: { break; } // +15
                case 3: { break; } // +20
                case 4: { break; } // +30
                                   //...
                default:
                    break;
            }
        }
        public void ResetbyPercent(Player P, int OpCode, int TypeCode)
        {
            //TODO: Player/Enemy ATK Modifier Reset
            switch (OpCode)
            {
                case 0: { break; } // +1%
                case 1: { break; } // +2%
                case 2: { break; } // +5%
                case 3: { break; } // +10%
                case 4: { break; } // +25%
                                   //...
                default:
                    break;
            }
        }
    }

    public class Enemy
    {
        private static List<string> names = new List<string> { "Alfred", "Boris", "Max", "Stepan", "Taras", "Ivan", "Van", "Billy", "Nicola", "Thomas",
                                                            "Albertina", "Susanna", "Kate", "Halyna", "Ayanami", "Yulia", "Akeno", "Raisa", "Xenovia", "Rias" };
        private static List<string> postnames = new List<string> { "The Calm", "The Mad", "The Clever", "The Brave", "The Strongest", "The Best", "The Beautiful", "The Ideal", "The Common", "The Worst"};
        private static List<string> prefixes = new List<string> { "Soilder", "Sergeant", "General", "Baron", "Railroad Monarch", "GClan Soilder", "GClan Sergeant", "GClan General", "GClan Owner Consult", "GClan Owner" };
        public string name;
        private readonly Random random1 = new Random();
        public Train EnemyTrain { get; set; }
        public List<(Resource, int)> Inventory { get; set; }
        public int LootEXP { get; set; }
        public Enemy()
        {

        }
        public Enemy(int LVL)
        {
            name = prefixes[random1.Next(prefixes.Count)] + " " + names[random1.Next(names.Count)] + " " + postnames[random1.Next(postnames.Count)];
            EnemyTrain = new Train(LVL);           
            int i = 0;
            LootEXP = LVL*5;
            Inventory = new List<(Resource, int)>();
            int index = 0;
            while (i < random1.Next(LVL, LVL+2))
            {
                if(LVL <= 20)
                {
                    index = random1.Next(1, LVL);
                }
                else index = random1.Next(1, 20);
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
                for (int i = 0; i < Inventory.Count; i++)
                {
                    if (Inventory[i].Item1.GetResName() == Item.Item1.GetResName())
                    {
                        Item.Item2 += Inventory[i].Item2;
                        Inventory[i] = Item;
                        Item = (null, 0);
                        break;
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
    [Serializable]
    public class Item
    {
        public enum Rarity
        {
            Common = 1,
            Uncommon,
            Rare,
            Epic,
            Legendary
        }
        public bool IsActive;
        public int Price;
        public Rarity rarity;
        public List<Buff> buffs;
        public Item()
        {

        }
        public Item(string ItemID)
        {
            Random random = new Random();
            IsActive = false;
            rarity = (Rarity)ItemID[0];
            Price = random.Next((ItemID[0] - 1) * 250, ItemID[0] * 250);
            for(int i = 0; i < ItemID[0]; i++)
            {
                buffs.Add(
                    new Buff(
                        "0" 
                    + random.Next(2).ToString() 
                    + i.ToString() 
                    + "1" 
                    + (random.Next(2)*5+ItemID[0]).ToString()
                    + random.Next(1,6).ToString()));
            }
        }
    }

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
}