using UnityEngine;

public class RecordStorage
{
    public float? Load(string id)
    {
        string key = GetKey(id);

        if (PlayerPrefs.HasKey(key))
        {
            return PlayerPrefs.GetFloat(key);
        }
        return null;
    }

    public void Save(string id, float time)
    {
        PlayerPrefs.SetFloat(GetKey(id), time);
        PlayerPrefs.Save();
    }

    private string GetKey(string id)
    {
        return $"record_{id}";
    }
}