using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Researcher : MonoBehaviour
{
    public ResourceGenerator ResourceGenerator;

    private WaitForFixedUpdate _waitForFixedUpdate = new();

    private void Awake() => ResourceGenerator = 
        GameObject.Find("ResourceGenerator").GetComponent<ResourceGenerator>();

    public IEnumerator SendBots(List<Bot> bots)
    {
        while (enabled)
        {
            if (ResourceGenerator.Resources.Count > 0)
                Explore(bots);

            yield return _waitForFixedUpdate;
        }
    }

    private void Explore(List<Bot> bots)
    {
        Resource resource = ResourceGenerator.Resources[0];

        if (resource.isActiveAndEnabled)
        {
            foreach (Bot bot in bots)
            {
                if (bot.ExplorationCoroutine == null)
                {
                    bot.StartExploration(resource);
                    break;
                }
            }
        }

    }
}
