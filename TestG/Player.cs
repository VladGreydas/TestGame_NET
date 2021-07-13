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
        public List<(Resource, int)> ResInventory { get; set; }
        public List<Item> ItemInventory { get; set; }
        public Player()
        {

        }
        public Player(string nickname)
        {
            ResInventory = new List<(Resource, int)>();
            ItemInventory = new List<Item>();
            Name = nickname;
            Money = 500;
            EXP = 0;
            MaxEXP = 100;
            LVL = 1;
            T = new Train();
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
                    //TODO: Attack Buffs
                    TotalAttack += w.DMG;
                    switch (w.Weapon_Type)
                    {
                        //TODO: Ammo Consuming
                        case Weapon.WeaponType.MACHINE_GUN: { break; }
                        case Weapon.WeaponType.LIGHT_CANNON: { break; }
                        case Weapon.WeaponType.HEAVY_CANNON: { break; }
                        case Weapon.WeaponType.FLAMETHROWER: { break; }
                        case Weapon.WeaponType.LASER: { break; }
                        case Weapon.WeaponType.ROCKETS: { break; }
                    }
                }
            }
            EnemyTrain.Wagons[AttackIndex-1].Armor -= TotalAttack; // +/* Buff(Target or AOE)
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
            foreach (var position in ResInventory)
            {
                Console.WriteLine("Resource: " + position.Item1.GetResName() + "; Amount = " + position.Item2.ToString());
            }
        }
        public void Travel()
        {
            Console.Clear();
            Random random = new Random();
            int index = random.Next(0, 20);
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
                            Task.Delay(1000 * index);
                            Console.WriteLine("Yay! We are here!");
                            Console.ReadLine();
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
        public void Add_Res(int index)
        {
            int amount = random1.Next(1, 11);
            (Resource, int) Item = ((new Resource(index), amount));
            if (ResInventory.Count < T.GetTotalCapacity())
            {
                for (int i = 0; i < ResInventory.Count; i++)
                {
                    if (ResInventory[i].Item1.GetResName() == Item.Item1.GetResName())
                    {
                        Item.Item2 += ResInventory[i].Item2;
                        ResInventory[i] = Item;
                        Item = (null, 0);
                        break;
                    }
                }
                if (Item != (null, 0))
                {
                    ResInventory.Add((new Resource(index), amount));
                }
            }
        }
        public void Claim_Res(Enemy enemy)
        {
            (Resource, int) Item;
            EXP += enemy.LootEXP;
            while(ResInventory.Count != T.GetTotalCapacity() && enemy.Inventory.Count != 0)
            {
                for (int i = 0; i < enemy.Inventory.Count; i++)
                {
                    Item = enemy.Inventory[i];
                    for (int j = 0; j < ResInventory.Count; j++)
                    {
                        if (ResInventory[j].Item1.GetResName() == Item.Item1.GetResName())
                        {
                            Item = ResInventory[j];
                            Item.Item2 += ResInventory[j].Item2;
                            ResInventory[j] = Item;
                            Item = (null, 0);
                            enemy.Inventory.RemoveAt(i);
                            break;
                        }
                    }
                    if (Item != (null, 0))
                    {
                        ResInventory.Add(Item);
                        enemy.Inventory.RemoveAt(i);
                    }
                }
                break;
            }         
        }
        public void Shop_BuyRes(Resource res, int amount)
        {
            (Resource, int) Item = (null, 0);
            if (ResInventory.Count - 1 < T.GetTotalCapacity())
            {
                if (Money >= res.Price * amount)
                {
                    foreach ((Resource, int) position in ResInventory)
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
                        ResInventory.Add(Item);
                    }
                    else
                    {                        
                        int index = ResInventory.BinarySearch(Item);
                        Item.Item2 += amount;
                        ResInventory[index] = Item;
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
            (Resource, int) Item = ResInventory[index];
            if (Item.Item2 > amount)
            {
                Item.Item2 -= amount;
                ResInventory[index] = Item;
                Money += (int)(Item.Item1.Price * amount);
            }
            else if (Item.Item2 == amount)
            {
                ResInventory.RemoveAt(index);
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
            for(int i = ResInventory.Count-1; i > 0; i--)
            {
                Money += (int)ResInventory[i].Item1.Price * ResInventory[i].Item2;
                ResInventory.RemoveAt(i);
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