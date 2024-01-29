using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 100f;
    public float gridSpacing = 1.0f;
    public GridManager gridManager; // —сылка на GridManager дл€ доступа к информации о сетке

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
        // —павните машинку на первом квадрате сетки
        transform.position = new Vector3(0, 0, 0);
        targetPosition = transform.position;
        isMoving = true;
    }

    private Vector3 moveDirection = Vector3.forward;

    void MoveTowardsTarget()
    {
        // –ассчитайте новую целевую позицию на основе текущей позиции и рассто€ни€ между квадратами сетки
       Vector3 nextPosition = transform.position + moveDirection * gridSpacing;

        // ѕроверьте, не выходит ли нова€ позици€ за границы сетки
        if (gridManager.IsInsideGrid(nextPosition))
        {
            targetPosition = nextPosition;

            // ƒвигайтесь к целевой позиции с использованием Lerp дл€ плавного перемещени€
            transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);

            // ѕроверьте, достигли ли близко к целевой позиции, и остановите движение
            if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
            {
                transform.position = targetPosition;
                isMoving = false;
            }

            // ѕоворачивайтесь вперед
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
            // ≈сли нова€ позици€ выходит за границы сетки, прекратите движение
            isMoving = false;
        }
    }
}
