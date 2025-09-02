using Core.Enums;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IBallController
{
    public void Init(BallType ballType, Color color);
    public void EnablePhysics();
    public void DisablePhysics();
    public UniTask CheckRest(CancellationToken token = default);
    public UniTask DestroyBall();
    public UniTask DestroyBallWithVFX();
    public BallType GetBallType();
    public Transform GetTransform();
}
