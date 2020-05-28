using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    static readonly string textFile = "C:/Users/Mich Ał/Desktop/Magisterka_DZ/ASCII/pkt.txt";
    Mesh mesh;
    List<Vector3> vertices = new List<Vector3>();
    void Start()
    {
        mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        GetComponent<MeshFilter>().mesh = mesh;
        this.vertices = ReadFile();
        UpdateMesh();
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.RecalculateNormals();
    }


    int[] CreateShape()
    {
        var triangles = new int[vertices.Count];
        int currentVertex = 0;
        int currentTriangle = 0;
        var xSize = vertices.FindAll(v => v.z == 146223).Count;
        for (int z = 0; z < vertices.Count; z++)
        {
            triangles[currentTriangle + 0] = currentVertex + 0;
            triangles[currentTriangle + 1] = currentVertex + xSize + 1;
            triangles[currentTriangle + 2] = currentVertex + 1;
            triangles[currentTriangle + 3] = currentVertex + 1;
            triangles[currentTriangle + 4] = currentVertex + xSize + 1;
            triangles[currentTriangle + 5] = currentVertex + xSize + 2;
            currentVertex++;
            currentTriangle += 6;
        }
        return triangles;
    }

    private List<Vector3> ReadFile()
    {
        var reader = new StreamReader(textFile);
        var lines = new List<String>();
        while (!reader.EndOfStream)
        {
            lines.Add(reader.ReadLine());
        }
        reader.Close();

        var vertices = new List<Vector3>();
        foreach (var item in lines)
        {
            var vals = item.Split(',');
            vertices.Add(new Vector3(
                float.Parse(vals[0], CultureInfo.InvariantCulture.NumberFormat),
                float.Parse(vals[2], CultureInfo.InvariantCulture.NumberFormat),
                float.Parse(vals[1], CultureInfo.InvariantCulture.NumberFormat)
            ));
        }
        return vertices;
    }
    
    private void OnDrawGizmos() {
        foreach (var item in vertices)
        {
            Gizmos.DrawSphere(item, .03f);
        }
    }
}