using System.Collections.Generic;
using UnityEngine;
using System;
namespace GameTwo
{
    [CreateAssetMenu(fileName = "New Level Data", menuName = "Level/Level Data")]
    public class LevelDataScriptable : ScriptableObject
    {
        public int stackCount;
        public List<StarData> starData;
        public List<DiamondData> diamondData;
        public LevelDataScriptable(int stackCount, List<StarData> starData, List<DiamondData> diamondData)
        {
            this.stackCount = stackCount;
            this.starData = starData;
            this.diamondData= diamondData;
        }
    }

    [Serializable]
    public class StarData
    {
        public BasicTransform transformData;
        public StarData(BasicTransform transformData)
        {
            this.transformData = transformData;
        }
    }
    [Serializable]
    public class DiamondData
    {
        public BasicTransform transformData;
        public DiamondData(BasicTransform transformData)
        {
            this.transformData = transformData;
        }
    }

    [Serializable]
    public class BasicTransform
    {
        public BasicTransform(Vector3 _startPosition, Quaternion _startRotation)
        {
            startPosition = _startPosition;
            startRotation = _startRotation;
        }
        public Vector3 startPosition;
        public Quaternion startRotation;
    }
}