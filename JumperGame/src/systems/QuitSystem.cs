namespace JumperGame.systems
{
    public class QuitSystem
    {
        private essentials.JumperGame _game;

        public QuitSystem(essentials.JumperGame game)
        {
            _game = game;
        }

        public void QuitGame()
        {
            _game.IsRunning = false;
        }
    }
}