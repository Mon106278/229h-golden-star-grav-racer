using UnityEngine;

public class ZeroGravity : MonoBehaviour
{
    [Header("��õ�駤��")]
    public float gravityMultiplier = 0.1f; // ��Ҥ����ç�����ǧ (0 = �����˹ѡ)
    public float floatDrag = 0.5f;        // �ç��ҹ㹾�鹷��
    public bool randomRotation = true;     // ����ѵ����عẺ�����������

    [Header("�Ϳ࿡��")]
    public ParticleSystem floatingParticles;
    public AudioClip zeroGSound;

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // �纤��������׹���������͡�ҡ⫹
            if (!rb.gameObject.GetComponent<GravityMemory>())
            {
                rb.gameObject.AddComponent<GravityMemory>();
            }

            // ��駤�ҿ��ԡ������
            rb.useGravity = false;
            rb.linearDamping = floatDrag;
            rb.angularDamping = floatDrag * 0.5f;

            // �����Ϳ࿡��
            if (floatingParticles) Instantiate(floatingParticles, other.transform.position, Quaternion.identity);
            if (zeroGSound) AudioSource.PlayClipAtPoint(zeroGSound, transform.position);

            if (randomRotation)
            {
                rb.AddTorque(new Vector3(
                    Random.Range(-1f, 1f),
                    Random.Range(-1f, 1f),
                    Random.Range(-1f, 1f)),
                    ForceMode.Impulse);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            GravityMemory memory = rb.GetComponent<GravityMemory>();
            if (memory)
            {
                // �׹��ҿ��ԡ�����
                rb.useGravity = memory.originalUseGravity;
                rb.linearDamping = memory.originalDrag;
                rb.angularDamping = memory.originalAngularDrag;
                Destroy(memory);
            }
        }
    }
}

// ʤ�Ի������Ѻ�Ӥ�ҵ�駵�
public class GravityMemory : MonoBehaviour
{
    public bool originalUseGravity;
    public float originalDrag;
    public float originalAngularDrag;

    void Awake()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        originalUseGravity = rb.useGravity;
        originalDrag = rb.linearDamping;
        originalAngularDrag = rb.angularDamping;
    }
}
