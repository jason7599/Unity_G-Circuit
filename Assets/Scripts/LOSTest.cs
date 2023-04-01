using UnityEngine;
using UnityEditor;
using System.Collections;

public class LOSTest : MonoBehaviour
{
    [SerializeField] private Transform _playerTr;

    private void Start()
    {
        // StartCoroutine(LookRandom());
    }

    private IEnumerator LookRandom()
    {
        while (true)
        {
            Quaternion startRotation = transform.rotation;
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.right * Random.Range(-1f, 1f) + Vector3.forward * Random.Range(-1f, 1f));

            float randomDuration = Random.Range(0.1f, 0.5f);
            float elapsed = 0f;
            while (elapsed < randomDuration)
            {
                elapsed += Time.deltaTime;

                transform.rotation = Quaternion.Lerp(startRotation, targetRotation, (elapsed / randomDuration));

                yield return null;
            }

            yield return new WaitForSeconds(Random.Range(0.5f, 1f));
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 eyePos = transform.position + Vector3.up * 1.5f;

        Vector3 dirToPlayer = _playerTr.position - eyePos; dirToPlayer.y = 0;

        float angleOffset = Vector3.Angle(transform.forward, dirToPlayer);
        Handles.Label(eyePos, $"Angle: {angleOffset}");

        // if (angleOffset <= 50f)
        // {
        //     if (Physics.Raycast(eyePos, dirToPlayer, out RaycastHit hit, 10f, (1 << 6)))
        //     {

        //     }
        // }
    }
}
