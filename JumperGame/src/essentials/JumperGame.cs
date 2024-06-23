using System;
using JumperGame.src.components;
using JumperGame.src.manager;

namespace JumperGame
{
    public class JumperGame
    {
        private RenderManager _rendering = new RenderManager();
        private PhysicsManager _physics = new PhysicsManager();
        private AudioManager _audio = new AudioManager();
        private InputManager _input = new InputManager();
        private GameObjectManager _gameObjectManager = new GameObjectManager();
        
        static int Main(string[] args)
        {
            JumperGame game = new JumperGame();
            
            // Start the game loop
            game.Run();

            // Shutdown the game
            game.Close();

            return 0;
        }
        
        public bool Initialize()
        {
            // Initialize the managers and check if initialization was successful
            bool isRenderingInitSuccessful = _rendering.Initialize();
            bool isPhysicsInitSuccessful = _physics.Initialize();
            bool isAudioInitSuccessful = _audio.Initialize();
            bool isInputInitSuccessful = _input.Initialize();
            bool isGameObjectManagerInitSuccessful = _gameObjectManager.Initialize();
            
            if (!isRenderingInitSuccessful || !isPhysicsInitSuccessful || !isAudioInitSuccessful || !isInputInitSuccessful || !isGameObjectManagerInitSuccessful)
            {
                Console.WriteLine("Failed to initialize the game!");
                Console.ReadLine();
                return false;
            }

            // If all initializations were successful, return true
            return true;
        }
        
        public void Run()
        {
            // Run the game loop
            if (Initialize())
            {
                while (true)
                {
                    // Update the managers
                    _rendering.Update();
                    //_physics.Update();
                    //_audio.Update();
                    //_input.Update();
                    //_gameObjectManager.Update();
                }
            }
        }
        
        public void Close()
        {
            // Shutdown the managers
            //_rendering.Close();
            //_physics.Close();
            //_audio.Close();
            //_input.Close();
            //_gameObjectManager.Close();
        }
    }
}