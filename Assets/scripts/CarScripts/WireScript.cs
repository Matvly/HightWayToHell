using UnityEngine;

public class WireScript : MonoBehaviour
{
    public Vector3 PosA;
    public Vector3 PosB;

    public float Voltage;

    private GameObject connectorReference;


    public GameObject boxA;
    public GameObject boxB;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        connectorReference = Resources.Load<GameObject>("Connector");
        if (boxA == null)
        {
            boxA = Instantiate(connectorReference, PosA, Quaternion.identity);
        }
        if (boxB == null)
        {
            boxB = Instantiate(connectorReference, PosB, Quaternion.identity);
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<LineRenderer>().SetPosition(0, boxA.transform.position);
        gameObject.GetComponent<LineRenderer>().SetPosition(1, boxB.transform.position);
    }
}
