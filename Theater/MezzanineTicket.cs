namespace Theater
{
    internal class MezzanineTicket : Ticket // класс MezzanineTicket, описывающий билет на бельэтаж
    {
        private static readonly decimal _price = 3999.99M; // цена билета на бельэтаж; для всех билетов на бельэтаж она одинаковая
        public static decimal Price { get { return _price; } } // геттер для _price
        public MezzanineTicket(ulong id, string play, DateTime dateTime, ushort auditorium, ushort seat) : base(id, play, dateTime, auditorium, seat) { }
        public override void Print()
        {
            base.Print();
            Console.WriteLine("Price: {0}", Price);
        }
    }
}