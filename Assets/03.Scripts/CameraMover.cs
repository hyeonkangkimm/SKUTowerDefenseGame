using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(KeyCode.A))
        {
            direction = -transform.right; // 카메라 기준 왼쪽
        }
        else if (Input.GetKey(KeyCode.D))
        {
            direction = transform.right;  // 카메라 기준 오른쪽
        }

        transform.position += direction.normalized * moveSpeed * Time.deltaTime;
    }
}
