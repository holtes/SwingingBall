using Core.Enums;
using Data.Configs;

namespace Domain.Game.Models
{
    public class ColumnModel
    {
        private readonly BallType[] _slots;

        public int Capacity { get; private set; }

        public ColumnModel(GameConfig config)
        {
            Capacity = config.GridSize.x;
            _slots = new BallType[Capacity];
            ClearColumn();
        }

        public int FindFirstEmpty()
        {
            for (int i = 0; i < Capacity; i++)
                if (_slots[i] == BallType.None) return i;
            return -1;
        }

        public bool Occupy(int index, BallType type)
        {
            if (index < 0 || index >= Capacity) return false;
            if (_slots[index] != BallType.None) return false;
            _slots[index] = type;
            return true;
        }

        public void ClearSlot(int index)
        {
            if (index < 0 || index >= Capacity) return;
            _slots[index] = BallType.None;
        }

        public void ClearColumn()
        {
            for (int i = 0; i < Capacity; i++)
                _slots[i] = BallType.None;
        }
    }
}