namespace Game
{   
    internal class Deck // класс колоды карт
    {
        private List<Card> _cards; // список карт
        private Random _rng = new Random(); // рандомайзер
        
        public List<Card> Cards { get { return _cards; } } // геттер для _cards

        public Deck() // конструктор класса Deck; создает колоду и перемешивает ее
        {
            _cards = new List<Card>();
            for (int i = 6; i < 15; ++i)
            {
                for (int j = 0; j < 4; ++j)
                {
                    _cards.Add(new Card((byte)i, (Values.Suits)j));
                }
            }
            Shuffle();
        }
        private void Shuffle() // метод перемешивания колоды
        {
            int n = _cards.Count;
            while (n > 1)
            {
                --n;
                int k = _rng.Next(n + 1);
                Card value = _cards[k];
                _cards[k] = _cards[n];
                _cards[n] = value;
            }
        }
        public int Size() // метод возврата текущего размера колоды
        {
            return _cards.Count;
        }
        public void GiveCard(Player player) // метод раздачи карты из колоды игроку
        {
            player.AddCard(_cards.First());
            RemoveCard();
        }
        private void RemoveCard() // метод удаления карты из колоды
        {
            _cards.Remove(_cards.First());
        }
    }
}