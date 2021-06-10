//using System.Collections;
//using System.Collections.Generic;

//public class CoroutineController
//{
//    public Coroutine StartCoroutine(IEnumerator routine)
//    {
//        var coroutine = coroutinePool.Get();
//        coroutine.Reset(routine);
//        incomingCoroutines.Add(coroutine);
//        return coroutine;
//    }

//    public void StopCoroutine(Coroutine coroutine)
//    {
//        int index = activeCoroutines.FindIndex((c) => c == coroutine);

//        if (index != -1)
//        {
//            coroutinePool.Return(activeCoroutines[index]);
//            activeCoroutines.RemoveAt(index);
//        }
//    }

//    public void UpdateCoroutines()
//    {
//        activeCoroutines.AddRange(incomingCoroutines);
//        incomingCoroutines.Clear();

//        for (int i = 0; i < activeCoroutines.Count; i++)
//        {
//            if (activeCoroutines[i].Update() == false)
//            {
//                coroutinePool.Return(activeCoroutines[i]);
//                activeCoroutines[i] = null;
//            }
//        }

//        activeCoroutines.RemoveAll((x) => { return x == null; });
//    }

//    private List<Coroutine> activeCoroutines = new List<Coroutine>();
//    private List<Coroutine> incomingCoroutines = new List<Coroutine>();
//    //private ObjectPool<Coroutine> coroutinePool = ObjectPool.Create<Coroutine>(null);//new ObjectPool<Coroutine>();
//}