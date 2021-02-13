using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace BlackJack
{
    
    enum Suits
    {
        Piki,
        Kresti,
        Chervi,
        Bubni
    }
    enum Types
    {
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Eleven
    }
    class Card
    {
        public Suits Suit;
        public Types Type;
        public Card(Suits suit,Types type)
        {
            Suit = suit;
            Type = type;
        }
    }
    class Dealer
    {
        public List<Card> Cards = new List<Card>();
        public void GetCard(Card[] m)
        {
            Random random = new Random();
            Cards.Add(m[random.Next(m.Length)]);
        }
        public int Sum()
        {
            int sum = 0;
            foreach (var s in Cards)
            {
                sum += ((int)s.Type) + 2;
            }
            return sum;
        }
        public void GetInfo()
        {
            for (int i = 0; i < Cards.Count; i++)
            {
                Console.Write((((int)Cards[i].Type) + 2) + " ");
            }
            Console.Write($"({Sum()})\n\n");
        }
    }
    class Player
    {
        public List<Card> Cards = new List<Card>();
        public void GetCard(Card[] m)
        {
            Random random = new Random();
            Cards.Add(m[random.Next(m.Length)]);
        }
        public long Balance= long.Parse(File.ReadAllText("Balance"));
        public long Bet;
        public bool HasInsurance;
        public int Sum()
        {
            int sum = 0;
            foreach (var s in Cards)
            {
                sum += ((int)s.Type) + 2;
            }
            return sum;
        }
        public void GetInfo()
        {
            
            for (int i = 0; i < Cards.Count; i++)
            {
                Console.Write((((int)Cards[i].Type) + 2) + " ");
            }
            Console.Write($"({Sum()})\n");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "BlackJack";
            #region Make Heap
            int mod(int delimoe,int delitel)
            {
                return delimoe-(delitel*(delimoe / delitel));
            }
            Card[] StandartHeap = 
            { 
                new Card(Suits.Kresti,Types.Two),new Card(Suits.Kresti,Types.Three),new Card(Suits.Kresti,Types.Four),new Card(Suits.Kresti,Types.Five),new Card(Suits.Kresti,Types.Six),new Card(Suits.Kresti,Types.Seven),new Card(Suits.Kresti,Types.Eight),new Card(Suits.Kresti,Types.Nine),new Card(Suits.Kresti,Types.Ten),new Card(Suits.Kresti,Types.Eleven),
                new Card(Suits.Bubni,Types.Two),new Card(Suits.Bubni,Types.Three),new Card(Suits.Bubni,Types.Four),new Card(Suits.Bubni,Types.Five),new Card(Suits.Bubni,Types.Six),new Card(Suits.Bubni,Types.Seven),new Card(Suits.Bubni,Types.Eight),new Card(Suits.Bubni,Types.Nine),new Card(Suits.Bubni,Types.Ten),new Card(Suits.Bubni,Types.Eleven),
                new Card(Suits.Chervi,Types.Two),new Card(Suits.Chervi,Types.Three),new Card(Suits.Chervi,Types.Four),new Card(Suits.Chervi,Types.Five),new Card(Suits.Chervi,Types.Six),new Card(Suits.Chervi,Types.Seven),new Card(Suits.Chervi,Types.Eight),new Card(Suits.Chervi,Types.Nine),new Card(Suits.Chervi,Types.Ten),new Card(Suits.Chervi,Types.Eleven),
                new Card(Suits.Piki,Types.Two),new Card(Suits.Piki,Types.Three),new Card(Suits.Piki,Types.Four),new Card(Suits.Piki,Types.Five),new Card(Suits.Piki,Types.Six),new Card(Suits.Piki,Types.Seven),new Card(Suits.Piki,Types.Eight),new Card(Suits.Piki,Types.Nine),new Card(Suits.Piki,Types.Ten),new Card(Suits.Piki,Types.Eleven)
            };
            Card[] HeapOfCard = new Card[StandartHeap.Length*6];
            for (int i = 0; i < HeapOfCard.Length; i++)
            {
                HeapOfCard[i] = StandartHeap[mod(i, StandartHeap.Length)];
            }
            #endregion
            for (; ; )
            {
            Begin:
                Console.Clear();
                Dealer dealer = new Dealer();
                Player player = new Player();
                int Mode = 0;
                Console.WriteLine($"Ваш баланс: {player.Balance}$\n");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Ваши ставки господа!");
                Console.ForegroundColor = ConsoleColor.Gray;
            retry:
                Console.Write("Ставка: ");
                player.Bet = long.Parse(Console.ReadLine());
                if (player.Bet > player.Balance)
                {
                    Console.WriteLine("Не достаточно средств!");
                    goto retry;
                }
                if (player.Bet <= 0)
                {
                    Console.WriteLine("Ставка должна быть больше 0!");
                    goto retry;
                }
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Ставки сделаны!");
                Console.ForegroundColor = ConsoleColor.Gray;
                Thread.Sleep(1000);

                player.GetCard(HeapOfCard);
                player.GetCard(HeapOfCard);

                dealer.GetCard(HeapOfCard);

                //User Way
                while (player.Sum() < 21 && Mode != 2 && Mode != 3)
                {
                    Console.Clear();
                    Console.WriteLine("1-Взять карту\n2-Хватит\n3-Удвоить\n");
                    if (dealer.Cards.Count == 1 && dealer.Sum() == 11)
                    {
                        Console.WriteLine("4-Застраховать\n");
                    }
                    else if (player.Cards.Count == 2 && dealer.Sum() == 21)
                    {
                        Console.WriteLine("4-Одинаковые деньги/Even money(1 к 1)");
                    }
                    Console.WriteLine("Дилер: \n");
                    dealer.GetInfo();
                    Console.WriteLine("Вы: \n");
                    player.GetInfo();
                    
                    Console.Write("Ваш выбор: ");
                    Mode=int.Parse(Console.ReadLine());
                    if (Mode == 1)
                    {
                        player.GetCard(HeapOfCard);
                    }
                    if (Mode == 3)
                    {
                        player.GetCard(HeapOfCard);
                        break;
                    }
                    if (Mode == 4)
                    {
                        player.HasInsurance = true;
                    }
                    else if (dealer.Cards.Count == 1 && dealer.Sum() == 11 && player.Cards.Count == 2 && dealer.Sum() == 21)
                    {
                        Console.Clear();
                        Console.WriteLine("Дилер: \n");
                        dealer.GetInfo();
                        Console.WriteLine("Вы: \n");
                        player.GetInfo();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\nВы выиграли {player.Bet}$");
                        player.Balance += player.Bet;
                        Console.ForegroundColor = ConsoleColor.Gray;
                        File.WriteAllText("Balance", player.Balance.ToString());
                        Console.ReadLine();
                        goto Begin;
                    }
                }
                if (player.Sum() > 21)
                {
                    Console.Clear();
                    Console.WriteLine("Дилер: \n");
                    dealer.GetInfo();
                    Console.WriteLine("Вы: \n");
                    player.GetInfo();
                    if (player.HasInsurance)
                    {
                        player.Balance -= (player.Bet / 2);
                        Console.WriteLine("Страховка оправдалась!");
                    }
                    if (Mode == 3)
                    {
                        player.Balance -= player.Bet * 2;
                    }
                    if (!player.HasInsurance && Mode != 3)
                    {
                        player.Balance -= player.Bet;
                    }
                    File.WriteAllText("Balance", player.Balance.ToString());
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nВы проиграли!");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.ReadLine();
                    goto Begin;
                }
                //Dealer Get Cards
                while (dealer.Sum() < 17)
                {
                    dealer.GetCard(HeapOfCard);

                    Console.Clear();
                    Console.WriteLine("Дилер: \n");
                    dealer.GetInfo();
                    Console.WriteLine("Вы: \n");
                    player.GetInfo();
                }
                
                if (dealer.Sum() > 21)
                {
                    Console.Clear();
                    Console.WriteLine("Дилер: \n");
                    dealer.GetInfo();
                    Console.WriteLine("Вы: \n");
                    player.GetInfo();
                    Console.ForegroundColor = ConsoleColor.Green;
                    if (player.Sum() == 21 && player.Cards.Count == 2)
                    {
                        Console.WriteLine($"\nВы выиграли {(player.Bet + (player.Bet / 2))}$");
                        player.Balance += (player.Bet + (player.Bet / 2));
                    }
                    else if (Mode == 3)
                    {
                        Console.WriteLine($"\nВы выиграли {player.Bet*2}$");
                        player.Balance += player.Bet*2;
                    }
                    else
                    {
                        Console.WriteLine($"\nВы выиграли {player.Bet}$");
                        player.Balance += player.Bet;
                    }
                    Console.ForegroundColor = ConsoleColor.Gray;
                    File.WriteAllText("Balance", player.Balance.ToString());
                    Console.ReadLine();
                    goto Begin;
                }
                if (dealer.Sum() > player.Sum())
                {
                    if (player.HasInsurance)
                    {
                        player.Balance -= (player.Bet / 2);
                        Console.WriteLine("Страховка оправдалась!");
                    }
                    if (Mode == 3)
                    {
                        player.Balance -= player.Bet * 2;
                    }
                    if (!player.HasInsurance && Mode != 3)
                    {
                        player.Balance -= player.Bet;
                    }
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nВы проиграли!");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                if (dealer.Sum() == player.Sum())
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nНичья!");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                if (dealer.Sum() < player.Sum())
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    if (player.Sum() == 21 && player.Cards.Count == 2)
                    {
                        Console.WriteLine($"\nВы выиграли {(player.Bet + (player.Bet / 2))}$");
                        player.Balance += (player.Bet + (player.Bet / 2));
                    }
                    else if (Mode == 3)
                    {
                        Console.WriteLine($"\nВы выиграли {player.Bet * 2}$");
                        player.Balance += player.Bet * 2;
                    }
                    else
                    {
                        Console.WriteLine($"\nВы выиграли {player.Bet}$");
                        player.Balance += player.Bet;
                    }
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                File.WriteAllText("Balance", player.Balance.ToString());
                Console.ReadLine();
            }
        }
    }
}
