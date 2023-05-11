using UnityEngine;
using System.Collections;

public class Break : MonoBehaviour
{
    [SerializeField] private bool useGravity = true;                            // �d�͂�L���ɂ��邩�ǂ���
    [SerializeField] private Vector3 explodeVel = new Vector3(0, 0, 0.1f);      // �����̒��S�n
    [SerializeField] private float explodeForce = 200f;                         // �����̈З�
    [SerializeField] private float explodeRange = 10f;                          // �����͈̔�
    private Rigidbody[] rigidBodies;

    void Start()
    {
        rigidBodies = GetComponentsInChildren<Rigidbody>();                     // �q(�j��)��Rigidbody���擾���Ă���
        StartCoroutine("BreakStart");                                           // ����Ƀf�B���C���|���邽�߃R���[�`�����g�p
    }

    IEnumerator BreakStart()
    {
        foreach (Rigidbody rb in rigidBodies)
        {
            rb.isKinematic = false;
            rb.useGravity = useGravity;
            rb.AddExplosionForce(explodeForce / 5, transform.position + explodeVel, explodeRange);
        }
        yield return new WaitForSeconds(0.02f);                                 // ��u���������ƂłЂъ�������o

        foreach (Rigidbody rb in rigidBodies)
        {
            rb.isKinematic = true;
        }
        yield return new WaitForSeconds(0.8f);

        foreach (Rigidbody rb in rigidBodies)
        {
            rb.isKinematic = false;
            rb.AddExplosionForce(explodeForce, transform.position + explodeVel, explodeRange);
        }
    }
}