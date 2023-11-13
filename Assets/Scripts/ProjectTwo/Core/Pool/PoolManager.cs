using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace GameTwo
{
    public class PoolManager : MonoBehaviour
    {
        private Pool<Stack> stackPool;
        private Pool<DroppableStackPiece> droppablePool;
        [SerializeField] private List<DroppableStackPiece> staticDroppableList;
        [SerializeField] private List<Stack> staticStackList;
        [SerializeField] private Transform[] instantiateTransforms;
        [SerializeField] private DroppableStackPiece droppablePrefab;
        [SerializeField] private Stack stackPrefab;

        public void Init()
        {
            droppablePool = new(staticDroppableList,droppablePrefab, instantiateTransforms[0]);
            stackPool = new(staticStackList, stackPrefab, instantiateTransforms[1]);
        }

        #region Stack Pool
        public Stack GetStack()
        {
            return stackPool.GetFromPool();
        }
        public void BackToPoolStack(Stack stack)
        {
            stackPool.BackToPool(stack);
        }
        #endregion

        #region Droppable Pool
        public DroppableStackPiece GetDroppable()
        {
            return droppablePool.GetFromPool();
        }
        public void BackToPoolDroppable(DroppableStackPiece droppable)
        {
            StartCoroutine(AutoCloser(droppable));
        }
        IEnumerator AutoCloser(DroppableStackPiece droppable)
        {
            yield return GameManager.droppableCloseDuration;
            droppablePool.BackToPool(droppable);
        }
        #endregion
    }
    public class Pool<T> : MonoBehaviour where T :MonoBehaviour, IPoolObject
    {
        private Queue<T> dynamicPool;
        private List<T> staticPoolList;
        private T objPrefab;
        private Transform instantiateTransform;
        #region Initiliaze
        public Pool(List<T> staticPoolList, T gridPrefab,Transform instantiateTransform)
        {
            dynamicPool = new Queue<T>();
            this.staticPoolList = staticPoolList;
            this.objPrefab = gridPrefab;
            this.instantiateTransform = instantiateTransform;
            AddStaticListToQueue();
        }
        private void AddStaticListToQueue()
        {
            for (var i = 0; i < staticPoolList.Count; i++)
            {
               staticPoolList[i].Init();
                staticPoolList[i].gameObject.SetActive(false);
                BackToPool(staticPoolList[i]);
            }
        }
        #endregion

        #region Get From Pool
        public T GetFromPool()
        {
            var obj = dynamicPool.Count > 0 ? dynamicPool.Dequeue() : CreateGrid();
            return obj;
        }
        private T CreateGrid()
        {
            var poolObj = Instantiate(objPrefab, instantiateTransform);
            poolObj.Init();
            return poolObj;
        }
        #endregion

        #region Back To Pool
        public void BackToPool(T obj)
        {
            obj.gameObject.SetActive(false);
            dynamicPool.Enqueue(obj);
        }
        #endregion
    }
}