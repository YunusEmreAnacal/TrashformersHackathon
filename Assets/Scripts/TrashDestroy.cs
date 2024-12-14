using UnityEngine;
using System.Collections;
public class TrashDestroy : MonoBehaviour
{

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Trash"))
        {
            // Nesneyi k���ltme i�lemini ba�lat
            StartCoroutine(ShrinkAndDestroy(collision.gameObject));
        }
    }

    private IEnumerator ShrinkAndDestroy(GameObject trashObject)
    {
        float shrinkDuration = 1f; // Nesnenin k���lme s�resi
        Vector3 initialScale = trashObject.transform.localScale;
        Vector3 targetScale = Vector3.zero; // Hedef boyut (yok olma boyutu)

        float elapsedTime = 0f;

        while (elapsedTime < shrinkDuration)
        {
            // Nesnenin boyutunu zamanla k���lt
            trashObject.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / shrinkDuration);
            elapsedTime += Time.deltaTime;

            yield return null; // Bir sonraki frame'e kadar bekle
        }

        // Son durumda boyutu s�f�rla

        // Nesneyi yok et
        

        Destroy(trashObject);
    }
}