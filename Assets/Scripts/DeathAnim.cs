using UnityEngine;

public class DeathAnim : MonoBehaviour
{
    [SerializeField]
    GameObject deathAnimationObject;

    void OnDestroy()
    {
        Instantiate(deathAnimationObject);
        deathAnimationObject.transform.localPosition = transform.position;
    }

}
