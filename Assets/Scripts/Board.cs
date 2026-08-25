using UnityEngine;

public class Board : MonoBehaviour
{
    public DifficultySettings settings;
    public TubeView tubeViewPrefab;
    public Transform tubeContainer;
    private Tube[] tubes;
    private TubeView[] tubeViews;


    void Start()
    {
        tubes = LevelGenerator.Generate(settings);
        tubeViews = new TubeView[tubes.Length];

        for(int i = 0; i < tubes.Length; i++)
        {
            TubeView view = Instantiate(tubeViewPrefab, tubeContainer);
            tubeViews[i] = view;
            view.Render(tubes[i]);

        }
    }
}
