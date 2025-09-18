using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    ParticleSystem ps;

    public GameObject firePostion;
    public GameObject BombFactory;

    public GameObject bulletEffect;

    public float throwPower = 15f;

    public int weaponPower = 5; 
    void Start()
    {
        ps = bulletEffect.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            var bombPrefab = Instantiate(BombFactory);
            bombPrefab.transform.position = firePostion.transform.position;

            Rigidbody rb = bombPrefab.GetComponent<Rigidbody>();
            rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
        }
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("shoot");
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            RaycastHit hitInfo = new RaycastHit();

            if (Physics.Raycast(ray, out hitInfo))
            {
                if(hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    EnemyFSM enemy = hitInfo.transform.GetComponent<EnemyFSM>();
                    enemy.HitEnemy(weaponPower);
                }

                bulletEffect.transform.position = hitInfo.point;
                bulletEffect.transform.forward = hitInfo.normal;

                ps.Play();
            }
        }
    }
}
