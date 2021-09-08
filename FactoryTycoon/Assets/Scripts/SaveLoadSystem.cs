using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject _helper = null;

    [SerializeField]
    private List<Button> buildings = null;
    private void Start()
    {
        if (GameState.Singleton.IsLoadedGame)
        {
            if (_helper != null)
            {
                Destroy(_helper);
            }

            if (buildings != null)
            {
                for (int i = 0; i < GameState.Singleton.GetTable() - 1; i++)
                {
                    buildings[i].interactable = false;
                    buildings[i].GetComponent<Outline>().enabled = true;
                }
            }
        }
    }

    public void LoadGame(InputField ipf)
    {
        int lastDigit = 0;

        foreach (var word in ipf.text)
        {
            if (char.IsDigit(word))
            {
                lastDigit = int.Parse(word.ToString());
            }
        }

        GameState.Singleton.SetLoadedGame(lastDigit);
    }

    public void ShowGeneratedCode(InputField ipf)
    {
        ipf.text = GenerateSaveCode();
    }

    private string GenerateSaveCode()
    {
        return "ehaeifhfuf" + GameState.Singleton.GetTable().ToString();
    }
}
