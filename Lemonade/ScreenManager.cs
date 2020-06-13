using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CC_Functions.Commandline.TUI;

namespace Lemonade
{
    //TODO implement visual feedback for weather events
    public class ScreenManager
    {
        private readonly AnimationScreen _animator;
        private readonly List<PlayerState> _players;
        private readonly ResultScreen _result;
        private readonly Settings _settings;
        private readonly TransactionScreen _transaction;
        private int _currentPlayer;
        private int _day;
        private bool _initialEvent = true;
        private bool _running;
        private GameState _state;
        private Weather _weather;

        public ScreenManager(Settings settings)
        {
            _settings = settings;
            _animator = new AnimationScreen(settings);
            _animator.Ok += () =>
            {
                _state = GameState.Transaction;
                _currentPlayer = 0;
                _initialEvent = true;
                _transaction.SetUp(_players[_currentPlayer], _settings, _currentPlayer, _day, _weather, signCost,
                    GlassCost);
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
                    _transaction.Tab();
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

        private int signCost => 15;
        private int GlassCost => _day < 5 ? 2 : 4;

        public void Run()
        {
            _running = true;
            _state = GameState.Setup;
            while (_running)
                switch (_state)
                {
                    case GameState.Setup:
                        if (_initialEvent)
                        {
                            _weather = new Weather(_settings);
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
                            _transaction.SetUp(_players[_currentPlayer], _settings, _currentPlayer, _day, _weather,
                                signCost, GlassCost);
                            _transaction.Render();
                            _initialEvent = false;
                        }
                        _transaction.ReadInput();
                        Thread.Sleep(100);
                        break;
                    case GameState.Event:
                        if (_initialEvent)
                        {
                            _result.Setup(_players, _weather);
                            _result.Render();
                            _day++;
                            if (_players.Any(s => s.Budget < GlassCost))
                            {
                                Console.Clear();
                                IEnumerable<PlayerState> lost = _players.Where(s => s.Budget < GlassCost);
                                Console.WriteLine($"The following players are out: {string.Join(", ", lost.Select(s => s.Number.ToString()))}");
                                Thread.Sleep(2000);
                                Console.Clear();
                                _players.RemoveAll(s => lost.Contains(s));
                            }
                            DiffDraw.Draw(_settings.Color, true);
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