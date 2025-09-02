using Core.Enums;
using Core.Tools;
using Core.Signals;
using Domain.Game.Models;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using R3;


namespace Domain.Game.Controllers
{
    public class CollectorController : MonoBehaviour
    {
        [SerializeField] private List<ColumnController> _columns;

        private CollectorModel _model;
        private SignalBus _signalBus;

        [Inject]
        private void Construct(CollectorModel collectorModel, SignalBus signalBus)
        {
            _model = collectorModel;
            _signalBus = signalBus;
        }

        private void Awake()
        {
            for (int i = 0; i < _columns.Count; i++)
            {
                var col = i;
                _columns[i]
                    .OnBallPlaced
                    .Subscribe(data => PlaceBall(data.Item1, col, data.Item2))
                    .AddTo(this);

                _columns[i]
                    .OnBallChangedSlot
                    .Subscribe(oldRow => ClearSlot(oldRow, col))
                    .AddTo(this);
            }
        }

        private void PlaceBall(int row, int col, BallType type)
        {
            _model.SetBall(row, col, type);

            if (_model.TryFindMatches(out var matches))
            {
                MatchBalls(matches);
            }
            else if (_model.IsFull())
            {
                ClearGrid();
                _signalBus.Fire(new OnGridFilledSignal());
            }
        }

        private void ClearSlot(int row, int col)
        {
            _model.ClearBall(row, col);
        }

        private void MatchBalls(List<Match> matches)
        {
            var columnsToRemove = new Dictionary<int, List<int>>();

            foreach (var match in matches)
            {
                foreach (var cell in match.Cells)
                {
                    int row = cell.x;
                    int col = cell.y;

                    if (!columnsToRemove.ContainsKey(col))
                        columnsToRemove[col] = new List<int>();

                    columnsToRemove[col].Add(row);

                    ClearSlot(row, col);
                }

                _signalBus.Fire(new OnMatchFoundSignal((BallType)match.TypeId));
            }

            foreach (var columnToRemove in columnsToRemove)
            {
                int colIndex = columnToRemove.Key;

                int[] removedRows = columnToRemove.Value.ToArray();

                _columns[colIndex].RemoveSlots(removedRows).Forget();
            }
        }

        private void ClearGrid()
        {
            _model.ClearGrid();

            foreach (var column in _columns)
                column.ClearColumn();
        }
    }
}