namespace Models
{
    public class Animal
    {
        public virtual byte maxSize { get; }
        public virtual byte minSize { get; }
        public virtual byte maxAge { get; }

        public string name;
        public byte age;
        public byte size;
        public Plant poisonPlant;
        public Plant allowedPlant;

        public Animal? maleParent;
        public Animal? femaleParent;

        public Animal(string name, Animal maleParent, Animal femaleParent, Plant[] l)
        {
            this.name = name;
            age = 1;
            this.maleParent = maleParent;
            this.femaleParent = femaleParent;
            poisonPlant = GetPoisonPlant(l);
        }

        public Plant GetPoisonPlant(Plant[] l)
        {
            var rnd = new Random();
            int i = rnd.Next(0, 2);
            return l[i];
        }

        public virtual void Eat(Plant plant, List<Animal> l)
        {
            if (plant == this.poisonPlant)
            {
                Console.WriteLine($"\nРастение {plant.name} оказалось ядовитым для {this.name}.\n");
                this.size--;
                plant.size--;
                if (this.size <= 0)
                {
                    Console.WriteLine($"\nОсобь {this.name} умирает от яда :(\n");
                    this.Die(l);
                }
                return;
            }

            if (size < maxSize)
            {
                this.size++;
                plant.size--;
                if (plant.name != "КОМБИКОРМ")
                {
                    allowedPlant = plant;
                }
                Console.WriteLine($"\n{this.name} ест растение {plant.name} и растет!\n");
            }
            else
            {
                this.Die(l);
                Console.WriteLine($"\n{this.name} ест растение {plant.name} и умирает от переедания!\n");
            }
        }

        public virtual void Die(List<Animal> l)
        {
            l.Remove(this);
        }

        public virtual void IsHungry(List<Animal> l)
        {
            this.size--;
            if (this.size < minSize)
            {
                Console.WriteLine($"\nОсобь {this.name} умирает от голода :(\n");
                Die(l);
            }
        }

        public virtual void IsOld(List<Animal> l)
        {
            this.age++;
            if (this.age > maxAge)
            {
                Console.WriteLine($"\nОсобь {this.name} умирает от старости.\n");
                Die(l);
            }
        }

        public virtual void GetPregnant(Animal male, List<Animal> l, Plant[] pl)
        {

        }

    }

    public sealed class AnimalMale : Animal
    {
        public override byte maxSize { get => 9; }
        public override byte minSize { get => 3; }
        public override byte maxAge { get => 8; }
        public AnimalMale(string name, Animal maleParent, Animal femaleParent, Plant[] l) : base(name, maleParent, femaleParent, l)
        {
            size = minSize;
        }

        public override void Eat(Plant plant, List<Animal> l)
        {
            base.Eat(plant, l);
        }

        public override void Die(List<Animal> l)
        {
            base.Die(l);
        }

        public override void IsHungry(List<Animal> l)
        {
            base.IsHungry(l);
        }

        public override void IsOld(List<Animal> l)
        {
            base.IsOld(l);
        }
    }

    public sealed class AnimalFemale : Animal
    {
        public override byte maxSize { get => 6; }
        public override byte minSize { get => 2; }
        public override byte maxAge { get => 10; }

        public bool canBePregnant = true;

        public AnimalFemale(string name, Animal maleParent, Animal femaleParent, Plant[] l) : base(name, maleParent, femaleParent, l)
        {
            size = minSize;
        }

        public override void Eat(Plant plant, List<Animal> l)
        {
            base.Eat(plant, l);
        }

        public override void Die(List<Animal> l)
        {
            base.Die(l);
        }
        public override void IsHungry(List<Animal> l)
        {
            base.IsHungry(l);
        }

        public override void IsOld(List<Animal> l)
        {
            base.IsOld(l);
        }

        public override void GetPregnant(Animal male, List<Animal> l, Plant[] pl)
        {
            Console.WriteLine("\n------------------");
            if (this.canBePregnant == true)
            {
                Random rnd = new Random();
                byte rndNum = Convert.ToByte(rnd.Next(0, 2));
                this.canBePregnant = false;

                if (rndNum == 0)
                {
                    Console.WriteLine("Это мальчик");
                    Console.WriteLine("Введите имя нового существа: ");
                    string? name = Console.ReadLine();

                    AnimalMale animal = new AnimalMale(name, male, this, pl);
                    Console.WriteLine($"Рожден мальчик с именем {animal.name} и родителями: {this.name} и {male.name}\n");
                    l.Add(animal);

                }
                else
                {
                    Console.WriteLine("Это девочка");
                    Console.WriteLine("Введите имя нового существа: ");
                    string? name = Console.ReadLine();

                    AnimalFemale animal = new AnimalFemale(name, male, this, pl);
                    Console.WriteLine($"Рождена девочка с именем {animal.name} и родителями: {this.name} и {male.name}\n");
                    l.Add(animal);
                }
            }
            else
            {
                Console.WriteLine($"Особь {this.name} не может забеременеть, так как уже недавно рожала.\n");
                return;
            }

            Console.WriteLine("\n------------------");
        }
    }

    public class Plant
    {
        public byte maxSize;
        public string name;
        public byte size;

        public Plant(string name, byte maxSize)
        {
            this.name = name;
            this.maxSize = maxSize;
            this.size = 10;
        }

        public virtual void GetGrow()
        {
            if (this.size < maxSize)
            {
                size = (byte)(size + 5);
            }
        }

        public virtual void IsEmptySize(Plant[] p, int index)
        {
            if (this.size == 0)
            {
                Console.WriteLine($"\nРастение {p[index].name} было съедено до основания...\n");
                p[index] = null;
            }
        }
    }

    public sealed class Grass : Plant
    {
        public Grass(string name) : base(name, 100)
        {

        }

        public override void GetGrow()
        {
            base.GetGrow();
        }

        public override void IsEmptySize(Plant[] p, int index)
        {
            base.IsEmptySize(p, index);
        }
    }

    public sealed class Tree : Plant
    {
        public Tree(string name) : base(name, 100)
        {

        }

        public override void GetGrow()
        {
            base.GetGrow();
        }

        public override void IsEmptySize(Plant[] p, int index)
        {
            base.IsEmptySize(p, index);
        }
    }

    public sealed class Feed : Plant
    {
        public Feed(string name) : base(name, 20)
        {

        }

        public override void GetGrow()
        {
            base.GetGrow();
        }

        public override void IsEmptySize(Plant[] p, int index)
        {
            if (this.size == 0)
            {
                Console.WriteLine("\nКомбикорм закончился, но скоро подвезут новый.\n");
            }
        }
    }
}