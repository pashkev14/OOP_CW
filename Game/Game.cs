namespace Game
{
    internal class Game : IDisplayable // класс игры
    {
        private Deck _deck; // колода
        private Card _trump; // козырь
        private Player _p1; // игрок 1
        private Player _p2; // игрок 2
        private Player _attacker; // кто нападает
        private Player _turn; // кто ходит
        private (Card, Card)[] _field; // игровой стол
        private List<Card> _dropped; // бито
        private byte _count = 0; // сколько карт было выложено на стол
        private byte _covered = 0; // сколько карт было покрыто
        private bool _game = true; // идет ли игра или нет
        private StreamWriter _log; // запись логов игры

        // конструктор класса Game; здесь же и происходит игра
        // и обработка внутриигровых событий
        public Game()
        {
            // интерфейс для нападающего игрока
            string msg_attack = "Что хотите сделать?\n1 - Подкинуть карту\n" +
                                "2 - Передать ход\n3 - Объявить бито\n" +
                                "Нажмите на клавишу с цифрой для выбора действия.\n" +
                                "Нажмите Esc, чтобы закончить игру.";
            // интерфейс для отбивающегося игрока
            string msg_defend = "Что хотите сделать?\n1 - Побить карту\n2 - Перевести\n" +
                                "3 - Забрать карты\n4 - Передать ход\n" +
                                "Нажмите на клавишу с цифрой для выбора действия.\n" +
                                "Нажмите Esc, чтобы выйти из игры.";
            // инициализация полей класса
            _deck = new Deck();
            _trump = _deck.Cards.Last();
            _p1 = new Player();
            _p2 = new Player();
            _field = new (Card, Card)[6];
            _dropped = new List<Card>();
            _log = new StreamWriter(Directory.GetCurrentDirectory()
                + "\\" + "log.txt");
            _log.WriteLine("Игра началась.");
            _log.WriteLine("Козырем стала карта " +
                Values.Ranks[_trump.Value] + '|' +
                _trump.Suit.ToString() + ".");
            // раздача карт и определение младшего козыря
            for (int i = 0; i < 6; ++i)
            {
                _deck.GiveCard(_p1);
                _deck.GiveCard(_p2);
            }
            if (_p1.MinTrump(_trump) <= _p2.MinTrump(_trump)) // у игрока 1 есть козырь
                                                              // младше козырей игрока 2,
            {                                                 // либо у них обоих нет козырей
                _turn = _p1; // передаем ход игроку 1
                _attacker = _p1; // и делаем его нападающим
                _log.WriteLine("Начинает игрок 1.");
            }
            else
            {
                _turn = _p2; // передаем ход игроку 2
                _attacker = _p2; // и делаем его нападающим
                _log.WriteLine("Начинает игрок 2.");
            }
            ConsoleKey key_main, key_second; // у меню два уровня вложенности,
                                             // поэтому и код имеет две переменные
                                             // сама игра
            while (_game)
            {
                Display();
                Console.Write("Игрок {0}, ", (_turn == _p1 ? 1 : 2));
                // обработка действий нападающего игрока
                if (_turn == _attacker)
                {
                    Console.WriteLine("нападайте!");
                    Console.WriteLine(msg_attack);
                    key_main = Console.ReadKey().Key;
                    switch (key_main)
                    {
                        case (ConsoleKey.Escape):
                            _game = false;
                            _log.WriteLine("Игра завершилась досрочно.");
                            break;
                        case (ConsoleKey.D1):
                            Display();
                            Console.WriteLine("Выберите карту из своей руки, " +
                                "которую хотите подкинуть:");
                            _turn.Display();
                            Console.WriteLine("\nНажмите 0, если хотите вернуться назад.");
                            key_second = Console.ReadKey().Key;
                            if (key_second == ConsoleKey.D0)
                            {
                                break;
                            }
                            Attack((int)key_second % 49);
                            break;
                        case (ConsoleKey.D2):
                            MoveToDefender();
                            break;
                        case (ConsoleKey.D3):
                            Drop();
                            break;
                    }
                }
                // обработка действий отбивающегося игрока
                else
                {
                    Console.WriteLine("отбивайтесь!");
                    Console.WriteLine(msg_defend);
                    key_main = Console.ReadKey().Key;
                    switch (key_main)
                    {
                        case (ConsoleKey.Escape):
                            _game = false;
                            _log.WriteLine("Игра завершилась досрочно.");
                            break;
                        case (ConsoleKey.D1):
                            Display();
                            Console.WriteLine("Выберите карту из своей руки, " +
                                "которой хотите покрыть:");
                            _turn.Display();
                            Console.WriteLine("\nНажмите 0, если хотите вернуться назад.");
                            key_second = Console.ReadKey().Key;
                            if (key_second == ConsoleKey.D0)
                            {
                                break;
                            }
                            Cover((int)key_second % 49);
                            break;
                        case (ConsoleKey.D2):
                            Display();
                            Console.WriteLine("Выберите карту из своей руки, " +
                                "которой хотите перевести:");
                            _turn.Display();
                            Console.WriteLine("\nНажмите 0, если хотите вернуться назад.");
                            key_second = Console.ReadKey().Key;
                            if (key_second == ConsoleKey.D0)
                            {
                                break;
                            }
                            Turn((int)key_second % 49);
                            break;
                        case (ConsoleKey.D3):
                            Take();
                            break;
                        case (ConsoleKey.D4):
                            MoveToAttacker();
                            break;
                    }
                }
            }
            // после игры закрываем файл протокола
            _log.Close();
        }
        // метод, определяющий отбивающегося игрока на текущем кону
        private Player Defender()
        {
            // если ходит нападающий, нужно вернуть другого игрока
            if (_turn == _attacker)
            {
                return (_turn == _p1 ? _p2 : _p1);
            }
            // в противном случае возвращаем ходящего игрока
            return _turn;
        }
        // метод, производящий попытку подкинуть карту в стол
        private void Attack(int index)
        {
            /* Условия допустимости подкидывания:
             * 1. Подкинутых карт меньше 6
             * 2. У отбивающегося игрока хватит карт покрыться
             * 3. Карта совпадает по старшинству с любой из выложенных в стол
             */
            if (_count < 6
                && _count - _covered <= Defender().SizeOfHand()
                && index <= _attacker.SizeOfHand() - 1
                && (_count == 0 || _field.Any(x => (x.Item1?.Value == _attacker.Hand[index].Value
                                              || x.Item2?.Value == _attacker.Hand[index].Value))))
            {
                // сформировать сообщение о сделанном ходе
                string log = "Игрок " + (_attacker == _p1 ? "1" : "2")
                    + " подкинул карту " + Values.Ranks[_p1.Hand[index].Value]
                    + '|' + _p1.Hand[index].Suit.ToString() + ".";
                // и записать его в протокол
                _log.WriteLine(log);
                // добавить на стол выбранную карту
                _field[_count].Item1 = _attacker.Hand[index];
                // и убрать ее у игрока
                _attacker.RemoveCard(index);
            }
        }
        // метод, производящий попытку передать ход отбивающемуся игроку
        private void MoveToDefender()
        {
            /* Условия допустимости передачи хода отбивающемуся:
             * 1. Нападающий подкинул хотя бы одну карту
             * 2. На стол еще не было выложено 12 карт
             */
            if (_count > 0 && (_count + _covered != 12))
            {
                // сформировать сообщение о сделанном ходе
                string log = "Игрок" + (_attacker == _p1 ? "1" : "2")
                    + " передал ход игроку" + (_attacker == _p1 ? "2" : "1") + ".";
                // и записать его в протокол
                _log.WriteLine(log);
                // передать ход отбивающемуся
                _turn = Defender();
            }
        }
        // метод, производящий попытку сбросить карты в бито и закончить кон
        private void Drop()
        {
            /* Условия допустимости объявления бито:
             * 1. Нападающий подкинул хотя бы одну карту
             * 2. Все подкинутые карты были покрыты
             */
            if (_count > 0 && _count == _covered)
            {
                // сформировать сообщение о сделанном ходе
                string log = "Игрок "
                    + (_attacker == _p1 ? "1" : "2") + " объявил бито.";
                // и записать его в протокол
                _log.WriteLine(log);
                // определим отбивающегося - он понадобится дальше
                Player _defender = Defender();
                // по правилам первый получает карты нападавший
                while (_deck.Size() > 0 && _attacker.SizeOfHand() < 6)
                {
                    _deck.GiveCard(_attacker);
                }
                // затем отбивавшийся
                while (_deck.Size() > 0 && _defender.SizeOfHand() < 6)
                {
                    _deck.GiveCard(_defender);
                }
                // если в колоде не осталось карт
                // и у игроков не осталось карт, то это ничья, игра заканчивается
                if (_deck.Size() == 0 
                    && _attacker.SizeOfHand() == 0 
                    && _defender.SizeOfHand() == 0)
                {
                    Console.WriteLine("Ничья!");
                    _log.WriteLine("Игра закончилась ничьей.");
                    _game = false;
                    return;
                }
                // сбросить все карты со стола в бито
                _dropped.AddRange(_field.Select(x => x.Item1).Where(x => x != null).ToList());
                _dropped.AddRange(_field.Select(x => x.Item2).Where(x => x != null).ToList());
                // и очистить стол
                for (int i = 0; i < 6; ++i)
                {
                    _field[i] = (null, null);
                }
                // передать ход
                _attacker = _defender;
                _turn = _attacker;
                // обнулить счетчики карт
                _count = 0;
                _covered = 0;
            }
        }
        // метод, производящий попытку покрыть картой
        private void Cover(int index)
        {
            /* Условия допустимости покрытия картой:
             * 1. Не все карты еще были покрыты
             * 2. Выбранная карта совпадает с покрываемой по масти и старше нее
             *    ИЛИ
             * 3. Выбранная карта козырная, покрываемая не козырная
             */
            if (_covered < _count && index <= _turn.SizeOfHand() - 1
                && ((_turn.Hand[index].Suit == _field[_covered].Item1.Suit
                && _turn.Hand[index].Value > _field[_covered].Item1.Value)
                || (_turn.Hand[index].Suit == _trump.Suit
                && _field[_covered].Item1.Suit != _trump.Suit)))
            {
                // сформировать сообщение о сделанном ходе
                string log = "Игрок " + (_turn == _p1 ? "1" : "2") + " покрыл карту"
                    + Values.Ranks[_field[_covered].Item1.Value] + '|'
                    + _field[_covered].Item1.Suit.ToString() + " картой "
                    + Values.Ranks[_turn.Hand[index].Value] + '|'
                    + _turn.Hand[index].Suit.ToString();
                // и записать его в протокол
                _log.WriteLine(log);
                // выложить выбранную карту на стол
                _field[_covered].Item2 = _p1.Hand[index];
                // убрать ее из руки игрока
                _turn.RemoveCard(index);
                ++_covered;
            }
        }
        // метод, производящий попытку перевести карты
        private void Turn(int index)
        {
            /* Условия допустимости перевода:
             * 1. Еще ни одной карты не было покрыто
             * 2. Выбранная карта имеет то же старшинство, что и подкинутые
             */
            if (_covered == 0
                && index <= _turn.SizeOfHand() - 1
                && _turn.Hand[index].Value == _field[0].Item1.Value)
            {
                // сформировать сообщение о сделанном ходе
                string log = "Игрок " + (_turn == _p1 ? "1" : "2")
                    + " перевел картой " + Values.Ranks[_turn.Hand[index].Value]
                    + '|' + _p1.Hand[index].Suit.ToString();
                // и записать его в протокол
                _log.WriteLine(log);
                // добавить выбранную карту на стол
                _field[_count].Item1 = _turn.Hand[index];
                // убрать ее из руки игрока
                _turn.RemoveCard(index);
                // перевести стрелки и передать ход
                ++_count;
                Player _defender = Defender();
                _turn = _attacker;
                _attacker = _defender;
            }
        }
        // метод, производящий попытку взять карты и закончить кон
        private void Take()
        {
            /* Условия, допускающие взятие карт:
             * 1. Еще не все карты были побиты
             */
            if (_count != _covered)
            {
                // если колода пуста и рука нападавшего пуста, нападавший победил
                if (_deck.Size() == 0 && _attacker.SizeOfHand() == 0)
                {
                    // вывести сообщение о победе на экран
                    Console.WriteLine("Игрок {0} победил!", _attacker == _p1 ? "1" : "2");
                    // сформировать сообщение о победе
                    string msg = "В игре победил игрок " + (_attacker == _p1 ? "1" : "2") + ".";
                    // и записать его в протокол
                    _log.WriteLine(msg);
                    _game = false;
                    return;
                }
                // сформировать сообщение о сделанном ходе
                string log = "Игрок " + (_turn == _p1 ? "1" : "2") + " забрал карты.";
                // и записать его в протокол
                _log.WriteLine(log);
                // отдать отбивавшемуся игроку все карты со стола
                _turn.Hand.AddRange(_field.Select(x => x.Item1).Where(x => x != null).ToList());
                _turn.Hand.AddRange(_field.Select(x => x.Item2).Where(x => x != null).ToList());
                // докинуть карты нападавшему
                while (_deck.Size() > 0 && _attacker.SizeOfHand() < 6)
                {
                    _deck.GiveCard(_attacker);
                }
                // докинуть карты отбивавшемуся
                while (_deck.Size() > 0 && _turn.SizeOfHand() < 6)
                {
                    _deck.GiveCard(_turn);
                }
                // очистить стол
                for (int i = 0; i < 6; ++i)
                {
                    _field[i] = (null, null);
                }
                // обнулить счетчики, передать ход
                _count = 0;
                _covered = 0;
                _turn = _attacker;
            }
        }
        // метод, производящий попытку передать ход нападающему игроку
        private void MoveToAttacker()
        {
            /* Условия допустимости передачи хода:
             * 1. Все выложенные карты были побиты
             */
            if (_covered == _count)
            {
                // сформировать сообщение о сделанном ходе
                string log = "Игрок " + (_turn == _p1 ? "1" : "2") 
                    + " передал ход игроку " + (_turn == _p1 ? "2" : "1") + ".";
                // и записать его в протокол
                _log.WriteLine(log);
                _turn = _attacker;
            }
        }
        // метод вывода игрового стола
        public void Display()
        {
            Console.Clear();
            // строка 1
            Console.WriteLine("Игровой стол:");
            // строка 2
            Console.WriteLine("+----------------------+");
            // строка 3
            Console.WriteLine("|                      |");
            // строка 4
            Console.Write("| ");
            if (_field[0].Item2 != null) { _field[0].Item2.Display(); }
            else { Console.Write("    "); }
            Console.Write("   ");
            if (_field[1].Item2 != null) { _field[1].Item2.Display(); }
            else { Console.Write("    "); }
            Console.Write("   ");
            if (_field[2].Item2 != null) { _field[2].Item2.Display(); }
            else { Console.Write("    "); }
            Console.WriteLine("   |  +" + (_trump.Value == 10 ? "------+" : "-----+"));
            // строка 5
            Console.Write("|   ");
            if (_field[0].Item1 != null) { _field[0].Item1.Display(); }
            else { Console.Write("    "); }
            Console.Write("   ");
            if (_field[1].Item1 != null) { _field[1].Item1.Display(); }
            else { Console.Write("    "); }
            Console.Write("   ");
            if (_field[2].Item1 != null) { _field[2].Item1.Display(); }
            else { Console.Write("    "); }
            Console.WriteLine(" |  |" + (_trump.Value == 10 ? "      |" : "     |"));
            // строка 6
            Console.WriteLine("|                      |  | " +
                Values.Ranks[_trump.Value] + "|" + _trump.Suit.ToString() + " |");
            // строка 7
            Console.Write("| ");
            if (_field[3].Item2 != null) { _field[3].Item2.Display(); }
            else { Console.Write("    "); }
            Console.Write("   ");
            if (_field[4].Item2 != null) { _field[4].Item2.Display(); }
            else { Console.Write("    "); }
            Console.Write("   ");
            if (_field[5].Item2 != null) { _field[5].Item2.Display(); }
            else { Console.Write("    "); }
            Console.WriteLine("   |  |" + (_trump.Value == 10 ? "      |" : "     |"));
            // строка 8
            Console.Write("|   ");
            if (_field[3].Item1 != null) { _field[3].Item1.Display(); }
            else { Console.Write("    "); }
            Console.Write("   ");
            if (_field[4].Item1 != null) { _field[4].Item1.Display(); }
            else { Console.Write("    "); }
            Console.Write("   ");
            if (_field[5].Item1 != null) { _field[5].Item1.Display(); }
            else { Console.Write("    "); }
            Console.WriteLine(" |  +" + (_trump.Value == 10 ? "------+" : "-----+"));
            // строки 9 и 10
            Console.WriteLine("|                      |\n+----------------------+");
        }
    }
}