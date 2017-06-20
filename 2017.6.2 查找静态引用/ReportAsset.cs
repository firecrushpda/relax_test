using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
using System.Text;

public partial class ReportAsset
{

    [MenuItem("Tools/Report/脚本Static引用")]
    static void StaticRef()
    {
        //静态引用
        LoadAssembly("Assembly-CSharp-firstpass");
        LoadAssembly("Assembly-CSharp");

    }

    static void LoadAssembly(string name)
    {
        Assembly assembly = null;
        try
        {   
            //读程序集
            assembly = Assembly.Load(name);
        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex.Message);
        }
        finally
        {
            if (assembly != null)
            {
                //遍历程序集中的类型
                foreach (Type type in assembly.GetTypes())
                {
                    try
                    {
                        //将程序集中的公共字段存在一个不会重复的HashSet中
                        HashSet<string> assetPaths = new HashSet<string>();
                        FieldInfo[] listFieldInfo = type.GetFields(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                        foreach (FieldInfo fieldInfo in listFieldInfo)
                        {
                            if (!fieldInfo.FieldType.IsValueType)
                            {
                                SearchProperties(fieldInfo.GetValue(null), assetPaths);
                            }

                        }
                        //打出HashSet中的静态引用对象名称
                        if (assetPaths.Count > 0)
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.AppendFormat("{0}.cs\n", type.ToString());
                            foreach (string path in assetPaths)
                            {
                                sb.AppendFormat("\t{0}\n", path);
                            }
                            Debug.LogError(sb.ToString());
                        }

                    }
                    catch (Exception ex)
                    {
                        Debug.LogWarning(ex.Message);
                    }
                }
            }
        }
    }
    //将解析的obj结果分类迭代解析成object
    //遍历object下的物体和子物体 添加到HashSet下
    static HashSet<string> SearchProperties(object obj, HashSet<string> assetPaths)
    {
        if (obj != null)
        {
            if (obj is UnityEngine.Object)
            {
                UnityEngine.Object[] depen = EditorUtility.CollectDependencies(new UnityEngine.Object[] { obj as UnityEngine.Object });
                foreach (var item in depen)
                {
                    string assetPath = AssetDatabase.GetAssetPath(item);
                    if (!string.IsNullOrEmpty(assetPath))
                    {
                        if (!assetPaths.Contains(assetPath))
                        {
                            assetPaths.Add(assetPath);
                        }
                    }
                }
            }
            else if (obj is IEnumerable)
            {
                foreach (object child in (obj as IEnumerable))
                {
                    SearchProperties(child, assetPaths);
                }
            }
            else if (obj is System.Object)
            {
                if (!obj.GetType().IsValueType)
                {
                    FieldInfo[] fieldInfos = obj.GetType().GetFields();
                    foreach (FieldInfo fieldInfo in fieldInfos)
                    {
                        object o = fieldInfo.GetValue(obj);
                        if (o != obj)
                        {
                            SearchProperties(fieldInfo.GetValue(obj), assetPaths);
                        }
                    }
                }
            }
        }
        return assetPaths;
    }

}
