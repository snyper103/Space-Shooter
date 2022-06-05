using UnityEngine;

public static class bestScore
{
    private static int[] _top5;

    static bestScore()
    {
        string score;

        _top5 = new int[5];

        for ( int i = 0; i < _top5.Length; ++i )
        {
            score = "Score"+i;

            _top5[i] = PlayerPrefs.GetInt(score, 0);
        }
    }

    public static void updateBestScore(int currentScore)
    {
        int aux;

        for ( int i = 0; i < _top5.Length; ++i )
            if ( currentScore >= _top5[i] )
            {
                aux = _top5[i];
                _top5[i] = currentScore;
                currentScore = aux;
            }
    }

    public static int popBestScore(int scoreIndex)
    {
        return _top5[scoreIndex];
    }

    public static int bestScoreSize()
    {
        return _top5.Length;
    }

    public static void saveBestScore()
    {
        string score;

        for ( int i = 0; i < _top5.Length; ++i )
        {
            score = "Score"+i;

            PlayerPrefs.SetInt(score, _top5[i]);
        }
    }
}
