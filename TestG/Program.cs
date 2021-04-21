using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TestG
{
    class Program
    {
        static readonly string path = @"D:\\Study\Програмування\.NET\TestG\Saves";
        static int MaxValue = 11;
        static Player Start(string Answer)
        {
            Player P1;
            switch (Answer)
            {
                case "1":
                    {
                        Console.WriteLine("Welcome! Enter your nickname:");
                        string nick = Console.ReadLine();
                        P1 = new Player(nick);
                        break;
                    }
                case "2": { P1 = Load(); break; }
                default:
                    {
                        Console.WriteLine("Enter valid variant");
                        P1 = null;
                        break;
                    }
            }
            return P1;
        }
        static bool Options(Player P)
        {
            Console.Clear();
            bool exit = false;
            Console.WriteLine("Additional: " + "\n1. Save" + "\n2.Exit");
            string chs = Console.ReadLine();
            if (chs == "1")
            {
                Save(P);
                Console.WriteLine("Progress saved");
                Console.ReadLine();
            }
            else
            exit = true;
            return exit;
        }
        static Player Load()
        {
            Player P;
            XmlAttributeOverrides attrOverrides =
           new XmlAttributeOverrides();
            XmlAttributes attrs = new XmlAttributes();

            // Creates an XmlElementAttribute to override the
            // field that returns Book objects. The overridden field  
            // returns Expanded objects instead.  
            XmlElementAttribute attr = new XmlElementAttribute
            {
                ElementName = "CargoWagon",
                Type = typeof(CargoWagon)
            };
            XmlElementAttribute attr1 = new XmlElementAttribute
            {
                ElementName = "WeaponWagon",
                Type = typeof(WeaponWagon)
            };

            // Adds the element to the collection of elements.  
            attrs.XmlElements.Add(attr);
            attrs.XmlElements.Add(attr1);

            attrOverrides.Add(typeof(Train), "Wagons", attrs);
            XmlSerializer formatter = new XmlSerializer(typeof(Player), attrOverrides);
            using (FileStream fstream = File.OpenRead($@"{path}\save.xml"))
            {
                P = (Player)formatter.Deserialize(fstream);
            }
            return P;
        }
        static void Save(Player P)
        {
            // Each overridden field, property, or type requires
            // an XmlAttributes instance.  
            XmlAttributes attrs = new XmlAttributes();

            // Creates an XmlElementAttribute instance to override the
            // field that returns Book objects. The overridden field  
            // returns Expanded objects instead.  
            XmlElementAttribute attr = new XmlElementAttribute
            {
                ElementName = "CargoWagon",
                Type = typeof(CargoWagon)
            };
            XmlElementAttribute attr1 = new XmlElementAttribute
            {
                ElementName = "WeaponWagon",
                Type = typeof(WeaponWagon)
            };

            // Adds the element to the collection of elements.  
            attrs.XmlElements.Add(attr);
            attrs.XmlElements.Add(attr1);

            // Creates the XmlAttributeOverrides instance.  
            XmlAttributeOverrides attrOverrides = new XmlAttributeOverrides();

            // Adds the type of the class that contains the overridden
            // member, as well as the XmlAttributes instance to override it
            // with, to the XmlAttributeOverrides.  
            attrOverrides.Add(typeof(Train), "Wagons", attrs);
            XmlSerializer formatter = new XmlSerializer(typeof(Player), attrOverrides);
            File.Delete($@"{path}\save.xml");
            using (FileStream fstream = new FileStream($@"{path}\save.xml", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fstream, P);
            }
        }
        static void Town(Player P)
        {
            bool ExitVar = false;
            while (!ExitVar)
            {
                Console.Clear();
                Console.WriteLine("You've entered " + P.SelectedTown.Name);
                Console.WriteLine("Choose where to go: "
                    + "\n1. Shop (buy wagons, locomotives, etc.)"
                    + "\n2. Workshop (upgrade your train)"
                    + "\n3. Saloon (casino and quest board)"
                    + "\n4. Leave");
                int Answer = Convert.ToInt32(Console.ReadLine());
                switch (Answer)
                {
                    case 1:
                        {
                            if (P.SelectedTown.Shop)
                            {
                                PreShop(P);
                            }
                            else 
                            { 
                                Console.WriteLine("Shop isn`t in " + P.SelectedTown.Name);

                                Console.ReadLine();
                            }
                            break;
                        }
                    case 2:
                        {
                            if (P.SelectedTown.Workshop)
                            {
                                Workshop(P);
                            }
                            else
                            {
                                Console.WriteLine("Workshop isn`t in " + P.SelectedTown.Name);

                                Console.ReadLine();
                            }
                            break;
                        }
                    case 3:
                        {
                            if (P.SelectedTown.Saloon)
                            {
                                Console.WriteLine("Coming soon...");

                                Console.ReadLine();
                            }
                            else
                            {
                                Console.WriteLine("Saloon isn`t in " + P.SelectedTown.Name);

                                Console.ReadLine();
                            }
                            break;
                        }
                    case 4:
                        {
                            ExitVar = true;
                            break;
                        }
                    default:
                        break;
                }
            }          
        }

        static Resource[] ShopPreparations_Resource()
        {
            Resource[] resources = new Resource[20];
            for(int i = 0; i < 20; i++)
            {
                resources[i] = new Resource(i);
            }
            return resources;
        }
        static Locomotive[] ShopPreparations_Locomotives()
        {
            Locomotive[] locomotive = new Locomotive[5];
            locomotive[0] = new Locomotive("Elrick", 4000, 375, 650, Locomotive.TypeFuel.Coal, 25, 1250);
            locomotive[1] = new Locomotive("SteamPunk Locomotive v.1", 5500, 525, 950, Locomotive.TypeFuel.Coal, 45, 3750);
            locomotive[2] = new Locomotive("Diesel Mary", 4500, 450, 550, Locomotive.TypeFuel.Diesel, 30, 2250);
            locomotive[3] = new Locomotive("Alfonso", 5000, 500, 875, Locomotive.TypeFuel.Diesel, 35, 2750);
            locomotive[4] = new Locomotive("SantaFe SF720", 6500, 725, 950, Locomotive.TypeFuel.Diesel, 50, 5000);
            return locomotive;
        }
        static List<Wagon> ShopPreparations_Wagons()
        {
            List<Wagon> wagons = new List<Wagon>
            {
                new CargoWagon(0),
                new CargoWagon("Cargo-2", 2000, 400, 400, 15),
                new CargoWagon("Cargo-3", 2500, 500, 450, 20),
                new CargoWagon("Cargo-4", 3000, 600, 500, 25),
                new CargoWagon("Cargo-5", 3500, 700, 550, 30),
                new WeaponWagon(0),
                new WeaponWagon("Weapon-2", 3250, 500, 650, 1, 250),
                new WeaponWagon("Weapon-3", 4000, 750, 800, 1, 500),
                new WeaponWagon("Weapon-4", 4750, 1250, 1000, 2, 850),
                new WeaponWagon("Weapon-5", 5500, 1500, 1250, 2, 1100)
            };
            return wagons;
        }
        static List<Weapon> ShopPreparations_Weapons()
        {
            List<Weapon> weapons = new List<Weapon>
            {
                new Weapon("Galting MK XVI", 175, Weapon.WeaponType.MACHINE_GUN, 400),
                new Weapon("Panzer Howitzer", 450, Weapon.WeaponType.HEAVY_CANNON, 750),
                new Weapon("KwK L45", 350, Weapon.WeaponType.LIGHT_CANNON, 550),
                new Weapon("C-40 SHERSHEN", 400, Weapon.WeaponType.ROCKETS, 650),
                new Weapon("RedLaser", 250, Weapon.WeaponType.LASER, 450)
            };
            return weapons;
        }
        static void PreShop(Player P)
        {
            Console.Clear();
            bool ExitVar = false;
            while (!ExitVar)
            {
                Console.Clear();
                int wagoncap = P.T.GetWagonSlots();
                int wagons = P.T.GetNullWagonSlotIndex();
                Console.WriteLine("Wagons: " + wagons + "/" + wagoncap);
                Console.WriteLine("Welcome to our shop!" 
                    + "\nWhat do you want?" 
                    + "\n1. Resourses" 
                    + "\n2. Train" 
                    + "\n3. Exit");
                string chs = Console.ReadLine();
                switch (chs)
                {
                    case "1": {ResShop(P); break; }
                    case "2": {TrainShop(P); break; }
                    case "3": { ExitVar = true; break; }
                    default:
                        break;
                }
            }            
        }
        static void ResShop(Player P)
        {           
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Proficite: " + P.SelectedTown.ProfResourse.GetResName() 
                    + "; Price: " + P.SelectedTown.ProfResourse.Price * P.SelectedTown.ProfMod + ";");
                Console.WriteLine("Deficite: " + P.SelectedTown.DefResourse.GetResName() 
                    + "; Price: " + P.SelectedTown.DefResourse.Price * P.SelectedTown.DefMod + ";");
                Console.WriteLine("1. Buy" + "\n2. Sell" + "\n3. Refuel" + "\n4.Repair" + "\n5. Exit");
                string A = Console.ReadLine();
                Resource[] resources = ShopPreparations_Resource();
                foreach (Resource res in resources)
                {
                    if (res.GetResName() == P.SelectedTown.ProfResourse.GetResName())
                    {
                        res.Price *= P.SelectedTown.ProfMod;
                    }
                    else if (res.GetResName() == P.SelectedTown.DefResourse.GetResName())
                    {
                        res.Price *= P.SelectedTown.DefMod;
                    }                   
                }
                switch (A)
                {
                    case "1":
                        {                            
                            int index = 0;
                            foreach (Resource res in resources)
                            {                                
                                Console.WriteLine((index + 1).ToString() + ". Resource: " + res.GetResName()
                                    + "; Price = " + res.Price.ToString());
                                index++;
                            }
                            Console.WriteLine("Choose which to buy");
                            index = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Enter amount");
                            int amount = Convert.ToInt32(Console.ReadLine());
                            P.Shop_BuyRes(resources[index-1], amount);
                            break; 
                        }
                    case "2": 
                        {
                            P.GetInventory();
                            Console.WriteLine("Choose which to sell");
                            string index = Console.ReadLine();
                            if(index == "all")
                            {
                                P.Shop_SellAll();
                            }
                            else if(Convert.ToInt32(index) <= P.Inventory.Count)
                            {
                                Resource res = new Resource(P.Inventory[Convert.ToInt32(index)].Item1.ID);
                                if (P.Inventory[Convert.ToInt32(index)].Item1.Price != res.Price)
                                {
                                    P.Inventory[Convert.ToInt32(index)].Item1.Price = res.Price;
                                }                               
                                Console.WriteLine("Enter amount");
                                int amount = Convert.ToInt32(Console.ReadLine());
                                P.Shop_SellRes(Convert.ToInt32(index)-1, amount);
                            }
                            else
                            {
                                Console.WriteLine("Resource is not found...");
                                Console.ReadLine();
                            }
                            
                            break; 
                        }
                    case "3": 
                        {
                            int price = (P.T.Locomotive_.Fuelcap - P.T.Locomotive_.Fuel) * 25;
                            Console.WriteLine("Refuel costs " + price + "Continue?");
                            string Answer = Console.ReadLine();
                            if (Answer == "Y" || Answer == "y" || Answer == "1")
                            {
                                P.Money -= price;
                                P.T.Locomotive_.Fuel = P.T.Locomotive_.Fuelcap;
                            }
                            break; 
                        }
                    case "4": 
                        {
                            int RepairedArmor = 0;
                            int RepairPrice = 0;
                            foreach(Wagon wagon in P.T.Wagons)
                            {
                                if(wagon.Armor != wagon.MaxArmor)
                                {
                                    RepairedArmor += (wagon.MaxArmor - wagon.Armor) / 10;
                                    RepairPrice += 5 * RepairedArmor;
                                }
                            }
                            Console.WriteLine("Repair costs " + RepairPrice + "\nContinue?");
                            string Answer = Console.ReadLine();
                            if(Answer == "Y" || Answer == "y" || Answer == "1")
                            {
                                P.Money -= RepairPrice;
                                foreach (Wagon wagon in P.T.Wagons)
                                {
                                    if (wagon.Armor != wagon.MaxArmor)
                                    {
                                        wagon.Armor = wagon.MaxArmor;
                                    }
                                }
                            }
                            break; 
                        }
                    case "5": { exit = true; break; }
                    default: { break; }
                }
            }
            
        }
        static void TrainShop(Player P)
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Train Shop" + "\n1. Lococmotives" + "\n2. Wagons" + "\n3. Weapons" + "\n4. Exit");
                string A = Console.ReadLine();                
                switch (A)
                {
                    case "1":
                        {
                            int index = 0;
                            Locomotive[] locomotives = ShopPreparations_Locomotives();
                            foreach (Locomotive loco in locomotives)
                            {
                                Console.WriteLine((index+1).ToString() + ". Name: " + loco.Name 
                                    + " Power: " + loco.Power 
                                    + " Armor: " + loco.Armor 
                                    + " Fuel Type " + loco.Type_Fuel 
                                    + " Fuel Capacity " + loco.Fuelcap 
                                    + " Price: " + loco.Price);
                                index++;
                            }
                            Console.WriteLine("Choose which to buy");
                            index = Convert.ToInt32(Console.ReadLine());
                            if(P.Money >= locomotives[index - 1].Price)
                            {                                
                                P.Money -= locomotives[index - 1].Price;
                                P.Money += P.T.Locomotive_.Price / 2;
                                int tempfuel = P.T.Locomotive_.Fuel;
                                P.T.Locomotive_ = locomotives[index - 1];
                                P.T.Locomotive_.Fuel = tempfuel;
                            }
                            break;
                        }
                    case "2":
                        {
                            Console.Clear();
                            Console.WriteLine("1. Buy" + "\n2. Sell");
                            A = Console.ReadLine();
                            switch (A)
                            {
                                case "1": 
                                    {
                                        int wagoncap = P.T.GetWagonSlots();
                                        int AvailableWagons = P.T.GetNullWagonSlotIndex();
                                        Console.WriteLine("Wagons: " + AvailableWagons + "/" + wagoncap);
                                        List<Wagon> wagons = ShopPreparations_Wagons();
                                        int index = 1;
                                        foreach(Wagon wagon in wagons)
                                        {
                                            Console.WriteLine("--------------------------------");
                                            wagon.GetWagonStats(index);
                                            Console.WriteLine("Price: " + wagon.Price);
                                            index++;
                                        }
                                        Console.WriteLine("Choose which to buy");
                                        index = Convert.ToInt32(Console.ReadLine());
                                        if (P.Money >= wagons[index - 1].Price && P.T.Wagons.Count < P.T.GetWagonSlots())
                                        {
                                            P.Money -= (int)wagons[index - 1].Price;
                                            P.T.Add_Wagon(wagons[index-1]);
                                        }
                                        break; 
                                    }
                                case "2": { break; }
                                default: { break; }
                            }
                            break;
                        }
                    case "3": 
                        {
                            List<Weapon> weapons = ShopPreparations_Weapons();
                            List<(WeaponWagon, int)> currentWagons = P.T.GetWeaponWagons();
                            int WeaponIndex = 1;
                            foreach(Weapon weapon in weapons)
                            {
                                Console.WriteLine("--------------------------------");
                                Console.WriteLine(WeaponIndex + ":");
                                weapon.GetWeaponStats();
                                Console.WriteLine("Price: " + weapon.Price);
                                WeaponIndex++;
                            }
                            Console.WriteLine("Choose which to buy");
                            WeaponIndex = Convert.ToInt32(Console.ReadLine());
                            int index = 0;
                            if (P.Money >= weapons[WeaponIndex - 1].Price)
                            {
                                foreach ((WeaponWagon, int) wagon in currentWagons)
                                {
                                    wagon.Item1.GetWagonStats(index + 1);
                                    index++;
                                }
                                Console.WriteLine("Choose wagon on which to install the weapon");
                                int w = Convert.ToInt32(Console.ReadLine());
                                int Weapon_Slot;
                                if (currentWagons[w - 1].Item1.Weapon_slots > 1)
                                {
                                    Console.WriteLine("Select slot:");
                                    Weapon_Slot = Convert.ToInt32(Console.ReadLine());
                                }
                                else 
                                { 
                                    Weapon_Slot = 0; 
                                }
                                if(Weapon_Slot != currentWagons[w - 1].Item1.Weapon_slots)
                                {                                   
                                    index = currentWagons[w - 1].Item2;
                                    currentWagons[w - 1].Item1.Weapons[Weapon_Slot] = weapons[WeaponIndex - 1];
                                    P.T.Wagons[index] = currentWagons[w - 1].Item1;
                                    P.Money -= currentWagons[w - 1].Item1.Weapons[currentWagons[w - 1].Item1.Weapons.Count - 1].Price;
                                }
                                else Console.WriteLine("Invalid weapon slot");
                            }
                            break; 
                        }
                    case "4": { exit = true; break; }
                    default: { break; }
                }
            }

        }

        static void Workshop(Player P)
        {
            Console.Clear();
            bool ExitVar = false;
            while (!ExitVar)
            {
                Console.WriteLine("Select which to upgrade:" 
                    + "\n1. Locomotive" 
                    + "\n2. Wagon" 
                    + "\n3. Exit");
                int Answer = Convert.ToInt32(Console.ReadLine());
                switch (Answer)
                {
                    case 1: 
                        {
                            P.Money = P.T.Locomotive_.Upgrade(P.Money);
                            break; 
                        }
                    case 2:
                        {
                            Console.Clear();
                            int index;
                            for (int i = 0; i < P.T.Wagons.Count; i++)
                            {
                                P.T.Wagons[i].GetWagonStats(i + 1);
                            }
                            Console.WriteLine("Enter wagon number");
                            index = Convert.ToInt32(Console.ReadLine());
                            if(index <= P.T.GetWagonSlots())
                            {
                                P.Money = P.T.Wagons[index - 1].Upgrade(P.Money);
                            }
                            else
                            {
                                Console.WriteLine("Incorrect wagon number. Try again");

                                Console.ReadLine();
                            }
                            break;
                        }
                    case 3:
                        { ExitVar = true; break; }
                    default:
                        break;
                }
            }          
        }

        static void Scout(Player P)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            int index = random.Next(MaxValue);
            if(index == 0 || index == 1 || index == 2)
            {
                Enemy enemy = new Enemy();
                MaxValue = 11;
                bool Exit = false;
                while(!Exit)
                {
                    Console.Clear();
                    Console.WriteLine("Enemy: " + enemy.name + "\nWagons remain: " + enemy.EnemyTrain.Wagons.Count + "\n----------------");
                    P.Attack(enemy.EnemyTrain);
                    if (P.T.Wagons.Count == 0)
                    {
                        Console.WriteLine("Defeat!");
                        Console.ReadLine();
                        Exit = true;
                        Main();
                    }
                    enemy.Attack(P.T);
                    if (enemy.EnemyTrain.Wagons.Count == 0)
                    {
                        Console.WriteLine("Victory!");
                        P.Claim_Res(enemy);
                        Console.ReadLine();
                        Exit = true;
                    }
                }
            }
            else
            {
                index = random.Next(11);
                (Resource, int) FindLoot = (new Resource(index), random.Next(1, MaxValue));
                P.Add_Res(index);
                Console.WriteLine("You found " + FindLoot.Item2 + "x of " + FindLoot.Item1.GetResName());
                Console.ReadLine();
                MaxValue--;
            }
        }
        static void Main()
        {
            Console.Clear();
            Player P1;
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            bool exit = false;
            Console.WriteLine("1. New Game" + "\n2. Load Game" + "\n3. Exit");
            string _Answer = Console.ReadLine();
            if (_Answer == "1" || _Answer == "2")
            {
                P1 = Start(_Answer);
            }
            else return;
            while (!exit)
            {
                Console.Clear();
                P1.Check();
                Console.WriteLine("You're in " + P1.SelectedTown.Name + "\nChoose an action: " 
                    + "\n1. Travel" 
                    + "\n2. Scout" 
                    + "\n3. Go to town" 
                    + "\n4. Get Stats" 
                    + "\n5. Options");
                int Answer = Convert.ToInt32(Console.ReadLine());
                switch (Answer)
                {
                    case 1: { P1.Travel(); break; }
                    case 2: { Scout(P1); break; }
                    case 3: { Town(P1); break; }
                    case 4: { P1.GetStats(); break; }
                    case 5: { exit = Options(P1); break; }
                    default:
                        break;
                }
            }          
        }
    }
}