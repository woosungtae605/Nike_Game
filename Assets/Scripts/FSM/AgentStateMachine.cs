using System;
using System.Collections.Generic;
using Agents;
using UnityEngine;

namespace FSM
{
    public class AgentStateMachine
    {
        public AgentState CurrentState { get; private set; }
        
        private Dictionary<int, AgentState> _stateDict;
        
        public AgentStateMachine(Agent owner, StateSO[] stateList)
        {
            _stateDict = new Dictionary<int, AgentState>();

            foreach (StateSO state in stateList)
            {
                Type type = Type.GetType(state.className);
                Debug.Assert(type != null, $"state class not found: {state.className}");

                AgentState agentState = Activator.CreateInstance(type, owner, state) as AgentState;
                
                _stateDict.Add(state.stateIndex, agentState);
            }
        }

        public void ChangeState(int nextStateIndex)
        {
            CurrentState?.Exit();
            AgentState nextState = _stateDict.GetValueOrDefault(nextStateIndex);
            Debug.Assert(nextState != null, $"state not found: {nextStateIndex}");
            
            CurrentState = nextState;
            CurrentState?.Enter();
        }
        
        public void UpdateMachine() => CurrentState?.Update();
    }
}