namespace Game
{
    internal class Player : IDisplayable // класс игрока
    {
        private List<Card> _hand; // рука игрока
        public List<Card> Hand { get { return _hand; } } // геттер для _hand

        public Player() // конструктор класса Player
        {
            _hand = new List<Card>();
        }
        public void AddCard(Card card) // метод добавления карты в руку игрока
        {
            _hand.Add(card);
        }
        public void RemoveCard(int index) // метод удаления карты с руки игрока по индексу
        {
            if (index <= _hand.Count - 1) 
            {
                _hand.RemoveAt(index);
            }
        }
        public int SizeOfHand() // метод, возвращающий кол-во карт в руке игрока
        {
            return _hand.Count;
        }
        public byte MinTrump(Card trump) // метод, подсчитывающий младший козырь в руке игрока
        {
            byte min = 15; // туз имеет значение 14, значит, если у игрока не было обнаружено
                           // ни одного козыря, вернется 15
            foreach (Card card in _hand)
            {
                if (card.Suit == trump.Suit && card.Value < min) // если очередная карта совпадает
                                                                 // по масти с козырем
                                                                 // и ее значение меньше 15,
                                                                 // это новый младший козырь
                {
                    min = card.Value;
                }
            }
            return min; // в конце вернуть найденное значение
        }
        public void Display() // метод вывода имеющихся у игрока карт
        {
            if (_hand.Count == 0) // если у игрока нет карт, ничего не выводим
            {
                Console.Write("");
                return;
            }
            for (int i = 0; i < _hand.Count; ++i)
            {
                Console.Write("[" + (i + 1).ToString() + "]" 
                    + " - " + Values.Ranks[_hand[i].Value] + "|" 
                    + _hand[i].Suit.ToString() + " ");
            }
        }
    }
}