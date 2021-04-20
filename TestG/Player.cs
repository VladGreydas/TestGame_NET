using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TestG
{
    [Serializable]
    public class Player
    {
        private readonly Random random1 = new Random();
        public string Name { get; set; }
        public int Money { get; set; }
        public int EXP { get; set; }
        public int MaxEXP { get; set; }
        public int LVL { get; set; }
        public Train T { get; set; }
        public Town[] Towns { get; set; }
        public Town SelectedTown { get; set; }
        public List<(Resource, int)> Inventory { get; set; }
        public Player()
        {

        }
        public Player(string nickname)
        {
            Inventory = new List<(Resource, int)>();
            Name = nickname;
            Money = 500;
            EXP = 0;
            MaxEXP = 100;
            LVL = 1;
            T = new Train(0);
            Towns = new Town[20];
            Towns[0] = new Town(0);
            for (int i = 1; i < 20; i++)
            {
                Towns[i] = new Town(i);
            }           
            SelectedTown = Towns[0];
        }

        public void Attack(Train EnemyTrain)
        {
            foreach(Wagon wagon in EnemyTrain.Wagons)
            {
                Console.WriteLine(wagon.GetType());
            }
            Console.WriteLine("Choose target:");
            int AttackIndex = Convert.ToInt32(Console.ReadLine());
            List<(WeaponWagon, int)> CurrentWeaponWagons = T.GetWeaponWagons();
            int TotalAttack = 0;
            foreach ((WeaponWagon, int) weaponwagon in CurrentWeaponWagons)
            {
                foreach (Weapon w in weaponwagon.Item1.Weapons)
                {
                    TotalAttack += w.DMG;
                }
            }
            EnemyTrain.Wagons[AttackIndex-1].Armor -= TotalAttack;
            CheckDestroyedWagons();
        }
        public void CheckDestroyedWagons()
        {
            for (int i = 0; i < T.Wagons.Count; i++)
            {
                if (T.Wagons[i].Armor <= 0)
                {
                    T.Wagons.RemoveAt(i);
                }
            }
        }
        public void GetStats()
        {
            Console.Clear();
            Console.WriteLine("Name:  " + Name);
            Console.WriteLine("EXP:   " + EXP + "/" + MaxEXP + ";");
            Console.WriteLine("LVL:   " + LVL + ";");
            Console.WriteLine("Money: " + Money + ";");
            Console.WriteLine(" ");
            Console.WriteLine("Inventory:");
            GetInventory();
            Console.WriteLine(" ");
            T.GetTrainStats();
            Console.ReadLine();
        }
        public void Check()
        {
            T.WagonCap = T.GetWagonSlots();
            if(EXP >= MaxEXP)
            {
                LVL++;
                EXP -= MaxEXP;
                MaxEXP *= LVL;
            }
        }
        public void GetInventory()
        {
            foreach (var position in Inventory)
            {
                Console.WriteLine("Resource: " + position.Item1.GetResName() + "; Amount = " + position.Item2.ToString());
            }
        }
        public void Travel()
        {
            Console.Clear();
            Random random = new Random();
            int index = random.Next(0, 9);
            bool exit = false;
            while (!exit)
            {
                if (Towns[index].Name != SelectedTown.Name)
                {
                    int RequiredFuel;
                    if (Towns[index].ID > SelectedTown.ID)
                    {
                        RequiredFuel = Towns[index].ID - SelectedTown.ID;
                    }
                    else { RequiredFuel = SelectedTown.ID - Towns[index].ID; }
                    Console.WriteLine("Destination town: " + Towns[index].Name + "." + "\nFuel required: " + RequiredFuel + "\nContinue?");
                    string Answer = Console.ReadLine();
                    if (Answer == "Y" || Answer == "y" || Answer == "1")
                    {
                        if (T.Locomotive_.Fuel >= RequiredFuel)
                        {
                            SelectedTown = Towns[index];
                            T.Locomotive_.Fuel -= RequiredFuel;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Not enough fuel...");
                            Console.ReadLine();
                            break;
                        }
                    }
                    else if(Answer == "N" || Answer == "n" || Answer == "0")
                    {
                        Console.WriteLine("You`ve decided to stay here...");
                        Console.ReadLine();
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Choose 'Y' or 'N'");
                        Console.ReadLine();
                    }
                }
                else index = random.Next(0, 9);
            }          
        }
        public void Debug_Add_Res(int index)
        {
            if (Inventory.Count < T.GetTotalCapacity())
            {
                int amount = random1.Next(1, 50);
                Inventory.Add((new Resource(index), amount));
            }           
        }
        public void Claim_Res(Enemy enemy)
        {
            (Resource, int) Item = (null, 0);
            EXP += enemy.LootEXP;
            for(int l = 0; l < enemy.Inventory.Count; l++)
            {
                for (int i = 0; i < Inventory.Count; i++)
                {
                    if (Inventory[i].Item1.GetResName() == enemy.Inventory[l].Item1.GetResName())
                    {
                        Item = Inventory[i];
                        Item.Item2 += enemy.Inventory[l].Item2;
                        Inventory[i] = Item;
                        enemy.Inventory.RemoveAt(l);
                        break;
                    }
                }
            }
            if (enemy.Inventory.Count > 0)
            {
                foreach ((Resource, int) Loot in enemy.Inventory)
                {
                    Money += (int)Loot.Item1.Price * Loot.Item2;
                }
            }
        }
        public void Shop_BuyRes(Resource res, int amount)
        {
            (Resource, int) Item = (null, 0);
            if (Inventory.Count - 1 < T.GetTotalCapacity())
            {
                if (Money >= res.Price * amount)
                {
                    foreach ((Resource, int) position in Inventory)
                    {
                        if (position.Item1.GetResName() == res.GetResName())
                        {
                            Item = position;
                            break;
                        }
                    }
                    if (Item == (null, 0))
                    {
                        Item = (res, amount);
                        Inventory.Add(Item);
                    }
                    else
                    {                        
                        int index = Inventory.BinarySearch(Item);
                        Item.Item2 += amount;
                        Inventory[index] = Item;
                    }
                    Money -= (int)res.Price * amount;
                }
            }
            else 
            { 
                Console.WriteLine("Not enough space");

                Console.ReadLine();
            }
        }
        public void Shop_SellRes(int index, int amount)
        {
            (Resource, int) Item = Inventory[index];
            if (Item.Item2 > amount)
            {
                Item.Item2 -= amount;
                Inventory[index] = Item;
                Money += (int)(Item.Item1.Price * amount);
            }
            else if (Item.Item2 == amount)
            {
                Inventory.RemoveAt(index);
                Money += (int)(Item.Item1.Price * amount);
            }
            else
            {
                Console.WriteLine("Sell amount is greater than available");
                Console.ReadLine();
            }
        }
        public void Shop_SellAll()
        {
            for(int i = Inventory.Count-1; i > 0; i--)
            {
                Money += (int)Inventory[i].Item1.Price * Inventory[i].Item2;
                Inventory.RemoveAt(i);
            }
        }
        ~Player()
        {

        }
    }

    [Serializable]
    public class Town
    {
        readonly Random rand = new Random((int)DateTime.Now.Ticks);
        public int ID { get; set; }
        public string Name { get; set; }
        private static readonly string[] TownNames = { "Poltava", "Kyiv", "Lviv", "Ternopil'", "Ivano-Frankivsk", "Uzhorod", "Zhytomyr", "Khmelnytsky", "Vinnytsia", "Odesa", "Rivne", "Lutsk", "Chervnitsi", "Mykolaiv", "Kherson", "Dnipro", "Kropyvnytsky", "Chernihiv", "Sumy", "Kharkiv" };
        public Resource DefResourse { get; set; }
        public Resource ProfResourse { get; set; }
        public float DefMod = 1.5f;
        public float ProfMod = 0.5f;
        public bool Shop { get; set; }
        public bool Workshop { get; set; }
        public bool Saloon { get; set; }
        public Town()
        {

        }
        public Town(int _ID)
        { 
            if(_ID == 0)
            {
                ID = _ID;
                Name = "Stari Zalizhnychnyky";
                Shop = true;
                Workshop = false;
                Saloon = false;
                DefResourse = new Resource(8);
                ProfResourse = new Resource(0);
            }
            else
            {
                int index;
                bool exit = false;
                while (!exit)
                {                    
                    index = rand.Next(20);
                    if (TownNames[index] != null)
                    {
                        ID = _ID;
                        Name = TownNames[index];
                        TownNames[index] = null;
                        exit = true;
                    }
                }
                index = rand.Next(0, 4);
                Shop = index != 0;                  // 75 %
                index = rand.Next(0, 4);
                Workshop = index == 1 || index == 2;// 50 %
                index = rand.Next(0, 4);
                Saloon = index == 1;                // 25 %
                if (Shop)
                {
                    index = rand.Next(20);
                    DefResourse = new Resource(index);
                    index = rand.Next(20);
                    ProfResourse = new Resource(index);
                    if(ProfResourse == DefResourse)
                    {
                        index = rand.Next(20);
                        ProfResourse = new Resource(index);
                    }
                }
            }          
        }
    }
}