using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexMeshChunk : MonoBehaviour
{
    public int index;
    public HexGrid grid;

    Mesh chunkMesh;
    List<Vector3> vertices;
    List<int> triangles;

    void Awake()
    {
        GetComponent<MeshFilter>().mesh = chunkMesh = new Mesh();
        chunkMesh.name = "Chunk Mesh " + index;
        vertices = new List<Vector3>();
        triangles = new List<int>();
    }

    void Start()
    {
        AddTileToMesh();
        RegenerateMesh();
    }

    HexTile[] GetTilesOfChunk()
    {
        int tCS = grid.tilesPerChunkSide;
        HexTile[] tiles = new HexTile[tCS * tCS];

        int firstTileOfChunk = GetFirstTileIndexOfChunk();

        for (int z = 0, i = 0; z < tCS; z++)
        {
            for (int x = 0; x < tCS; x++, i++)
            {
                tiles[i] = grid.tiles[firstTileOfChunk + x + z * tCS * grid.chunkCountX];
            }
        }

        return tiles;

        int GetFirstTileIndexOfChunk()
        {
            int chunkZ = index / grid.chunkCountX;
            int chunkX = index % grid.chunkCountX;

            return chunkZ * grid.tilesPerChunkSide * grid.tilesPerChunkSide * grid.chunkCountX + chunkX * grid.tilesPerChunkSide;
        }
    }



    void AddTileToMesh()
    {
        HexTile[] ctiles = GetTilesOfChunk();

        for (int i = 0; i < ctiles.Length; i++)
        {
            vertices.AddRange(ctiles[i].GetInnerVertices());
            int[] t = new int[ctiles.Length * 3 * 6];

            int startVertex = i * 7;

            t[0] = startVertex;
            t[1] = startVertex + 1;
            t[2] = startVertex + 2;

            t[3] = startVertex;
            t[4] = startVertex + 2;
            t[5] = startVertex + 3;

            t[6] = startVertex;
            t[7] = startVertex + 3;
            t[8] = startVertex + 4;

            t[9] = startVertex;
            t[10] = startVertex + 4;
            t[11] = startVertex + 5;

            t[12] = startVertex;
            t[13] = startVertex + 5;
            t[14] = startVertex + 6;

            t[15] = startVertex;
            t[16] = startVertex + 6;
            t[17] = startVertex + 1;

            triangles.AddRange(t);
        }
    }

    void RegenerateMesh()
    {
        chunkMesh.vertices = vertices.ToArray();
        chunkMesh.triangles = triangles.ToArray();
        chunkMesh.RecalculateNormals();
    }

}