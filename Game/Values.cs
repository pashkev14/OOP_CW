namespace Game
{
    internal struct Values // структура Values,
                           // дающая доступ к старшинствам и мастям
    {
        // словарь перевода числовых значений в привычные нам старшинства
        public static Dictionary<byte, string> Ranks = new() 
        {
            { 6, "6" },
            { 7, "7" },
            { 8, "8" },
            { 9, "9" },
            { 10, "10" },
            { 11, "В" },
            { 12, "Д" },
            { 13, "К" },
            { 14, "Т" }
        };

        public enum Suits { Ч, П, Б, Т }; // масти
    }
}