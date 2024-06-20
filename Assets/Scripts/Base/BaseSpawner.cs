using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpawner : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private Researcher _researcher;
    [SerializeField] private ResourceStorage _resourceStorage;

    private WaitForFixedUpdate _waitForFixedUpdate;

    public Base GetNewBase(Flag flag)
    {
        flag.gameObject.SetActive(false);

        Base newBase = Instantiate(_base, flag.transform.position, Quaternion.identity);
        _researcher.AddBase(newBase);

        return newBase;
    }

    public IEnumerator BuildNewBase(Flag flag, int price, List<Bot> bots, Base @base)
    {
        bool isFreeBotFound = false;

        while (isFreeBotFound == false)
        {
            if (_resourceStorage.ResourceCount >= price)
            {
                foreach (Bot bot in bots)
                {
                    if (bot.ExplorationCoroutine == null)
                    {
                        @base.RemoveBot(bot);

                        _resourceStorage.RemoveResources(price);

                        bot.StartBuild(flag);

                        isFreeBotFound = true;                       

                        break;
                    }
                }
            }

            yield return _waitForFixedUpdate;
        }
    }
}
