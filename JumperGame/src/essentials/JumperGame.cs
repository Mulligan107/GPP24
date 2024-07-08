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
        private entitySystem _entitySystem;
        private RenderManager _rendering;
        private AudioManager _audio;
        
        private PhysicsSystem _physicsSystem;
        private InputSystem _inputSystem;
        private ColorSystem _colorSystem;
        private QuitSystem _quitSystem;
        
        private ColorComponent _colorComponent;
        
        public bool IsRunning;

        static int Main(string[] args)
        {
            JumperGame game = new JumperGame();
            
            //Initialize the game
            game.InitializeSystems();
            
            // Start the game loop
            game.Run();

            return 0;
        }
        
        private void InitializeSystems()
        {
            _colorComponent = new ColorComponent();
            
            _entitySystem = new entitySystem();
            _rendering = new RenderManager(_colorComponent);
            _physicsSystem = new PhysicsSystem();
            _audio = new AudioManager();
            
            _inputSystem = new InputSystem();
            _colorSystem = new ColorSystem(_colorComponent);
            _quitSystem = new QuitSystem(this);

            _inputSystem.KeyPressed += _colorSystem.ChangeColor;
            _inputSystem.GameQuitRequested += _quitSystem.QuitGame;
        }
        
        public bool InitializeSdl()
        {
            // Initialize the managers and check if initialization was successful
            bool isRenderingInitSuccessful = _rendering.Initialize();
            bool isPhysicsInitSuccessful = _physicsSystem.Initialize();
            bool isAudioInitSuccessful = _audio.Initialize();
            
            if (!isRenderingInitSuccessful || !isPhysicsInitSuccessful || !isAudioInitSuccessful)
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
            if (InitializeSdl())
            {
                SDL.SDL_Event e;
                while (IsRunning)
                {
                    while (SDL.SDL_PollEvent(out e) != 0)
                    {
                        // Process the input events
                        _inputSystem.ProcessInput(e);
                    }
            
                    // Retrieve all entities
                    var entities = _entitySystem.GetAllEntities();

                    // Update the managers
                    _rendering.Update();
                    _physicsSystem.Update(entities/*, deltaTime*/);
                    
                    //_entitySystem.Update(deltaTime);
                    
                    // _audio.Update();
                    // _input.Update();
                }
            }
        }
    }
}