using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject menuPanel;
    public GameObject helpPanel;
    public void startGame()
    {
        SceneManager.LoadScene("Sample1");
    }
    public void exit()
    {
        Application.Quit();
    }
    public void ResetGameData()
    {
        PlayerPrefs.DeleteAll();
    }
    public void help()
    {
        menuPanel.SetActive(false); 
        helpPanel.SetActive(true);
    }
    public void back()
    {
        menuPanel.SetActive(true);
        helpPanel.SetActive(false);
    }
    public void GotoUrl()
    {
        Application.OpenURL("https://www.treesaregood.org/treeowner/plantingatree");
    }
}
