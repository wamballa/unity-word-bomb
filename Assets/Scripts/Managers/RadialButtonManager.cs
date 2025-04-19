using UnityEngine;
using UnityEngine.UI;

public class RadialButtonManager : MonoBehaviour
{

    [SerializeField] private RMF_RadialMenu radialMenu;
    [SerializeField] private Button shuffleButton;

    private float timer;
    private float timerDelay = .1f;

    void Start()
    {
        shuffleButton?.onClick.AddListener(OnShuffleButtonPressed);
        OnShuffleButtonPressed();
    }

    private void Update()
    {

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnShuffleButtonPressed()
    {
        if (Time.time > timer)
        {
            Debug.Log("[RadialButtonManager] ShuffleRadialMenu");
            radialMenu.ShuffleElementsRuntime();
            timer = Time.time + timerDelay;
        }

    }

}
