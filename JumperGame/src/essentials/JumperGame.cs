using System;
using JumperGame.src.components;
using JumperGame.src.components.testComponents;
using JumperGame.src.manager;
using SDL2;

namespace JumperGame
{
    public class JumperGame
    {
        private RenderManager _rendering;
        private PhysicsManager _physics;
        private AudioManager _audio;
        private InputManager _input;
        private GameObjectManager _gameObjectManager;
        private InputComponent _inputComponent;
        
        public bool IsRunning;

        public JumperGame(ColorComponent colorComponent)
        {
            _rendering = new RenderManager(colorComponent);
            _physics = new PhysicsManager();
            _audio = new AudioManager();
            _input = new InputManager();
            _gameObjectManager = new GameObjectManager();
            
            _inputComponent = new InputComponent(colorComponent, this);
        }
        
        static int Main(string[] args)
        {
            ColorComponent colorComponent = new ColorComponent();
            JumperGame game = new JumperGame(colorComponent);

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
            IsRunning = true;
            
            // Run the game loop
            if (Initialize())
            {
                SDL.SDL_Event e;
                while (IsRunning)
                {
                    while (SDL.SDL_PollEvent(out e) != 0)
                    {
                        // Process the input events
                        _inputComponent.ProcessInput(e);
                    }
                    
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