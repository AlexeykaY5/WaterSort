using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BoardView : MonoBehaviour
{
    public event Action<int> TubeClicked;

    [SerializeField] private TubeView tubeViewPrefab;
    [SerializeField] private GridLayoutGroup grid;

    private IGameState state;
    private ColorPalette palette;

    private readonly List<TubeView> tubeViews = new List<TubeView>();
    private readonly List<List<string>> drawnContents = new List<List<string>>();
    private int drawnSelected = -1;
    private int drawnRound = -1;
    private Vector2 baseCellSize;
    private int maxColumns;

    [Inject]
    public void Construct(IGameState state, ColorPalette palette)
    {
        this.state = state;
        this.palette = palette;
    }

    private void Awake()
    {
        baseCellSize = grid.cellSize;
        maxColumns = grid.constraintCount;
    }

    private void Update()
    {
        if (state.TubeStates.Count == 0)
        {
            return;
        }

        if (tubeViews.Count == 0)
        {
            Build();
        }

        bool isNewRound = state.Round != drawnRound;
        if (isNewRound)
        {
            drawnRound = state.Round;
            for (int i = 0; i < tubeViews.Count; i++)
            {
                tubeViews[i].ResetFill();
                drawnContents[i].Clear();
            }
        }

        if (state.SelectedIndex != drawnSelected)
        {
            drawnSelected = state.SelectedIndex;
            for (int i = 0; i < tubeViews.Count; i++)
            {
                tubeViews[i].SetSelected(i == drawnSelected);
            }
        }

        int pourFrom = -1;
        int pourTo = -1;

        for (int i = 0; i < tubeViews.Count; i++)
        {
            IReadOnlyList<string> contents = state.TubeStates[i].Contents;
            List<string> drawn = drawnContents[i];

            if (IsSame(contents, drawn))
            {
                continue;
            }

            if (contents.Count < drawn.Count)
            {
                pourFrom = i;
            }
            else if (contents.Count > drawn.Count)
            {
                pourTo = i;
            }

            drawn.Clear();
            drawn.AddRange(contents);
            tubeViews[i].Render(ToColors(contents));
        }

        if (!isNewRound && pourFrom != -1 && pourTo != -1)
        {
            tubeViews[pourFrom].Pour.Play(tubeViews[pourTo].Pour);
        }
    }

    public bool IsBusy()
    {
        foreach (TubeView view in tubeViews)
        {
            if (view.Pour.IsPouring)
            {
                return true;
            }
        }

        for (int i = 0; i < tubeViews.Count; i++)
        {
            if (!IsSame(state.TubeStates[i].Contents, drawnContents[i]))
            {
                return true;
            }
        }

        return false;
    }

    private void Build()
    {
        int tubeCount = state.TubeStates.Count;
        int capacity = state.TubeStates[0].Capacity;

        FitCells(tubeCount);

        for (int i = 0; i < tubeCount; i++)
        {
            TubeView view = Instantiate(tubeViewPrefab, transform);
            view.Init(i);
            view.Build(capacity);
            view.Clicked += OnTubeClicked;
            tubeViews.Add(view);
            drawnContents.Add(new List<string>());
        }
    }

    private Color[] ToColors(IReadOnlyList<string> colorNames)
    {
        Color[] result = new Color[colorNames.Count];
        for (int i = 0; i < colorNames.Count; i++)
        {
            result[i] = palette.GetColor(colorNames[i]);
        }
        return result;
    }

    private static bool IsSame(IReadOnlyList<string> current, List<string> drawn)
    {
        if (current.Count != drawn.Count)
        {
            return false;
        }

        for (int i = 0; i < current.Count; i++)
        {
            if (current[i] != drawn[i])
            {
                return false;
            }
        }
        return true;
    }

    private void FitCells(int tubeCount)
    {
        int columns = Mathf.Min(tubeCount, maxColumns);
        int rows = Mathf.CeilToInt((float)tubeCount / columns);

        Rect rect = ((RectTransform)transform).rect;
        float width = rect.width - grid.padding.horizontal;
        float height = rect.height - grid.padding.vertical;

        if (width <= 0 || height <= 0)
        {
            return;
        }

        float cellWidth = (width - grid.spacing.x * (columns - 1)) / columns;
        float cellHeight = (height - grid.spacing.y * (rows - 1)) / rows;

        float scale = Mathf.Min(cellWidth / baseCellSize.x, cellHeight / baseCellSize.y, 1f);
        grid.cellSize = baseCellSize * scale;
    }

    private void OnTubeClicked(int index)
    {
        if (IsBusy())
        {
            return;
        }
        TubeClicked?.Invoke(index);
    }
}


