using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PetSimulator
{
    class Program
    {
        static string creatorName = "Erdem Eren";
        static string studentNumber = "2305041018";

        static List<Pet> adoptedPets = new List<Pet>();
        static bool running = true;

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Pet Simulator!");

            Task.Run(() => StatDecayLoop());

            while (running)
            {
                ShowMainMenu();
            }
        }

        static void ShowMainMenu()
        {
            Console.WriteLine("\nMain Menu:");
            Console.WriteLine("1. Adopt a Pet");
            Console.WriteLine("2. View Pets");
            Console.WriteLine("3. Use Item on Pet");
            Console.WriteLine("4. Display Creator Info");
            Console.WriteLine("5. Exit");
            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AdoptPet();
                    break;
                case "2":
                    ViewPets();
                    break;
                case "3":
                    UseItem();
                    break;
                case "4":
                    Console.WriteLine($"Creator: {Erdem Eren }, Student Number: {2305041018}");
                    break;
                case "5":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Try again.");
                    break;
            }
        }

        static void AdoptPet()
        {
            Console.WriteLine("Choose a pet type:");
            Console.WriteLine("1. Dog");
            Console.WriteLine("2. Cat");
            Console.WriteLine("3. Dragon");

            string choice = Console.ReadLine();
            string type = choice switch
            {
                "1" => "Dog",
                "2" => "Cat",
                "3" => "Dragon",
                _ => "Unknown"
            };

            if (type == "Unknown")
            {
                Console.WriteLine("Invalid choice.");
                return;
            }

            Console.Write("Enter a name for your pet: ");
            string name = Console.ReadLine();

            Pet newPet = new Pet(name, type);
            adoptedPets.Add(newPet);
            Console.WriteLine($"{name} the {type} has been adopted!");
        }

        static void ViewPets()
        {
            if (adoptedPets.Count == 0)
            {
                Console.WriteLine("No pets adopted yet.");
                return;
            }

            foreach (var pet in adoptedPets)
            {
                Console.WriteLine(pet);
            }
        }

        static void UseItem()
        {
            if (adoptedPets.Count == 0)
            {
                Console.WriteLine("You have no pets.");
                return;
            }

            Console.WriteLine("Choose a pet:");
            for (int i = 0; i < adoptedPets.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {adoptedPets[i].Name}");
            }

            if (!int.TryParse(Console.ReadLine(), out int petIndex) || petIndex < 1 || petIndex > adoptedPets.Count)
            {
                Console.WriteLine("Invalid selection.");
                return;
            }

            Pet selectedPet = adoptedPets[petIndex - 1];

            Console.WriteLine("Choose an item:");
            Console.WriteLine("1. Food (restores Hunger)");
            Console.WriteLine("2. Bed (restores Sleep)");
            Console.WriteLine("3. Toy (restores Fun)");
            string itemChoice = Console.ReadLine();

            Item item = itemChoice switch
            {
                "1" => new Item("Food", "Hunger", 20, 2),
                "2" => new Item("Bed", "Sleep", 20, 3),
                "3" => new Item("Toy", "Fun", 20, 1),
                _ => null
            };

            if (item == null)
            {
                Console.WriteLine("Invalid item.");
                return;
            }

            Console.WriteLine($"Using {item.Name} on {selectedPet.Name}...");
            Thread.Sleep(item.Duration * 1000);
            selectedPet.UseItem(item);
            Console.WriteLine($"{item.Name} used. {selectedPet.Name}'s {item.StatAffected} increased by {item.Amount}.");
        }

        static void StatDecayLoop()
        {
            while (running)
            {
                Thread.Sleep(5000); 

                List<Pet> deadPets = new List<Pet>();

                foreach (var pet in adoptedPets)
                {
                    pet.DecreaseStats(1);

                    if (pet.IsDead())
                    {
                        deadPets.Add(pet);
                    }
                }

                foreach (var pet in deadPets)
                {
                    Console.WriteLine($"\n{pet.Name} the {pet.Type} has died!");
                    adoptedPets.Remove(pet);
                }
            }
        }
    }

    class Pet
    {
        public string Name { get; }
        public string Type { get; }
        public int Hunger { get; private set; }
        public int Sleep { get; private set; }
        public int Fun { get; private set; }

        public Pet(string name, string type)
        {
            Name = name;
            Type = type;
            Hunger = 50;
            Sleep = 50;
            Fun = 50;
        }

        public void DecreaseStats(int amount)
        {
            Hunger = Math.Max(0, Hunger - amount);
            Sleep = Math.Max(0, Sleep - amount);
            Fun = Math.Max(0, Fun - amount);
        }

        public void UseItem(Item item)
        {
            switch (item.StatAffected)
            {
                case "Hunger": Hunger = Math.Min(100, Hunger + item.Amount); break;
                case "Sleep": Sleep = Math.Min(100, Sleep + item.Amount); break;
                case "Fun": Fun = Math.Min(100, Fun + item.Amount); break;
            }
        }

        public bool IsDead()
        {
            return Hunger == 0 || Sleep == 0 || Fun == 0;
        }

        public override string ToString()
        {
            return $"{Name} the {Type} - Hunger: {Hunger}, Sleep: {Sleep}, Fun: {Fun}";
        }
    }

    class Item
    {
        public string Name { get; }
        public string StatAffected { get; }
        public int Amount { get; }
        public int Duration { get; } 
        

        public Item(string name, string statAffected, int amount, int duration)
        {
            Name = name;
            StatAffected = statAffected;
            Amount = amount;
            Duration = duration;
        }
    }
}

