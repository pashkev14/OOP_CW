namespace Game
{
    internal class Card : IDisplayable // класс карты
    {
        private readonly byte _value; // старшинство карты в числовом формате;
                                      // словарь старшинств содержится в Values
        private readonly Values.Suits _suit; // масть карты

        public byte Value { get { return _value; } } // геттер для _value
        public Values.Suits Suit { get { return _suit;} } // геттер для _suit

        public Card(byte val, Values.Suits suit) // конструктор класса Card;
                                                 // присвоение значений происходит
                                                 // только здесь
        {
            _value = val;
            _suit = suit;
        }
        public void Display() // метод вывода карты
        {
            string toPrint = (_value == 10 ? "" : " ") + 
                Values.Ranks[_value] + "|" 
                + _suit.ToString(); // сформировать строку
            Console.Write(toPrint); // и передать ее в вывод
        }
    }
}