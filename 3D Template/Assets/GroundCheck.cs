using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [Header("Ground Check Settings")]
    public Transform groundCheckPoint;     
    public Vector3 boxSize = new Vector3(0.5f, 0.1f, 0.5f);
    public float castDistance = 0.1f;
    public LayerMask groundLayer;

    [Header("State")]
    public bool isGrounded;

    void Update()
    {
        CheckGrounded();
    }

    private void CheckGrounded()
    {
       
        isGrounded = Physics.BoxCast(
            groundCheckPoint.position,
            boxSize * 0.5f,
            Vector3.down,
            Quaternion.identity,
            castDistance,
            groundLayer
                                    );
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheckPoint == null) return;

        Gizmos.color = isGrounded ? Color.green : Color.red;// change colors later
       
        Gizmos.DrawWireCube(
            groundCheckPoint.position + Vector3.down * castDistance,
            boxSize
                             );
    }
}
