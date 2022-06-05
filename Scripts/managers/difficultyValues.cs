using UnityEngine;

public static class difficultyValues
{
    public enum Difficulties
    {
        Easy,
        Medium,
        Hard
    };

    public static Difficulties difficulty;
    public static bool isCoopMode;

    static difficultyValues()
    {
        switch ( PlayerPrefs.GetInt("Difficulty", 0) )
        {
            case 0:
                difficulty = Difficulties.Easy;
                break;
            case 1:
                difficulty = Difficulties.Medium;
                break;
            case 2:
                difficulty = Difficulties.Hard;
                break;
        }
    }

    public static void saveDifficultyValue()
    {
        PlayerPrefs.SetInt("Difficulty", (int)difficulty);
    }
}
