namespace Theater
{
    internal class AmphitheaterTicket : Ticket // класс AmphitheaterTicket, описывающий билет в амфитеатр
    {
        private static readonly decimal _price = 4999.99M; // цена билета в амфитеатр; для всех билетов в амфитеатр она одинаковая
        public static decimal Price { get { return _price; } } // геттер для _price
        public AmphitheaterTicket(ulong id, string play, DateTime dateTime, ushort auditorium, ushort seat) : base(id, play, dateTime, auditorium, seat) { }
        public override void Print()
        {
            base.Print();
            Console.WriteLine("Price: {0}", Price);
        }
    }
}