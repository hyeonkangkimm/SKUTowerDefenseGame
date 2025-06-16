using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public float moveSpeed = 5f;

    // 이동 가능한 Z좌표 범위
    public float minZ = 12.8f;
    public float maxZ = 112.7f;

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

        // 이동 처리
        transform.position += direction.normalized * moveSpeed * Time.deltaTime;

        // Z축 위치 제한
        Vector3 pos = transform.position;
        pos.z = Mathf.Clamp(pos.z, minZ, maxZ);
        transform.position = pos;
    }
}
