namespace Theater
{
    internal abstract class Ticket : IPrintable // абстрактный класс Ticket, описывающий общую модель билета и реализующий интерфейс IPrintable для презентабельного вывода информации о билете
    {
        protected readonly ulong _id; // идентификационный номер билета
        protected readonly string _play; // название постановки
        protected readonly DateTime _datetime; // дата и время постановки
        protected readonly ushort _auditorium; // номер зала
        protected readonly ushort _seat; // номер места в зале
        public ulong ID { get { return _id; } } // геттер для _id
        public string Play { get { return _play; } } // геттер для _play
        public DateTime DateTime { get { return _datetime; } } // геттер для _datetime
        public ushort Auditorium { get { return _auditorium; } } // геттер для _auditorium
        public ushort Seat { get { return _seat; } } // геттер для _seat
        public Ticket(ulong id, string play, DateTime dateTime, ushort auditorium, ushort seat) // конструктор класса
        {
            _id = id;
            _play = play;
            _datetime = dateTime;
            _auditorium = auditorium;
            _seat = seat;
        }
        public string SeatType()
        {
            string type = GetType().Name.ToString();
            byte index = 1;
            while (!char.IsUpper(type[index]))
            {
                ++index;
            }
            return type.ToString().Substring(0, index);
        }
        public virtual void Print() // реализация интерфейсного метода Print; выводит информацию о билете
        {
            Console.WriteLine("Ticket #{0}\nPlay: {1}\nDate/Time: {2}\nAuditorium: {3}\nSeat number: {4}\nType of seat: {5}", ID, Play, DateTime, Auditorium, Seat, SeatType());
        }
    }
}