using System;
using JumperGame.src.components;
using JumperGame.src.components.testComponents;
using JumperGame.src.manager;
using JumperGame.systems;
using SDL2;

namespace JumperGame
{
    public class JumperGame
    {
        private RenderManager _rendering;
        private PhysicsManager _physics;
        private AudioManager _audio;
        private GameObjectManager _gameObjectManager;
        
        private ColorComponent _colorComponent;
        
        private InputSystem _inputSystem;
        private ColorSystem _colorSystem;
        private QuitSystem _quitSystem;
        
        public bool IsRunning;

        static int Main(string[] args)
        {
            ColorComponent colorComponent = new ColorComponent();
            
            JumperGame game = new JumperGame(colorComponent);

            // Start the game loop
            game.Run();

            return 0;
        }
        
        public JumperGame(ColorComponent colorComponent)
        {
            _colorComponent = colorComponent;
            
            _rendering = new RenderManager(_colorComponent);
            _physics = new PhysicsManager();
            _audio = new AudioManager();
            _gameObjectManager = new GameObjectManager();
            
            InitializeSystems();
        }
        
        private void InitializeSystems()
        {
            _inputSystem = new InputSystem();
            _colorSystem = new ColorSystem(_colorComponent);
            _quitSystem = new QuitSystem(this);

            _inputSystem.KeyPressed += _colorSystem.ChangeColor;
            _inputSystem.GameQuitRequested += _quitSystem.QuitGame;
        }
        
        public bool Initialize()
        {
            // Initialize the managers and check if initialization was successful
            bool isRenderingInitSuccessful = _rendering.Initialize();
            bool isPhysicsInitSuccessful = _physics.Initialize();
            bool isAudioInitSuccessful = _audio.Initialize();
            bool isGameObjectManagerInitSuccessful = _gameObjectManager.Initialize();
            
            if (!isRenderingInitSuccessful || !isPhysicsInitSuccessful || !isAudioInitSuccessful || !isGameObjectManagerInitSuccessful)
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
                        _inputSystem.ProcessInput(e);
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
    }
}