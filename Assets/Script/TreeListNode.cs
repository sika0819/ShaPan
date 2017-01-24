using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
class MLTree<T> : ITree<TreeNode<T>>
{
    private TreeNode<T> root;//根节点
    public TreeNode<T> RootNode {
        get {
            return root;
        }set {
            root = value;
        }
    }
    public MLTree() {
        root = null;
    }
    public MLTree(TreeNode<T> node)
    {
        root = node;
    }
    public TreeNode<T> Child(TreeNode<T> t, int index)
    {
        if (t.hasChild() && index > t.Childs.Count)
            return t.Childs[index];
        else
            return null;
    }

    public void Clear()
    {//待测试；
        TreeNode<T> temp = root;
        CSequeue<TreeNode<T>> queue = new CSequeue<TreeNode<T>>();
        queue.Enqueue(temp);
        while (!queue.IsEmpty()) {
            temp = (TreeNode<T>)queue.Dequeue();
            for (int i = 0; i < temp.Childs.Count; i++)
            {
                queue.Enqueue(temp.Childs[i]);
                temp.Childs.Clear();
            }
        }
        root = null;
    }
    /// <summary>
    /// 删除t的第i颗子树，返回第i颗子树的根节点
    /// </summary>
    /// <param name="t"></param>
    /// <param name="i"></param>
    /// <returns></returns>
    public TreeNode<T> Delete(TreeNode<T> t, int i)
    {
        t = FindNode(t);
        TreeNode<T> node = null;
        if (t != null && t.Childs.Count > i)
        {
            node = t.Childs[i];
            t.Childs[i] = null;
        }
        return node;
    }

    public int GetDepth(TreeNode<T> t)
    {
        int len;
        if (root == null) {
            return 0;
        }
        for (int i = 0; i < t.Childs.Count; i++) {
            if (t.Childs[i] != null)
            {
                len = GetDepth(t.Childs[i]);
                return len + 1;
            }
        }
        return 0;
    }

    public bool Insert(TreeNode<T> s, TreeNode<T> t, int i)
    {
        if (s.hasChild())
        {
            s.Childs.Insert(i, t);
        }
        return false;
    }
    public void AddChild(TreeNode<T> s,TreeNode<T> t)
    {
        if (s.hasChild())
        {
            s.Childs.Add(t);
        }
    }
    public bool isEmpty()
    {
        return root == null;
    }

    public TreeNode<T> Parent(TreeNode<T> t)
    {
        TreeNode<T> temp = root;
        if (isEmpty() || t == null)//没数据的情况
            return null;
        if (root.Data.Equals(t.Data))
        {//根节点，当然没有
            return null;
        }
        Queue<TreeNode<T>> queue = new Queue<TreeNode<T>>();
        queue.Enqueue(temp);
        while (queue.Count > 0)
        {
            temp = (TreeNode<T>)queue.Dequeue();
            if (temp.hasChild())
            {
                for (int i = 0; i < temp.Childs.Count; i++)
                {
                    if (temp.Childs[i].Data.Equals(t.Data))
                    {
                        return temp;
                    }
                    else
                    {
                        queue.Enqueue(temp.Childs[i]);
                    }
                }
            }
        }
        return temp;
    }
    TreeNode<T> FindNode(TreeNode<T> t)
    {
        if (root.Data.Equals(t.Data))
            return root;
        TreeNode<T> pt = Parent(t);
        foreach (TreeNode<T>temp in pt.Childs)
        {
            if (temp != null && temp.Data.Equals(t.Data))
            {
                return temp;
            }
        }
        return null;
    }
    public TreeNode<T> RightSibling(TreeNode<T> t)
    {
        TreeNode<T> pt = Parent(t);
        if (pt != null)
        {
            int i = FindRank(t);
            return Child(pt, i + 1);
        }
        else
        {
            return null;
        }
    }

    public TreeNode<T> Root()
    {
        return root;
    }
    private int FindRank(TreeNode<T> t)
    {
        TreeNode<T> pt = Parent(t);
        for (int i = 0; i < pt.Childs.Count; i++)
        {
            TreeNode<T> temp = pt.Childs[i];
            if (temp != null && temp.Data.Equals(t.Data))
            {
                return i;
            }
        }
        return -1;
    }
    //先序遍历
    //根结点->遍历根结点的左子树->遍历根结点的右子树 
    public void preorder(TreeNode<T> root)
    {
        if (root == null)
            return;
        Console.WriteLine(root.Data + " ");
        for (int i = 0; i < root.Childs.Count; i++)
        {
            preorder(root.Childs[i]);
        }
    }


    //后序遍历
    //遍历根结点的左子树->遍历根结点的右子树->根结点
    public void postorder(TreeNode<T> root)
    {
        if (root == null)
        { return; }
        for (int i = 0; i < root.Childs.Count; i++)
        {
            postorder(root.Childs[i]);
        }
        Console.WriteLine(root.Data + " ");
    }


    //层次遍历
    //引入队列 
    public void LevelOrder(TreeNode<T> root)
    {
        Console.WriteLine("遍历开始：");
        if (root == null)
        {
            Console.WriteLine("没有结点！");
            return;
        }

        TreeNode<T> temp = root;

        CSequeue<TreeNode<T>> queue = new CSequeue<TreeNode<T>>(50);
        queue.Enqueue(temp);
        while (!queue.IsEmpty())
        {
            temp = (TreeNode<T>)queue.Dequeue();
            Console.WriteLine(temp.Data + " ");
            for (int i = 0; i < temp.Childs.Count; i++)
            {
                if (temp.Childs[i] != null)
                {
                    queue.Enqueue(temp.Childs[i]);
                }
            }
        }
        Console.WriteLine("遍历结束！");
    }

    public void Traverse(int TraverseType)
    {
        if (TraverseType == 0) preorder(root);
        else if (TraverseType == 1) postorder(root);
        else LevelOrder(root);

    }
}
