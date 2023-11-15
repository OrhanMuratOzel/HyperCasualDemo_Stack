using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace GameTwo
{
    public class PoolManager : MonoBehaviour
    {

        [SerializeField] private PoolVariables<DroppableStackPiece> droppablePool;
        [Space]
        [SerializeField] private PoolVariables<Stack> stackPool;
        [Space]

        [SerializeField] private PoolVariables<Diamond> diamondPool;
        [Space]
        [SerializeField] private PoolVariables<Star> starPool;
        public void Init()
        {
            droppablePool.SetPool();
            stackPool.SetPool();

            diamondPool.SetPool();
            starPool.SetPool();
        }

        #region Star Pool
        public Star GetStar()
        {
            var obj = starPool.pool.GetFromPool();
            if (obj is null)
            {
                obj = Instantiate(starPool.prefab, starPool.instantiateTransform);
                obj.Init();
            }

            return obj == null ? Instantiate(starPool.prefab, starPool.instantiateTransform) : obj;
        }
        public void BackToPoolStar(Star star)
        {
            starPool.pool.BackToPool(star);
        }
        #endregion

        #region Diamond Pool
        public Diamond GetDiamond()
        {
            var obj = diamondPool.pool.GetFromPool();
            if (obj is null)
            {
                obj = Instantiate(diamondPool.prefab, diamondPool.instantiateTransform);
                obj.Init();
            }
            return obj == null ? Instantiate(diamondPool.prefab, diamondPool.instantiateTransform) : obj;
        }
        public void BackToPoolDiamond(Diamond diamond)
        {
            diamondPool.pool.BackToPool(diamond);
        }
        #endregion

        #region Stack Pool
        public Stack GetStack()
        {
            var obj = stackPool.pool.GetFromPool();
            if (obj is null)
            {
                obj = Instantiate(stackPool.prefab, stackPool.instantiateTransform);
                obj.Init();
            }

            return obj == null ? Instantiate(stackPool.prefab, stackPool.instantiateTransform) : obj;
        }
        public void BackToPoolStack(Stack stack)
        {
            stackPool.pool.BackToPool(stack);
        }
        #endregion

        #region Droppable Pool
        public DroppableStackPiece GetDroppable()
        {
            var obj = droppablePool.pool.GetFromPool();
            if (obj is null)
            {
                obj = Instantiate(droppablePool.prefab, droppablePool.instantiateTransform);
                obj.Init();
            }
            return obj;
        }
        public void BackToPoolDroppable(DroppableStackPiece droppable)
        {
            StartCoroutine(AutoCloser(droppable));
        }
        IEnumerator AutoCloser(DroppableStackPiece droppable)
        {
            yield return GameManager.droppableCloseDuration;
            droppablePool.pool.BackToPool(droppable);
        }
        #endregion
    }
    [System.Serializable]
    public class PoolVariables<T> where T : MonoBehaviour, IPoolObject
    {
        public T prefab;
        public List<T> staticList;
        [HideInInspector] public Pool<T> pool;
        public Transform instantiateTransform;
        public void SetPool()
        {
            pool = new Pool<T>();
            pool.SetPool(staticList);
        }
    }
    public class Pool<T> where T : MonoBehaviour, IPoolObject
    {
        private Queue<T> dynamicPool;
        private List<T> staticPoolList;

        #region Initiliaze
        public void SetPool(List<T> staticPoolList)
        {
            dynamicPool = new Queue<T>();
            this.staticPoolList = staticPoolList;
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
            var obj = dynamicPool.Count > 0 ? dynamicPool.Dequeue() : null;
            return obj;
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