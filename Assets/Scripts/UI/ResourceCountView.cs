using TMPro;
using UnityEngine;

public class ResourceCountView : MonoBehaviour
{
    [SerializeField] private ResourceStorage _resourceCounter;
    [SerializeField] private TextMeshProUGUI _count;

    private void OnEnable() => _resourceCounter.ResourceAdded += ShowInfo;

    private void OnDisable() => _resourceCounter.ResourceAdded -= ShowInfo;

    private void ShowInfo(int count) => _count.text = count.ToString();
}
