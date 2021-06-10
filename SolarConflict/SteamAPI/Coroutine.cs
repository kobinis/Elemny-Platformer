using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Coroutine
{
    public bool Paused { get; set; }

    internal void Reset(IEnumerator routine)
    {
        routines.Clear();
        routines.Push(routine);
        Paused = false;
    }

    internal bool Update()
    {
        if (!Paused)
        {
            if (routines.Peek().MoveNext() == false)
            {
                routines.Pop();
                if (routines.Count == 0)
                {
                    return false;
                }
            }
            else if (routines.Peek().Current is IEnumerator)
            {
                routines.Push((IEnumerator)routines.Peek().Current);
            }
        }

        return true;
    }

    private Stack<IEnumerator> routines = new Stack<IEnumerator>();
}