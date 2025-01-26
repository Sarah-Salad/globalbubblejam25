using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;
using Random = UnityEngine.Random;

/**
 * Record to pair up Swimmer instance, timer instance, and
 * abductee instance
 */
public record Abductee
{
    public GameObject swimmerReference;
    public GameObject associatedTimer;
    public GameObject abducteeInstance;

    public Abductee(GameObject swimmer, GameObject timer) {
        swimmerReference = swimmer;
        associatedTimer = timer;
        abducteeInstance = null;
    }
}

/**
 * TODO: Update comment to reflect new logic
 * General idea: This game manager holds the locations for active abductees,
 * and handles scene transitions. It's expected this GM is in the hierarchy,
 * lest abductees won't spawn. Swimmers should be placed in the MainScene
 * scene, which this game manager will grab their locations from.
 * 
 * Surface create flow:
 * - GM loads surface scene
 * - GM goes through swimmers in the scene and removes any swimmers whose
 *   names are already in the abductee list (for first load, this will be
 *   empty/nobody, so nothing will be deleted)
 * 
 * Abduct flow:
 * - Swimmer creates a timer
 * - Swimmer publishes a new abductee to this GM
 * - GM picks a location for the new abductee from waypoints not in use,
 *   and adds the transform and swimmers guid to abductees list
 * - Swimmer adds guid to the timer object
 * - When timer completes, it sends a signal to this GM to remove the guid
 *   from the abductees list, and remove their objects from the mall scene
 *   if the player is currently there
 *
 * Mall create flow:
 * - GM loads the mall scene
 * - GM creates abductees based on what's in the abductees list
 */
public class GameManager : MonoBehaviour
{
    public int surfaceSceneIndex;
    public int mallSceneIndex;
    public int winSceneIndex;
    public int loseSceneIndex;
    public bool inMall;
    private bool _doneInitialLoad;
    private List<Abductee> _abductees;

    private bool hasDived;
    
    public GameObject abducteePrefab;

    public List<RuntimeAnimatorController> abducteeAnimations;
    public List<Sprite> abducteeSprites;
    
    private void Start()
    {
        inMall = false;
        hasDived = false;
        _abductees = new List<Abductee>();
        TransitionToSurface();
        _doneInitialLoad = true;
    }

    public void AddAbductee(GameObject swimmer, GameObject timer)
    {

        _abductees.Add(new Abductee(swimmer, timer));
        Debug.Log($"Queued abductee to spawn in mall");

        // Don't show the help text if the player has already dived under the water once before
        if (!hasDived)
        {
            GetComponent<HelpTextManager>().ShowHelpText();
        }
    }

    public void RemoveAbducteeViaTimer(GameObject myTimer) {
        // https://stackoverflow.com/a/27851493
        foreach (Abductee abductee in _abductees.ToList()) {
            if (abductee.associatedTimer == myTimer) {
                Destroy(abductee.associatedTimer.gameObject);
                Destroy(abductee.abducteeInstance);
                _abductees.Remove(abductee);
            }
        }
    }

    public void TransitionToSurface()
    {
        // Unity is in love with just dying so we only unload the mall if the game isn't loading for the first time
        if (_doneInitialLoad)
        {
            SceneManager.UnloadSceneAsync(mallSceneIndex);
        }

        SceneManager.LoadSceneAsync(surfaceSceneIndex, LoadSceneMode.Additive).GetAwaiter().OnCompleted(() =>
        {
            List<Swimmer> swimmers = GameObject.FindGameObjectsWithTag("Swimmer").ToList().ConvertAll(go => go.GetComponent<Swimmer>());

            foreach (Swimmer swimmer in swimmers)
            {
                // If our originating swimmer is still on the surface then delete their gameobject
                if (_abductees.Find(abdcte => abdcte.swimmerReference == swimmer.gameObject) != null)
                {
                    Destroy(swimmer.gameObject);
                }
            }

            inMall = false;
        });
    }
    
    public void TransitionToMall()
    {
        GetComponent<HelpTextManager>().HideHelpText();
        SceneManager.UnloadSceneAsync(surfaceSceneIndex);
        SceneManager.LoadSceneAsync(mallSceneIndex, LoadSceneMode.Additive).GetAwaiter().OnCompleted(() =>
        {
            foreach (Abductee abductee in _abductees)
            {
                spawnAbducteeAtOpenWaypoint(abductee);
            }
            
            inMall = true;
            hasDived = true;
        });
    }

    public void TransitionToWinScene()
    {
        SceneManager.LoadSceneAsync(winSceneIndex);
    }

    public void TransitionToLoseScene()
    {
        SceneManager.LoadSceneAsync(loseSceneIndex);
    }

    private void spawnAbducteeAtOpenWaypoint(Abductee abductee) {
        // Find open waypoint
        GameObject[] waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        GameObject[] openWaypoints = waypoints.Where(waypoint => waypoint.GetComponent<Waypoint>().isOpen()).ToArray();
        //Vector2[] openWaypointCoords = openWaypoints.Select(
        //    waypoint => new Vector2(waypoint.transform.position.x, waypoint.transform.position.x)).ToArray();
        GameObject destWaypoint = openWaypoints[UnityEngine.Random.Range(0, openWaypoints.Length - 1)];
        Vector2 destWaypointCoords = new Vector2(destWaypoint.transform.position.x, destWaypoint.transform.position.y);
        // Spawn abudctee at waypoint and spawn in the correct location
        abductee.abducteeInstance = Instantiate(abducteePrefab);
        abductee.abducteeInstance.transform.SetParent(destWaypoint.transform);
        abductee.abducteeInstance.transform.position = destWaypointCoords;
        
        int personalityIndex = Random.Range(0, abducteeAnimations.Count);
        abductee.abducteeInstance.GetComponent<SpriteRenderer>().sprite = abducteeSprites[personalityIndex];
        abductee.abducteeInstance.GetComponent<Animator>().runtimeAnimatorController = abducteeAnimations[personalityIndex];
        // tie it to the waypoint object so it will be considered inhabited next go around
        destWaypoint.GetComponent<Waypoint>().inhabitedBy = abductee.abducteeInstance;
    }
}
