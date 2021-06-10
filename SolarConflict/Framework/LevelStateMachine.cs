using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace SolarConflict
{
    using StateActivity = Func<int, LevelStateMachine.StateOutput>;

    [Serializable]
    public class LevelStateMachine
    {
        [Serializable]
        public class StateOutput
        {
            private int output;
            public int Output { get { return output; } }
            private String nextState;
            public String NextState { get { return nextState; } }

            public StateOutput(String nextState)
            {
                this.output = 0;
                this.nextState = nextState;
            }

            public StateOutput(int output)
            {
                this.output = output;
                nextState = null;
            }

            public static implicit operator StateOutput(string output)
            {
                StateOutput o = new StateOutput(output);
                return o;
            }

            public static implicit operator StateOutput(int output)
            {
                StateOutput o = new StateOutput(output);
                return o;
            }

            public static implicit operator StateOutput(bool output)
            {
                StateOutput o = new StateOutput(output ? 1 : 0);
                return o;
            }

        }

        [Serializable]
        private class LevelState
        {
            private String name;
            public String Name
            {
                get { return name; }
            }
            private int stateTime;
            private Scene scene;
            private StateActivity activity;
            public LevelState(Scene scene, StateActivity stateActivity, String name)
            {
                this.name = name;
                stateTime = 0;
                this.scene = scene;
                activity = stateActivity;
            }
            //return value:
            // -1 go to previous state
            // 0 stay in current state
            // 1 go to next state
            public StateOutput ActivateState()
            {
                stateTime++;
                return activity(stateTime);
            }
        }

        private LinkedList<LevelState> stateMachine;
        private LinkedListNode<LevelState> currentState;
        private KeyboardState prevKeyboardState;
        private KeyboardState keyboardState;
        private int nextStateId;

        public void SetState(string name)
        {

            for (LinkedListNode<LevelState> it = stateMachine.First; it != null; it = it.Next)
            {
                if (it.Value.Name == name)
                {
                    currentState = it;
                    return;
                }
            }
        }

        public LevelStateMachine()
        {
            stateMachine = new LinkedList<LevelState>();
            keyboardState = new KeyboardState();
            nextStateId = 0;
        }

        public void AddState(Scene scene, StateActivity impl, String name = null)
        {
            if (name == null)
                name = nextStateId++.ToString();
            var tempState = stateMachine.AddLast(new LevelState(scene, impl, name));
            if (stateMachine.Count == 1)
            {
                currentState = tempState;
            }
        }

        public void ActivateState()
        {
           // prevKeyboardState = keyboardState;//Debug - press n to jump to next state
           // keyboardState = Keyboard.GetState();
            //if (keyboardState.IsKeyDown(Keys.N) && prevKeyboardState.IsKeyUp(Keys.N) && currentState.Next != null)
            //{
            //    currentState = currentState.Next;
            //    return;
            //}

            if (currentState == null)
                return;
            var res = currentState.Value.ActivateState();
            currentState = GetNextState(res);
        }

        private LinkedListNode<LevelState> GetNextState(StateOutput output)
        {
            if (output.NextState == null)
            {
                switch (output.Output)
                {
                    case 0: return currentState;
                    case 1: return currentState.Next;
                    case -1: return currentState.Previous;
                }
            }
            else
            {
                for (LinkedListNode<LevelState> it = stateMachine.First; it != null; it = it.Next)
                {
                    if (it.Value.Name == output.NextState)
                        return it;
                }
            }
            throw new Exception("Invalid state name");
        }
    }
}
