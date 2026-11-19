using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardView : MonoBehaviour
{
    [SerializeField] private TubeView tubeViewPrefab;
    [SerializeField] private GridLayoutGroup grid;
    [SerializeField] private GameController controller;
    [SerializeField] private ColorPalette palette;

    private readonly List<TubeView> tubeViews = new List<TubeView>();

    private void Awake()
    {
        controller.LevelStarted += OnLevelStarted;
        controller.SelectionChanged += OnSelectionChanged;
        controller.Poured += OnPoured;
    }

    private void OnDestroy()
    {
        controller.LevelStarted -= OnLevelStarted;
        controller.SelectionChanged -= OnSelectionChanged;
        controller.Poured -= OnPoured;
    }

    private void OnLevelStarted()
    {
        if(tubeViews.Count == 0)
        {
            Build();
        }

        for(int i = 0; i < tubeViews.Count; i++)
        {
            tubeViews[i].SetSelected(false);
            tubeViews[i].ResetFill();
            tubeViews[i].Render(ToColors(controller.Model.TubeStates[i].Contents), false);
        }
    }

    private void OnSelectionChanged(int previous, int current)
    {
        if (previous != -1)
        {
            tubeViews[previous].SetSelected(false);
        }

        if (current != -1)
        {
            tubeViews[current].SetSelected(true);
        }
    }

    private void OnPoured(int fromIndex, int toIndex)
    {
        tubeViews[fromIndex].Render(ToColors(controller.Model.TubeStates[fromIndex].Contents), true);
        tubeViews[toIndex].Render(ToColors(controller.Model.TubeStates[toIndex].Contents), true);
        tubeViews[fromIndex].Pour.Play(tubeViews[toIndex].Pour, () => controller.PourFinished(fromIndex, toIndex));
    }

    private void Build()
    {
        int tubeCount = controller.Model.TubeStates.Count;
        int capacity = controller.Model.TubeStates[0].Capacity;

        FitCells(tubeCount);

        for (int i = 0; i < tubeCount; i++)
        {
            TubeView view = Instantiate(tubeViewPrefab, transform);
            view.Init(i);
            view.Build(capacity);
            view.Clicked += OnTubeClicked;
            tubeViews.Add(view);
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

    private void FitCells(int tubeCount)
    {
        int columns = Mathf.Min(tubeCount, grid.constraintCount);
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

        float scale = Mathf.Min(cellWidth / grid.cellSize.x, cellHeight / grid.cellSize.y, 1f);
        grid.cellSize = grid.cellSize * scale;
    }

    private void OnTubeClicked(int index)
    {
        controller.ClickTube(index);
    }
}