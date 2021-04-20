using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TestG
{
    [Serializable]
    public class Train
    {
        public Locomotive Locomotive_ { get; set; }
        public List<Wagon> Wagons { get; set; }
        public int WagonCap { get; set; }
        public Train()
        {

        }
        public Train(int ID)
        {
            switch (ID)
            {
                case 0: //Player Train
                    {
                        Locomotive_ = new Locomotive(0);
                        WagonCap = GetWagonSlots();
                        Wagons = new List<Wagon>
                        {
                            new CargoWagon(0),
                            new WeaponWagon(0)
                        };
                        break; 
                    }
                case 1: //Enemy trains
                    {
                        Random random = new Random();
                        Locomotive_ = new Locomotive("Mad Mary", 4500, 625, 1000, Locomotive.TypeFuel.Uranium, 50, 10000);
                        WagonCap = GetWagonSlots();
                        Wagons = new List<Wagon>();
                        while(Wagons.Count <= WagonCap)
                        {
                            int index = random.Next(0, 10);
                            if (index < 3)
                            {
                                int chs = random.Next(2);
                                if(chs == 0)
                                {
                                    Wagons.Add(new CargoWagon(0));
                                    continue;
                                }
                                else
                                {
                                    Wagons.Add(new WeaponWagon(0));
                                    continue;
                                }                                                               
                            }
                            else if(index >= 4 && index < 7)
                            {
                                int chs = random.Next(2);
                                if (chs == 0)
                                {
                                    Wagons.Add(new CargoWagon("Cargo-2", 2000, 400, 400, 15));
                                    continue;
                                }
                                else
                                {
                                    Wagons.Add(new WeaponWagon("Weapon-2", 3250, 500, 650, 1, 250));
                                    continue;
                                }
                            }
                            else if(index <= 7 && index < 9)
                            {
                                int chs = random.Next(2);
                                if (chs == 0)
                                {
                                    Wagons.Add(new CargoWagon("Cargo-3", 2500, 500, 450, 20));
                                    continue;
                                }
                                else
                                {
                                    Wagons.Add(new WeaponWagon("Weapon-3", 4000, 750, 800, 1, 500));
                                    continue;
                                }
                            }
                            else
                            {
                                int chs = random.Next(2);
                                if (chs == 0)
                                {
                                    Wagons.Add(new CargoWagon("Cargo-5", 3500, 700, 550, 30));
                                    continue;
                                }
                                else
                                {
                                    Wagons.Add(new WeaponWagon("Weapon-5", 5500, 1500, 1250, 2, 1100));
                                    continue;
                                }
                            }
                        }
                        break;
                    }
            }
            
        }
        public List<(WeaponWagon, int)> GetWeaponWagons()
        {
            List<(WeaponWagon, int)> weaponWagons = new List<(WeaponWagon, int)>();
            int index = 0;
            foreach(Wagon wagon in Wagons)
            {
                if (wagon.GetType() == typeof(WeaponWagon))
                {
                    weaponWagons.Add(((WeaponWagon)wagon, index));
                }
                index++;
            }
            return weaponWagons;
        }
        public void GetTrainStats()
        {
            Locomotive_.GetLocoStats();
            for(int i = 0; i < Wagons.Count; i++)
            {
                Wagons[i].GetWagonStats(i + 1);
            }
        }
        public int GetWagonSlots()
        {
            return Locomotive_.Power / 100;
        }
        public int GetTotalCapacity()
        {
            int TotalCap = 0;
            foreach (var wagon in Wagons)
            {
                if (wagon != null)
                {
                    if (wagon.GetType() == typeof(CargoWagon))
                    {
                        TotalCap += wagon.GetSpec();
                    }
                }                
            }
            return TotalCap;
        }
        public void Add_Wagon(Wagon wagon)
        {
            if (Wagons.Count < GetWagonSlots())
            {
                Wagons.Add(wagon);
            }            
        }
        public int GetNullWagonSlotIndex()
        {
            int index = 0;
            foreach(Wagon wagon in Wagons)
            {
                index++;
            }
            return index;
        }
    }
}
