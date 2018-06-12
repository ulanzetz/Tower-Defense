using UnityEngine;

abstract class Bot : MonoBehaviour
{
    public PlayerController Controller
    {
        set
        {
            controller = value;
            controller.OnReachDestination += unitID => OnUnitReachDestination(unitID);
            OnStart();
        }
    }

    private PlayerController controller;

    protected abstract void OnStart();
    protected abstract void OnUnitReachDestination(int unitID);
    protected int BuyUnitAndGetID() => controller.BuyUnitAndGetID();
    protected void BuildTower(Vector2 positon) => controller.BuildTower(positon);
    protected Map Map => controller.Map;
    protected bool Left => controller.Left;
    protected int Gold => controller.Gold;
    protected void MoveUnit(int unitID, Vector2 destination) => controller.MoveUnit(unitID, destination);
    protected Vector2 GetUnitPosition(int unitID) => controller.GetUnitPositon(unitID);
}