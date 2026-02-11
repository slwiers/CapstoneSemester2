using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;

public static class GlobalDialogueManager
{
    public static Story currentStory = null;

    public static void CreateStory()
    {
        // Load the resouce (asset) based on its filename.
        TextAsset textAsset = Resources.Load<TextAsset>("Main");

        // Use the loaded text asset to load the ink story.
        currentStory = new Story(textAsset.text);
    }

    public static void JumpToCharacter(string pathname)
    {
        if (currentStory != null)
        {
            currentStory.ChoosePathString(pathname);
        }
    }
}
