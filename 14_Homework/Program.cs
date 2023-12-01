using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _14_Homework
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Создаем экземпляр игры и запускаем ее
            Game game = new Game();
            game.StartGame();
        }
    }

    class Game
    {
        private List<Player> players;  // Список игроков
        private List<Karta> deck;      // Колода карт

        // Конструктор инициализирует игроков, создает и перемешивает колоду, раздает карты игрокам
        public Game()
        {
            players = new List<Player>();
            deck = GenerateDeck();
            ShuffleDeck();
            InitializePlayers(2);  // Инициализируем игру с двумя игроками
            DealCards();
        }

        // Запускает игровой цикл до тех пор, пока игра не завершится, затем определяет победителя
        public void StartGame()
        {
            while (!GameIsOver())
            {
                PlayRound();
            }

            // Определяем победителя по количеству очков
            Player winner = players.OrderByDescending(p => p.GetScore()).First();
            Console.WriteLine($"Игрок {winner.Name} выигрывает игру!");
        }

        // Осуществляет ход в раунде, где каждый игрок кладет одну карту, и выигрывает тот, у кого карта больше
        private void PlayRound()
        {
            // Получаем карты, сыгранные каждым игроком в раунде
            List<Karta> roundCards = players.Select(p => p.PlayCard()).ToList();
            // Определяем победителя раунда
            Player roundWinner = GetRoundWinner(roundCards);
            // Выводим информацию о победителе раунда
            Console.WriteLine($"{roundWinner.Name} выигрывает раунд!");
            // Передаем выигранные карты победителю раунда
            roundWinner.AddCardsToDeck(roundCards);
        }

        // Определяет победителя раунда на основе наивысшей карты
        private Player GetRoundWinner(List<Karta> roundCards)
        {
            return players.OrderByDescending(p => p.GetTopCardValue(roundCards)).First();
        }

        // Проверяет, завершена ли игра, смотря на наличие карт у каждого игрока
        private bool GameIsOver()
        {
            return players.Any(p => p.GetCardCount() == 0);
        }

        // Инициализирует указанное количество игроков с предопределенными именами
        private void InitializePlayers(int playerCount)
        {
            for (int i = 1; i <= playerCount; i++)
            {
                players.Add(new Player($"Игрок {i}"));
            }
        }

        // Раздает карты из колоды игрокам поочередно
        private void DealCards()
        {
            int playerIndex = 0;

            foreach (var card in deck)
            {
                players[playerIndex].AddCard(card);
                playerIndex = (playerIndex + 1) % players.Count;
            }
        }

        // Генерирует колоду карт
        private List<Karta> GenerateDeck()
        {
            List<Karta> newDeck = new List<Karta>();

            foreach (KartaType type in Enum.GetValues(typeof(KartaType)))
            {
                foreach (KartaSuit suit in Enum.GetValues(typeof(KartaSuit)))
                {
                    newDeck.Add(new Karta(type, suit));
                }
            }

            return newDeck;
        }

        // Перемешивает колоду карт с использованием алгоритма Фишера-Йейтса
        private void ShuffleDeck()
        {
            Random random = new Random();
            deck = deck.OrderBy(card => random.Next()).ToList();
        }
    }

    // Представляет игрока в игре
    class Player
    {
        private List<Karta> hand;  // Список карт в руке игрока

        public string Name { get; }  // Имя игрока

        // Конструктор инициализирует игрока с именем и пустой рукой
        public Player(string name)
        {
            Name = name;
            hand = new List<Karta>();
        }

        // Игрок кладет карту из своей руки
        public Karta PlayCard()
        {
            Karta playedCard = hand.First();
            hand.Remove(playedCard);
            return playedCard;
        }

        // Добавляет карту в руку игрока
        public void AddCard(Karta card)
        {
            hand.Add(card);
        }

        // Добавляет список карт в руку игрока
        public void AddCardsToDeck(List<Karta> cards)
        {
            hand.AddRange(cards);
        }

        // Возвращает количество карт в руке игрока
        public int GetCardCount()
        {
            return hand.Count;
        }

        // Возвращает значение верхней карты игрока в данном раунде
        public int GetTopCardValue(List<Karta> roundCards)
        {
            return roundCards.Contains(hand.First()) ? 1 : 0;
        }

        // Возвращает счет игрока, равный количеству карт в его руке
        public int GetScore()
        {
            return hand.Count;
        }
    }

    // Представляет игральную карту
    class Karta
    {
        public KartaType Type { get; }  // Тип карты (например, Шестерка, Семерка, ...)
        public KartaSuit Suit { get; }  // Масть карты (например, Черви, Бубны, ...)

        // Конструктор инициализирует карту типом и мастью
        public Karta(KartaType type, KartaSuit suit)
        {
            Type = type;
            Suit = suit;
        }
    }

    // Перечисление, представляющее различные типы игральных карт
    enum KartaType
    {
        Six = 6,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace
    }

    // Перечисление, представляющее различные масти игральных карт
    enum KartaSuit
    {
        Hearts,
        Diamonds,
        Clubs,
        Spades
    }
}
