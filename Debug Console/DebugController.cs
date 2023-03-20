using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

namespace Team11.DebugConsole
{
    public class DebugController : MonoBehaviour, InputScheme.IDebugActions
    {
        private bool _showConsole;
        private bool _showHelp;

        private InputScheme _controls;
        private string _input;
        // public static DebugCommand x;
        // public static DebugCommand<int> x2;
        public static DebugCommand help;

        public List<object> commandList;

        private Vector2 scroll;
        
        public static DebugController instance;
        private void Awake()
        {
            _controls = new InputScheme();
            _controls.Debug.Enable();
            // _controls.Debug.SetCallbacks(this);
            _controls.Debug.ToggleConsole.performed += OnToggleConsole;
            _controls.Debug.Return.performed += OnReturn;
    
            // x = new DebugCommand("test", "Test Cheat", "test", () => Debug.Log("Test"));
            // x2 = new DebugCommand<int>("test2", "Test Cheat 2", "test2 <int>", (value) => Debug.Log("Test "+ value));
            
            help = new DebugCommand("help", "Show a list of commands", "help",
                () =>
                {
                    _showHelp = !_showHelp;
                }
            );
            
            commandList = new List<object>
            {
                // x,
                // x2,
                help
            };

            instance = this;
        }

        private void OnDestroy()
        {
            _controls.Debug.ToggleConsole.performed -= OnToggleConsole;
            _controls.Debug.Return.performed -= OnReturn;
            _controls.Debug.Disable();
        }

        public void OnToggleConsole(InputAction.CallbackContext _)
        {
            _showConsole = !_showConsole;
            _input = "";
        }

        public void OnReturn(InputAction.CallbackContext _)
        {
            if (_showConsole)
            {
                if (!_input.Equals("help"))
                {
                    _showConsole = false;
                    _showHelp = false;
                }
                HandleInput();
                _input = "";
            }
        }
        
        private void OnGUI()
        {
            if (!_showConsole)
            {
                return;
            }

            float y = 0;

            if (_showHelp)
            {
                GUI.Box(new Rect(0,y,Screen.width,100),"");

                Rect viewport = new Rect(0, 0, Screen.width - 30, 20 * commandList.Count);

                scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, 90), scroll, viewport);

                for (int i = 0; i < commandList.Count; i++)
                {
                   DebugCommandBase command = commandList[i] as DebugCommandBase;
                   string label = $"{command.CommandFormat} - {command.CommandDescription}";
                   Rect labelRect = new Rect(5, 20 * i, viewport.width - 100, 20);
                   GUI.Label(labelRect,label);
                }
                
                GUI.EndScrollView();
                
                y += 100;
            }
            
            GUI.Box(new Rect(0,y,Screen.width,30),"");
            GUI.backgroundColor = new Color(0,0,0,0);
            GUI.SetNextControlName("DebugText");
            _input = GUI.TextField(new Rect(10f,y+5f, Screen.width-20f,20f),_input);
            GUI.FocusControl("DebugText");
            if (string.IsNullOrEmpty(_input))
                GUI.TextArea(new Rect(10f, y + 5f, Screen.width - 20f, 20f), "use 'help' to see a list of all commands");
        }

        private void HandleInput()
        {
            string[] properties = _input.Split(' ');
            
            for (int i = 0; i < commandList.Count; i++)
            {
                DebugCommandBase commandBase = commandList[i] as DebugCommandBase;
                if (_input.Contains(commandBase.CommandId))
                {
                    if (commandList[i] is DebugCommand)
                    {
                        (commandList[i] as DebugCommand)?.Invoke();
                    }
                    else if (commandList[i] is DebugCommand<int>)
                    {
                        (commandList[i] as DebugCommand<int>)?.Invoke(int.Parse(properties[1]));
                    }
                    else if (commandList[i] is DebugCommand<bool>)
                    {
                        (commandList[i] as DebugCommand<bool>)?.Invoke(bool.Parse(properties[1]));
                    }
                }
            }
        }
    }
}