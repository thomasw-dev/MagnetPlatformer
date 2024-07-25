using System.Collections;
using UnityEngine;

namespace Experimental.Coroutine
{
    public class Coroutine : MonoBehaviour
    {
        [SerializeField] bool _inCoroutine = false;

        int i = 0;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(MyCoourtine());
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                StopCoroutine(MyCoourtine());
            }
        }

        IEnumerator MyCoourtine()
        {
            _inCoroutine = true;
            Debug.Log("Started Coroutine...");

            i = 0;
            while (i < 1000)
            {
                i++;
                Debug.Log(i);
                yield return new WaitForSeconds(Time.deltaTime);
            }

            _inCoroutine = false;
            Debug.Log("Coroutine Stopped!!!");
            yield break;
        }
    }
}