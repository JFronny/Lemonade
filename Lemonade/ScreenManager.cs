using System;
using System.Collections.Generic;
using System.Threading;
using CC_Functions.Commandline.TUI;

namespace Lemonade
{
    //TODO implement thunderstorm chance on bad weather
    public class ScreenManager
    {
        private readonly AnimationScreen _animator;
        private readonly ResultScreen _result;
        private readonly Settings _settings;
        private readonly TransactionScreen _transaction;
        private GameState _state;
        private bool _running;
        private List<PlayerState> _players;
        private int _currentPlayer;
        private bool _initialEvent = true;
        private int _day;
        private Weather _weather;
        private int signCost => 15;
        private int GlassCost => _day < 5 ? 2 : 4;

        public ScreenManager(Settings settings)
        {
            _settings = settings;
            _animator = new AnimationScreen(settings);
            _animator.Ok += () =>
            {
                _state = GameState.Transaction;
                _currentPlayer = 0;
                _initialEvent = true;
                _transaction.SetUp(_players[_currentPlayer], _settings, _currentPlayer, _day, _weather, signCost, GlassCost);
            };
            _transaction = new TransactionScreen(settings);
            _transaction.Ok += (glasses, price, signs) =>
            {
                _initialEvent = true;
                _players[_currentPlayer].Glasses = glasses;
                _players[_currentPlayer].GlassPrice = price;
                _players[_currentPlayer].Signs = signs;
                _players[_currentPlayer].CalculateIncome(signCost, GlassCost, _weather, settings);
                _currentPlayer++;
                if (_currentPlayer >= _players.Count)
                {
                    _state = GameState.Event;
                    _currentPlayer = 0;
                }
                else
                {
                    _transaction.Tab(true);
                    _transaction.Tab(false);
                }
            };
            _result = new ResultScreen(settings);
            _result.Ok += () =>
            {
                _initialEvent = true;
                _state = GameState.Setup;
            };
            _state = GameState.Setup;
            _players = new List<PlayerState>();
            for (int i = 0; i < settings.PlayerCount; i++) _players.Add(new PlayerState(i + 1));
        }

        public void Run()
        {
            _running = true;
            _state = GameState.Setup;
            while (_running)
            {
                switch (_state)
                {
                    case GameState.Setup:
                        if (_initialEvent)
                        {
                            _day++;
                            _weather = new Weather();
                            _animator.SetWeather(_weather);
                            _animator.Render();
                            _initialEvent = false;
                        }
                        _animator.ReadInput();
                        Thread.Sleep(100);
                        break;
                    case GameState.Transaction:
                        if (_initialEvent)
                        {
                            _transaction.SetUp(_players[_currentPlayer], _settings, _currentPlayer, _day, _weather, signCost, GlassCost);
                            _transaction.Render();
                            _initialEvent = false;
                        }
                        _transaction.ReadInput();
                        Thread.Sleep(100);
                        break;
                    case GameState.Event:
                        if (_initialEvent)
                        {
                            _result.Setup(_players);
                            _result.Render();
                            _initialEvent = false;
                        }
                        _result.ReadInput();
                        Thread.Sleep(100);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}