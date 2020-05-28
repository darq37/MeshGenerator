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
    static readonly string textFile = "C:/Users/Mich Ał/Desktop/Magisterka_DZ/ASCII/points.txt";
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    private float[] heights = new float[5621952];

    public int xSize = 1974;
    public int zSize = 2848;

    void Start()
    {
        mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        GetComponent<MeshFilter>().mesh = mesh;

        ReadFile();
        CreateShape();
        UpdateMesh();
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }


    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        

        for (int index = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                //adding perlin noise as height
                float y = Mathf.PerlinNoise(x * .3f, z * .3f) * 2f;
                vertices[index] = new Vector3(x, heights[index], z);
                index++;
            }
        }

        triangles = new int[xSize * zSize * 6];

        int currentVertex = 0;
        int currentTriangle = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
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

            currentVertex++;
        }
    }

    private void ReadFile()
    {
        var reader = new StreamReader(textFile);
        var lines = reader.ReadLine();
        while (!reader.EndOfStream)
        {
            var values = lines.Split(',');
            for (int i = 0; i < heights.Length; i++)
            {
                heights[i] = float.Parse(values[2], CultureInfo.InvariantCulture.NumberFormat);
                System.Console.WriteLine(heights[]);
            }

        }
        reader.Close();

    }

   

}