using UnityEngine;

public class CameraLogic : MonoBehaviour
{
    //skrypt kamer

    //zmienne pomocnicze przechowujace kamery
    private GameObject cameraOn;
    private GameObject cameraOff;

    //wszystkei kamery w scenie
    [SerializeField]
    private GameObject[] cameras;

    void Start()
    {
        //wlacza kamere pierwsza
        cameraOn = cameras[0];
        cameraOff = cameras[1];
        cameraOn.SetActive(true);
        cameraOff.SetActive(false);
        //wylacza pozostale
        for (int i = 2;i<cameras.Length;i++)
        {
            cameras[i].SetActive(false);
        }
    }

    
    public void cameraChange(string number)
    {
        //wlacza kamere o danym numerze
        Debug.Log("Wywolanie");
        Debug.Log(number);
        int i = int.Parse(number);
        //sprawdzanie czy ju¿ jest wlaczona
        if (cameraOn == cameras[i-1])
            return;
        //wlaczanie
        cameraOff = cameras[i - 1];
        if (cameraOff != null )
        {
            Debug.Log("tak");
            if (!cameraOff.activeSelf) 
            {
                cameraOn.SetActive(false);
                cameraOff.SetActive(true);

                cameraOn = cameraOff;

                Debug.Log(cameraOn.name);
                Debug.Log(cameraOn.activeSelf);
            }
        }
        else
        {
            Debug.Log("Nie znaleziono kamery");
        }
    }

    public void CurrentCameraOnOrOf()
    {
        if (cameraOn.activeSelf)
            cameraOn.SetActive(false);
        else
            cameraOn.SetActive(true);
    }
}
