using UnityEngine;

/// <summary>
/// Движение камеры по оси X
/// </summary>
class CameraMover : MonoBehaviour
{
    /// <summary>
    /// Левая и правая границы движения по оси X
    /// </summary>
    public Vector2 XBorders = new Vector2(-412f, 412f);
    private float cachedX;

    private void Start() => cachedX = transform.position.x;

    private void Update()
    {
        var inputX = Input.GetAxis("Mouse X");
        if (inputX == 0)
            return;
        var newX = Mathf.Clamp(cachedX + inputX, XBorders.x, XBorders.y);
        if (cachedX != newX)
        {
            cachedX = newX;
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
    }
}