namespace Theater
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ParterreTicket p_ticket = new ParterreTicket(33227280, "Swan Lake", new DateTime(2024, 02, 24, 14, 00, 00), 3, 94);
            BalconyTicket blc_ticket = new BalconyTicket(26613214, "Swan Lake", new DateTime(2024, 03, 15, 17, 00, 00), 1, 12);
            BoxTicket bx_ticket = new BoxTicket(46415333, "Swan Lake", new DateTime(2024, 03, 17, 19, 00, 00), 2, 1);
            MezzanineTicket m_ticket = new MezzanineTicket(89574542, "Swan Lake", new DateTime(2024, 03, 16, 20, 00, 00), 1, 42);
            p_ticket.Print();
            Console.WriteLine();
            blc_ticket.Print();
            Console.WriteLine();
            bx_ticket.Print();
            Console.WriteLine();
            m_ticket.Print();
        }
    }
}