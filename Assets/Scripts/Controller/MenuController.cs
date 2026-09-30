using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private MenuView menuView;
    [SerializeField] private DifficultySettings[] difficulties;

    private readonly RecordStorage recordStorage = new RecordStorage();

    private void Awake()
    {
        menuView.PlayClicked += OnPlayClicked;
    }

    private void Start()
    {
        foreach(DifficultySettings difficulty in difficulties)
        {
            menuView.AddCard(difficulty.difficultyName, recordStorage.Load(difficulty.id));
        }
    }

    private void OnPlayClicked(int index)
    {
        SceneFlow.LoadGame(difficulties[index]);
    }
}
