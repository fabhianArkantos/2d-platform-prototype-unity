using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    #region Components
    [Header("Components")]
    [SerializeField]
    private GameObject followObject;
    private Rigidbody2D rb;
    [SerializeField]
    private Camera cam;
    #endregion

    #region Physics
    [Header("Psysics")]
    [SerializeField]
    private float speed = 3f;
    #endregion

    #region Variables
    [SerializeField]
    private Vector2 followOffset;
    private Vector2 threshold;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        threshold = calculateThreshold();
        rb = followObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector2 followObjectVector = followObject.transform.position;
        float xDifference = Vector2.Distance(Vector2.right * transform.position.x, Vector2.right * followObjectVector.x);
        float yDifference = Vector2.Distance(Vector2.up * transform.position.y, Vector2.up * followObjectVector.y);

        Vector3 newPosition = transform.position;

        if (Mathf.Abs(xDifference) >= threshold.x)
        {
            newPosition.x = followObjectVector.x;
        }
        if (Mathf.Abs(yDifference) >= threshold.y)
        {
            newPosition.y = followObjectVector.y;
        }

        float moveSpeed = rb.velocity.magnitude > speed ? rb.velocity.magnitude : speed;

        transform.position = Vector3.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);
    }

    private Vector3 calculateThreshold()
    {
        Rect aspect = cam.pixelRect;
        Vector2 th = new Vector2(cam.orthographicSize * aspect.width / aspect.height, cam.orthographicSize);
        th.x -= followOffset.x;
        th.y -= followOffset.y;

        return th;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector2 border = calculateThreshold();
        Gizmos.DrawWireCube(transform.position, new Vector3(border.x * 2, border.y * 2, 1));
    }
}
