using UnityEngine;

public class BombAction : MonoBehaviour
{
    public GameObject bombEffect;

    public int attackPower = 10;
    public float explosionRadius = 5f;
    private void OnCollisionEnter(Collision collision)
    {
        
        Bomb();

    }

    private void Bomb()
    {

        //폭발 반경 내 적 
        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius,1 << 8);

        for(int i=0; i<cols.Length; ++i)
        {
            cols[i].GetComponent<EnemyFSM>().HitEnemy(attackPower);

        }

        GameObject eff = Instantiate(bombEffect);
        eff.transform.position = transform.position;
        Destroy(gameObject);

    }
}
