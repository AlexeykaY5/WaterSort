using System;

public class LevelSelection
{
    private string selectedId;

    public string SelectedId
    {
        get
        {
            if (selectedId == null)
            {
                throw new InvalidOperationException("Difficulty is not selected, start the game from the Menu scene");
            }
            return selectedId;
        }
    }

    public void Select(string id)
    {
        selectedId = id;
    }
}