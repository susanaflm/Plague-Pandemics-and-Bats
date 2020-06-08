using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace IPCA.KeyboardManager {

    public class KeyboardManager : GameComponent {

        // SINGLETON

        // Padrão no desenvolvimento de software Orientado a Objetos

        // Permitir o acesso ubiquo a uma instancia de uma classe
        // Permitir o acesso à UNICA instância de uma classe.

        internal enum KeyState { Down, Up, GoingDown, GoingUp }

        /*
            Associar a cada tecla o seu estado na frame atual
             - Premida  (down)
             - Libertada  (up)
             - A ser premida (goingdown)
             - a ler libertada (goingup)
 
            A -> { UP
                   goingUp -> [ f1(), f2(); ],
                   down -> [ f3(); ] }
            S -> GOINGDOWN
            E -> DOWN
 
        */

        internal class KeyActions {
            internal KeyState state;
            internal Dictionary <KeyState, List<Action>> actions;
            internal KeyActions (KeyState initialState) {
                state = initialState;
                actions = new Dictionary<KeyState, List<Action>>();
                foreach (KeyState ks in Enum.GetValues(typeof(KeyState)))
                {
                    actions[ks] = new List<Action>();
                }
            }
        }

        public static KeyboardManager instance;

        Dictionary<Keys, KeyActions> keyState;

        public KeyboardManager(Game game) : base(game)
        {
            keyState = new Dictionary<Keys, KeyActions>();
            if (KeyboardManager.instance == null) {
                KeyboardManager.instance = this;
            } else {
                throw new System.Exception("Singleton with more than one instance for KeyboardManager");
            }
        }


        public override void Update(GameTime gameTime)
        {
            // Detetar mudanças de estado nas teclas... (todas)

            // Obter todas as teclas premidas neste momento.
            Keys[] pressedKeys = Keyboard.GetState().GetPressedKeys();

            foreach (Keys key in pressedKeys) {
                // A tecla é conhecida?
                if (keyState.ContainsKey(key)) {
                    switch (keyState[key].state)
                    {
                        case KeyState.Down:
                        case KeyState.GoingDown:
                            keyState[key].state = KeyState.Down;
                            break;
                        case KeyState.Up:
                        case KeyState.GoingUp:
                            keyState[key].state = KeyState.GoingDown;
                            break;
                    }
                }
                else {
                    keyState[key] = new KeyActions(KeyState.GoingDown);
                }
            }

            // teclas que conhecemos mas que nao estao premidas
            foreach (Keys key in keyState.Keys.ToArray())
            {
                if (!pressedKeys.Contains(key)) {
                    switch (keyState[key].state)
                    {
                        case KeyState.GoingDown:
                        case KeyState.Down:
                            keyState[key].state = KeyState.GoingUp;
                            break;
                        case KeyState.Up:
                        case KeyState.GoingUp:
                            keyState[key].state = KeyState.Up;
                            break;
                    }
                }
            }

            // executar eventos
            foreach (Keys key in keyState.Keys)
            {
                KeyState currentKeyState = keyState[key].state;
                foreach (Action a in keyState[key].actions[ currentKeyState ]) 
                {
                    a();
                }
            }

        }

        bool _IsKeyDown(Keys k) {
            return keyState.ContainsKey(k) && (keyState[k].state == KeyState.Down || keyState[k].state == KeyState.GoingDown);
        }
        bool _IsKeyUp(Keys k) {
            return !keyState.ContainsKey(k) || keyState[k].state == KeyState.Up || keyState[k].state == KeyState.GoingUp;
        }
        bool _IsKeyGoingDown(Keys k) {
            return keyState.ContainsKey(k) && keyState[k].state == KeyState.GoingDown;
        }
        bool _IsKeyGoingUp(Keys k) {
            return keyState.ContainsKey(k) && keyState[k].state == KeyState.GoingUp;
        }

        public static bool IsKeyDown(Keys k) { return KeyboardManager.instance._IsKeyDown(k); }
        public static bool IsKeyGoingDown(Keys k) { return KeyboardManager.instance._IsKeyGoingDown(k); }
        public static bool IsKeyUp(Keys k) { return KeyboardManager.instance._IsKeyUp(k); }
        public static bool IsKeyGoingUp(Keys k) { return KeyboardManager.instance._IsKeyGoingUp(k); }

        public static void SetDownAction(Keys k, Action a) { KeyboardManager.instance._SetAction(KeyState.Down, k, a); }
        public static void SetGoingDownAction(Keys k, Action a) { KeyboardManager.instance._SetAction(KeyState.GoingDown, k, a); }
        public static void SetUpAction(Keys k, Action a) { KeyboardManager.instance._SetAction(KeyState.Up, k, a); }
        public static void SetGoingUpAction(Keys k, Action a) { KeyboardManager.instance._SetAction(KeyState.GoingUp, k, a); }

        internal void _SetAction(KeyState ks, Keys key, Action a) 
        {
            if (!keyState.ContainsKey(key)) {
                keyState[key] = new KeyActions(KeyState.Up);
            }
            keyState[key].actions[ks].Add(a);
        }            

    } 

}