using UnityEngine;
using UnityEngine.Playables;
public class TimelineControllerScript : MonoBehaviour
{
    //zarzadza animacjami
    [SerializeField]
    //trzeba uzupelnic w inspektorze
    private PlayableDirector[] playableDirectors;

    public PlayableDirector GiveDirector(int id)
    {
        //oddaje okreslona animacje
        return playableDirectors[id];
    }
}
