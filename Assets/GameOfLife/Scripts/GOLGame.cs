using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOLGame : MonoBehaviour
{
    
    [SerializeField] private ComputeShader cellularAutomata;
    [SerializeField] private ComputeShader previousTexture;

    public RenderTexture Result;
    public RenderTexture PreviousResult;
    
    // Start is called before the first frame update
    void Start()
    {
        // inject game to render feature
        
        Result = new RenderTexture(2048, 2048, 24)
        {
            enableRandomWrite = true
        };
        Result.Create();
        
        PreviousResult = new RenderTexture(2048, 2048, 24)
        {
            enableRandomWrite = true
        };
        PreviousResult.Create();
        
        cellularAutomata.SetTexture(0, "Result", Result);
        cellularAutomata.SetTexture(0, "PreviousResult", PreviousResult);
        
        previousTexture.SetTexture(0, "Result", Result);
        previousTexture.SetTexture(0, "PreviousResult", PreviousResult);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateMap()
    {
        cellularAutomata.Dispatch(
            0,
            Result.width / 8,
            Result.height / 8,
            1
        );
        previousTexture.Dispatch(
            0,
            PreviousResult.width / 8,
            PreviousResult.height / 8,
            1
        );
    }
}
