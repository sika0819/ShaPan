using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.CodeDom;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Text;
using System.IO;
using System;
using System.Collections.Generic;
using System.Reflection;

[CustomEditor(typeof(Manager))]
public class ExportName: Editor  {
    Manager manager;
    Dictionary<string, Anim> AnimationDic;
    Dictionary<string, WayPoint> WayPointDic;
    void OnSceneGUI()
    {
        manager = (Manager)target;
        AnimationDic = new Dictionary<string, Anim>();
        WayPointDic = new Dictionary<string, WayPoint>();
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("导出动画名称"))
        {
#if UNITY_EDITOR
            GenerateScript();
#endif
        }
    }
    void GenerateScript()
    {
        AnimationDic = new Dictionary<string, Anim>();
        WayPointDic = new Dictionary<string, WayPoint>();
        Anim[] allAnim = GameObject.FindObjectsOfType<Anim>();
        WayPoint[] allWay = GameObject.FindObjectsOfType<WayPoint>();
        AnimArea[] allArea = GameObject.FindObjectsOfType<AnimArea>();
        for (int iLoop = 0; iLoop < allAnim.Length; iLoop++)
        {
            AnimationDic.Add(allAnim[iLoop].name, allAnim[iLoop]);
        }
        for (int iLoop = 0; iLoop < allWay.Length; iLoop++)
        {
            WayPointDic.Add(allWay[iLoop].animName, allWay[iLoop]);
        }
        //准备一个代码编译器单元  
        CodeCompileUnit unit = new CodeCompileUnit();
        //准备必要的命名空间（这个是指要生成的类的空间）  
        CodeNamespace sampleNamespace = new CodeNamespace("");
        //导入必要的命名空间  
        sampleNamespace.Imports.Add(new CodeNamespaceImport("System"));
        sampleNamespace.Imports.Add(new CodeNamespaceImport("UnityEngine"));
        sampleNamespace.Imports.Add(new CodeNamespaceImport("System.Collections"));
        //准备要生成的类的定义  
        CodeTypeDeclaration Customerclass = new CodeTypeDeclaration("AnimName");
        //指定这是一个Class  
        Customerclass.IsClass = true;
        Customerclass.TypeAttributes = TypeAttributes.Public;
        
        //把这个类放在这个命名空间下  
        sampleNamespace.Types.Add(Customerclass);
   
        //这是输出文件  
        string outputFile = Application.dataPath+"/Script/AnimName.cs";
        //添加字段  
        foreach (KeyValuePair<string, Anim> item in AnimationDic)
        {
            CodeMemberField field = new CodeMemberField(typeof(System.String), item.Key);
            field.InitExpression = new CodePrimitiveExpression(item.Key);
            field.Attributes = MemberAttributes.Public|MemberAttributes.Static;
            Customerclass.Members.Add(field);
        }

        CodeTypeDeclaration Customerclass2 = new CodeTypeDeclaration("WayPointName");
        //指定这是一个Class  
        Customerclass2.IsClass = true;
        Customerclass2.TypeAttributes = TypeAttributes.Public;
        foreach (KeyValuePair<string, WayPoint> item in WayPointDic)
        {
            CodeMemberField field = new CodeMemberField(typeof(System.String), item.Key);
            field.InitExpression = new CodePrimitiveExpression(item.Key);
            field.Attributes = MemberAttributes.Public | MemberAttributes.Static;
            Customerclass2.Members.Add(field);
        }
        //把这个类放在这个命名空间下  
        sampleNamespace.Types.Add(Customerclass2);
        CodeTypeDeclaration Customerclass3 = new CodeTypeDeclaration("AreaName");
        //指定这是一个Class  
        Customerclass3.IsClass = true;
        Customerclass3.TypeAttributes = TypeAttributes.Public;

        
        for (int iLoop = 0; iLoop < allArea.Length; iLoop++)
        {
            CodeMemberField field = new CodeMemberField(typeof(System.String),allArea[iLoop].AnimName);
            byte[] nameBytes = Encoding.ASCII.GetBytes(allArea[iLoop].AnimName);
            string value = Encoding.ASCII.GetString(nameBytes);
            field.InitExpression = new CodePrimitiveExpression(value);
            
            field.Attributes = MemberAttributes.Public | MemberAttributes.Const;
            Customerclass3.Members.Add(field);
        }
        //把这个类放在这个命名空间下  
        sampleNamespace.Types.Add(Customerclass3);
        //把该命名空间加入到编译器单元的命名空间集合中  
        unit.Namespaces.Add(sampleNamespace);
        //添加方法（使用CodeMemberMethod)--此处略  
        //添加构造器(使用CodeConstructor) --此处略  
        //添加程序入口点（使用CodeEntryPointMethod） --此处略  
        //添加事件（使用CodeMemberEvent) --此处略  
        //添加特征(使用 CodeAttributeDeclaration)  
        Customerclass.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(SerializableAttribute))));
        //生成代码  
        CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
        CodeGeneratorOptions options = new CodeGeneratorOptions();
        options.BracingStyle = "C";
        options.BlankLinesBetweenMembers = true;
        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(outputFile))
        {
            provider.GenerateCodeFromCompileUnit(unit, sw, options);
        }
    }

}
