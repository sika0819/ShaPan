using UnityEngine;
using System.Collections;

class CSequeue<T>
{
    private int maxsize;       //循环顺序队列的容量
    private T[] data;          //数组，用于存储循环顺序队列中的数据元素
    private int front;         //指示最近一个已经离开队列的元素所占有的位置 循环顺序队列的对头
    private int rear;          //指示最近一个进入队列的元素的位置           循环顺序队列的队尾

    public T this[int index]
    {
        get { return data[index]; }
        set { data[index] = value; }
    }

    //容量属性
    public int Maxsize
    {
        get { return maxsize; }
        set { maxsize = value; }
    }

    //对头指示器属性
    public int Front
    {
        get { return front; }
        set { front = value; }
    }

    //队尾指示器属性
    public int Rear
    {
        get { return rear; }
        set { rear = value; }
    }

    public CSequeue()
    {

    }

    public CSequeue(int size)
    {
        data = new T[size];
        maxsize = size;
        front = rear = -1;
    }

    //判断循环顺序队列是否为满
    public bool IsFull()
    {
        if ((front == -1 && rear == maxsize - 1) || (rear + 1) % maxsize == front)
            return true;
        else
            return false;
    }

    //清空循环顺序列表
    public void Clear()
    {
        front = rear = -1;
    }

    //判断循环顺序队列是否为空
    public bool IsEmpty()
    {
        if (front == rear)
            return true;
        else
            return false;
    }

    //入队操作
    public void Enqueue(T elem)
    {
        if (IsFull())
        {
            return;
        }
        rear = (rear + 1) % maxsize;
        data[rear] = elem;
    }

    //出队操作
    public T Dequeue()
    {
        if (IsEmpty())
        {
            return default(T);
        }
        front = (front + 1) % maxsize;
        return data[front];
    }

    //获取对头数据元素
    public T GetFront()
    {
        if (IsEmpty())
        {
            return default(T);
        }
        return data[(front + 1) % maxsize];//front从-1开始
    }

    //求循环顺序队列的长度
    public int GetLength()
    {
        return (rear - front + maxsize) % maxsize;
    }
}

