using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 100f;
    public float gridSpacing = 1.0f;
    public GridManager gridManager; // ������ �� GridManager ��� ������� � ���������� � �����

    private Vector3 targetPosition;
    private bool isMoving = false;

    void Start()
    {
        SpawnOnFirstGrid();
    }

    void Update()
    {
        if (isMoving)
        {
            MoveTowardsTarget();
        }
    }

    void SpawnOnFirstGrid()
    {
        // �������� ������� �� ������ �������� �����
        transform.position = new Vector3(0, 0, 0);
        targetPosition = transform.position;
        isMoving = true;
    }

    private Vector3 moveDirection = Vector3.forward;

    void MoveTowardsTarget()
    {
        // ����������� ����� ������� ������� �� ������ ������� ������� � ���������� ����� ���������� �����
       Vector3 nextPosition = transform.position + moveDirection * gridSpacing;

        // ���������, �� ������� �� ����� ������� �� ������� �����
        if (gridManager.IsInsideGrid(nextPosition))
        {
            targetPosition = nextPosition;

            // ���������� � ������� ������� � �������������� Lerp ��� �������� �����������
            transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);

            // ���������, �������� �� ������ � ������� �������, � ���������� ��������
            if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
            {
                transform.position = targetPosition;
                isMoving = false;
            }

            // ��������������� ������
            if (isMoving)
            {
                Vector3 targetDirection = targetPosition - transform.position;
                targetDirection.y = 0;
                Quaternion rotationToTarget = Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationToTarget, rotationSpeed * Time.deltaTime);
            }
        }
        else
        {
            // ���� ����� ������� ������� �� ������� �����, ���������� ��������
            isMoving = false;
        }
    }
}
