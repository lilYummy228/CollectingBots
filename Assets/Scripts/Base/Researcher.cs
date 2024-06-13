using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Researcher : MonoBehaviour
{
    [SerializeField] private ResourceGenerator _resourceGenerator;

    private WaitForFixedUpdate _waitForFixedUpdate = new();

    public ResourceGenerator ResourceGenerator => _resourceGenerator;

    public IEnumerator SendBots(List<Bot> bots)
    {
        while (enabled)
        {
            if (_resourceGenerator.Resources.Count > 0)
            {
                Resource resource = _resourceGenerator.Resources[0];

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

            yield return _waitForFixedUpdate;
        }
    }
}
