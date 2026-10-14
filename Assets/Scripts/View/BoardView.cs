using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardView : MonoBehaviour
{
    public event Action<int> TubeClicked;

    [SerializeField] private TubeView tubeViewPrefab;
    [SerializeField] private GridLayoutGroup grid;

    private readonly List<int> layerCounts = new List<int>();
    private int pourFrom = -1;
    private int pourTo = -1;
    private Vector2 baseCellSize;
    private int maxColumns;
    private readonly List<TubeView> tubeViews = new List<TubeView>();

    private void Awake()
    {
        baseCellSize = grid.cellSize;
        maxColumns = grid.constraintCount;
    }

    private void Update()
    {
        if(pourFrom != -1 && pourTo != -1)
        {
            tubeViews[pourFrom].Pour.Play(tubeViews[pourTo].Pour);
        }

        pourFrom = -1;
        pourTo = -1;
    }

    public void Build(int tubeCount, int capacity)
    {
        FitCells(tubeCount);

        foreach(TubeView view in tubeViews)
        {
            Destroy(view.gameObject);
        }

        tubeViews.Clear();
        layerCounts.Clear();

        for(int i = 0; i < tubeCount; i++)
        {
            TubeView view = Instantiate(tubeViewPrefab, transform);
            view.Init(i);
            view.Build(capacity);
            view.Clicked += OnTubeClicked;
            tubeViews.Add(view);
            layerCounts.Add(0);
        }
    }

    public void RenderTube(int tubeId, IReadOnlyList<Color> colors)
    {
        int oldCount = layerCounts[tubeId];
        if(colors.Count < oldCount)
        {
            pourFrom = tubeId;
        }else if(colors.Count > oldCount)
        {
            pourTo = tubeId;
        }

        layerCounts[tubeId] = colors.Count;
        tubeViews[tubeId].Render(colors);
    }

    public void SetSelected(int selectedId)
    {
        for(int i = 0; i < tubeViews.Count; i++)
        {
            tubeViews[i].SetSelected(i == selectedId);
        }
    }

    public bool IsAnyPouring()
    {
        foreach (TubeView view in tubeViews)
        {
            if (view.Pour.IsPouring)
            {
                return true;
            }
        }
        return false;
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
        if (IsAnyPouring())
        {
            return;
        }
        TubeClicked?.Invoke(index);
    }
}
