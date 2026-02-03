using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SceneButtonManager : MonoBehaviour
{
    
    public void GoToSecurityScene()   => LoadSceneSafe("SecurityRoom5");
    public void GoToServerScene()     => LoadSceneSafe("ServerRoom2");
    public void GoToTechnicalScene()  => LoadSceneSafe("TechnicalRoom1");
    public void GoToWaterScene()      => LoadSceneSafe("WaterRoom7");
    public void GoToACScene()         => LoadSceneSafe("ACRoom8");
    public void GoToCafeScene()       => LoadSceneSafe("CafeteriaRoom6");
    public void GoToCourtyardScene()  => LoadSceneSafe("CourtyardRoom4");
    public void GoToDamScene()        => LoadSceneSafe("DamBeachRoom3");
    public void GoToGameplayScene()   => LoadSceneSafe("WaterPuzzle1");

   
    void LoadSceneSafe(string sceneName)
    {
        int buildCount = SceneManager.sceneCountInBuildSettings;
        bool found = false;

        for (int i = 0; i < buildCount; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string nameOnly = Path.GetFileNameWithoutExtension(path);
            if (string.Equals(nameOnly, sceneName, System.StringComparison.OrdinalIgnoreCase))
            {
                found = true;
                break;
            }
        }

        if (!found)
        {
            Debug.LogError($"Scene '{sceneName}' is not in Build Settings. Add it via File > Build Settings > Scenes In Build.");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }

}
