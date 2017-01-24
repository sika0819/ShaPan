using UnityEngine;
using System.Collections;
using System;
public interface ITree<T> {
    T Root();//求根节点
    T Parent(T t);//求节点t的父节点
    T Child(T t, int index);//求节点t的第index个孩子节点
    T RightSibling(T t);//求节点t第一个右兄弟节点
    bool Insert(T s, T t, int i);//将树s加入树种作为节点t的第i颗子树
    T Delete(T t, int i);//删除节点t的第i颗子树
    void Traverse(int TraverseType);//按某种方式便历树
    void Clear();//清空树
    bool isEmpty();//判断是否为空
    int GetDepth(T t);//求树的深度
}
