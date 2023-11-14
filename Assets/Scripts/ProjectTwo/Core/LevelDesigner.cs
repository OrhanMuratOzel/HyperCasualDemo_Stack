using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEditor;

namespace GameTwo
{
    public class LevelDesigner : MonoBehaviour
    {
        [SerializeField] private int lvID = 0;
        [SerializeField,Range(0,40)] private int platformLength = 1;
        [Space]
        [SerializeField] private Transform finishLane;
        [SerializeField] private StarData[] stars;
        [SerializeField] private DiamondData[] diamonds;
        [SerializeField] LevelDataScriptable currentLevel;

        public LevelDataScriptable CreateLevel()
        {
            var savePath = $"Assets/LevelData/GameTwo/LevelData {lvID}.asset";
            LevelDataScriptable tmp;
            if (AssetDatabase.LoadAssetAtPath<LevelDataScriptable>(savePath) is not null)
            {
                Debug.Log("File Exits");
                tmp = AssetDatabase.LoadAssetAtPath<LevelDataScriptable>(savePath);
                SetLevelObject(tmp);
            }
            else
            {
                var example = ScriptableObject.CreateInstance<LevelDataScriptable>();
                AssetDatabase.CreateAsset(example, savePath);
                SetLevelObject(example);
                tmp = example;
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            EditorUtility.SetDirty(tmp);
            currentLevel = tmp;
            if (currentLevel)
                Selection.activeObject = tmp;
            return tmp;
        }
        public void LoadLevel()
        {

        }
        private void SetLevelObject(LevelDataScriptable data)
        {
            //data.starData.Add();
            //- Set Starts
            //- Set Diamonds
            //- set Platform Length
            data.platformLength = platformLength;
        }
        private void AddStars(LevelDataScriptable data)
        {
           /* foreach (var item in stars)
            {
                data.starData.Add(new StarData(new BasicTransform(item.transformData)));
            }*/
        }
        private void AddDiamonds(LevelDataScriptable data)
        {
            /* foreach (var item in stars)
             {
                 data.starData.Add(new StarData(new BasicTransform(item.transformData)));
             }*/
        }
        private void SetPlatformLength(int Length)
        {
            var newPosition = Vector3.zero;
            newPosition.z = (1+Length)*2.5f;
            finishLane.transform.position = newPosition;
        }
        private void OnValidate()
        {
            if (Application.isPlaying)
                return;
            SetPlatformLength(platformLength);
        }
    }
}