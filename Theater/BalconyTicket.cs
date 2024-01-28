namespace Theater
{
    internal class BalconyTicket : Ticket // класс BalconyTicket, описывающий билет на балкон
    {
        private static readonly decimal _price = 2999.99M; // цена билета на балкон; для всех билетов на балкон она одинаковая
        public static decimal Price { get { return _price; } } // геттер для _price
        public BalconyTicket(ulong id, string play, DateTime dateTime, ushort auditorium, ushort seat) : base(id, play, dateTime, auditorium, seat) { }
        public override void Print()
        {
            base.Print();
            Console.WriteLine("Price: {0}", Price);
        }
    }
}