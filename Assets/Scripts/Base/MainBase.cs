using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainBase : Base, ISelectable
{
    [SerializeField] private int _botSpawnCount = 3;
    [SerializeField] private int _baseBuildPrice = 5;

    [SerializeField] private BaseFlagSetter _baseFlagSetter;  

    public override void OnEnable()
    {
        base.OnEnable();
        _baseFlagSetter.FlagSet += StartBuildBase;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        _baseFlagSetter.FlagSet -= StartBuildBase;
    }

    public override void Start()
    {
        base.Start();
        Spawner.SpawnBots(_botSpawnCount);
        PlayerInput.Base.Select.performed += OnSelect;
    }

    private void OnSelect(InputAction.CallbackContext context) => _baseFlagSetter.TrySetFlag();

    private void StartBuildBase(Flag flag) => StartCoroutine(BuildNewBase(flag));

    private IEnumerator BuildNewBase(Flag flag)
    {
        bool isFreeBotFound = false;

        while (isFreeBotFound == false)
        {
            if (ResourceStorage.ResourceCount >= _baseBuildPrice)
            {
                foreach (Bot bot in Spawner.SpawnedBots)
                {
                    if (bot.ExplorationCoroutine == null)
                    {
                        Spawner.RemoveBot(bot);

                        ResourceStorage.RemoveResource(_baseBuildPrice);

                        bot.StartBuild(flag);

                        isFreeBotFound = true;

                        break;
                    }
                }
            }

            yield return WaitForFixedUpdate;
        }
    }
}
