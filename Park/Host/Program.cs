using System.Collections.Generic;
using Models;

namespace Park
{
    class Program
    {
        delegate void End(List<Animal> l);

        static void Main(string[] args)
        {
            Console.WriteLine("Вы - работник зоопарка, и к вам попал неизвестный вид животных.\n" +
                              "Ваша задача узнать, какими растениями они могут питаться, а какие для них ядовиты.\n" +
                              "Накормите животных, не дайте им умереть, занимайтесь разведением\n" +
                              "И многое другое в нашей новой игре под названием:\n" +
                              "\"Почему же так сложно, я ведь хотел просто убирать за мартышками!\"\n" +
                              "                                                   Все права не защищены\n");

            var animalList = new List<Animal>();
            var plantList = new Plant[3];
            Initialization(animalList, plantList);
            int move = 1;
            Console.WriteLine($"Ход номер: {move}\n");
            Menu(animalList, plantList, move);
        }

        static void Initialization(List<Animal> l, Plant[] p)
        {
            p[0] = new Grass("Травушка-муравушка");
            p[1] = new Tree("Деревце кудрявое");
            p[2] = new Feed("КОМБИКОРМ");
            l.Add(new AnimalMale("Пушок", null, null, p));
            l.Add(new AnimalMale("Кренделек", null, null, p));
            l.Add(new AnimalFemale("Снежинка", null, null, p));
            l.Add(new AnimalFemale("Звездочка", null, null, p));
        }

        static void Menu(List<Animal> l, Plant[] p, int move)
        {
            Console.WriteLine("Выберите действие: \n" +
                              "1. Посмотреть список животных и растений.\n" +
                              "2. Накормить животное.\n" +
                              "3. Заняться разведением.\n" +
                              "4. Закончить ход.\n" +
                              "5. Закрыть игру\n");
            while (true)
            {
                Console.WriteLine("Введите число: ");
                string choice = Console.ReadLine();
                int.TryParse(choice, out int x);
                if (x > 0 && x < 6)
                {
                    switch (x)
                    {
                        case 1:
                            GetFullList(l, p, move);
                            break;
                        case 2:
                            GiveFood(l, p, move);
                            break;
                        case 3:
                            MakePregnant(l, p, move);
                            break;
                        case 4:
                            EndMove(l, p, move);
                            break;
                        case 5:
                            Console.WriteLine("Будьте уверены, как только вы покинете игру, все ваши питомцы умрут с голоду. Потому что никому, кроме вас, они нужны.");
                            Console.WriteLine("Введите Y, чтобы выйти.");
                            string? exit = Console.ReadLine();
                            switch (exit)
                            {
                                case "Y":
                                    return;
                                default:
                                    Menu(l, p, move);
                                    break;
                            }
                            break;
                        default:
                            Console.WriteLine("Что-то пошло не так");
                            Menu(l, p, move);
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Неверное число\n");
                    continue;
                }
            }

            static void GetFullList(List<Animal> l, Plant[] p, int move)
            {
                Console.WriteLine("\n------------------------------------------------------");
                Console.WriteLine("Список животных:\n");
                foreach (var a in l)
                {
                    Console.WriteLine($"| Индекс: {l.IndexOf(a)}, Имя: {a?.name}, Размер: {a.size}, Возраст: {a.age}, Мама: {a?.femaleParent?.name}, Папа: {a?.maleParent?.name}, Съедобное растение: {a?.allowedPlant?.name}");
                }
                Console.WriteLine("Список растений:\n");
                for (int i = 0; i < p.Length; i++)
                {
                    Console.WriteLine($"| Индекс: {i}, Название: {p[i]?.name}, Размер: {p[i]?.size}");
                }
                Console.WriteLine("------------------------------------------------------\n");
                Menu(l, p, move);
            }

            static void GiveFood(List<Animal> l, Plant[] p, int move)
            {
                Console.WriteLine("Чтобы накормить животное, необходимо выбрать индекс особи и индекс растения");
                while (true)
                {
                    Console.WriteLine("Введите индекс животного: ");
                    string aChoice = Console.ReadLine();
                    int.TryParse(aChoice, out int x);
                    Console.WriteLine("Введите индекс растения: ");
                    string pChoice = Console.ReadLine();
                    int.TryParse(pChoice, out int y);
                    try
                    {
                        l.ElementAt(x).Eat(p[y], l);
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("Неверные данные. Попробуйте еще раз.");
                        continue;
                    }
                }
                Menu(l, p, move);
            }

            static void MakePregnant(List<Animal> l, Plant[] p, int move)
            {
                Console.WriteLine("Чтобы заняться разведением, введите сначала индекс самки, а затем самца: ");
                while (true)
                {
                    Console.WriteLine("Введите индекс самки: ");
                    string aChoice = Console.ReadLine();
                    int.TryParse(aChoice, out int x);
                    Console.WriteLine("Введите индекс самца: ");
                    string pChoice = Console.ReadLine();
                    int.TryParse(pChoice, out int y);
                    try
                    {
                        l.ElementAt(x).GetPregnant(l.ElementAt(y), l, p);
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("Неверные данные. Попробуйте еще раз.\n");
                        continue;
                    }
                }
                Menu(l, p, move);
            }

            static void EndMove(List<Animal> l, Plant[] p, int move)
            {
                Console.WriteLine("\n------------------------------------------------------");
                Console.WriteLine("Рассчитываются итоги\n");
                End endOfMove = null;
                foreach (var a in l)
                {
                    try
                    {
                        if (a.GetType() == typeof(AnimalFemale))
                        {
                            AnimalFemale temp = (AnimalFemale)a;
                            temp.canBePregnant = true;
                        }
                        endOfMove += a.IsHungry;
                        endOfMove += a.IsOld;
                    }
                    catch
                    {
                        continue;
                    }
                }
                endOfMove?.Invoke(l);
                for (int i = 0; i < p.Length; i++)
                {
                    p[i]?.IsEmptySize(p, i);
                    p[i]?.GetGrow();
                }
                Console.WriteLine("------------------------------------------------------\n");
                move++;
                Console.WriteLine($"Ход номер: {move}\n");
                Menu(l, p, move);
            }
        }
    }
}