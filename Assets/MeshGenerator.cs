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
        ReadPointsFromFile();
        UpdateMesh();
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices.OrderByDescending(v => v.x).ToArray();
        mesh.triangles = getTriangles();
        mesh.RecalculateNormals();
    }


    int[] getTriangles()
    {
        
        int vert = 0;
        int tris = 0;
        var xSize = (int)((vertices.Max(v => v.x)) - (vertices.Min(v => v.x)));
        var zSize = (int)((vertices.Max(v => v.z)) - (vertices.Min(v => v.z)));
        var triangles = new int[xSize * zSize * 6];
      

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;
                vert++;
                tris += 6;
            }
        vert++;
        }
        return triangles;
    }

    private void ReadPointsFromFile()
    {
        var reader = new StreamReader(textFile);
        var lines = new List<String>();
        while (!reader.EndOfStream)
        {
            lines.Add(reader.ReadLine());
        }
        reader.Close();

        foreach (var line in lines)
        {
            var values = line.Split(',');
            vertices.Add(new Vector3(
                float.Parse(values[0], CultureInfo.InvariantCulture.NumberFormat),
                float.Parse(values[2], CultureInfo.InvariantCulture.NumberFormat),
                float.Parse(values[1], CultureInfo.InvariantCulture.NumberFormat)
            ));
        }
    }
    private void OnDrawGizmos()
    {
        foreach (var item in vertices)
        {
            Gizmos.DrawSphere(item, .1f);
        }
    }



}