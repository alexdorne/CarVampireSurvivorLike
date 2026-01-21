using System.Collections;
using UnityEngine;

public class MolotovCocktailProjectile : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayerMask; 

    private void OnCollisionEnter(Collision collision) {
        if (!collision.gameObject.CompareTag("Player")) {
            Explode(); 
        }
    }

    private void Explode() {
        GameObject fire = PoolManager.Instance.Get("Fire"); 

        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f, groundLayerMask)) {
            fire.transform.position = hit.point + Vector3.up * 0.1f; 
        }
        else {
            fire.transform.position = transform.position;
        }

        PoolManager.Instance.Return("MolotovCocktails", this.gameObject);
    }
}
