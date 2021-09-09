using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadSystem : MonoBehaviour
{
    private const string _secretCode = "ehaeifhfuf";
    [SerializeField]
    private GameObject _helper = null;

    [SerializeField]
    private GameObject _warringText = null;

    [SerializeField]
    private SceneLoader _sceneLoader = null;

    [SerializeField]
    private List<Button> _buildings = null;
    private void Start()
    {
        if (GameState.Singleton.IsLoadedGame)
        {
            if (_helper != null)
            {
                Destroy(_helper);
            }
        }
        if (_buildings.Count != 0)
        {
            for (int i = 0; i < 8; i++)
            {
                if (i < GameState.Singleton.GetTable() - 1)
                {
                    _buildings[i].interactable = false;
                    _buildings[i].GetComponent<Outline>().enabled = true;
                    _buildings[i].GetComponent<Outline>().effectColor = new Color(0.3f, 1f, 0.05f, 0.75f);
                }
                else if (i > GameState.Singleton.GetTable() - 1)
                {
                    _buildings[i].interactable = false;
                    _buildings[i].GetComponent<Outline>().enabled = true;
                    _buildings[i].GetComponent<Outline>().effectColor = new Color(1f, 0.05f, 0.05f, 0.75f);
                }
            }
        }
    }

    public void LoadGame(InputField ipf)
    {
        int lastDigit = 0;
        var saveCode = new StringBuilder();

        foreach (var word in ipf.text)
        {
            if (char.IsDigit(word))
            {
                lastDigit = int.Parse(word.ToString());
            }
            else if (char.IsLetter(word))
            {
                saveCode.Append(word);
            }
        }

        if (saveCode.ToString() == _secretCode && lastDigit > 0 && lastDigit <= 8)
        {
            _warringText.SetActive(false);
            GameState.Singleton.SetLoadedGame(lastDigit);
            _sceneLoader.MainMenu();
        }
        else
        {
            _warringText.SetActive(true);
        }
    }

    public void ShowGeneratedCode(InputField ipf)
    {
        ipf.text = GenerateSaveCode();
    }

    private string GenerateSaveCode()
    {
        var saveCode = new StringBuilder();
        saveCode.Append(_secretCode);

        for (int i = 0; i < saveCode.Length; i++)
        {
            if (Random.Range(0, 5) == 0)
            {
                saveCode.Insert(i, Random.Range(0, 30).ToString());
            }
        }
        saveCode.Append(GameState.Singleton.GetTable().ToString());

        return saveCode.ToString();
    }
}
