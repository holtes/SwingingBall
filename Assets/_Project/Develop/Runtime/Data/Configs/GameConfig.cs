using UnityEngine;

namespace Data.Configs
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [SerializeField] private Vector2Int _gridSize;
        [SerializeField] private int _fullGridChances = 1;
        [SerializeField] private float _sceneFadeDuration = 1f;

        public Vector2Int GridSize => _gridSize;
        public int FullGridChances => _fullGridChances;
        public float SceneFadeDuration => _sceneFadeDuration;
    }
}