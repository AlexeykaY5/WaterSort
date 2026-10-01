using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardView : MonoBehaviour
{
    public event Action<int> TubeClicked;

    [SerializeField] private TubeView tubeViewPrefab;
    [SerializeField] private GridLayoutGroup grid;

    private Vector2 baseCellSize;
    private int maxColumns;
    private readonly List<TubeView> tubeViews = new List<TubeView>();

    private void Awake()
    {
        baseCellSize = grid.cellSize;
        maxColumns = grid.constraintCount;
    }

    public void Build(int tubeCount, int capacity)
    {
        FitCells(tubeCount);

        foreach(TubeView view in tubeViews)
        {
            Destroy(view.gameObject);
        }

        tubeViews.Clear();

        for(int i = 0; i < tubeCount; i++)
        {
            TubeView view = Instantiate(tubeViewPrefab, transform);
            view.Init(i);
            view.Build(capacity);
            view.Clicked += OnTubeClicked;
            tubeViews.Add(view);
        }

    }

    public void RenderTube(int tubeId, IReadOnlyList<Color> colors)
    {
        tubeViews[tubeId].Render(colors);
    }

    public void SetSelected(int selectedId)
    {
        for(int i = 0; i < tubeViews.Count; i++)
        {
            tubeViews[i].SetSelected(i == selectedId);
        }
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
        TubeClicked?.Invoke(index);
    }
}
