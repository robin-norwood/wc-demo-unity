using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetToken : MonoBehaviour
{

    public GameObject tokenPrefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenerateToken() {
        Vector3 pos = new Vector3(2,2,2); // FIXME...attach to mouse, or...?

        GameObject token = (GameObject) Instantiate(this.tokenPrefab, pos, Quaternion.identity);
        token.transform.Rotate(0, -90, 0);
    }
}
