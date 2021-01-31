using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pool<T>
{

    /** The maximum number of objects that will be pooled. */
    public int max;

    /** The highest number of free objects. Can be reset any time. */
    public int peak;

    private readonly Stack<T> freeObjects;

    /** Creates a pool with an initial capacity of 16 and no maximum. */
    public Pool() : this(int.MaxValue)
    {

    }

    /** @param max The maximum number of free objects to store in this pool. */
    public Pool(int max)
    {
        freeObjects = new Stack<T>();
        this.max = max;
    }

    abstract protected T NewObject();


    /** Returns an object from this pool. The object may be new (from {@link #newObject()}) or reused (previously
	 * {@link #free(Object) freed}). */
    public T Obtain()
    {
        return freeObjects.Count == 0 ? NewObject() : freeObjects.Pop();
    }

    /** Puts the specified object in the pool, making it eligible to be returned by {@link #obtain()}. If the pool already contains
	 * {@link #max} free objects, the specified object is reset but not added to the pool.
	 * <p>
	 * The pool does not check if an object is already freed, so the same object must not be freed multiple times. */
    public void Free(T obj)
    {
        if (obj == null) throw new ArgumentException("object cannot be null.");
        if (freeObjects.Count < max)
        {
            freeObjects.Push(obj);
            peak = Mathf.Max(peak, freeObjects.Count);
        }
        Reset(obj);
    }

    /** Called when an object is freed to clear the state of the object for possible later reuse. The default implementation calls
	 * {@link Poolable#reset()} if the object is {@link Poolable}. */
    protected void Reset(T obj)
    {
        if (obj is Poolable) ((Poolable)obj).Reset();
    }

    /** Puts the specified objects in the pool. Null objects within the array are silently ignored.
	 * <p>
	 * The pool does not check if an object is already freed, so the same object must not be freed multiple times.
	 * @see #free(Object) */
    public void FreeAll(List<T> objects)
    {
        if (objects == null) throw new ArgumentException("objects cannot be null.");
        Stack<T> freeObjects = this.freeObjects;
        int max = this.max;
        for (int i = 0; i < objects.Count; i++)
        {
            T obj = objects[i];
            if (obj == null) continue;
            if (freeObjects.Count < max) freeObjects.Push(obj);
            Reset(obj);
        }
        peak = Mathf.Max(peak, freeObjects.Count);
    }

    /** Removes all free objects from this pool. */
    public void Clear()
    {
        freeObjects.Clear();
    }

    /** The number of objects available to be obtained. */
    public int GetFree()
    {
        return freeObjects.Count;
    }

    /** Objects implementing this interface will have {@link #reset()} called when passed to {@link Pool#free(Object)}. */
    public interface Poolable
    {
        /** Resets the object for reuse. Object references should be nulled and fields may be set to default values. */
        void Reset();
    }
}
