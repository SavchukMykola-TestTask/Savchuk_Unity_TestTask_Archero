using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OneMonsterData
{
    public GameObject Monster;
    public float MinimalAbscissa;
    public float MaximalAbscissa;
    public float Ordinate;
    public float Applicate;
}

public class ObstaclesAndEnemiesSpawner : MonoBehaviour
{
    [SerializeField] private List<OneMonsterData> MonstersData = new();
    [SerializeField] private List<float> ApplicatesOfObstacles = new();
    [SerializeField] private List<GameObject> PossibleTemplatesOfObstacles = new();

    private void Awake()
    {
        for (int i = 0; i < MonstersData.Count; i++)
        {
            MonstersData[i].Monster.transform.position = new Vector3(UnityEngine.Random.Range(MonstersData[i].MinimalAbscissa, MonstersData[i].MaximalAbscissa), MonstersData[i].Ordinate, MonstersData[i].Applicate);
        }
        for (int i = 0; i < ApplicatesOfObstacles.Count; i++)
        {
            int IndexOfSelectedTemplate = UnityEngine.Random.Range(0, PossibleTemplatesOfObstacles.Count);
            GameObject NewObstacle = Instantiate(PossibleTemplatesOfObstacles[IndexOfSelectedTemplate]);
            NewObstacle.transform.position = new(NewObstacle.transform.position.x, NewObstacle.transform.position.y, ApplicatesOfObstacles[i]);
        }
    }
}
