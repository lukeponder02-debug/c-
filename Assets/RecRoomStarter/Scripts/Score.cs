namespace RecRoomStarter
{
    // Keeps track of the player's points. "static" means there's only one score for the whole game.
    public static class Score
    {
        public static int Points { get; private set; }

        public static void Add(int amount) => Points += amount;

        public static void Reset() => Points = 0;
    }
}
