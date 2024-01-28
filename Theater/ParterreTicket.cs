namespace Theater
{
    internal class ParterreTicket : Ticket // класс ParterreTicket, описывающий билет в партер
    {
        private static readonly decimal _price = 5999.99M; // цена билета в партер; для всех билетов в партер она одинаковая
        public static decimal Price { get { return _price; } } // геттер для _price
        public ParterreTicket(ulong id, string play, DateTime dateTime, ushort auditorium, ushort seat) : base(id, play, dateTime, auditorium, seat) { }
        public override void Print()
        {
            base.Print();
            Console.WriteLine("Price: {0}", Price);
        }
    }
}