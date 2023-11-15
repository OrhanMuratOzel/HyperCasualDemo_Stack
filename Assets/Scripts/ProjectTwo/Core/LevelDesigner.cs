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
        [SerializeField] private Star[] stars;
        [SerializeField] private Diamond[] diamonds;
        [SerializeField] LevelDataScriptable currentLevel;
        private List<Star> starsActiveList;
        private List<Diamond> diamondActiveList;
        private PoolManager poolManager;
        public LevelDataScriptable GetSelectedLevel => currentLevel;
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
        public void Init(PoolManager poolManager)
        {
            this.poolManager = poolManager;
            starsActiveList = new();
            diamondActiveList = new();
            CloseStaticList();
        }
        private void CloseStaticList()
        {
            foreach (var item in stars)
            {
                item.gameObject.SetActive(false);
            }
            foreach (var item in diamonds)
            {
                item.gameObject.SetActive(false);
            }
        }
        public void LoadLevel(LevelDataScriptable lvData)
        {
            ResetStarsAndDiamonds();
            SetPlatformLength(lvData.platformLength);
            SetStars(lvData);
            SetDiamonds(lvData);
        }
        private void SetLevelObject(LevelDataScriptable data)
        {
            data.platformLength = platformLength;
            data.stackCount = platformLength;
            AddStars(data);
            AddDiamonds(data);
        }
        private void AddStars(LevelDataScriptable data)
        {
            data.starData = new();
            foreach (var item in stars)
            {
                if(item.gameObject.activeInHierarchy)
                    data.starData.Add(new StarData(new BasicTransform(item.transform.localPosition, item.transform.localRotation)));
            }
        }
        private void AddDiamonds(LevelDataScriptable data)
        {
            data.diamondData = new();
            foreach (var item in diamonds)
            {
                if (item.gameObject.activeInHierarchy)
                    data.diamondData.Add(new DiamondData(new BasicTransform(item.transform.localPosition, item.transform.localRotation)));
            }
        }
        private void SetPlatformLength(int Length)
        {
            var newPosition = Vector3.zero;
            newPosition.z = (1+Length)*2.5f;
            finishLane.transform.position = newPosition;
        }
        private void ResetStarsAndDiamonds()
        {
            foreach (var item in starsActiveList)
            {
                poolManager.BackToPoolStar(item);
            }
            foreach (var item in diamondActiveList)
            {
                poolManager.BackToPoolDiamond(item);
            }
            diamondActiveList.Clear();
            starsActiveList.Clear();
        }
        private void SetStars(LevelDataScriptable data)
        {
            Star tmpStar;
            foreach (var item in data.starData)
            {
                tmpStar = poolManager.GetStar();
                tmpStar.transform.SetPositionAndRotation(item.transformData.startPosition, item.transformData.startRotation);
                tmpStar.gameObject.SetActive(true);
                tmpStar.Reset();
                starsActiveList.Add(tmpStar);
            }
        }
        private void SetDiamonds(LevelDataScriptable data)
        {
            Diamond tmpDiamond;
            foreach (var item in data.diamondData)
            {
                tmpDiamond = poolManager.GetDiamond();
                tmpDiamond.transform.SetPositionAndRotation(item.transformData.startPosition, item.transformData.startRotation);
                tmpDiamond.gameObject.SetActive(true);
                tmpDiamond.Reset();
                diamondActiveList.Add(tmpDiamond);
            }
        }
        private void OnValidate()
        {
            if (Application.isPlaying)
                return;
            SetPlatformLength(platformLength);
        }
    }
}