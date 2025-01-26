using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

/**
 * Record to pair up the names and mall spawn loactions
 */
public record Abductee
{
    public string Name { get; }
    public Vector2 MallSpawnLocation { get; }

    public Abductee(string name, Vector2 mallSpawnLocation)
    {
        Name = name;
        MallSpawnLocation = mallSpawnLocation;
    }
}

/**
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
    public bool inMall;
    private bool _doneInitialLoad;

    public List<Vector2> abducteeWaypoints;
    private List<Abductee> _abductees;
    
    public GameObject abducteePrefab;

    private void Start()
    {
        inMall = false;
        _abductees = new List<Abductee>();
        TransitionToSurface();
        _doneInitialLoad = true;
    }

    public void AddAbductee(string abducteeName)
    {
        // Don't reuse spawn locations already in use by another abductee
        List<Vector2> availableWaypoints = abducteeWaypoints.FindAll(waypoint => 
            !_abductees.Any(abductee => abductee.MallSpawnLocation.Equals(waypoint))
        );

        if (availableWaypoints.Count == 0)
        {
            Debug.LogWarning("There are no available waypoints");
            return;
        }
        
        Vector2 spawnLocation = availableWaypoints[Random.Range(0, availableWaypoints.Count)];
        _abductees.Add(new Abductee(abducteeName, spawnLocation));
        Debug.Log($"Added abductee {abducteeName} to location {spawnLocation} in mall");
    }

    public void RemoveAbductee(string abducteeName)
    {
        _abductees.RemoveAll(x => x.Name == abducteeName);

        if (inMall)
        {
            Destroy(GameObject.Find(abducteeName));
        }
        
        // Todo respawn swimmer on surface when timer is up
        
        Debug.Log($"Abductee {abducteeName} from mall");
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
                if (_abductees.Find(abdcte => abdcte.Name == swimmer.name) != null)
                {
                    Destroy(swimmer.gameObject);
                }
            }

            inMall = false;
        });
    }
    
    public void TransitionToMall()
    {
        SceneManager.UnloadSceneAsync(surfaceSceneIndex);
        SceneManager.LoadSceneAsync(mallSceneIndex, LoadSceneMode.Additive).GetAwaiter().OnCompleted(() =>
        {
            foreach (Abductee abductee in _abductees)
            {
                Transform parent = GameObject.Find("AbducteesGoHere").transform;
                GameObject abducteeObj = Instantiate(abducteePrefab, parent);
                abducteeObj.transform.position = abductee.MallSpawnLocation;
                abducteeObj.name = abductee.Name;
            }
            
            inMall = true;
        });
    }
}
