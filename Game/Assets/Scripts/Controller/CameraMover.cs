using UnityEngine;

/// <summary>
/// Движение камеры по оси X
/// </summary>
class CameraMover : MonoBehaviour
{
    /// <summary>
    /// Левая и правая границы движения
    /// </summary>
    public Vector2 XBorders = new Vector2(-412f, 412f);

    /// <summary>
    /// Верхняя и нижняя границы движения
    /// </summary>
    public Vector2 ZBorders = new Vector2(-160f, -100f);

    public float XSpeed = 2f;
    public float ZSpeed = 0.8f;

    private float cachedX;
    private float cachedZ;

    private void Start()
    {
        cachedX = transform.position.x;
        cachedZ = transform.position.z;
    }

    private void Update()
    {
        var inputX = XSpeed * Input.GetAxis("Mouse X");
        var inputZ = ZSpeed * -Input.GetAxis("Mouse Y");
        if (inputX == 0 && inputZ == 0)
            return;
        var newX = Mathf.Clamp(cachedX + inputX, XBorders.x, XBorders.y);
        var newZ = Mathf.Clamp(cachedZ + inputZ, ZBorders.x, ZBorders.y);
        if (cachedX != newX)
            cachedX = newX;
        if (cachedZ != newZ)
            cachedZ = newZ;
        transform.position = new Vector3(cachedX, transform.position.y, cachedZ);
    }
}