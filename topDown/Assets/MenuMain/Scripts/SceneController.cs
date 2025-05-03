using UnityEngine;

public class SceneController : MonoBehaviour
{
    public OptionManager optionsPanel;
    void Start()
    {
        optionsPanel = GameObject.FindGameObjectWithTag("Options").GetComponent<OptionManager>();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            ShowOptions();
        }
    }
    
    public void ShowOptions()
    {
        optionsPanel.screenOptions.SetActive(true);
    }
}
