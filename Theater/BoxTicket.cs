namespace Theater
{
    internal class BoxTicket : Ticket // класс BoxTicket, описывающий билет в ложу
    {
        private static readonly decimal _price = 14999.99M; // цена билета в ложу; для всех билетов в ложу она одинаковая
        public static decimal Price { get { return _price; } } // геттер для _price
        public BoxTicket(ulong id, string play, DateTime dateTime, ushort auditorium, ushort seat) : base(id, play, dateTime, auditorium, seat) { }
        public override void Print()
        {
            base.Print();
            Console.WriteLine("Price: {0}", Price);
        }
    }
}