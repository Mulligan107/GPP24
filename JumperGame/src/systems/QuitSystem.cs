namespace JumperGame.systems
{
    public class QuitSystem
    {
        private JumperGame _game;

        public QuitSystem(JumperGame game)
        {
            _game = game;
        }

        public void QuitGame()
        {
            _game.IsRunning = false;
        }
    }
}