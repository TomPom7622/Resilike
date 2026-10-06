using UnityEngine;

public class BoxCast3DVisualizer : MonoBehaviour
{
    [Header("BoxCast Settings")]
    public Vector3 boxHalfExtents = Vector3.one * 0.5f;  // po³owa rozmiaru boxa
    public float castDistance = 2f;
    public LayerMask layerMask;

    [Header("Gizmos Colors")]
    public Color startColor = Color.green;
    public Color endColor = Color.red;
    public Color connectColor = Color.yellow;
    public Color hitColor = Color.magenta;

    private void OnDrawGizmos()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward; // U¿ywamy lokalnego forward obiektu
        Quaternion boxOrientation = transform.rotation; // obrót boxa = obrót obiektu

        // W³aœciwy BoxCast
        if (Physics.BoxCast(origin, boxHalfExtents, direction, out RaycastHit hit, boxOrientation, castDistance, layerMask))
        {
            Gizmos.color = hitColor;
            Gizmos.DrawSphere(hit.point, 0.1f);
        }

        // Wizualizacja
        DrawBoxCast(origin, boxHalfExtents, boxOrientation, direction, castDistance);
    }

    private void DrawBoxCast(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Vector3 direction, float distance)
    {
        // Lokalne naro¿niki boxa
        Vector3[] corners = new Vector3[8];
        int i = 0;

        for (int x = -1; x <= 1; x += 2)
        {
            for (int y = -1; y <= 1; y += 2)
            {
                for (int z = -1; z <= 1; z += 2)
                {
                    Vector3 localCorner = Vector3.Scale(new Vector3(x, y, z), halfExtents);
                    corners[i++] = origin + orientation * localCorner;
                }
            }
        }

        Vector3 castOffset = direction.normalized * distance;

        // Indeksy naro¿ników dla rysowania krawêdzi boxa
        int[,] edges = new int[,]
        {
            {0,1},{1,3},{3,2},{2,0}, // dó³
            {4,5},{5,7},{7,6},{6,4}, // góra
            {0,4},{1,5},{2,6},{3,7}  // po³¹czenia góra-dó³
        };

        for (int e = 0; e < edges.GetLength(0); e++)
        {
            Vector3 startA = corners[edges[e, 0]];
            Vector3 startB = corners[edges[e, 1]];

            Vector3 endA = startA + castOffset;
            Vector3 endB = startB + castOffset;

            // Box pocz¹tkowy
            Gizmos.color = startColor;
            Gizmos.DrawLine(startA, startB);

            // Box koñcowy
            Gizmos.color = endColor;
            Gizmos.DrawLine(endA, endB);

            // Po³¹czenie pocz¹tek-koniec
            Gizmos.color = connectColor;
            Gizmos.DrawLine(startA, endA);
            Gizmos.DrawLine(startB, endB);
        }
    }
}