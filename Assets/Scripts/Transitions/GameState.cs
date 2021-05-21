using System;
using System.Collections;
using UnityEngine;

namespace Transitions
{
    public class GameState
    {
        private static GameState _instance;
        public static GameState Instance { get => _instance; }

        static GameState() => _instance = new GameState();

        private GameState() {
            _isBeginning = true;
        }

        private bool _isBeginning;
        private bool _isEnd;

        public void SetBeginning(bool isBeginning)
        {
            _isBeginning = isBeginning;
        }

        public bool IsBeginOfLevel()
        {
            return _isBeginning;
        }

        public void SetEnd(bool isEnd)
        {
            _isEnd = isEnd;
        }

        public bool IsEndOfLevel()
        {
            return _isEnd;
        }
    }
}